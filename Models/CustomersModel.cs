
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class CustomersModel
    {
        private List<Customer> _customers;
        public IReadOnlyList<Customer> customers => _customers.AsReadOnly();

        public bool AddCustomer(Customer customer)
        {
            if(customer != null && !_customers.Any(c=> c.Id == customer.Id))
            {
                _customers.Add(customer);
                return true;    
            }
            return false;
        }
        public bool RemoveCustomer(List<Customer> customer)
        {
            int totalCount = customer.Count;
            int countRemoved = 0;
            foreach (Customer c in customer)
            {
                if (_customers.Contains(c))
                {
                    countRemoved++;
                    _customers.Remove(c);
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

