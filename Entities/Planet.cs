using System;
using System.Collections.Generic;

namespace StarWars.Entities
{
    public class Planet : Entity
    {
        public List<string>? residents { get; set; }
        public string? rotation_period { get; set; }
        public string? url { get; set; }
        public string? gravity { get; set; }
        public string? terrain { get; set; }
        public string? climate { get; set; }
        public string? name { get; set; }
        public string? surface_water { get; set; }
        public string? population { get; set; }
        public string? orbital_period { get; set; }
    }
}
