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

        public void CheckConnection(out bool dbStatusOK)
        {
            try
            {

                this.Database.OpenConnection();
                this.Database.ExecuteSqlCommand("SELECT 1");
                this.Database.CloseConnection();
                dbStatusOK = true;
            }
            catch (Exception ex)
            {
                dbStatusOK = false;
                throw ex;
            }
            finally
            {
                this.Database.CloseConnection();
            }
        }
    }
}
