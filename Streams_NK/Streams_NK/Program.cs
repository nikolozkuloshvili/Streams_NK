namespace Streams_NK;

internal class Program
{
    const string ReadPath = @"G:\ToRead.txt";
    const string WritePath = @"G:\ToWrite.txt";

    static void Main()
    {
        FileStream read = new FileStream(ReadPath, FileMode.Open, FileAccess.Read);
        FileStream write = new FileStream(WritePath, FileMode.Open, FileAccess.Write);

        StreamHelper.RewriteWithLineNumbersToNewStream(read, write);
        string copiedText = File.ReadAllText(WritePath);

        Console.WriteLine($"Rewriten text to New Text File\n{copiedText}");
        StreamHelper.GetCharachtersCount(read);
        StreamHelper.GetCharachtersCount(read);
        Console.WriteLine($"Total amount of Characters = {StreamHelper.GetCharachtersCount(read)}");
        Console.WriteLine();
        StreamHelper.GetWordsCount(read);
        Console.WriteLine($"Total amount of Words = {StreamHelper.GetWordsCount(read)}");
        Console.WriteLine();

        Console.WriteLine($"Sum of Numbers = {StreamHelper.GetNumbersSum(read)}");
        Console.WriteLine();
        var dictionary = StreamHelper.ToCharacterDictionarySortedByCount(read);
        foreach (var item in dictionary)
            Console.WriteLine($"{item.Key} = {item.Value}");
    }
}
