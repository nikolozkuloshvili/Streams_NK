using System.Collections;
namespace Genealogy_Tree;

public class PersonList : IList<Person>
{
    private readonly List<Person> _persons = new();
    private HashSet<int> _uniqIdHolder = new();

    public Person this[int index]
    {
        get => _persons[index];
        set
        {
            Person existingPerson = _persons[index];
            if (existingPerson.Id == value.Id)
            {
                _persons[index] = value;
                return;
            }

            if (_uniqIdHolder.Contains(value.Id))
            {
                throw new ArgumentException("Person with the same Id already exists in the list.");
            }

            _uniqIdHolder.Remove(existingPerson.Id);
            _uniqIdHolder.Add(value.Id);
            _persons[index] = value;
        }
    }

    public void Insert(int index, Person person)
    {
        if (_uniqIdHolder.Add(person.Id))
        {
            _persons.Insert(index, person);
            return;
        }
        throw new ArgumentException("Person with the same Id already exists in the list.");
    }

    public void Add(Person person)
    {
        if (_uniqIdHolder.Add(person.Id))
        {
            _persons.Add(person);
            return;
        }
        throw new ArgumentException("Person with the same Id already exists in the list.");
    }

    public void AddRange(IEnumerable<Person> persons)
    {
        HashSet<int> temp = new HashSet<int>();

        foreach (var p in persons)
        {
            if (_uniqIdHolder.Contains(p.Id))
                throw new ArgumentException("Person with the same Id already exists in the list.");

            temp.Add(p.Id);
        }

        if (temp.Count == persons.Count())
        {
            foreach (var item in persons)
            {
                _persons.Add(item);
                _uniqIdHolder.Add(item.Id);
            }
        }
    }

    public void InsertRange(int index, IEnumerable<Person> persons)
    {
        HashSet<int> temp = new HashSet<int>();
        foreach (var p in persons)
        {
            if (_uniqIdHolder.Contains(p.Id))
                throw new ArgumentException("Person with the same Id already exists in the list.");

            temp.Add(p.Id);
        }

        if (temp.Count == persons.Count())
        {
            foreach (var item in persons)
            {
                _persons.Insert(index, item);
                _uniqIdHolder.Add(item.Id);
                index++;
            }
        }
    }

    public void Save(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanWrite)
            throw new ArgumentException("The provided stream must be writable.", nameof(stream));

        if (!stream.CanSeek)
            throw new ArgumentException("The provided stream must support seeking to reset the position after writing.", nameof(stream));

        var originalPosition = stream.Position;
        try
        {
            stream.Seek(0, SeekOrigin.Begin);

            using BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);

            foreach (Person person in this)
            {
                ChildrenToBinaryWriter(person.Children, writer);

                person.WriteToBinary(writer);
            }
        }
        finally
        {
            stream.Seek(originalPosition, SeekOrigin.Begin);
        }
    }
    private static void ChildrenToBinaryWriter(IEnumerable<Person> children, BinaryWriter writer)
    {
        foreach (var child in children)
        {
            child.WriteToBinary(writer);
            ChildrenToBinaryWriter(child.Children, writer);
        }
    }

    public void Load(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanRead)
            throw new ArgumentException("The provided stream must be readable.", nameof(stream));

        if (!stream.CanSeek)
            throw new ArgumentException("The provided stream must support seeking to reset the position after reading.", nameof(stream));

        var originalPosition = stream.Position;
        try
        {
            stream.Seek(0, SeekOrigin.Begin);

            using BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);

            Dictionary<int, Person> allPeople = new();
            while (stream.Position < stream.Length)
            {
                Person person = Person.ReadFromBinary(reader);
                allPeople[person.Id] = person;
            }

            foreach (var person in allPeople.Values)
            {
                if (person.ParentId != default && allPeople.TryGetValue(person.ParentId, out Person? parent))
                {
                    parent.AddChild(person);
                }
            }

            foreach (var person in allPeople.Values)
            {
                if (person.ParentId == default || !allPeople.ContainsKey(person.ParentId))
                {
                    this.Add(person);
                }
            }
        }
        finally
        {
            stream.Seek(originalPosition, SeekOrigin.Begin);
        }
    }

    public void Save(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        using FileStream fileStream = new(filePath, FileMode.Create);
        Save(fileStream);
    }

    public void Load(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");

        using FileStream fileStream = new(filePath, FileMode.Open);
        Load(fileStream);
    }

    public void Clear()
    {
        _persons.Clear(); _uniqIdHolder.Clear();
    }

    public bool Remove(Person item)
    {
        if (item == null)
            return false;

        if (_uniqIdHolder.Remove(item.Id) && _persons.Remove(item))
            return true;

        return false;
    }

    public void RemoveAt(int index)
    {
        Person person = _persons[index];
        _uniqIdHolder.Remove(person.Id);
        _persons.RemoveAt(index);
    }

    public int Count => _persons.Count;
    public bool IsReadOnly => false;
    public bool Contains(Person person) => _persons.Contains(person);
    public void CopyTo(Person[] array, int arrayIndex) => _persons.CopyTo(array, arrayIndex);
    public int IndexOf(Person person) => _persons.IndexOf(person);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Person> GetEnumerator() => _persons.GetEnumerator();
}
