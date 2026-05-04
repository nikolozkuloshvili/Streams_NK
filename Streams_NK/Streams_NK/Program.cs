using System.Security.Authentication;

namespace Encryption;

internal static class Program
{
    private const string _password = "Sudo";

    static void Main()
    {
        while (true)
        {
            try
            {
                int choice = InputHelper.GetUserChoice();
                if (choice == 0)
                    break;

                string readPath = InputHelper.GetReadPath();
                if (readPath == "0")
                    break;

                string fileName = Path.GetFileName(readPath);
                string folderPath = readPath.Replace(fileName, "");

                InputHelper.ValidatePassword(_password);

                if (choice == 1)
                {
                    string writePath = folderPath + "Encrypted_" + fileName;
                    FileEncrypter(readPath, writePath);
                    Console.WriteLine();
                    Console.WriteLine($"Encrypted Text:\n{File.ReadAllText(writePath)}");
                }

                if (choice == 2)
                {
                    string writePath = folderPath + "Unecrypted_" + fileName;
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
            catch (AuthenticationException)
            {
                Console.WriteLine("Too many password failed attempts: Try again later.");
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
        using StreamWriter writer = new StreamWriter(writePath);

        while (!reader.EndOfStream)
        {
            string textToUnencrypt = reader.ReadLine()!;
            string unEncryptedText = "";
            foreach (char c in textToUnencrypt)
                unEncryptedText += (char)((c + 50) / 2);

            writer.WriteLine(unEncryptedText);
        }
    }

    private static void FileEncrypter(string readPath, string writePath)
    {
        using StreamReader reader = new StreamReader(readPath);
        using StreamWriter writer = new StreamWriter(writePath);

        while (!reader.EndOfStream)
        {
            string textToncrypt = reader.ReadLine()!;
            string encryptedText = "";
            foreach (char c in textToncrypt)
                encryptedText += (char)((c * 2) - 50);

            writer.WriteLine(encryptedText);
        }
    }
}