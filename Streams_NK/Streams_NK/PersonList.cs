using System.Collections;

namespace FamilyTree;

public class PersonList : IList<Person>
{
    private List<Person> _list { get; set; } = new List<Person>();
    public int Count => _list.Count;
    public bool IsReadOnly => true;

    public Person this[int index]
    {
        get => _list[index];
        set => throw new ArgumentException("The list is read-only.");
    }

    public void Add(Person person)
    {
        ValidatePerson(person);
        _list.Add(person);
    }

    public void Insert(int index, Person person)
    {
        ValidatePerson(person);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        _list.Insert(index, person);
    }

    public void InsertRange(int index, IEnumerable<Person> persons)
    {
        ArgumentNullException.ThrowIfNull(persons);
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        if (index == Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be equal to the count of the list.");


        foreach (var person in persons)
        {
            ValidatePerson(person);
            Insert(index, person);
            index++;
        }
    }


    public void Save(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var position = stream.Position;

        try
        {
            if (!stream.CanSeek)
                throw new ArgumentException("The provided stream must support seeking.", nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);

            if (!stream.CanWrite)
                throw new ArgumentException("The provided stream must be writable.", nameof(stream));

            using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);

            HashSet<int> personsId = new HashSet<int>();
            WriteGenerationTree(_list, writer, personsId);
        }
        finally
        {
            stream.Seek(position, SeekOrigin.Begin);
        }
    }

    public void Load(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var position = stream.Position;

        try
        {
            if (!stream.CanSeek)
                throw new ArgumentException("The provided stream must support seeking.", nameof(stream));

            if (!stream.CanRead)
                throw new ArgumentException("The provided stream must be readable.", nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new StreamReader(stream, leaveOpen: true);
            Console.WriteLine(reader.ReadToEnd());
        }
        finally
        {
            stream.Seek(position, SeekOrigin.Begin);
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

    private void ValidatePerson(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        foreach (var p in _list)
        {
            if (p.Id == person.Id)
                throw new ArgumentException($"A person with Id {person.Id} already exists.");
        }
    }

    private void WriteGenerationTree(IEnumerable<Person> family, StreamWriter writer, HashSet<int> personsId)
    {
        foreach (var person in family)
        {
            if (personsId.Add(person.Id))
            {
                writer.WriteLine(person.ToString());
            }

            if (person.Children != null && person.Children.Count > 0)
            {
                WriteGenerationTree(person.Children, writer, personsId);
            }
        }
    }

    public int IndexOf(Person item) => _list.IndexOf(item);

    public void RemoveAt(int index) => _list.RemoveAt(index);

    public void Clear() => _list.Clear();

    public bool Contains(Person item) => _list.Contains(item);

    public void CopyTo(Person[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public bool Remove(Person item) => _list.Remove(item);

    public IEnumerator<Person> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}