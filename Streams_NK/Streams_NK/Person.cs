namespace FamilyTree;

public class Person
{
    public int Id { get; }
    public int ParentId { get; private set; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
    public Gender Gender { get; }
    public List<Person> Children { get; set; } = new List<Person>();

    public Person(int id, string firstName, string lastName, DateTime dateOfBirth, Gender gender)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id must be a positive integer.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or whitespace.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));

        if (dateOfBirth > DateTime.Now)
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "Date of birth cannot be in the future.");

        if (dateOfBirth < DateTime.Now.AddYears(-120))
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "Date of birth cannot be more than 120 years ago.");

        DateOfBirth = dateOfBirth;

        if (!Enum.IsDefined(typeof(Gender), gender))
            throw new ArgumentOutOfRangeException(nameof(gender), "Invalid gender value.");

        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Id = id;
    }

    public Person(int id, int parentId, string firstName, string lastName, DateTime dateOfBirth, Gender gender) : this(id, firstName, lastName, dateOfBirth, gender)
    {
        if (ParentId == id)
            throw new ArgumentException("ParentId cannot be the same as Id.", nameof(ParentId));

        ParentId = parentId;
    }


    public void AddChild(Person child)
    {
        foreach (var kid in Children)
        {
            if (child.Id == kid.Id)
                throw new InvalidOperationException($"Child with Id {child.Id} is already added.");
        }

        if (child.Id == this.Id)
            throw new InvalidOperationException("Cannot add self as a child.");

        if (child.ParentId != default)
            throw new InvalidOperationException("Child already has a parent.");

        if (child.Id == ParentId)
            throw new InvalidOperationException("Child cannot have parent as a child.");

        if (child.DateOfBirth <= this.DateOfBirth)
            throw new InvalidOperationException("Child must be younger than the parent.");

        child.ParentId = this.Id;

        Children.Add(child);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} (Id: {Id}, ParentId: {ParentId}, DOB: {DateOfBirth.ToShortDateString()}, Gender: {Gender})";
    }
}

public enum Gender
{
    Male,
    Female,
}