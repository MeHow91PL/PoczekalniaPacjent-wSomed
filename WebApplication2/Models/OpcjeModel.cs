using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Poczekalniav1.Models
{
    public class OpcjeModel
    {
        public DbConnString DatabaseConnString { get; set; } = new DbConnString();
        [Display(Name = "Kolor tła")]
        public string BackgroundColor { get; set; }
        [Display(Name = "Obrazek tła")]
        public string BackgroundImg { get; set; }
        [Display(Name = "Przeźroczystość tła")]
        public short BackgroundImgOpacity { get; set; }
    }

    public class DbConnString
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string DataSource { get; set; }
        //private string _connectionString;

        public string ConnectionString
        {
            get { return "User Id=" + UserID + ";Password=" + Password + ";Data Source=" + DataSource; }
            set
            {
                string connString = value;
                if (sprawdzPoprawnoscConnStringa(connString))
                {
                    string[] splitedConnString = connString.Split(';');
                    this.UserID =
                        splitedConnString.SingleOrDefault(s => s.ToLower().Contains("User Id=".ToLower())).Replace("User Id=", "");
                    this.Password =
                            splitedConnString.SingleOrDefault(s => s.ToLower().Contains("Password=".ToLower())).Replace("Password=", "");
                    this.DataSource=
                            splitedConnString.SingleOrDefault(s => s.ToLower().Contains("Data Source=".ToLower())).Replace("Data Source=", "");
                }
            }
        }

        private bool sprawdzPoprawnoscConnStringa(string connString)
        {
            if (connString.Contains("User Id=") && connString.Contains("Password=")
                && connString.Contains("Data Source=") && connString.Contains(';'))
            {
                return true;
            }
            else
            {
                throw new Exception("Podany connection string ma nieprawidłowy format");
            }
        }
    }
}