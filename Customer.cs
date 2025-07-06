using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public class Customer
    {
        public Customer(string name, string email, string phone, string address, string notes, CustomerCategory category, ContactMethod preferredContactMethod)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            Notes = notes;
            Category = category;
            PreferredContactMethod = preferredContactMethod;
            _previousJobs = new List<Job>();
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public CustomerCategory Category { get; set; }
        public ContactMethod PreferredContactMethod { get; set; }

        public string Id { get; set; }

        private List<Job> _previousJobs;
        public IReadOnlyList<Job> PreviousJobs => _previousJobs.AsReadOnly();

        public bool AddJob(Job job)
        {
            if (job != null)
            {
                _previousJobs.Add(job);
                return true;
            }
            return false;
        }
        public bool RemoveJobs(List<Job> jobs)
        {
            int totalCount = jobs.Count;
            int countRemoved = 0;
            foreach (Job job in jobs)
            {
                if (PreviousJobs.Contains(job))
                {
                    countRemoved++;
                    _previousJobs.Remove(job);
                }

            }
            if (countRemoved == totalCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
