using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Poczekalniav1.Models
{
    public class OpcjeModel
    {
        #region Opcje bazy danych
        public DbConnString DatabaseConnString { get; set; } = new DbConnString();
        [Display(Name = "Kolor tła")]
        #endregion

        #region Opcje Tła
        public string BackgroundColor { get; set; }
        [Display(Name = "Obrazek tła")]
        public string BackgroundImg { get; set; }
        [Display(Name = "Przeźroczystość tła")]
        public short BackgroundImgOpacity { get; set; }
        [Display(Name = "Rozmazuj tylko pod wyświetloną kolejką")]
        public bool OnlyWithNumberQueue { get; set; }
        [Display(Name = "Rozmazanie tła")]
        public short BackgroundBlur { get; set; }
        #endregion

        #region Opcje layoutu
        [Display(Name = "Wyświetlanie kolejki wezwanych pacjentów")]
        public bool WezwaniPacjenci { get; set; }

        #region Kolejka wezwanych
        public KolejkaWezwanych KolejkaWezwanych { get; set; }
        #endregion

        #endregion
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
                    this.DataSource =
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

    public class KolejkaWezwanych
    {
        [Display(Name = "Nagłówek")]
        public Kafelek Header { get; set; }

        #region Numerek
        [Display(Name = "Kolor numerku od"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string NumberColorFrom { get; set; }

        [Display(Name = "Kolor numerku do"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string NumberColorTo { get; set; }

        [Display(Name = "Rozmiar czcionki numerku")]
        public byte NumerPacjentaFontSize { get; set; }

        [Display(Name = "Rozmiar czcionki nazwy gabinetu")]
        public byte GabinetFontSize { get; set; }
        #endregion
    }

    public class Kafelek
    {
        [Display(Name = "Kolor od"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string ColorFrom { get; set; }
        [Display(Name = "Kolor do"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string ColorTo { get; set; }
        [Display(Name = "Czcionka")]
        public Font Font { get; set; }
        [Display(Name = "Wysykość"), Range(10, 100, ErrorMessage = "Wysykość musi mieścić się w zakresie 10-100")]
        public byte Height { get; set; }
        [Display(Name = "Włączyć cień")]
        public bool HasShadow { get; set; }
        [Display(Name = "Ust. cienia")]
        public BoxShadow BoxShadow { get; set; }
    }

    public class Font
    {
        [Display(Name = "Rozmiar"), Range(1,250, ErrorMessage = "Rozmiar czcionki musi mieścić się w zakresie 1-250")]
        public byte FontSize { get; set; }
    }

    public class BoxShadow
    {
        [Display(Name = "Horyzonatlne przesunięcie")]
        public byte HorizontalOffset { get; set; }
        [Display(Name = "Vertykalne przesunięcie")]
        public byte VerticalOffset { get; set; }
        [Display(Name = "Rozmazanie")]
        public byte Blur { get; set; }
        [Display(Name = "Rozszerzenie")]
        public byte Spread { get; set; }
        [Display(Name = "Kolor")]
        public Color Color { get; set; }
    }
}