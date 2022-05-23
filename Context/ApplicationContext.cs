using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.File;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Context
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User>Users { get; set; }
        public DbSet<Role>Roles { get; set; }
        public DbSet<UserRole>UserRoles { get; set; }
        public DbSet<Admin>Admins { get; set; }
        public DbSet<Buyer>Buyers{ get; set; }
        public DbSet<Payment>Payments{ get; set; }
        public DbSet<Property>Properties{ get; set; }
        public DbSet<Realtor>Realtors{ get; set; }
        public DbSet<VisitationRequest>VisitationRequests{ get; set; }
        public DbSet<PropertyDocument>PropertyDocuments{ get; set; }
        public DbSet<PropertyImage>PropertyImages{ get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() {Id = 1, RoleName = RoleConstant.Administrator.ToString()},
                new Role(){Id = 2,RoleName = RoleConstant.Realtor.ToString()},
                new Role(){Id = 3,RoleName = RoleConstant.Buyer.ToString()}
            );
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,Email = "oladejimujib@yahoo.com",Password = "password",FirstName = "Mujib",LastName = "Oladeji",
                    PhoneNumber = "08136794915",ProfilePicture = null
                }
            );
            modelBuilder.Entity<Admin>().HasData(
                new Admin(){Id = 1,Address = "Apata,Ibadan",RegId = "Ad0001",UserId = 1,}
                );
            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                Id = 1,
                RoleId = 1,
                UserId = 1,
            });
        }
    }
}