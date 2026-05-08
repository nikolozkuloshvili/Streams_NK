namespace FamilyTree;

internal class Program
{
    static void Main()
    {
        try
        {
            PersonList list = new PersonList();
            Person adam = new Person(id: 1,parentId: 1, firstName: "Adam", lastName: "Human", dateOfBirth: new DateTime(1960, 1, 1),Gender.Male);
            Person eva = new Person(id: 2, firstName: "Eva", lastName: "Human", dateOfBirth: new DateTime(1980, 2, 2), Gender.Female);

            list.Add(adam);
            list.Add(eva);
            adam.AddChild(eva);

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
