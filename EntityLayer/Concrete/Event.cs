using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? EventPhoto { get; set; }
        public int Capacity { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsAvailable { get; set; }
    }
}
