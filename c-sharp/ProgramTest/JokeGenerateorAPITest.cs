using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokeGenerator;

namespace JokeGeneratorTest
{
    [TestClass]
    public class JokeGeneratorTest
    {
        private const string NameApiBase = "https://www.names.privserv.com/api/";
        private const string ChuckNorrisApiBase = "https://api.chucknorris.io";
        private const string JokesCategories = "/jokes/categories";
        private const string JokesRandom = "jokes/random";

        [TestMethod]
        public async Task TestAPI_GetRandomNames()
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(NameApiBase) })
            {
                var response = await client.GetAsync("");
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var result = JsonConvert.DeserializeObject<Name>(client.GetStringAsync("").Result);
                var name = result.name;
                var surname = result.surname;

                Assert.IsTrue(!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(surname));
            }
        }

        [TestMethod]
        public async Task TestAPI_GetJokeCategories()
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(ChuckNorrisApiBase) })
            {
                var response = await client.GetAsync(JokesCategories);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var result = Task.FromResult(client.GetStringAsync(JokesCategories).Result).Result;

                Assert.IsTrue(!String.IsNullOrEmpty(result));
            }
        }

        [TestMethod]
        public async Task TestAPI_GetRandomJokesWithoutCategories()
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(ChuckNorrisApiBase) })
            {
                var response = await client.GetAsync(JokesRandom);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var result = Task.FromResult(client.GetStringAsync(JokesRandom).Result).Result;

                Assert.IsTrue(!String.IsNullOrEmpty(result));

                Jokes value = JsonConvert.DeserializeObject<Jokes>(result);

                Assert.IsTrue(!String.IsNullOrEmpty(value.JokesValue));
                Assert.IsTrue(value.Category.Length == 0);
            }
        }

        [TestMethod]
        public async Task TestAPI_GetRandomJokesWithCategories()
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(ChuckNorrisApiBase) })
            {
                StringBuilder url = new StringBuilder(JokesRandom);
                url.Append(url.ToString().Contains('?') ? "&" : "?");
                url.Append("category=");
                url.Append("animal");

                var response = await client.GetAsync(url.ToString());
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

                var result = Task.FromResult(client.GetStringAsync(url.ToString()).Result).Result;
                Jokes value = JsonConvert.DeserializeObject<Jokes>(result);

                Assert.IsTrue(!String.IsNullOrEmpty(value.JokesValue));
                Assert.AreEqual("animal", value.Category[0]);
            }
        }
    }
}
