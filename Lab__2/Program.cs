using System;
using System.Collections;
using System.Collections.Generic;

public class Task
{
    //Класне завдання...Справді,якщо на пітоні воно займе максимум 15 строк,то на шарпі я натанцював з бубном майже 50
    public static IEnumerable<int> PrimeNumGenerator()
    {
        yield return 2;
        var primes = new List<int> { 2 };
        int num = 3;
        while (true)
        {
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
            num += 2;
        }
    }

    public static void Main()
    {
        var gen = PrimeNumGenerator();

        using (var enumerator = gen.GetEnumerator())
        {
            for (int i = 0; i < 20; i++)
            {
                enumerator.MoveNext();
                Console.WriteLine(enumerator.Current);
            }
        }
    }
}
