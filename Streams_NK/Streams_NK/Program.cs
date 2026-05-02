namespace Encryption;

public static class Program
{
    private const string Password = "123";
    public static string ReadPath = null!;
    public static string WritePath = null!;
    public static int Choice = default;
    static void Main()
    {
        try
        {
            InputHelper.GetUserChoice();

            InputHelper.GetReadPath();

            InputHelper.ValidatePassword(Password);

            if (Choice == 1)
                FileEncrypter();

            if (Choice == 2)
                FileUnEcrypter();
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error Message:\t{ex.Message}");
            Console.WriteLine($"Error Type:\t{ex.GetType().Name}");
        }
    }

    private static void FileUnEcrypter()
    {
        string textToUnencrypt = null!;
        string unEncryptedText = null!;

        using StreamReader reader = new StreamReader(ReadPath);

        InputHelper.GetWritePath();
        using FileStream file = new FileStream(WritePath, FileMode.Create);
        using StreamWriter writer = new StreamWriter(file);

        while (!reader.EndOfStream)
        {
            textToUnencrypt += reader.ReadLine();
            foreach (char c in textToUnencrypt)
                unEncryptedText += (char)((c + 50) / 2);

            writer.Write(unEncryptedText);
        }
    }
    private static void FileEncrypter()
    {
        string textToncrypt = null!; 
        string encryptedText = null!;

        using StreamReader reader = new StreamReader(ReadPath);

        InputHelper.GetWritePath();
        using FileStream file = new FileStream(WritePath, FileMode.Create);
        using StreamWriter writer = new StreamWriter(file);

        while (!reader.EndOfStream)
        {
            textToncrypt += reader.ReadLine();
            foreach (char c in textToncrypt)
                encryptedText += (char)((c * 2) - 50);

            writer.Write(encryptedText);
        }
    }
}