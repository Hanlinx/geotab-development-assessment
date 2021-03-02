using Newtonsoft.Json;

namespace JokeGenerator
{
    public class Jokes
    {
        [JsonProperty("categories")]
        public string[] Category { get; set; }

        [JsonProperty("value")]
        public string JokesValue { get; set; }
    }
}