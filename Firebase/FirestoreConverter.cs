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
    }
}
