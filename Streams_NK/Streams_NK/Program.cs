namespace Streams_NK;

internal class Program
{
    const string ReadPath = @"G:\ToRead.txt";
    const string WritePath = @"G:\ToWrite.txt";

    static void Main()
    {
        TxtFileHelper.Rewrite_NumberLinesText_ToNewTextFile(ReadPath, WritePath);
        string copiedText = File.ReadAllText(WritePath);

        Console.WriteLine($"Rewriten text to New Text File\n{copiedText}");
        
        Console.WriteLine($"Total amount of Characters = {TxtFileHelper.GetAmountOf_Charachters_InTextFile(ReadPath)}");
        Console.WriteLine();

        Console.WriteLine($"Total amount of Words = {TxtFileHelper.GetAmountOf_Words_InTextFile(ReadPath)}");
        Console.WriteLine();

        Console.WriteLine($"Sum of Numbers = {TxtFileHelper.GetNumbers_Sum_FromTextFile(ReadPath)}");
        Console.WriteLine();

        var list = TxtFileHelper.GetSorted_List_OfEachCharacterAmounts_InTextFile(ReadPath);
        foreach (var item in list)
            Console.WriteLine($"{item.Name} = {item.Count}");
    }
}
