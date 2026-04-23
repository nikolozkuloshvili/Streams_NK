namespace Streams_NK;

static class StreamHelper
{
    public static double GetNumbersSum(Stream stream)
    {
        double sum = 0;
        try
        {
            IsNotNullAndIsReadable(stream);
            var originalPosition = stream.Position;
            StreamReader reader = new StreamReader(stream, leaveOpen: true);
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                while (!reader.EndOfStream)
                {
                    if (double.TryParse(reader.ReadLine()!, out double number))
                        sum += number;
                }
                stream.Seek(originalPosition, SeekOrigin.Begin);
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not calculate sum:\n Error: {ex.Message}");
        }

        return sum;
    }

    public static void RewriteWithLineNumbersToNewStream(Stream read, Stream write)
    {
        try
        {
            IsNotNullAndIsReadable(read);
            IsNotNullAndIsWritable(write);

            var originalPosition = read.Position;

            StreamReader reader = new StreamReader(read, leaveOpen: true);
            StreamWriter writer = new StreamWriter(write);
            try
            {
                read.Seek(0, SeekOrigin.Begin);

                int i = 1;
                while (!reader.EndOfStream)
                {
                    writer.WriteLine($"{i++}.{reader.ReadLine()}");
                }
                read.Seek(originalPosition, SeekOrigin.Begin);
            }
            finally
            {
                read.Seek(originalPosition, SeekOrigin.Begin);
                reader.Close(); writer.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not rewrite to new stream:\n Error: {ex.Message}");
        }
    }

    public static int GetCharachtersCount(Stream stream)
    {
        int symbolsCount = 0; int whiteSpaceCount = 0;
        try
        {
            IsNotNullAndIsReadable(stream);

            var originalPosition = stream.Position;

            StreamReader reader = new StreamReader(stream, leaveOpen: true);
            try
            {
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
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not count characters:\n Error: {ex.Message}");
        }

        return symbolsCount - whiteSpaceCount;
    }

    public static int GetWordsCount(Stream stream)
    {
        int count = 0;
        try
        {
            IsNotNullAndIsReadable(stream);

            var originalPosition = stream.Position;

            StreamReader reader = new StreamReader(stream, leaveOpen: true);
            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                while (!reader.EndOfStream)
                {
                    string[] text = reader.ReadLine()!.Split(new char[] { ' ', '\n', '\r', '\t', '\0' }, StringSplitOptions.RemoveEmptyEntries);
                    count += text.Length;
                }
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not count words:\n Error: {ex.Message}");
        }

        return count;
    }

    public static IDictionary<char, int> ToCharDictionarySortedByCount(Stream stream)
    {
        SortedDictionary<char, int> dictionary = new SortedDictionary<char, int>();

        try
        {
            IsNotNullAndIsReadable(stream);

            var originalPosition = stream.Position;

            StreamReader reader = new StreamReader(stream, leaveOpen: true);
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                string text = null!;

                while (!reader.EndOfStream)
                    text += reader.ReadLine()!;

                foreach (char item in text)
                {
                    if (char.IsWhiteSpace(item) || item == 0)
                        continue;

                    if (dictionary.ContainsKey(item))
                    {
                        dictionary[item]++;
                    }
                    else
                    {
                        dictionary[item] = 1;
                    }
                }
            }
            finally
            {
                stream.Seek(originalPosition, SeekOrigin.Begin);
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not create dictionary:\n Error: {ex.Message}");
        }

        var sortedByCount = dictionary.OrderByDescending(x => x.Value);
        var sortedDictionary = sortedByCount.ToDictionary(x => x.Key, x => x.Value);

        return sortedDictionary;
    }

    private static void IsNotNullAndIsReadable(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));
        if (!stream.CanRead)
            throw new ArgumentException("The stream must be Readable.", nameof(stream));
    }

    private static void IsNotNullAndIsWritable(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));
        if (!stream.CanWrite)
            throw new ArgumentException("The stream must be Writable.", nameof(stream));
    }

    private static bool IsWhiteSpaceCharacter(char text) => !char.IsLetterOrDigit(text) && !char.IsPunctuation(text) && !char.IsSymbol(text);
}
