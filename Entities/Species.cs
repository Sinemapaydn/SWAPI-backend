using System;
using System.Collections.Generic;

namespace StarWars.Entities
{
    public class Species : Entity
    {
        public string? homeworld { get; set; }
        public string? eye_colors { get; set; }
        public string? skin_colors { get; set; }
        public string? url { get; set; }
        public string? classification { get; set; }
        public string? name { get; set; }
        public string? designation { get; set; }
        public string? average_height { get; set; }
        public string? average_lifespan { get; set; }
        public string? hair_colors { get; set; }
    }
}
