using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public class Job
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public JobStatus Status { get; set; } // e.g., "Pending", "In Progress", "Completed"
        public Customer? Customer { get; set; } // Reference to the customer associated with this job
        public Job(string id, string description, DateTime startDate, DateTime endDate, decimal cost, JobStatus status, Customer customer)
        {
            Id = id;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Cost = cost;
            Status = status;
            Customer = customer;
        }
    }
}
