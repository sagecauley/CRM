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

            // For enum, parse string to enum
            var categoryString = GetString("Category");
            CustomerCategory category = CustomerCategory.NonProfit; // default
            Enum.TryParse(categoryString, out category);

            var contactString = GetString("PreferredContactMethod");
            ContactMethod preferredContactMethod = ContactMethod.Other; // default
            Enum.TryParse(contactString, out preferredContactMethod);

            // Construct Customer (use empty string or generate id if needed)
            return new Customer(name, email, phone, address, notes, category, preferredContactMethod);
        }

        public static Dictionary<string, object> ToFirestoreFormat(Job job)
        {
            var fields = new Dictionary<string, object>
    {
        { "Description", new { stringValue = job.Description } },
        { "StartDate", new { timestampValue = job.StartDate.ToString("o") } },
        { "EndDate", new { timestampValue = job.EndDate.ToString("o") } },
        { "Cost", new { doubleValue = (double)job.Cost } },
        { "Status", new { stringValue = job.Status.ToString() } }
    };

            if (job.Customer != null)
            {
                fields.Add("Customer", new
                {
                    mapValue = new
                    {
                        fields = ToFirestoreFormat(job.Customer)
                    }
                });
            }

            return fields;
        }

        public static Job FromFirestoreFormatJob(JsonElement fields)
        {
            var description = fields.GetProperty("Description").GetProperty("stringValue").GetString();
            var startDate = DateTime.Parse(fields.GetProperty("StartDate").GetProperty("timestampValue").GetString());
            var endDate = DateTime.Parse(fields.GetProperty("EndDate").GetProperty("timestampValue").GetString());
            var cost = (decimal)fields.GetProperty("Cost").GetProperty("doubleValue").GetDouble();
            var statusStr = fields.GetProperty("Status").GetProperty("stringValue").GetString();
            var status = Enum.TryParse<JobStatus>(statusStr, out var parsedStatus) ? parsedStatus : JobStatus.Pending;

            Customer customer = null;
            if (fields.TryGetProperty("Customer", out var customerElement))
            {
                var customerFields = customerElement.GetProperty("mapValue").GetProperty("fields");
                customer = FromFirestoreFormat(customerFields);
            }

            return new Job("", description, startDate, endDate, cost, status, customer);
        }

    }
}
