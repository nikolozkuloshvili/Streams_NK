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
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int Choice) || Choice < 1 || Choice > 2)
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
            try
            {
                Console.Write("Enter the file path: ");
                string readPath = Console.ReadLine() ?? "";
                if (File.Exists(readPath))
                    return readPath;
                else
                    Console.WriteLine("Wrong path: Try Again");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"ReadPath Error Message: {ex.Message}");
            }
        }
    }

    public static void ValidatePassword(string Password)
    {
        while (true)
        {
            Console.Write("Enter the password: ");
            string inputPassword = Console.ReadLine() ?? "";
            if (inputPassword == Password)
                break;
            else
                Console.WriteLine("Incorrect Password: Try Again.");
        }
    }
}
