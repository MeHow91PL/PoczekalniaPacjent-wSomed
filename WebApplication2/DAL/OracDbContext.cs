using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Poczekalniav1.DAL
{
    public class OracDbContext : DbContext
    {

        public OracDbContext() : base("OracleDbContext")
        {

        }


        // urucohmienie  Inicjalizatora
        //
        static OracDbContext()
        {
            //Database.SetInitializer<Poczekalniav1Context>(new Poczekalniav1Initializer());
        }

        internal static OracDbContext Create()
        {
            return new OracDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.HasDefaultSchema("public");
        //}


//        #region Połączenie do bazy z kodu
//        var originalConnectionString = ConfigurationManager.ConnectionStrings["your_connection_string"].ConnectionString;
//        var entityBuilder = new EntityConnectionStringBuilder(originalConnectionString);
//        var factory = DbProviderFactories.GetFactory(entityBuilder.Provider);
//        var providerBuilder = factory.CreateConnectionStringBuilder();

//        providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;

//providerBuilder.Add("Password", "Password123");

//entityBuilder.ProviderConnectionString = providerBuilder.ToString();

//using (var context = new YourContext(entityBuilder.ToString()))
//{
//    // TODO
//}
//        #endregion
    }
}