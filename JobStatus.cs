using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public enum JobStatus
    {
        Pending,        // Job has been created but not yet started
        InProgress,     // Job is currently being worked on
        Completed,      // Job has been finished
        Cancelled,      // Job was cancelled before completion
        OnHold          // Job is temporarily paused
    }
}
