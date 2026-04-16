namespace Streams_NK;

static class TxtFileHelper
{
    public static double GetNumbers_Sum_FromTextFile(string path)
    {
        string text = File.ReadAllText(path);
        double sum = 0;
        string number;

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        char[] symbols = new char[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            if (!IsNumber(text[i]) && !IsFloat_Or_Negative(text[i]))
            {
                number = new string(symbols).Replace("\0", null);

                if (number == "" || number == "\t")
                    continue;

                sum += double.Parse(number);

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

    public static void Rewrite_NumberLinesText_ToNewTextFile(string pathRead, string pathWrite)
    {
        StreamReader reader = new StreamReader(pathRead);
        StreamWriter writer = new StreamWriter(pathWrite);

        int i = 1;
        while (!reader.EndOfStream)
        {
            writer.WriteLine($"{i}.{reader.ReadLine()}"); i++;
        }
        writer.Close();
    }

    public static int GetAmountOf_Charachters_InTextFile(string path)
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

    public static int GetAmountOf_Words_InTextFile(string path)
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

    public static List<Symbol> GetSorted_List_OfEachCharacterAmounts_InTextFile(string path)
    {
        string text = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));

        char[] symbols = Get_CharsArray_FromString(text);

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

        return list;
    }

    private static bool IsNumber(char text) => (text >= '0' && text <= '9');

    private static bool IsFloat_Or_Negative(char text) => (text == ',' || text == '-');

    private static bool IsSymbol_Or_Letter(char text) => !(text == ' ' || text == '\n' || text == '\r' || text == '\t' || text == 0); //0 = null

    private static char[] Get_CharsArray_FromString(string text)
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
