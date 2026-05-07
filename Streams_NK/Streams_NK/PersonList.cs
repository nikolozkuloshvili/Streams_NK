namespace FamilyTree;

public class PersonList : List<Person>
{

    public new void Add(Person person)
    {
        ValidatePersonIsNotInTheListAndIsNotNull(person);
        base.Add(person);
    }

    public new void Insert(int index, Person person)
    {
        ValidatePersonIsNotInTheListAndIsNotNull(person);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        base.Insert(index, person);
    }

    public new void InsertRange(int index, IEnumerable<Person> person)
    {
        ArgumentNullException.ThrowIfNull(person);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        base.InsertRange(index, person);
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
            WriteGenerationTree(this, writer, personsId);
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

    private void ValidatePersonIsNotInTheListAndIsNotNull(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        foreach (var p in this)
        {
            if (p.Id == person.Id)
                throw new ArgumentException($"A person with Id {person.Id} already exists.");
        }
    }

    private void WriteGenerationTree(List<Person> family, StreamWriter writer, HashSet<int> personsId)
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
}