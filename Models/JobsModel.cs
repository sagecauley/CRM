using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class JobsModel
    {
        public JobsModel() { }

        private List<Job> _jobs;

        public IReadOnlyList<Job> Jobs => _jobs.AsReadOnly();

        public bool AddJob(Job job)
        {
            if (job != null && !_jobs.Any(j => j.Id == job.Id))
            {
                _jobs.Add(job);
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
                if (_jobs.Contains(job))
                {
                    _jobs.Remove(job);
                    countRemoved++;
                }
            }
            if (countRemoved == totalCount)
            {
                return true;
            }
            return false;
        }
    }
}
