using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WCount02
{
    /// <summary>
    /// Tested on a 10.3 MB file
    /// I have left out the validation checks on the constructors.
    /// If this was an api the classes would implement interfaces and would be injected.
    /// </summary>
    public class FileLoader
    {
        string text = null;
        string textFileName = null;
        public FileLoader(string filename)
        {
            textFileName = filename;
        }
        public string GetFileData()
        {
            try
            {
                text = File.ReadAllText(textFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return text;
        }
    }

    public class ProcessWords
    {
        private string _regexPattern;
        private string _data;
        public ProcessWords(string data, string regexPattern)
        {
            _data = data;
            _regexPattern = regexPattern;
        }
        public string[] ProduceWordList()
        {
            Regex reg_exp = new Regex(_regexPattern);

            var wordString = reg_exp.Replace(_data, " ").ToLower();
            var words = wordString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
    }


    public enum SortOrder
    {
        Assending,
        Descending
    }

    public class Analyser
    {
        private string[] _words; 
        public Analyser(string[] words)
        {
            _words = words;
        }

        public void Display(int range, SortOrder order)
        {
            var groups = _words.GroupBy(w => w);
            if (order == SortOrder.Assending)
            {
                groups = groups.OrderByDescending(g => g.Count()).Take(range);
                groups = groups.OrderBy(g => g.Count());
            }
            else
            {
                groups = groups.OrderByDescending(g => g.Count()).Take(20);
            }

            foreach (var item in groups)
            {
                Console.WriteLine($"{item.Count()} {item.Key}");
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter file path");
                return;
            }
            else
            {

                var loader = new FileLoader(args[0]);
                var data = loader.GetFileData();

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var process = new ProcessWords(data, "[^a-zA-Z0-9]");
                    var words = process.ProduceWordList();

                    if(words.Length > 0)
                    {
                        var anal = new Analyser(words);
                        anal.Display(20, SortOrder.Descending);
                    }
                    
                }
            }
        }
    
    }
}
