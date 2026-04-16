namespace Streams_NK;

static class TxtFileHelper
{
    public static double NumberSummer(string path)
    {
        string text = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        ValidateFile_HasOnly_Numbers(text);

        char[] symbols = new char[text.Length];
        string number;

        int count = 0;
        double sum = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (!IsSymbol_Or_Letter(text[i]))
            {
                number = new string(symbols).Replace("\0", null);

                if (number == "" || number == "\t")
                    continue;

                sum += int.Parse(number);
                count++;

                Array.Clear(symbols);
                continue;
            }

            symbols[i] = text[i];
        }

        number = new string(symbols).Replace("\0", "");
        if (number == "" || number == "\t")
            return sum;

        sum += double.Parse(number);

        return sum;
    }

    public static void TextNumberingHelper(StreamReader reader, StreamWriter writer)
    {
        int i = 1;
        while (!reader.EndOfStream)
        {
            writer.WriteLine($"{i}.{reader.ReadLine()}"); i++;
        }
        writer.Close();
    }

    public static int LettersAndSymbols_Counter(string path)
    {
        string text = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        int enterOrSpaceCount = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (!IsSymbol_Or_Letter(text[i]))
                enterOrSpaceCount++;
        }

        int symbolsCount = text.Length - enterOrSpaceCount;
        return symbolsCount;
    }

    public static int WordCounter(string path)
    {
        string text = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        int count = 0;
        bool wordStarted = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (!IsSymbol_Or_Letter(text[i]))
            {
                wordStarted = false;
            }
            else if (wordStarted == false)
            {
                count++;
                wordStarted = true;
            }
        }

        return count;
    }

    public static void EachUnique_Char_Counter(string path)
    {
        string text = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        char[] symbols = GetCharsFromString(text);

        int count = 0;

        HashSet<char> unique = new HashSet<char>();
        List<Symbol> list = new List<Symbol>();

        for (int i = 0; i < symbols.Length; i++)
        {
            if (!IsSymbol_Or_Letter(symbols[i]))
                break;

            if (unique.Add(symbols[i]))
            {
                count = 0;
                for (int j = 0; j < text.Length; j++)
                {
                    if (text[j] == symbols[i])
                        count++;
                }

                list.Add(new Symbol(symbols[i], count));
            }
        }

        list.Sort();
        foreach (var item in list)
            Console.WriteLine($"{item.Name} = {item.Count}");
    }

    private static void ValidateFile_HasOnly_Numbers(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        for (int i = 0; i < text.Length; i++)
        {
            if (!(IsNumber(text[i]) || IsSymbol_Or_Letter(text[i])))
                throw new InvalidDataException();
        }
    }

    private static bool IsSymbol_Or_Letter(char text)
    {
        if (text == 32 || text == 10 || text == 13 || text == 0 || text == 9)
            return false;

        return true;
    }

    private static bool IsNumber(char? text)
    {
        if (text >= '0' && text <= '9')
            return false;

        return true;
    }

    private static char[] GetCharsFromString(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        char[] symbols = new char[text.Length];
        int count = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (!IsSymbol_Or_Letter(text[i]))
            {
                count++; continue;
            }

            symbols[i - count] = text[i];
        }

        return symbols;
    }
}
