namespace Streams_NK;

static class StreamHelper
{
    public static double GetNumbersSum(Stream stream)
    {
        IsNotNullAndIsReadable(stream);

        var originalPosition = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);

        double sum = 0;
        StreamReader reader = new StreamReader(stream, leaveOpen: true);
        while (!reader.EndOfStream)
        {
            if (double.TryParse(reader.ReadLine()!, out double number))
                sum += number;
        }
        stream.Seek(originalPosition, SeekOrigin.Begin);
        reader.Close();

        return sum;
    }

    public static void RewriteWithLineNumbersToNewStream(Stream read, Stream write)
    {
        IsNotNullAndIsReadable(read);

        if (write == null)
            throw new ArgumentNullException(nameof(write));

        if (!write.CanWrite)
            throw new ArgumentException("The stream must be writable.", nameof(write));

        StreamReader reader = new StreamReader(read, leaveOpen: true);
        StreamWriter writer = new StreamWriter(write);
        var originalPosition = read.Position;
        read.Seek(0, SeekOrigin.Begin);

        int i = 1;
        while (!reader.EndOfStream)
        {
            writer.WriteLine($"{i++}.{reader.ReadLine()}");
        }
        read.Seek(originalPosition, SeekOrigin.Begin);

        writer.Close(); reader.Close();
    }

    public static int GetCharachtersCount(Stream stream)
    {
        IsNotNullAndIsReadable(stream);

        StreamReader reader = new StreamReader(stream, leaveOpen: true);

        int symbolsCount = 0; int whiteSpaceCount = 0;
        var originalPosition = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);

        while (!reader.EndOfStream)
        {
            string text = reader.ReadLine()!;
            symbolsCount += text.Length;
            for (int i = 0; i < text.Length; i++)
            {
                if (IsWhiteSpaceCharacter(text[i]))
                    whiteSpaceCount++;
            }
        }
        stream.Seek(originalPosition, SeekOrigin.Begin);
        reader.Close();

        return symbolsCount - whiteSpaceCount;
    }

    public static int GetWordsCount(Stream stream)
    {
        IsNotNullAndIsReadable(stream);

        StreamReader reader = new StreamReader(stream, leaveOpen: true);
        var originalPosition = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);

        int count = 0;
        while (!reader.EndOfStream)
        {
            string[] text = reader.ReadLine()!.Split(new char[] { ' ', '\n', '\r', '\t', '\0' }, StringSplitOptions.RemoveEmptyEntries);
            count += text.Length;
        }

        stream.Seek(originalPosition, SeekOrigin.Begin);
        reader.Close();

        return count;
    }

    public static IDictionary<char, int> ToCharacterDictionarySortedByCount(Stream stream)
    {
        IsNotNullAndIsReadable(stream);

        StreamReader reader = new StreamReader(stream, leaveOpen: true);
        var originalPosition = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);

        int count = 0;

        SortedDictionary<char, int> dictionary = new SortedDictionary<char, int>();

        string text = null!;
        while (!reader.EndOfStream)
        {
            text += reader.ReadLine()!;
            if (reader.EndOfStream)
            {
                char[] temp = text.ToCharArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (IsWhiteSpaceCharacter(temp[i]))
                        continue;

                    if (!dictionary.ContainsKey(temp[i]))
                    {
                        count = 0;
                        for (int j = 0; j < text.Length; j++)
                        {
                            if (text[j] == temp[i])
                                count++;
                        }

                        dictionary.Add(temp[i], count);
                    }
                }
            }
        }
        stream.Seek(originalPosition, SeekOrigin.Begin);

        var sortedByCount = dictionary.OrderByDescending(x => x.Value);
        var sortedDictionary = sortedByCount.ToDictionary(x => x.Key, x => x.Value);
        reader.Close();

        return sortedDictionary;
    }

    private static void IsNotNullAndIsReadable(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));
        if (!stream.CanRead)
            throw new ArgumentException("The stream must be readable.", nameof(stream));
    }

    private static bool IsWhiteSpaceCharacter(char text) => !char.IsLetterOrDigit(text) && !char.IsPunctuation(text) && !char.IsSymbol(text);
}
