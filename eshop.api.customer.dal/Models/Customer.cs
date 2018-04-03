namespace eshop.api.customer.dal.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool DeepCopy(Customer cust)
        {
            FirstName = cust.FirstName;
            LastName = cust.LastName;
            Username = cust.Username;
            Password = cust.Password;
            return true;
        }
    }
}