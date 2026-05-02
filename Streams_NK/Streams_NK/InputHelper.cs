namespace Encryption;

public static class InputHelper
{
    public static void GetUserChoice()
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

            Program.Choice = Choice;
            break;
        }
    }

    public static void GetReadPath()
    {
        while (true)
        {
            try
            {
                Console.Write("Enter the file path: ");
                Program.ReadPath = Console.ReadLine() ?? "";
                using FileStream stream = new FileStream(Program.ReadPath, FileMode.Open);
                break;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error Message:\t{ex.Message}");
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

    public static void GetWritePath()
    {
        while (true)
        {
            try
            {
                Console.Write("Enter the write path: ");
                Program.WritePath = Console.ReadLine() ?? "";
                break;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error Message:\t{ex.Message}");
            }
        }
    }
}
