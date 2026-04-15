namespace Streams_NK;

internal class Program
{
    const string ReadPath = @"G:\ToRead.txt";
    const string WritePath = @"G:\ToWrite.txt";

    static void Main()
    {
        StreamReader read = new StreamReader(ReadPath);
        StreamWriter write = new StreamWriter(WritePath);

        string text = File.ReadAllText(ReadPath);
        TxtFileHelper.TextNumberingHelper(read, write);

        string copiedText = File.ReadAllText(WritePath);
        Console.WriteLine(copiedText);
        Console.WriteLine();

        Console.WriteLine($"Total amount of Characters = {TxtFileHelper.LettersAndSymbols_Counter(ReadPath)}");
        Console.WriteLine();

        Console.WriteLine($"Total amount of Words = {TxtFileHelper.WordCounter(ReadPath)}");
        Console.WriteLine();

        Console.WriteLine($"Sum of Numbers = {TxtFileHelper.NumberSummer(ReadPath)}");
        Console.WriteLine();

        TxtFileHelper.EachUnique_Char_Counter(ReadPath);
        Console.WriteLine();
    }
}
