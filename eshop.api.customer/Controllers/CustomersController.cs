using System.Linq;
using System.Collections.Generic;
using eshop.api.customer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace eshop.api.customer.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private static List<Customer> customers = null;

        static CustomersController()
        {
            LoadCustomersFromFile();
        }

        private static void LoadCustomersFromFile()
        {
            customers = JsonConvert.DeserializeObject<List<Customer>>(System.IO.File.ReadAllText(@"customers.json"));
        }

        private void WriteCustomersToFile()
        {
            try
            {
                System.IO.File.WriteAllText(@"customers.json", JsonConvert.SerializeObject(customers));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET api/customers/health
        [HttpGet]
        [Route("health")]
        public IActionResult GetHealth(string health)
        {
            bool fileExists = System.IO.File.Exists("./customers.json");
            IActionResult response = fileExists ? Ok("Service is Healthy") : StatusCode(500, "Customers file not available");
            return response;
        }

        // POST api/customers/login
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]JObject value)
        {
            Customer customer = JsonConvert.DeserializeObject<Customer>(value.ToString());

            byte[] bytes = Encoding.UTF8.GetBytes(customer.Password);
            string encodedPassword = Convert.ToBase64String(bytes);

            Customer customerObj = customers.Find(x => x.Username == customer.Username && x.Password == encodedPassword);

            IActionResult response = null;
            if (customerObj != null)
            {
                JObject successobj = new JObject()
                {
                    { "StatusMessage", "Customer Authorised" },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(customerObj)) }
                };
                response = Ok(successobj);
                
            }
            else
            {
                response = StatusCode(401, "Customer Unauthorised");
            }

            return response;
        }

        // GET api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            return new ObjectResult(customers);
        }

        // GET api/customers/5
        [HttpGet("{username}")]
        public IActionResult GetCustomerByUsername(string username)
        {
            Customer customer = customers.Find(c => c.Username == username);
            if (customer != null)
                return new ObjectResult(customer);
            else
                return NotFound($"Customer with Username - {username} not found");
        }

        // POST api/customers
        [HttpPost]
        public IActionResult AddCustomer([FromBody]JObject value)
        {
            Customer cust;
            try
            {
                // create new customer object
                cust = JsonConvert.DeserializeObject<Customer>(value.ToString());
                cust.CustomerId = Guid.NewGuid().ToString();

                byte[] bytes = Encoding.UTF8.GetBytes(cust.Password);
                string encodedPassword = Convert.ToBase64String(bytes);

                cust.Password = encodedPassword;

                // add new customer to list
                customers.Add(cust);
                WriteCustomersToFile();
            }
            catch (Exception ex)
            {
                // log the exception
                // internal server errror
                return StatusCode(500, ex.Message);
            }
            return Ok($"Customer added successfully. New Customer Id - {cust.CustomerId}");
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public IActionResult ChangeCustomer(string id, [FromBody]JObject value)
        {
            try
            {
                Customer inputCustomer = JsonConvert.DeserializeObject<Customer>(value.ToString());
                Customer customerToUpdate = customers.Find(cust => cust.CustomerId == id);
                if (customerToUpdate == null)
                {
                    return NotFound($"Customer with {id} not found");
                }
                byte[] bytes = Encoding.UTF8.GetBytes(inputCustomer.Password);
                string encodedPassword = Convert.ToBase64String(bytes);

                inputCustomer.Password = encodedPassword;

                customerToUpdate.DeepCopy(inputCustomer);
                WriteCustomersToFile();
            }
            catch (System.Exception ex)
            {
                // log error/exception
                return StatusCode(500, ex.Message);
            }
            return Ok($"Customer with ID - {id} updated successfully");
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            try
            {
                Customer customer = customers.Find(x => x.CustomerId == id);
                if (customer == null)
                {
                    return NotFound($"Customer with {id} not found");
                }
                customers.Remove(customer);
                WriteCustomersToFile();
            }
            catch (System.Exception ex)
            {
                // log the exception
                return StatusCode(500, ex.Message);
            }
            return Ok($"Customer with custome id - {id} deleted successfully");
        }
    }
}
