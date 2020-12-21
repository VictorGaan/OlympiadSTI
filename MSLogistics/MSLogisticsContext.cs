using Microsoft.EntityFrameworkCore;
using Database;
using System;

namespace MSLogistics
{
    public class MSLogisticsContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientPoint> ClientPoints { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrganizationAccount> OrganizationAccounts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TypeDelivery> TypeDeliveries { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"data source={System.IO.Path.GetFullPath("MSLogistics.sqlite")}");
    }
}
