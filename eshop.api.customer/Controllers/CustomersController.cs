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
using System.Threading;

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
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await customerService.GetCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error while getting customers - {ex.Message}");
            }
            
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await customerService.GetCustomer(id);

            if (customer == null)
            {
                return NotFound($"Customer with Id - {id} not found");
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] string id, [FromBody] Customer customer)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine("Update customer endpoint started");
              
                ReturnResult result = await customerService.UpdateCustomerAsync(id, customer);
             
                if (result.UpdatedCustomer == null)
                {
                    return NotFound($"Customer with id {id} not found");
                }
                JObject successobj = new JObject()
                {
                    { "StatusMessage", result.StatusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(result.UpdatedCustomer)) }
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
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ReturnResult result = await customerService.InsertCustomerAsync(customer);
                JObject successobj = new JObject()
                {
                    { "StatusMessage", result.StatusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(result.UpdatedCustomer)) }
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
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ReturnResult result = await customerService.DeleteCustomerAsync(id);
                if (result.UpdatedCustomer == null)
                {
                    return NotFound($"Customer with id {id} not found");
                }
                JObject successobj = new JObject()
                {
                    { "StatusMessage", result.StatusMessage },
                    { "Customer", JObject.Parse(JsonConvert.SerializeObject(result.UpdatedCustomer)) }
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
        public async Task<IActionResult> Login([FromBody] Customer customer)
        {
            ReturnResult result = new ReturnResult();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var task = customerService.AuthenticateAsync(customer.Username, customer.Password);

                //Console.WriteLine("Do Something else till Long Authenticate process completes");

                result = await task;
                if (result.UpdatedCustomer != null)
                {
                    JObject successobj = new JObject()
                    {
                        { "StatusMessage", result.StatusMessage },
                        { "Customer", JObject.Parse(JsonConvert.SerializeObject(result.UpdatedCustomer)) }
                    };
                    return Ok(successobj);
                }
                else
                {
                    return  StatusCode(401, result.StatusMessage);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}