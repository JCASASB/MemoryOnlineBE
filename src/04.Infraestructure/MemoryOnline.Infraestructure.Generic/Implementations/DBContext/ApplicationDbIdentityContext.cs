//using Hispalance.Infraestructure.DB;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using StadaClubs.Infraestructure.DB;
//using System;

//namespace Hispalance.Infraestructure.EF
//{
//    public class ApplicationDbIdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IMyDbContext
//    {
//        //ya esta definido en base del abstract
//        // public DbSet<ApplicationUser> Users { get; set; }

//        private IMyConfiguration _config;

//        public ApplicationDbIdentityContext(IMyConfiguration config)
//        {
//            _config = config;
//        }

//        //ATENTION!!! not to delete for the migrations system ???
//        public ApplicationDbIdentityContext()
//        {
//        }




//        protected override void OnConfiguring(DbContextOptionsBuilder options)
//        {
//            //var filePath = Path.Combine(
//            //    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
//            //    @"Database\Database\kanban.db");

//            //options.UseSqlite("Data Source=kanban.db");



//            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

//            Console.Write("lest see");

//            var server = _config.GetSection("DBSection:server").Value;
//            var port = _config.GetSection("DBSection:port").Value;
//            var database = _config.GetSection("DBSection:database").Value;
//            var user = _config.GetSection("DBSection:user").Value;
//            var pass = _config.GetSection("DBSection:pass").Value;

//            var connectionString = String.Format("server={0};Port={1};database={2};uid={3};pwd={4}",
//                server, port, database, user, pass);

//            //var connectionString = "server=localhost;Port=3311;database=milocal;uid=root;pwd=root";


//            Console.Write(connectionString);

//            //options.UseMySql(connectionString,
//            //    mySqlOptions =>
//            //    {
//            //        mySqlOptions.ServerVersion(new Version(5, 1, 73), ServerType.MySql); // replace with your Server Version and Type
//            //    });

//            base.OnConfiguring(options);
//        }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Referentials
//            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
//            //        modelBuilder.ApplyConfiguration(new UserResultConfiguration());
//        }
//    }

//}

