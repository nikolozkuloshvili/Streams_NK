namespace Genealogy_Tree;

public class Person
{
    private readonly List<Person> _children = new List<Person>();
    private int _id;
    private int _parentId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private Person? _parent = null;
    private DateTime _dateOfBirth;

    public int Id
    {
        get => _id;
        private set
        {
            if (value <= 0)
                throw new ArgumentException(nameof(value), "Id must be a positive integer");
            if (value == this._parentId)
                throw new ArgumentException(nameof(value), "Id cannot be the same as the Parent Id");
            _id = value;
        }
    }

    public int ParentId
    {
        get => _parentId;
        private set
        {
            if (value < 0)
                throw new ArgumentException(nameof(value), "Parent Id must be a non-negative integer");
            if (value == this._id)
                throw new ArgumentException(nameof(value), "Parent Id cannot be the same as the person's Id");
            _parentId = value;
        }
    }

    public string FirstName
    {
        get => _firstName!;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value), "The First name cannot be Empty");
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName!;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value), "The Last name cannot be Empty");
            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        private set
        {
            if (value > DateTime.Now)
                throw new ArgumentOutOfRangeException(nameof(value), "Date of Birth cannot be in the future");
            if (value <= DateTime.Now.AddYears(-120))
                throw new ArgumentOutOfRangeException(nameof(value), "Date of Birth cannot be before ");
            _dateOfBirth = value;
        }
    }

    public Gender Gender { get; private set; }
    public IReadOnlyCollection<Person> Children => _children.AsReadOnly();

    public Person(int id, string firstName, string lastName, DateTime dateOfBirth, Gender gender)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public void AddChild(Person child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child), "Child cannot be null");

        if (child.ParentId != default && child.ParentId != this.Id)
            throw new ArgumentException("This child already has a parent");

        if (this.ParentId == child.Id)
            throw new ArgumentException("Child cannot be add as Parent");

        if (child.Id == this.Id)
            throw new ArgumentException("Child cannot be the same as Parent");

        foreach (var kid in _children)
        {
            if (child.Id == kid.Id)
                throw new ArgumentException("Child with this id is already added");
        }

        ValidateChildParent(child, this._parent!);
        ValidateChildren(this.Children, child.Children);

        child._parentId = this.Id;
        child._parent = this;

        _children.Add(child);
    }

    public void RemoveChild(Person child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child), "Do not even try to delete a null Child!");

        if (!_children.Remove(child))
            throw new ArgumentException("The child you are trying to remove does not exist in the children list!");

        child._parent = null;
        child._parentId = default;
    }

    private static void ValidateChildParent(Person child, Person parent)
    {
        if (child == parent)
            throw new ArgumentException("This Child is your Grate Ancestor");

        if (parent == null || parent.Id == default)
        {
            return;
        }

        if (child.Id == parent.Id)
            throw new ArgumentException("This Child is your Grate Ancestor");

        if (parent._parent != null)
        {
            ValidateChildParent(child, parent._parent);
        }
    }

    private static void ValidateChildren(IEnumerable<Person> person, IEnumerable<Person> children)
    {
        foreach (var kid in children)
        {
            foreach (var child in person)
            {
                if (kid.Id == child.Id)
                {
                    throw new ArgumentException($"Duplicate child detected: {kid.Id}");
                }

                if (child._children != null)
                {
                    ValidateChildren(kid.Children, child._children);
                }
            }
        }
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} (Id: {Id}, ParentId: {ParentId}, DOB: {DateOfBirth.ToShortDateString()}, Gender: {Gender})";
    }

    public void WriteToBinary(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(ParentId);
        writer.Write(FirstName);
        writer.Write(LastName);
        writer.Write(DateOfBirth.ToBinary());
        writer.Write((int)Gender);
    }

    public static Person ReadFromBinary(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        int parentId = reader.ReadInt32();
        string firstName = reader.ReadString();
        string lastName = reader.ReadString();
        DateTime dateOfBirth = DateTime.FromBinary(reader.ReadInt64());
        Gender gender = (Gender)reader.ReadInt32();

        Person person = new Person(id, firstName, lastName, dateOfBirth, gender);
        person._parentId = parentId;
        return person;
    }
}

public enum Gender
{
    Male,
    Female,
}
