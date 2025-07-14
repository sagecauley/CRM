using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Firebase
{
    public static class FirestoreConverter
    {
        public static object ToFirestoreFormat<T>(T obj)
        {
            var fields = new Dictionary<string, object>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;
                if (prop.Name == "PreviousJobs")
                    continue;

                string propName = prop.Name;

                object wrappedValue = value switch
                {
                    string s => new { stringValue = s },
                    int i => new { integerValue = i.ToString() }, // Firestore expects string integers
                    long l => new { integerValue = l.ToString() },
                    bool b => new { booleanValue = b },
                    Enum e => new { stringValue = e.ToString() },
                    _ => new { stringValue = value.ToString() } // Fallback
                };

                fields[propName] = wrappedValue;
            }

            return new { fields };
        }

        public static Customer FromFirestoreFormat(JsonElement fields)
        {
            // Example getting a string value safely
            string GetString(string key) =>
                fields.TryGetProperty(key, out var prop) && prop.TryGetProperty("stringValue", out var val)
                    ? val.GetString()
                    : null;

            var name = GetString("Name");
            var email = GetString("Email");
            var phone = GetString("Phone");
            var address = GetString("Address");
            var notes = GetString("Notes");
            var id = GetString("Id");

            // For enum, parse string to enum
            var categoryString = GetString("Category");
            CustomerCategory category = CustomerCategory.NonProfit; // default
            Enum.TryParse(categoryString, out category);

            var contactString = GetString("PreferredContactMethod");
            ContactMethod preferredContactMethod = ContactMethod.Other; // default
            Enum.TryParse(contactString, out preferredContactMethod);

            // Construct Customer (use empty string or generate id if needed)
            Customer c = new Customer(name, email, phone, address, notes, category, preferredContactMethod);
            c.Id = id; // Set the ID after construction
            return c;
        }

        public static object ToFirestoreFormat(Job job)
        {
            var fields = new Dictionary<string, object>
    {
        { "Name", new Dictionary<string, object> { { "stringValue", job.Name } } },
        { "Description", new Dictionary<string, object> { { "stringValue", job.Description } } },
        { "StartDate", new Dictionary<string, object> { { "timestampValue", job.StartDate.ToUniversalTime().ToString("o") } } },
        { "Cost", new Dictionary<string, object> { { "stringValue", job.Cost } } }, // use doubleValue for decimals
        { "Status", new Dictionary<string, object> { { "stringValue", job.Status.ToString() } } },
        
    };
            if (!string.IsNullOrEmpty(job.Id))
            {
                fields.Add("Id", new Dictionary<string, object> { { "stringValue", job.Id } });
            }
            if (!string.IsNullOrEmpty(job.CustomerId))
            {
                fields.Add("CustomerId", new Dictionary<string, object> { { "stringValue", job.CustomerId } });
            }

            return new { fields };
        }

        public static Job FromFirestoreFormatJob(JsonElement fields)
        {
            var name = fields.GetProperty("Name").GetProperty("stringValue").GetString();
            var description = fields.GetProperty("Description").GetProperty("stringValue").GetString();
            var startDate = DateTime.Parse(fields.GetProperty("StartDate").GetProperty("timestampValue").GetString());
            var cost = fields.GetProperty("Cost").GetProperty("stringValue").GetString();
            var statusStr = fields.GetProperty("Status").GetProperty("stringValue").GetString();
            var status = Enum.TryParse<JobStatus>(statusStr, out var parsedStatus) ? parsedStatus : JobStatus.Pending;

            string customerId = null;
            if (fields.TryGetProperty("CustomerId", out var customerIdElement))
            {
                customerId = customerIdElement.GetProperty("stringValue").GetString();
            }
            Job j = new Job(name, description, startDate, cost, status, customerId);
            return j;
        }

    }
}
