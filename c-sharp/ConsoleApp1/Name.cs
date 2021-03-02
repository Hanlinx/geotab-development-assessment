using Newtonsoft.Json;

namespace JokeGenerator
{
    public class Name
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("surname")]
        public string surname { get; set; }
    }
}
