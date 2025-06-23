using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM
{
    public class FirebaseAuthService
    {
        private const string ApiKey = "AIzaSyAMZH68z6W0Bpk0QpyOuqHrLD9diOXWmcI";
        private const string SignInUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + ApiKey;

        public async Task<string> GetValidIdTokenAsync()
        {
            var expireString = await SecureStorage.GetAsync("tokenExpire");
            var idToken = await SecureStorage.GetAsync("idToken");

            if (string.IsNullOrEmpty(expireString) || string.IsNullOrEmpty(idToken))
                return null;

            if (!DateTime.TryParseExact(
        expireString,
        "o",
        CultureInfo.InvariantCulture,
        DateTimeStyles.RoundtripKind,
        out var expire))
                return null;

            if (DateTime.UtcNow < expire)
                return idToken;

            // Token expired — try refreshing
            var refreshToken = await SecureStorage.GetAsync("refreshToken");
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var refreshRequest = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };

            var json = JsonSerializer.Serialize(refreshRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync($"https://securetoken.googleapis.com/v1/token?key={ApiKey}", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var resultJson = await response.Content.ReadAsStringAsync();
            var tokenResult = JsonSerializer.Deserialize<FirebaseRefreshResponse>(resultJson);

            // Store new tokens
            await SecureStorage.SetAsync("idToken", tokenResult.idToken);
            await SecureStorage.SetAsync("refreshToken", tokenResult.refreshToken);

            var newExpiry = DateTime.UtcNow.AddSeconds(int.Parse(tokenResult.expiresIn));
            await SecureStorage.SetAsync("tokenExpire", newExpiry.ToString("o"));

            return tokenResult.idToken;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var client = new HttpClient();

            var requestBody = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(SignInUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(resultJson);

                // Save tokens securely
                await SecureStorage.SetAsync("idToken", result.idToken);
                await SecureStorage.SetAsync("refreshToken", result.refreshToken);
                await SecureStorage.SetAsync("userId", result.localId);

                var expiresInSec = int.Parse(result.expiresIn);
                var expiresAt = DateTime.UtcNow.AddSeconds(expiresInSec);
                await SecureStorage.SetAsync("tokenExpire", expiresAt.ToString("o"));
                return true;
            }

            return false;
        }

        public async Task<bool> SignUpAsync(string email, string password)
        {
            var client = new HttpClient();

            var requestBody = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string signUpUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + ApiKey;

            var response = await client.PostAsync(signUpUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(resultJson);
               
                return true;
            }

            return false;
        }

        public async Task<bool> LogoutAsync()
        {
            if(SecureStorage.Remove("idToken") && SecureStorage.Remove("refreshToken") && SecureStorage.Remove("userId") && SecureStorage.Remove("tokenExpire"))
            {
                return true;
            }
            return false;
        }

        private class FirebaseLoginResponse
        {
            public string idToken { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
            public string localId { get; set; }
        }

        private class FirebaseRefreshResponse
        {
            public string idToken { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
        }
    }
}
