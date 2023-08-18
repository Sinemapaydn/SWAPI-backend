using System.Collections.Generic;

namespace StarWars.Entities
{
    public class Film : Entity
    {
        public short episode_id { get; set; }
        public string? url { get; set; }
        public string? title { get; set; }
        public string? producer { get; set; }
        public string? director { get; set; }
        public List<string>? characters { get; set; }
    }
}
