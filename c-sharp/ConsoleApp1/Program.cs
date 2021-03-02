using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        private const string ChuckNorrisApiBase = "https://api.chucknorris.io";
        private const string NameApiBase = "https://www.names.privserv.com/api/";
        private static string[] _categoriesList;
        private static HashSet<string> categoriesHashSet;
        private static List<string> _jokeList;

        static char key;
        static Tuple<string, string> names;
        static ConsolePrinter printer;

        static void Main()
        {
            StartProgram();
        }

        /// <summary>
        /// Start joke generater program
        /// </summary>    
        /// <returns></returns>
        private static void StartProgram()
        {
            printer = new ConsolePrinter();
            printer.PrintStartScreen();
            while (true)
            {
                GetEnteredKey(Console.ReadKey());
                if (key == '?')   // display instructions
                {
                    printer.PrintMsg("\n Welcome to Chuck Norris joke generater program, You can press 'c' to get categories, Press r to get random jokes or Press e to exit ");

                }
                else if (key == 'c')  // display categories          
                {
                    _categoriesList = GetCategories();
                    categoriesHashSet = FormatCategories(_categoriesList);
                    printer.PrintMsg("\nJoke categories are:");
                    printer.PrintMsg(string.Join(", ", categoriesHashSet));
                }
                else if (key == 'r')   // get random jokes
                {
                    printer.PrintMsg("\nWant to use a random name? y/n");
                    GetEnteredKey(Console.ReadKey());

                    while (key != 'y' && key != 'n')
                    {
                        printer.PrintMsg("\nInvalid input , please enter y or n");
                        GetEnteredKey(Console.ReadKey());
                    }

                    if (key == 'y')     // use random name
                    {
                        GetNames();
                    }
                    else if (key == 'n')
                    {
                        names = null;
                    }

                    printer.PrintMsg("\nWant to specify a category? y/n");
                    GetEnteredKey(Console.ReadKey());

                    while (key != 'y' && key != 'n')
                    {
                        printer.PrintMsg("\nInvalid input , please enter y or n");
                        GetEnteredKey(Console.ReadKey());
                    }

                    if (key == 'y')    // jokes with specify category
                    {
                        printer.PrintMsg("\nHow many jokes do you want? Please enter a number between 1 to 9 and Press ENTER");

                        int num = ValidateInputNumberOfJokes(Console.ReadLine());
                        while (num == -1)
                        {
                            printer.PrintMsg("\nInvalid input , please enter number between 1 to 9 and Press ENTER");
                            num = ValidateInputNumberOfJokes(Console.ReadLine());
                        }

                        if (num != -1)
                        {
                            _categoriesList = GetCategories();
                            categoriesHashSet = FormatCategories(_categoriesList);
                            printer.PrintMsg("\nPlease enter a category from below list and Press ENTER;");
                            printer.PrintMsg(string.Join(", ", categoriesHashSet));

                            string inputCategory = Console.ReadLine();
                            while (!categoriesHashSet.Contains('"' + inputCategory + '"'))    // check user enter category is valid 
                            {
                                printer.PrintMsg("\nInvalid input , please enter specify a category from below list and Press ENTER");
                                printer.PrintMsg(string.Join(", ", categoriesHashSet));
                                inputCategory = Console.ReadLine();
                            }

                            _jokeList = GetRandomJokes(inputCategory, num);
                            printer.PrintResults(_jokeList , num);
                        }

                    }
                    else if (key == 'n')   // jokes without specify category
                    {
                        printer.PrintMsg("\nHow many jokes do you want? Please enter a number between 1 to 9 and Press ENTER");
                        int num = ValidateInputNumberOfJokes(Console.ReadLine());
                        while (num == -1)
                        {
                            printer.PrintMsg("\nInvalid input , please enter number between 1 to 9");
                            num = ValidateInputNumberOfJokes(Console.ReadLine());
                        }

                        if (num != -1)
                        {
                            _jokeList = GetRandomJokes(null, num);
                            printer.PrintResults(_jokeList , num);
                        }
                    }
                }
                else if (key == 'e')  // exit program
                {
                    printer.PrintMsg("\nAre you sure to exit the program? Press 'y' to exit , Press other key to continue");
                    GetEnteredKey(Console.ReadKey());
                    if (key == 'y')
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        printer.PrintMsg("\nContinue Game...");
                    }

                }
                else
                {
                    printer.PrintMsg("\nInvalid input, please Enter a valid input or Press ? to get instructions. ");
                }
            }
        }

        /// <summary>
        /// split the category list into a hashset
        /// </summary>
        /// <param name="categories"></param>string array categories
        /// <returns>a hashset contains unique categories</returns>
        public static HashSet<string> FormatCategories(string[] categories)
        {
            categories = categories[0].Split('[');
            categories = categories[1].Split(']');
            return categories[0].Split(',').ToHashSet();
        }

        /// <summary>
        /// validate the number of jokes enter, only allow single digit from 1 to 9 
        /// </summary>
        /// <param name="input"></param> is string of user input
        /// <returns>integer from 1 to 9 or -1</returns>
        public static int ValidateInputNumberOfJokes(string num)
        {
            if (num.Length == 1 && num.All(char.IsDigit) && !num.Equals("0"))
            {
                return Int32.Parse(num);
            }
            return -1;
        }

        /// <summary>
        /// map keystroke to standard character
        /// </summary>
        /// <param name="consoleKeyInfo"></param>
        /// <returns></returns>
        private static void GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.C:
                    key = 'c';
                    break;
                case ConsoleKey.R:
                    key = 'r';
                    break;
                case ConsoleKey.Y:
                    key = 'y';
                    break;
                case ConsoleKey.N:
                    key = 'n';
                    break;
                case ConsoleKey.E:
                    key = 'e';
                    break;
                case ConsoleKey.Oem2:
                    key = '?';
                    break;
                default:
                    key = '#';
                    break;
            }
        }

        /// <summary>
        /// retrieve a list of jokes with random name or use chuck norris  
        /// </summary>
        /// <param name="category"></param> category of jokes
        /// <param name="number"></param> number of jokes
        /// <returns>a list of random jokes</returns>
        private static List<string> GetRandomJokes(string category, int number)
        {
            var jokeList = JsonFeed.GetRandomJokes(ChuckNorrisApiBase, category, number);

            // change name chuck norris with a random name 
            if (names != null && names.Item1 != null && names.Item2 != null)
            {
                jokeList = jokeList.Select(x => x.Replace("Chuck", names.Item1)).ToList();
                jokeList = jokeList.Select(x => x.Replace("Norris", names.Item2)).ToList();
            }
            return jokeList;
        }

        /// <summary>
        /// retrieve a string array contains joke categories
        /// </summary>
        /// <param></param>
        /// <returns>retrieve a string array contains joke categories</returns>
        private static string[] GetCategories()
        {
            return JsonFeed.GetCategories(ChuckNorrisApiBase);
        }

        /// <summary>
        /// retrieve name with firstname and lastname
        /// </summary>
        /// <param></param>
        /// <returns>a Name obj with firstname and lastname</returns>
        private static void GetNames()
        {
            var result = JsonFeed.GetNames(NameApiBase);
            if (result != null && result.name != null && result.surname != null)
            {
                names = Tuple.Create(result.name.ToString(), result.surname.ToString());
            }
        }
    }
}