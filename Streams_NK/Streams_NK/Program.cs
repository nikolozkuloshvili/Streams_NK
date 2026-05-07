namespace FamilyTree;

internal class Program
{
    static void Main()
    {
        try
        {
            PersonList list = new PersonList();
            Person tamta = new Person(1, "Tamta", "Kuloshvili", new DateTime(2000, 12, 12), Gender.Female);
            Person anano = new Person(2, "Anano", "Kuloshvili", new DateTime(2001, 12, 12), Gender.Female);
            Person zviad = new Person(4, "Zviad", "Kuloshvili", new DateTime(2000, 3, 29), Gender.Male);
            Person kid1 = new Person(3, "Kid1", "Kuloshvili", new DateTime(2020, 1, 1), Gender.Male);
            Person kid2 = new Person(5, "Kid2", "Kuloshvili", new DateTime(2021, 1, 1), Gender.Female);

            zviad.AddChild(tamta);

            list.Add(zviad);
            list.Add(tamta);
            tamta.AddChild(anano);
            tamta.AddChild(kid2);
            list.Add(kid1);

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
