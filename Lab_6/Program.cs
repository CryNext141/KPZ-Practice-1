using System;
using System.IO;
using System.Net;

namespace WordsCounter
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path=@"C:\Users\thelo\Source\Repos\KPZ-Practice-1\source_data\article.txt";
            get_text_info(path);

            download_csv("https://support.staffbase.com/hc/en-us/article_attachments/360009197031/username.csv");
        }

        public static void get_text_info(string file_path)
        {
            string text = File.ReadAllText(file_path).ToLower();

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            foreach (string word in text.Split())
            {
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word]++;
                }
                else
                {
                    wordCounts[word] = 1;
                }
            }

            foreach (KeyValuePair<string, int> pair in wordCounts)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
        }

        public static void download_csv(string url)
        {
        WebClient client = new WebClient();
        string directory = @"C:\Users\thelo\Source\Repos\KPZ-Practice-1\source_data\";
        string file_path = Path.Combine(directory, "username.csv");

        client.DownloadFile(url, file_path);

        string[] lines = File.ReadAllLines(file_path);
        Array.Resize(ref lines, lines.Length - 2);
        File.WriteAllLines(file_path, lines);
        Console.WriteLine("Completed!");
        }
    }
}