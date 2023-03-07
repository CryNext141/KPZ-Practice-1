using System;
using System.Runtime.InteropServices;

public class Task
{
    public static void Main(string[] args)
    {
        List<string> text = new List<string>
        { 
            "racecar", 
            "hello", 
            "level", 
            "world" ,
            "anna"
        };

        List<string> ip_address = new List<string>
        {
            "2",
            "195.162.83.28",
            "1.2.3.4",
            "255.256.267.300",
            "127.0.0.1",
        };

        Palindrom(text);

        foreach (var ip in ip_address)
        {
            Console.WriteLine($"{ip} : {validate_ip(ip)}");
        }
        Console.WriteLine();

        getOS();
    }

    public static void Palindrom(List<string> palList)
    {
        foreach (string word in palList)
        {
            string reversed = new string(word.Reverse().ToArray());
            if (word == reversed)
            {
                Console.WriteLine(word + " is a palindrome");
            }
        }
        Console.WriteLine();
    }

    public static bool validate_ip(string ipString)
    {
        if (String.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }

    public static void getOS()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
        {
            Console.WriteLine("Windows");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
        {
            Console.WriteLine("Linux");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) 
        {
            Console.WriteLine("MacOS");
        }
        Console.WriteLine();
    }
}