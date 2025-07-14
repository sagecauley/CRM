using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace CRM.Firebase
{
    public class FirebaseFirestore
    {
        private const string ProjectId = "crm-maui-9671c";
        private readonly FirebaseAuthService _authService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public FirebaseFirestore(FirebaseAuthService authService)
        {
            _authService = authService;
        }

        private async Task<string> GetAuthHeaderAsync()
        {
            var idToken = await _authService.GetValidIdTokenAsync();
            return idToken;
        }

        public async Task<string> AddCustomerAsync(Customer customer)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return null;

            var firestoreBody = FirestoreConverter.ToFirestoreFormat(customer);
            var json = JsonSerializer.Serialize(firestoreBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/customers";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var resultJson = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(resultJson);
            var fullName = doc.RootElement.GetProperty("name").GetString();
            var documentId = fullName?.Split('/').Last();


            return documentId;
        }

        public async Task<bool> EditCustomerAsync(string docId, Customer customer)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return false;

            var firestoreBody = FirestoreConverter.ToFirestoreFormat(customer);
            var json = JsonSerializer.Serialize(firestoreBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/customers/{docId}";

            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(string docId)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return false;

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/customers/{docId}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<Dictionary<string, Customer>> GetAllCustomersAsync()
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return null;

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/customers";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var customers = new Dictionary<string, Customer>();

            if (!doc.RootElement.TryGetProperty("documents", out var documents))
                return customers; // No documents found

            foreach (var document in documents.EnumerateArray())
            {
                // The document name is like: projects/{projectId}/databases/(default)/documents/customers/{docId}
                var fullName = document.GetProperty("name").GetString();
                var docId = fullName?.Split('/').Last();

                if (docId == null)
                    continue;

                var fields = document.GetProperty("fields");

                // Parse fields back into a Customer object (write a helper method for this)
                var customer = FirestoreConverter.FromFirestoreFormat(fields);

                if (customer != null)
                    customer.Id = docId;
                    customers.Add(docId, customer);
            }

            return customers;
        }

        public async Task<string> AddJobAsync(Job job)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return null;

            var firestoreBody = FirestoreConverter.ToFirestoreFormat(job);
            var json = JsonSerializer.Serialize(firestoreBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/jobs";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var resultJson = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(resultJson);
            var fullName = doc.RootElement.GetProperty("name").GetString();
            var documentId = fullName?.Split('/').Last();

            return documentId;
        }

        public async Task<bool> EditJobAsync(string docId, Job job)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return false;

            var firestoreBody = FirestoreConverter.ToFirestoreFormat(job);
            var json = JsonSerializer.Serialize(firestoreBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/jobs/{docId}";

            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteJobAsync(string docId)
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return false;

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/jobs/{docId}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<Dictionary<string, Job>> GetAllJobsAsync()
        {
            var idToken = await GetAuthHeaderAsync();
            if (string.IsNullOrEmpty(idToken))
                return null;

            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/jobs";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var jobs = new Dictionary<string, Job>();

            if (!doc.RootElement.TryGetProperty("documents", out var documents))
                return jobs;

            foreach (var document in documents.EnumerateArray())
            {
                var fullName = document.GetProperty("name").GetString();
                var docId = fullName?.Split('/').Last();

                if (docId == null)
                    continue;

                var fields = document.GetProperty("fields");
                var job = FirestoreConverter.FromFirestoreFormatJob(fields); // separate method
                job.Id = docId;
                if (job != null)
                    jobs.Add(docId, job);
            }

            return jobs;
        }
    }
}
