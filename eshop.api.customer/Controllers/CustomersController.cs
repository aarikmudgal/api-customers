using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eshop.api.customer.dal.Models;
using eshop.api.customer.dal.DBContext;
using eshop.api.customer.dal.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace eshop.api.customer.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly CustomerContext _context;
        ICustomerService customerService;
        public bool DBDriven = true;

        public CustomersController(CustomerContext context)
        {
            _context = context;
            if (DBDriven)
            {
                customerService = new CustomerDBService(_context);
            }
            else
            {
                //customerService = new CustomerFileService();
            }
        }

        // GET api/customers/health
        [HttpGet]
        [Route("health")]
        public IActionResult GetHealth(string health)
        {
            bool dbConnOk = false;
            string statusMessage = string.Empty;
            try
            {
                _context.CheckConnection(out dbConnOk);
                statusMessage = $"Order service is Healthy";

            }
            catch (Exception ex)
            {
                statusMessage = $"Order database or service not available - {ex.Message}";

            }
            IActionResult response = dbConnOk ? Ok(statusMessage) : StatusCode(500, statusMessage);
            return response;
        }

        // GET: api/Customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                return new ObjectResult(customerService.GetCustomers());
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error while getting customers - {ex.Message}");
            }
            
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public IActionResult GetCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = customerService.GetCustomer(id);

            if (customer == null)
            {
                return NotFound($"Customer with Id - {id} not found");
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer([FromRoute] string id, [FromBody] Customer customer)
        {
            string statusMessage;
            Customer updatedCustomer;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            try
            {
                bool status = customerService.UpdateCustomer(id, customer, out updatedCustomer, out statusMessage);
                if (updatedCustomer == null)
                {
                    return NotFound($"Customer with id {id} not found");
                }
                JObject successobj = new JObject()
                {
                    { "StatusMessage", statusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(updatedCustomer)) }
                };
                return Ok(successobj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //throw;
            }

        }

        // POST: api/Customers
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            Customer addedCustomer;
            string statusMessage;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                customerService.InsertCustomer(customer, out addedCustomer, out statusMessage);
                JObject successobj = new JObject()
                {
                    { "StatusMessage", statusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(addedCustomer)) }
                };
                return Ok(successobj);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message + " Inner Exception- " + ex.InnerException.Message);
            }

        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer([FromRoute] string id)
        {
            Customer deletedCustomer;
            string statusMessage;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var status = customerService.DeleteCustomer(id, out deletedCustomer, out statusMessage);
                if (deletedCustomer == null)
                {
                    return NotFound($"Customer with id {id} not found");
                }
                JObject successobj = new JObject()
                {
                    { "StatusMessage", statusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(deletedCustomer)) }
                };
                return Ok(successobj);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // DELETE: api/Customers/5
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Customer customer)
        {
            string statusMessage;
            Customer validCustomer;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isValidUser = customerService.Authenticate(customer.Username, customer.Password, out validCustomer, out statusMessage);
                if (isValidUser == true)
                {
                    JObject successobj = new JObject()
                    {
                        { "StatusMessage", statusMessage },
                        { "Customer", JObject.Parse(JsonConvert.SerializeObject(validCustomer)) }
                    };
                    return Ok(successobj);
                }
                else
                {
                    return  StatusCode(401, statusMessage);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
