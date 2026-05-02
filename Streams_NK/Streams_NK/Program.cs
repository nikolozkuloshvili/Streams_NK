namespace Encryption;

internal static class Program
{
    private const string _password = "123";

    static void Main()
    {
        while (true)
        {
            try
            {
                int choice = InputHelper.GetUserChoice();

                string readPath = InputHelper.GetReadPath();
                string directory = Path.GetDirectoryName(readPath)!;
                string fileName = Path.GetFileName(readPath);

                InputHelper.ValidatePassword(_password);

                if (choice == 1)
                {
                    string writePath = directory + "Encrypted_" + fileName;
                    FileEncrypter(readPath, writePath);
                    Console.WriteLine();
                    Console.WriteLine($"Encrypted Text:\n{File.ReadAllText(writePath)}");
                }

                if (choice == 2)
                {
                    string writePath = directory + "Unecrypted_" + fileName;
                    FileUnecrypter(readPath, writePath);
                    Console.WriteLine();
                    Console.WriteLine($"Unecrypted Text:\n{File.ReadAllText(writePath)}");
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

    private static void FileUnecrypter(string readPath, string writePath)
    {
        using StreamReader reader = new StreamReader(readPath);

        using FileStream file = new FileStream(writePath, FileMode.Create);
        var originalPosition = file.Position;

        using StreamWriter writer = new StreamWriter(file);

        try
        {
            file.Seek(0, SeekOrigin.Begin);

            string unEncryptedText = null!;
            while (!reader.EndOfStream)
            {
                string textToUnencrypt = reader.ReadLine()!;
                unEncryptedText = "";
                foreach (char c in textToUnencrypt)
                    unEncryptedText += (char)((c + 50) / 2);

                writer.WriteLine(unEncryptedText);
            }
        }

        finally
        {
            file.Seek(originalPosition, SeekOrigin.Begin);
        }
    }

    private static void FileEncrypter(string readPath, string writePath)
    {
        using StreamReader reader = new StreamReader(readPath);
        using FileStream file = new FileStream(writePath, FileMode.Create);
        var originalPosition = file.Position;

        using StreamWriter writer = new StreamWriter(file);

        try
        {
            file.Seek(0, SeekOrigin.Begin);

            string encryptedText = null!;
            while (!reader.EndOfStream)
            {
                string textToncrypt = reader.ReadLine()!;
                encryptedText = "";
                foreach (char c in textToncrypt)
                    encryptedText += (char)((c * 2) - 50);

                writer.WriteLine(encryptedText);
            }
        }

        finally
        {
            file.Seek(originalPosition, SeekOrigin.Begin);
        }
    }
}