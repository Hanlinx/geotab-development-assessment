using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class ConsolePrinter
    {
        /// <summary>
        /// print msg on console
        /// </summary>
        /// <param name="msg"></param> string message
        /// <returns></returns>
        public void PrintMsg(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// print program start msg 
        /// </summary>
        /// <returns></returns>
        public void PrintStartScreen()
        {
            Console.WriteLine("========================================================");
            Console.WriteLine("".PadLeft(10) + "Welcome to Chuck Norris Joke Generator");
            Console.WriteLine("".PadLeft(15) + "Press '?' to get instructions.");
            Console.WriteLine("".PadLeft(15) + "Press 'c' to get categories");
            Console.WriteLine("".PadLeft(15) + "Press 'r' to get random jokes");
            Console.WriteLine("".PadLeft(15) + "Press 'e' to exit the program");
            Console.WriteLine("========================================================");
        }

        /// <summary>
        /// print jokes on console
        /// </summary>
        /// <param name="jokeList"></param> list of jokes to print
        /// <returns></returns>
        public void PrintResults(List<string> jokeList, int num)
        {
            int index = 1;
            if (jokeList != null && jokeList.Count > 0)
            {
                if(num != jokeList.Count)
                {
                    Console.WriteLine("\nI didn't find all jokes that you requested, here are jokes I found: ");
                }
                else
                {
                    Console.WriteLine("\nI find all jokes that you requested, here are jokes I found: ");
                }
                
                foreach (var joke in jokeList)
                {
                    Console.WriteLine(index + ":" + "[" + string.Join(",", joke) + "]");
                    Console.WriteLine("\n");
                    index++;
                }
            }
        }
    }
}
