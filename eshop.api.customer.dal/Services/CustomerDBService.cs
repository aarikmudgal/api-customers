using eshop.api.customer.dal.DBContext;
using eshop.api.customer.dal.Models;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eshop.api.customer.dal.Services
{
    public class CustomerDBService : ICustomerService
    {
        private readonly CustomerContext _context;
        public CustomerDBService(CustomerContext context)
        {
            _context = context;

            CheckConnection();
        }

        private void CheckConnection()
        {
            try
            {
                _context.Database.GetDbConnection();
                _context.Database.OpenConnection();
            }
            catch (Exception)
            {
                // log db connectivity issue
                throw;
            }
        }

        public async Task<IList<Customer>> GetCustomers()
        {
            try
            {
               return await _context.Customers.ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<Customer> GetCustomer(string id)
        {
            return await _context.Customers.SingleOrDefaultAsync(m => m.CustomerId == id);
        }

        public async Task<ReturnResult> UpdateCustomerAsync(string id, Customer customer)
        {
            // _context.Entry(customer).State = EntityState.Modified;
            ReturnResult result = new ReturnResult();
            try
            {
                Console.WriteLine("Async update starated");
                Console.WriteLine("Check if customer exists");
                if (!CustomerExists(id))
                {
                    result.UpdatedCustomer = null;
                    result.StatusMessage = $"The Customer ID {id} does not exist.";
                    return result;
                }

                Console.WriteLine("encryption started");
                byte[] bytes = Encoding.UTF8.GetBytes(customer.Password);
                string encodedPassword = Convert.ToBase64String(bytes);

                customer.Password = encodedPassword;

                await _context.SaveChangesAsync();
                result.StatusMessage = $"Customer details updated successfully for customer Id - {id}";
                result.UpdatedCustomer = customer;
                return result;
            }
            catch (DbUpdateConcurrencyException e)
            {
                result.UpdatedCustomer = null;
                result.StatusMessage = e.Message;
                throw e;
            }
        }
        public async Task<ReturnResult> InsertCustomerAsync(Customer customer)
        {
            try
            {
                customer.CustomerId = Guid.NewGuid().ToString();

                byte[] bytes = Encoding.UTF8.GetBytes(customer.Password);
                string encodedPassword = Convert.ToBase64String(bytes);

                customer.Password = encodedPassword;

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                ReturnResult result = new ReturnResult()
                {
                    StatusMessage = "New customer added successfully",
                    UpdatedCustomer = customer
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ReturnResult> DeleteCustomerAsync(string id)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                var customer = await _context.Customers.SingleOrDefaultAsync(m => m.CustomerId == id);
                if (customer == null)
                {
                    result.UpdatedCustomer = null;
                    result.StatusMessage = $"Customer with id - {id} not found";
                    return result;
                }
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                result.UpdatedCustomer = customer;
                result.StatusMessage = $"Customer with id - {id} deleted successfully";
                return result;
            }
            catch (Exception e)
            {
                result.StatusMessage = e.Message;
                result.UpdatedCustomer = null;
                throw e;
            }
        }
        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        public async Task<ReturnResult> AuthenticateAsync(string username, string password)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                string encodedPassword = Convert.ToBase64String(bytes);

                var customer = await _context.Customers.SingleOrDefaultAsync(cust => cust.Username == username && cust.Password == encodedPassword);

                if (customer == null)
                {
                    result.UpdatedCustomer = null;
                    result.StatusMessage = $"Customer Unauthorised";
                }
                else
                {
                    result.UpdatedCustomer = customer;
                    result.StatusMessage = $"Customer Authorised";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
