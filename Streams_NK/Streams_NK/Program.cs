namespace Encryption;

public static class Program
{
    private const string _password = "123";
    private static string _readPath = null!;
    private static string _writePath = null!;
    private static int _choice = default;

    static void Main()
    {
        while (true)
        {
            try
            {
                _choice = InputHelper.GetUserChoice();

                _readPath = InputHelper.GetReadPath();

                InputHelper.ValidatePassword(_password);

                if (_choice == 1)
                {
                    FileEncrypter();
                    Console.WriteLine();
                    Console.WriteLine($"Encrypted Text: {File.ReadAllText(_writePath)}");
                }

                if (_choice == 2)
                {
                    FileUnecrypter();
                    Console.WriteLine();
                    Console.WriteLine($"Unecrypted Text: {File.ReadAllText(_writePath)}");
                }

                Console.WriteLine();
                Console.WriteLine("Do you want to continue? (Y/N)");
                string? again = Console.ReadLine();
                if (!string.Equals(again, "Y", StringComparison.OrdinalIgnoreCase))
                    break;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error Message:\t{ex.Message}");
                Console.WriteLine($"Error Type:\t{ex.GetType().Name}");
            }
        }
    }

    private static void FileUnecrypter()
    {
        string textToUnencrypt = null!;
        string unEncryptedText = null!;

        using StreamReader reader = new StreamReader(_readPath);

        _writePath = InputHelper.GetWritePath();
        using FileStream file = new FileStream(_writePath, FileMode.Create);
        var originalPosition = file.Position;

        using StreamWriter writer = new StreamWriter(file);

        try
        {
            file.Seek(0, SeekOrigin.Begin);

            while (!reader.EndOfStream)
            {
                textToUnencrypt += reader.ReadLine();
                foreach (char c in textToUnencrypt)
                    unEncryptedText += (char)((c + 50) / 2);
            }

            writer.Write(unEncryptedText);
        }

        finally
        {
            file.Seek(originalPosition, SeekOrigin.Begin);
        }
    }

    private static void FileEncrypter()
    {
        string textToncrypt = null!;
        string encryptedText = null!;

        using StreamReader reader = new StreamReader(_readPath);

        _writePath = InputHelper.GetWritePath();
        using FileStream file = new FileStream(_writePath, FileMode.Create);
        var originalPosition = file.Position;

        using StreamWriter writer = new StreamWriter(file);

        try
        {
            file.Seek(0, SeekOrigin.Begin);

            while (!reader.EndOfStream)
            {
                textToncrypt += reader.ReadLine();
                foreach (char c in textToncrypt)
                    encryptedText += (char)((c * 2) - 50);
            }

            writer.Write(encryptedText);
        }

        finally
        {
            file.Seek(originalPosition, SeekOrigin.Begin);
        }
    }
}