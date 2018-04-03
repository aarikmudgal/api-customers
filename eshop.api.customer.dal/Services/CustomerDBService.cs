using eshop.api.customer.dal.DBContext;
using eshop.api.customer.dal.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop.api.customer.dal.Services
{
    public class CustomerDBService : ICustomerService
    {
        private readonly CustomerContext _context;
        public CustomerDBService(CustomerContext context)
        {
            _context = context;
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        public Customer GetCustomer(string id)
        {
            return _context.Customers.SingleOrDefault(m => m.CustomerId == id);
        }

        public bool UpdateCustomer(string id, Customer customer, out Customer updatedCustomer, out string statusMessage)
        {
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                if (!CustomerExists(id))
                {
                    updatedCustomer = null;
                    statusMessage = $"The Customer ID {id} does not exist.";
                }

                _context.SaveChangesAsync();
                statusMessage = $"Customer details updated successfully for customer Id - {id}";
                updatedCustomer = customer;
                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                statusMessage = e.Message;
                throw e;
            }
        }
        public bool InsertCustomer(Customer customer, out Customer addedCustomer, out string statusMessage)
        {
            try
            {
                customer.CustomerId = Guid.NewGuid().ToString();

                byte[] bytes = Encoding.UTF8.GetBytes(customer.Password);
                string encodedPassword = Convert.ToBase64String(bytes);

                customer.Password = encodedPassword;

                _context.Customers.Add(customer);
                _context.SaveChangesAsync();
                addedCustomer = customer;
                statusMessage = "New customer added successfully";
                return true;
            }
            catch (Exception e)
            {
                statusMessage = e.Message;
                addedCustomer = null;
                throw e;
            }

        }
        public bool DeleteCustomer(string id, out Customer deletedCustomer, out string statusMessage)
        {
            try
            {
                var customer = _context.Customers.SingleOrDefault(m => m.CustomerId == id);
                if (customer == null)
                {
                    deletedCustomer = null;
                    statusMessage = $"Customer with id - {id} not found";
                    return false;
                }
                _context.Customers.Remove(customer);
                _context.SaveChangesAsync();
                deletedCustomer = customer;
                statusMessage = "Customer with id - {id} deleted successfully";
                return true;
            }
            catch (Exception e)
            {
                statusMessage = e.Message;
                deletedCustomer = null;
                throw e;
            }
        }
        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        public bool Authenticate(string username, string password, out Customer customerObj, out string statusMessage)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                string encodedPassword = Convert.ToBase64String(bytes);

                var customer = _context.Customers.SingleOrDefault(cust => cust.Username == username && cust.Password == encodedPassword);

                if (customer == null)
                {
                    customerObj = null;
                    statusMessage = $"Customer Unauthorised";
                    return false;
                }
                else
                {
                    customerObj = customer;
                    statusMessage = $"Customer Authorised";
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
