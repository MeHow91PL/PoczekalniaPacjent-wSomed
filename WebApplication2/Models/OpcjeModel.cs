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
        #endregion

        #region Opcje Tła
        [Display(Name = "Kolor tła")]
        public string BackgroundColor { get; set; }
        [Display(Name = "Obrazek tła")]
        public string BackgroundImg { get; set; }
        [Display(Name = "Przeźroczystość tła")]
        public short BackgroundImgOpacity { get; set; }
        [Display(Name = "Rozmazuj tylko pod wyświetloną kolejką")]
        public bool OnlyWithNumberQueue { get; set; }
        [Display(Name = "Rozmazanie tła")]
        public short BackgroundBlur { get; set; }
        [Display(Name = "Dzwięk włączony")]
        public bool IsSummonSound { get; set; }
        [Display(Name = "Dzwięk wezwania")]
        public string SummonSound { get; set; }
        #endregion

        #region Opcje layoutu
        [Display(Name = "Wyświetlanie kolejki wezwanych pacjentów")]
        public bool WezwaniPacjenci { get; set; }
        public KolejkaWezwanych KolejkaWezwanych { get; set; }
        public WzywanyNumer WzywanyNumer { get; set; }
        #endregion
    }


    public class DbConnString
    {
        [Display(Name = "User ID")]
        public string UserID { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Data source")]
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

    public class WzywanyNumer : Kafelek
    {
        public int CzasWyswietlania { get; set; }
        public Font NumberFont { get; set; }
        public Font TimeFont { get; set; }
        public Font SurgeryFont { get; set; }
    }

    public class KolejkaWezwanych
    {
        [Display(Name = "Nagłówek")]
        public KafelekNaglowka Header { get; set; }
        public KafelekNumerka Numerek { get; set; }
    }

    public class Kafelek
    {
        [Display(Name = "Kolor od"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string ColorFrom { get; set; }
        [Display(Name = "Kolor do"), StringLength(6, ErrorMessage = "Zły format koloru!")]
        public string ColorTo { get; set; }
        [Display(Name = "Wysykość"), Range(10, 100, ErrorMessage = "Wysykość musi mieścić się w zakresie 10-100")]
        public byte Height { get; set; }
        [Display(Name = "Kolor czcionki")]
        public string FontColor { get; set; }
        [Display(Name = "Włączyć cień")]
        public bool HasShadow { get; set; }
        [Display(Name = "Ust. cienia")]
        public BoxShadow BoxShadow { get; set; }
    }

    public class KafelekNaglowka : Kafelek
    {
        [Display(Name = "Czcionka")]
        public Font Font { get; set; }

        public bool ShowEmployeeName { get; set; }
        public bool ShowSurgeryName { get; set; }
        public bool ShowSurgeryNr { get; set; }
    }

    public class KafelekNumerka : Kafelek
    {
        [Display(Name = "Numer rozm.")]
        public Font NumerPacjentaFont { get; set; }
        [Display(Name = "Godz rozm.")]
        public Font TimeFont { get; set; }
        [Display(Name = "Gabinet rozm.")]
        public Font GabinetFont { get; set; }
    }

    public class Font
    {
        [Display(Name = "Rozmiar czcionki"), Range(1,250, ErrorMessage = "Rozmiar czcionki musi mieścić się w zakresie 1-250")]
        public byte FontSize { get; set; }
        [Display(Name = "Kolor czcionki")]
        public string FontColor { get; set; }
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
        [Display(Name = "Kolor cienia")]
        public string Color { get; set; }
        [Display(Name ="Krycie cienia"), Range(1, 100 , ErrorMessage = "Wartość musi być z zakresu 1-100")]
        public byte ColorAlpha { get; set; }
    }
}