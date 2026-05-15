namespace Genealogy_Tree;

internal class Program
{
    static void Main()
    {
        Person father = new(id: 1, firstName: "Father", lastName: "None", dateOfBirth: new DateTime(1980, 1, 1), gender: Gender.Male);
        Person father2 = new(id: 7, firstName: "Father", lastName: "None", dateOfBirth: new DateTime(1980, 1, 1), gender: Gender.Male);

        Person daugther = new(id: 2, firstName: "Daugther", lastName: "None", dateOfBirth: new DateTime(2005, 5, 15), gender: Gender.Female);
        Person daugther2 = new(id: 2, firstName: "Daugther", lastName: "None", dateOfBirth: new DateTime(2005, 5, 15), gender: Gender.Female);

        Person grandDaugther = new(id: 3, firstName: "grandDaugther", lastName: "None", dateOfBirth: new DateTime(2025, 5, 15), gender: Gender.Female);
        Person grandSon = new(id: 4, firstName: "grandSon", lastName: "None", dateOfBirth: new DateTime(2025, 5, 15), gender: Gender.Male);
        Person grandDaugther2 = new(id: 5, firstName: "grandDaugther2", lastName: "None", dateOfBirth: new DateTime(2025, 5, 15), gender: Gender.Female);

        PersonList list1 = new();
        PersonList list2 = new();

        father.AddChild(daugther);
        daugther.AddChild(grandDaugther);
        grandDaugther.AddChild(grandSon);
        grandSon.AddChild(grandDaugther2);

        //father2.AddChild(daugther2);
        //father.AddChild(father2);

        list1.Add(father);
        list1.Save("genealogy_tree");

        list2.Load("genealogy_tree");

        foreach (var person in list2)
        {
            Console.WriteLine(person);
        }
    }
}
