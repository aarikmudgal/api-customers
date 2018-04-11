using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop.api.customer.dal.Models;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.customer.dal.DBContext
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        public bool CheckConnection()
        {
            try
            {
                this.Database.OpenConnection();
                this.Database.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
