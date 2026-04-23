namespace Streams_NK;

internal class Program
{
    const string ReadPath = @"G:\ToRead.txt";
    const string WritePath = @"G:\ToWrite.txt";

    static void Main()
    {
        try
        {
            FileStream read = new FileStream(ReadPath, FileMode.Open, FileAccess.Read);
            FileStream write = new FileStream(WritePath, FileMode.Open, FileAccess.Write);

            StreamHelper.RewriteWithLineNumbersToNewStream(read, write);
            Console.WriteLine($"Rewriten text to New Text File\n{File.ReadAllText(WritePath)}");

            Console.WriteLine($"Total amount of Characters = {StreamHelper.GetCharachtersCount(read)}");
            Console.WriteLine();

            Console.WriteLine($"Total amount of Words = {StreamHelper.GetWordsCount(read)}");
            Console.WriteLine();

            Console.WriteLine($"Sum of Numbers = {StreamHelper.GetNumbersSum(read)}");
            Console.WriteLine();

            foreach (var item in StreamHelper.ToCharDictionarySortedByCount(read))
                Console.WriteLine($"{item.Key} = {item.Value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
