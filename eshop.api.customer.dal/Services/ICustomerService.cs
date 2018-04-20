using eshop.api.customer.dal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop.api.customer.dal.Services
{
    public interface ICustomerService
    {
        Task<IList<Customer>> GetCustomers();
        Task<Customer> GetCustomer(string id);
        Task<ReturnResult> UpdateCustomerAsync(string id, Customer customer);
        Task<ReturnResult> InsertCustomerAsync(Customer customer);
        Task<ReturnResult> DeleteCustomerAsync(string id);
        Task<ReturnResult> AuthenticateAsync(string username, string password);
    }
}
