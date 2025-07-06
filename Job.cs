using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public class Job
    {
        public string Id { get; set; } = null;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string Cost { get; set; } = "0";
        public JobStatus Status { get; set; } // e.g., "Pending", "In Progress", "Completed"
        public string CustomerId{get; set; } // Reference to the customer associated with this job
        public Job(string name, string description, DateTime startDate, string cost, JobStatus status, string customer)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            Cost = cost;
            Status = status;
            CustomerId = customer;
        }
    }
}
