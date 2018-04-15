using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilaSharp.Types
{
    public class User
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        //[JsonProperty("profile")]
        //public Profile Profile { get; set; }

        [JsonProperty("perfs")]
        public Perfs Perfs { get; set; }
    }
}
