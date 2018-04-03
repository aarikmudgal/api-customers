//using eshop.api.customer.dal.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace eshop.api.customer.dal.Services
//{
//    public class CustomerFileService : ICustomerService
//    {
//        private static List<Customer> customers = null;

//        static CustomerFileService()
//        {
//            LoadCustomersFromFile();
//        }

//        private static void LoadCustomersFromFile()
//        {
//            customers = JsonConvert.DeserializeObject<List<Customer>>(System.IO.File.ReadAllText(@"customers.json"));
//        }

//        private void WriteCustomersToFile()
//        {
//            try
//            {
//                System.IO.File.WriteAllText(@"customers.json", JsonConvert.SerializeObject(customers));
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        //public IActionResult GetHealth(string health)
//        //{
//        //    bool fileExists = System.IO.File.Exists("./customers.json");
//        //    IActionResult response = fileExists ? Ok("Service is Healthy") : StatusCode(500, "Customers file not available");
//        //    return response;
//        //}

//        //public IActionResult Login([FromBody]JObject value)
//        //{
//        //    Customer customer = JsonConvert.DeserializeObject<Customer>(value.ToString());

//        //    byte[] bytes = Encoding.UTF8.GetBytes(customer.Password);
//        //    string encodedPassword = Convert.ToBase64String(bytes);

//        //    bool customerExists = customers.Exists(x => x.Username == customer.Username && x.Password == encodedPassword);
//        //    IActionResult response = customerExists ? Ok("Customer Authorised") : StatusCode(401, "Customer Unauthorised");
//        //    return response;
//        //}

//        //public IActionResult GetCustomers()
//        //{
//        //    return new ObjectResult(customers);
//        //}

//        //public IActionResult GetCustomerByUsername(string username)
//        //{
//        //    Customer customer = customers.Find(c => c.Username == username);
//        //    if (customer != null)
//        //        return new ObjectResult(customer);
//        //    else
//        //        return NotFound($"Customer with Username - {username} not found");
//        //}

//        //public IActionResult AddCustomer([FromBody]JObject value)
//        //{
//        //    Customer cust;
//        //    try
//        //    {
//        //        // create new customer object
//        //        cust = JsonConvert.DeserializeObject<Customer>(value.ToString());
//        //        cust.CustomerId = Guid.NewGuid().ToString();

//        //        byte[] bytes = Encoding.UTF8.GetBytes(cust.Password);
//        //        string encodedPassword = Convert.ToBase64String(bytes);

//        //        cust.Password = encodedPassword;

//        //        // add new customer to list
//        //        customers.Add(cust);
//        //        WriteCustomersToFile();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // log the exception
//        //        // internal server errror
//        //        return StatusCode(500, ex.Message);
//        //    }
//        //    return Ok($"Customer added successfully. New Customer Id - {cust.CustomerId}");
//        //}

//        //public IActionResult ChangeCustomer(string id, [FromBody]JObject value)
//        //{
//        //    try
//        //    {
//        //        Customer inputCustomer = JsonConvert.DeserializeObject<Customer>(value.ToString());
//        //        Customer customerToUpdate = customers.Find(cust => cust.CustomerId == id);
//        //        if (customerToUpdate == null)
//        //        {
//        //            return NotFound($"Customer with {id} not found");
//        //        }
//        //        byte[] bytes = Encoding.UTF8.GetBytes(inputCustomer.Password);
//        //        string encodedPassword = Convert.ToBase64String(bytes);

//        //        inputCustomer.Password = encodedPassword;

//        //        customerToUpdate.DeepCopy(inputCustomer);
//        //        WriteCustomersToFile();
//        //    }
//        //    catch (System.Exception ex)
//        //    {
//        //        // log error/exception
//        //        return StatusCode(500, ex.Message);
//        //    }
//        //    return Ok($"Customer with ID - {id} updated successfully");
//        //}


//        //public IActionResult DeleteCustomer(string id)
//        //{
//        //    try
//        //    {
//        //        Customer customer = customers.Find(x => x.CustomerId == id);
//        //        if (customer == null)
//        //        {
//        //            return NotFound($"Customer with {id} not found");
//        //        }
//        //        customers.Remove(customer);
//        //        WriteCustomersToFile();
//        //    }
//        //    catch (System.Exception ex)
//        //    {
//        //        // log the exception
//        //        return StatusCode(500, ex.Message);
//        //    }
//        //    return Ok($"Customer with custome id - {id} deleted successfully");
//        //}


//        public IEnumerable<Customer> GetCustomers()
//        {
//            return customers;
//        }
//        public Customer GetCustomer(string id)
//        {
//            foreach (Customer customer in customers)
//            {
//                if (customer.CustomerId == id)
//                {
//                    return customer;
//                }
//            }
//            return null;
//        }
//        public List<string> UpdateCustomer(string id, Customer customer)
//        {
//            List<string> result = new List<string>();
//            try
//            {
//                Customer inputCustomer = JsonConvert.DeserializeObject<Customer>(customer.ToString());
//                Customer customerToUpdate = customers.Find(cust => cust.CustomerId == id);
//                if (customerToUpdate == null)
//                {
//                    result.Add("true");
//                    result.Add($"Customer with {id} not found");
//                    return result;
//                }
//                byte[] bytes = Encoding.UTF8.GetBytes(inputCustomer.Password);
//                string encodedPassword = Convert.ToBase64String(bytes);

//                inputCustomer.Password = encodedPassword;

//                customerToUpdate.DeepCopy(inputCustomer);
//                WriteCustomersToFile();
//                result.Add("success");
//            }
//            catch (System.Exception ex)
//            {
//                // log error/exception
//                result.Add("true");
//                result.Add(ex.Message);
//            }
//            return result;
//        }
//        public List<string> InsertCustomer(Customer customer)
//        {
//            Customer cust;
//            List<string> result = new List<string>();
//            try
//            {
//                cust = JsonConvert.DeserializeObject<Customer>(customer.ToString());
//                cust.CustomerId = Guid.NewGuid().ToString();

//                byte[] bytes = Encoding.UTF8.GetBytes(cust.Password);
//                string encodedPassword = Convert.ToBase64String(bytes);

//                cust.Password = encodedPassword;

//                // add new customer to list
//                customers.Add(cust);
//                WriteCustomersToFile();
//                result.Add("success");
//            }
//            catch (System.Exception ex)
//            {
//                // log the exception
//                // internal server errror
//                result.Add("true");
//                result.Add(ex.Message);
//            }
//            return result;
//        }
//        public List<string> DeleteCustomer(string id)
//        {
//            List<string> result = new List<string>();
//            try
//            {
//                customers.Remove(customers.Find(x => x.CustomerId == id));
//                WriteCustomersToFile();
//                result.Add("success");
//            }
//            catch (System.Exception ex)
//            {
//                result.Add("true");
//                result.Add(ex.Message);
//            }
//            return result;
//        }
//    }
//}
