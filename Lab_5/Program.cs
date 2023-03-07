using System.Runtime.InteropServices;

namespace ForTest
{
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
                
                "195.162.83.28",
                "1.2.3.4",
                "255.256.267.300",
                "127.0.0.1",
            };

            Palindrome(text);


            foreach (string ip in ip_address)
            {
                Console.WriteLine(ip + " : " + validate_ip(ip));
            }

            var gen = PrimeNumGenerator();

            using (var enumerator = gen.GetEnumerator())
            {
                for (int i = 0; i < 20; i++)
                {
                    enumerator.MoveNext();
                    Console.WriteLine(enumerator.Current);
                }
            }
            getOS();
        }

        public static List<string> Palindrome(List<string> palList)
        {
            List<string> text = new List<string>
            {
                "racecar",
                "hello",
                "level",
                "world" ,
                "anna"
            };
            foreach (string word in palList)
            {
                string reversed = new string(word.Reverse().ToArray());
                if (word == reversed)
                {
                    Console.WriteLine(word + " is a palindrome");
                }
            }
            Console.WriteLine();

            return text;

        }

        public static IEnumerable<int> PrimeNumGenerator()
        {
            yield return 2;
            var primes = new List<int> { 2 };
            int num = 2;
            while (true)
            {
                num += 1;
                bool isPrime = true;
                foreach (int p in primes)
                {
                    if (num % p == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(num);
                    yield return num;
                }
            }

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
                Console.Write("Windows");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.Write("Linux");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.Write("MacOS");
            }
            
        }
    }

}