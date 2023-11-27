using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Canvas_Texting
{
    public class ToDoItem
    {
        public string? context_name { get; set; }
        public Assignment? assignment { get; set; }
    }

    public class Assignment
    {
        public int id { get; set; }
        public string? name { get; set; }
        public AssignDate[]? all_dates { get; set; }
    }

    public class AssignDate
    {
        public int id { get; set; }
        [JsonProperty("base")]
        public bool Base { get; set; }
        public string? title { get; set; }
        public string? due_at { get; set; }
        public string? unlock_at { get; set; }
        public string? lock_at { get; set; }
    }
}
