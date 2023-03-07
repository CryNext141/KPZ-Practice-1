using System.Runtime.InteropServices;
using Task = ForTest.Task;

[TestFixture]
public class TaskTests
{
    public class PalindromeTests
    {
        [Test]
        public void PrimeNumGenerator_Test()
        {
            // Arrange
            var expectedPrimes = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };
            int expected101stPrime = 547;

            // Act
            var primeNums = new List<int>();
            using (var enumerator = Task.PrimeNumGenerator().GetEnumerator())
            {
                for (int i = 0; i < 101; i++)
                {
                    enumerator.MoveNext();
                    primeNums.Add(enumerator.Current);
                }
            }

            // Assert
            Assert.AreEqual(expectedPrimes, primeNums.Take(12).ToList());
            Assert.AreEqual(expected101stPrime, primeNums[100]);
        }

        [Test]
        public void Palindrome_InputIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            List<string> emptyList = new List<string>();

            // Act
            List<string> result = Task.Palindrome(emptyList);

            // Assert
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void Palindrome_InputHasNonStringElements_ReturnsEmptyList()
        {
            // Arrange
            List<string> nonStringList = new List<string> { "hello", "world", "1", "true" };

            // Act
            List<string> result = Task.Palindrome(nonStringList);

            // Assert
            Assert.That(result, Is.Empty);
        }


        [Test]
        public void Palindrome_InputHasNoPalindromeWords_ReturnsOriginalList()
        {
            // Arrange
            List<string> inputList = new List<string> { "hello", "world" };

            // Act
            List<string> result = Task.Palindrome(inputList);

            // Assert
            Assert.That(result, Is.EquivalentTo(inputList));
        }

        [Test]
        public void Palindrome_InputHasOddAndEvenPalindromeWords_ReturnsOriginalListPlusPalindromes()
        {
            // Arrange
            List<string> inputList = new List<string> { "racecar", "hello", "level", "world", "anna" };
            List<string> expectedOutput = new List<string> { "racecar", "hello", "level", "world", "anna" };

            // Act
            List<string> result = Task.Palindrome(inputList);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedOutput));
        }
    }

    [Test]
    public void TestEmptyString()
    {
        bool result = Task.validate_ip("");
        Assert.IsFalse(result);
    }

    [Test]
    public void TestNullString()
    {
        bool result = Task.validate_ip(null);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestInvalidFormat()
    {
        bool result = Task.validate_ip("192.168.1");
        Assert.IsFalse(result);
    }

    [Test]
    public void TestInvalidValue()
    {
        bool result = Task.validate_ip("256.0.0.1");
        Assert.IsFalse(result);
    }

    [Test]
    public void TestValidIP()
    {
        bool result = Task.validate_ip("192.168.0.1");
        Assert.IsTrue(result);
    }

    [Test]
    public void TestGetOS()
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            Task.getOS();

            string expected = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expected = "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                expected = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                expected = "MacOS";
            }
            Assert.AreEqual(expected, sw.ToString());
        }
    }
}
