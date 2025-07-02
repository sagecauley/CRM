using CRM.Firebase;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public class Controller
    {
        private FirebaseAuthService _auth;
        private FirebaseFirestore _firestore;
        private CustomersModel _customersModel;
        public Controller(FirebaseAuthService authService, FirebaseFirestore ffs, CustomersModel cm)
        {
            _auth = authService;
            _firestore = ffs;
            _customersModel = cm;
        }

        public async Task<bool> OnLogin(string username, string password)
        {
            return await _auth.LoginAsync(username, password);
        }

        public async Task<bool> OnCreateAccount(string username, string password)
        {
            return await _auth.SignUpAsync(username, password);
        }

        public async Task<bool> OnLogout()
        {
            return await _auth.LogoutAsync();
        }
        public async Task<bool> AddCustomer(Customer c)
        {
            string success = await _firestore.AddCustomerAsync(c);
            if (success != null)
            {
                _customersModel.Customers.Add(success, c);
                return true;
            }
            return false;
        }

        public async Task<bool> AddJob(Job j)
        {
            bool success = await _firestore.AddJobAsync(j);
        }
        public async Task<bool> EditCustomer(string id, Customer c)
        {
            if(_customersModel.Customers.ContainsKey(id))
            {
                bool success = await _firestore.EditCustomerAsync(id, c);
                if (success)
                {
                    _customersModel.Customers[id] = c; // Update the customer in the model
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteCustomer(string iD)
        {
            bool success = await _firestore.DeleteCustomerAsync(iD);
            if (success)
            {
                
                return true;
            }
            return false;
        }

        public async Task<bool> Initalize()
        {
            var customers = await _firestore.GetAllCustomersAsync();

            if (customers == null)
                return false;

            _customersModel.Customers.Clear();

            foreach (var kvp in customers)
            {
                _customersModel.Customers.Add(kvp.Key, kvp.Value);
            }

            return true;
        }

    }
}
