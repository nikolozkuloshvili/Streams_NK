namespace FamilyTree;

internal class Program
{
    static void Main()
    {
        try
        {
            PersonList list = new PersonList();
            Person person1 = new Person(id: 1,parentId: 1, firstName: "John", lastName: "Doe", dateOfBirth: new DateTime(1990, 1, 1),Gender.Male);
            Person person2 = new Person(id: 2, firstName: "John", lastName: "Doe", dateOfBirth: new DateTime(1990, 1, 1), Gender.Male);

            list.Add(person1);
            list.Add(person2);
            person1.AddChild(person2);

            using var file = new FileStream("Saved.txt", FileMode.Create);

            list.Save(file);
            list.Load(file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}
