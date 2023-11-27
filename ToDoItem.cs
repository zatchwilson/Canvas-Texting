using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Canvas_Texting
{
    /// <summary>
    /// Generate a ToDoItem, which pulls uses only the necessary information from the Canvas ToDo Request
    /// </summary>
    public class ToDoItem
    {
        public string? context_name { get; set; }
        public Assignment? assignment { get; set; }
    }

    /// <summary>
    /// General Assignment class, used to get class id, name, and the due date
    /// </summary>
    public class Assignment
    {
        public int id { get; set; }
        public string? name { get; set; }
        public AssignDate[]? all_dates { get; set; }
    }

    /// <summary>
    /// Date class, used to get specific date information for an assignment
    /// </summary>
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
