using System.Security.Authentication;

namespace Encryption;

public static class InputHelper
{
    public static int GetUserChoice()
    {
        while (true)
        {
            Console.WriteLine("What you want to do:");
            Console.WriteLine("1 - Encrypt");
            Console.WriteLine("2 - UnEncrypt");
            Console.WriteLine("0 - Exit");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int Choice) || Choice < 0 || Choice > 2)
            {
                Console.WriteLine("Invalid choice, try again.\n");
                continue;
            }

            return Choice;
        }
    }

    public static string GetReadPath()
    {
        while (true)
        {
            Console.WriteLine("Enter the file path: ");
            Console.WriteLine("0 - Exit");
            string readPath = Console.ReadLine() ?? "";

            if (readPath == "0")
                return readPath;

            if (File.Exists(readPath))
                return readPath;

            Console.WriteLine("Wrong path: Try Again");
        }
    }

    public static void ValidatePassword(string Password)
    {
        int count = 3;
        while (true)
        {
            Console.WriteLine("Enter the password: ");
            Console.WriteLine($"Attempts Left: {count--}");
            string inputPassword = Console.ReadLine() ?? "";

            if (inputPassword == Password)
                break;

            if (count == 0)
                throw new AuthenticationException();

            Console.WriteLine("Incorrect Password: Try Again.");
        }
    }
}
