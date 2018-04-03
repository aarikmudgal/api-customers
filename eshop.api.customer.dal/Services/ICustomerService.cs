using eshop.api.customer.dal.Models;
using System.Collections.Generic;

namespace eshop.api.customer.dal.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(string id);
        bool UpdateCustomer(string id, Customer customer, out Customer updatedCustomer, out string statusMessage);
        bool InsertCustomer(Customer customer, out Customer addedCustomer, out string statusMessage);
        bool DeleteCustomer(string id, out Customer deletedCustomer, out string statusMessage);
        bool Authenticate(string username, string password, out Customer customerObj, out string statusMessage);
    }
}
