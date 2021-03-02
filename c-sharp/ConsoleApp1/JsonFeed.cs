using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokeGenerator;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class JsonFeed
    {
        private const string JokesCategories = "/jokes/categories";
        private const string JokesRandom = "jokes/random";

        /// <summary>
        /// retrieve a list of jokes from https://api.chucknorris.io
        /// </summary>
        /// <param name="urlJoke"></param> string url address 
        /// <param name="category"></param> selected joke category , if category is undefined will retrieve random jokes from any categories  
        /// <param name="number"></param> number of joke to retrieve
        /// <returns> a list of random jokes </returns>
        public static List<string> GetRandomJokes(string urlJoke, string category, int number)
        {
            List<string> result = new List<string>();
            HashSet<string> jokeHashSet = new HashSet<string>();

            try
            {
                using (HttpClient client = new HttpClient { BaseAddress = new Uri(urlJoke) })
                {
                    StringBuilder url = new StringBuilder(JokesRandom);
                    if (category != null)
                    {
                        url.Append(url.ToString().Contains('?') ? "&" : "?");
                        url.Append("category=");
                        url.Append(category);
                    }

                    int attempt = 0;

                    // note: category: "career" only has 3 unique jokes but user can request up to 9 unique jokes, 
                    // I add a attempt<30 here to prevent infinity loop; it will break the loop after 30 temps.   
                    while (number > 0 && attempt < 30)  
                    {
                        string joke = Task.FromResult(client.GetStringAsync(url.ToString()).Result).Result;
                        var jokeObj = JsonConvert.DeserializeObject<Jokes>(joke);

                        if (jokeObj != null && jokeObj.JokesValue != null)
                        {
                            attempt++;
                            // check duplicate jokes, only unique jokes will add to result
                            if (jokeHashSet.Contains(jokeObj.JokesValue)) continue;
                            jokeHashSet.Add(jokeObj.JokesValue);
                            result.Add(jokeObj.JokesValue);
                            number--;
                        }                      
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error Message: {0}", e.Message);
                Environment.Exit(-1);
            }

            return result;
        }

        /// <summary>
        /// retrieve a random name from https://www.names.privserv.com/api/
        /// </summary>
        /// <param name="url"></param> string url address 
        /// <returns> a random name obj</returns>
		public static Name GetNames(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient { BaseAddress = new Uri(url) })
                {
                    var result = client.GetStringAsync("").Result;
                    return JsonConvert.DeserializeObject<Name>(result);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error Message: {0}", e.Message);
                Environment.Exit(-1);
            }
            return new Name();
        }

        /// <summary>
        /// retrieve a string array contains joke categories
        /// </summary>
        /// <param name="url"></param> string url address 
        /// <returns>string array of joke categories</returns>
        public static string[] GetCategories(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient { BaseAddress = new Uri(url) })
                {
                    return new string[] { Task.FromResult(client.GetStringAsync(JokesCategories).Result).Result };
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error Message: {0}", e.Message);
                Environment.Exit(-1);
            }
            return new string[] { " " };
        }
    }
}