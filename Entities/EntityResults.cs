using System;
using System.Collections.Generic;

namespace StarWars.Entities
{
    public class EntityResults<T> : Entity where T : Entity
    {
        public string? previous { get; set; }
        public string? next { get; set; }
        public string? previous_page_no { get; set; }
        public string? next_page_no { get; set; }
        public long count { get; set; }
        public List<T>? results { get; set; }
    }
}
