﻿using Poczekalniav1.Infrastructure;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Poczekalniav1.DAL
{
    public class OracleDbManager : DbManager
    {
        private string connString = OpcjeAplikacjiManager.DatabaseConnectionString;
        static OracDbContext bd1 = new OracDbContext();

        //public DbInfo GetDbInfo()
        //{
        //    DbInfo dbinfo = new DbInfo { ConnString = db.Database.Connection.ConnectionString, DataSourcce = db.Database.Connection.DataSource };
        //    return dbinfo ;
        //}


        //"User Id=gabinet;Password=Kam$oft1;Data Source=localhost"
        public override bool CzyPolaczonoPoprawnie()
        {
            bd1.Database.Connection.ConnectionString = OpcjeAplikacjiManager.DatabaseConnectionString;
            if (!PolaczonoZBaza) return false;
            else return true;
            //throw new Exception("Nie udało się nawiązać połączenia z bazą danych! /n" +
            //    "Szczególy: ");
        }

        public bool PolaczonoZBaza
        {
            get
            {
                try
                {
                    return true;
                }
                catch (Exception) { return false; }
            }
        }

        public override int IloscProszonychPacjentow
        {
            get { return PobierzProszonychPacjentow().Count; }
        }

        private Queue<ProszonyPacjentModel> _kolejkaWezwan = new Queue<ProszonyPacjentModel>();
        public override Queue<ProszonyPacjentModel> KolejkaWezwan
        {
            get
            {
                if (IloscProszonychPacjentow > 0)
                {
                    foreach (var item in PobierzProszonychPacjentow())
                    {
                        _kolejkaWezwan.Enqueue(item);
                    }
                }
                return _kolejkaWezwan;
            }
        }


        public override ProszonyPacjentModel PobierzProszonegoPacjentaPoId(int idPacjenta)
        {
            throw new NotImplementedException();
        }

        private static List<ProszonyPacjentModel> lastCorrectList;
        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow()
        {
            using (OracDbContext db = new OracDbContext())
            {
                if (PolaczonoZBaza)
                {

                    List<ProszonyPacjentModel> t =
                            db.Database.SqlQuery<ProszonyPacjentModel>("select p.GABINET_ID, p.GABINET_NAZWA, p.GABINET_NUMER, p.NUMER_DZIENNY, to_char(d.GODZINA, 'HH24:MI') as GODZINA " +
                                "from PROSZENI_PACJENCI p join PACJENCI_NA_DZIS d on (d.NUMER_DZIENNY = p.NUMER_DZIENNY)").ToList();
                    //lastCorrectList.Clear();
                    //lastCorrectList.AddRange(t);
                    return t;
                }
                else
                {
                    //return lastCorrectList;
                    return new List<ProszonyPacjentModel>();
                }
            }
        }

        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow(int idPoczekalni)
        {
            throw new NotImplementedException();
        }

        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow(string nazwaGabinetu)
        {
            throw new NotImplementedException();
        }

        private class OracDbContext : DbContext
        {

            public OracDbContext() : base("OracleDbContext")
            {
                //Właściwe ustawienie
                this.Database.Connection.ConnectionString = OpcjeAplikacjiManager.DatabaseConnectionString;

                //Ust. do testów lokalnych
                //this.Database.Connection.ConnectionString = "User Id=gabinet;Password=Kam$oft1;Data Source=localhost";
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
}