using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Poczekalniav1.Infrastructure
{
    public static class OpcjeAplikacjiManager
    {
        private static XElement _optionFile = XElement.Load(Paths.UserConfigPath);
        private static XElement _webConfigAppFile = XElement.Load(Paths.WebConfigApp);

        public static string DatabaseConnectionString
        {
            get
            {
                string conn = (string)_webConfigAppFile.Element("connectionStrings").Element("add").
                    Attribute("connectionString").Value;
                return conn;
            }
            set
            {
                _webConfigAppFile.Element("connectionStrings").Element("add").
                     Attribute("connectionString").Value = value;
                _webConfigAppFile.Save(Paths.WebConfigApp);
            }
        }

        public static string BackgroundColor
        {
            get { return (string)_optionFile.Element("UserConfig").Element("BackgroundColor").Element("Color"); }
            set
            {
                _optionFile.Element("UserConfig").Element("BackgroundColor").Element("Color").Value = FormatujKolor(value);
                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static string BackgroundImage
        {
            get { return (string)_optionFile.Element("UserConfig").Element("BackgroundImage").Element("FileName"); }
            set { _optionFile.Element("UserConfig").Element("BackgroundImage").Element("FileName").Value = value; _optionFile.Save(Paths.UserConfigPath); }
        }

        public static short BackgroundImageOpacity
        {
            get { return (short)_optionFile.Element("UserConfig").Element("BackgroundImage").Element("Opacity"); }
            set
            {
                short val = value;
                if (val >= 0 && val <= 100)
                {
                    _optionFile.Element("UserConfig").Element("BackgroundImage").Element("Opacity").Value = value.ToString();
                    _optionFile.Save(Paths.UserConfigPath);
                }
                else
                    throw new Exception("Przeźroczystość musi być wartością z zakresu 0-100");
            }
        }

        public static short BackgroundBlur
        {
            get { return (short)_optionFile.Element("UserConfig").Element("BackgroundBlur"); }
            set
            {
                short val = value;
                _optionFile.Element("UserConfig").Element("BackgroundBlur").Value = value.ToString();
                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static bool WezwaniPacjenci
        {
            get { return (bool)_optionFile.Element("UserConfig").Element("WezwaniPacjenci"); }
            set
            {
                _optionFile.Element("UserConfig").Element("WezwaniPacjenci").Value = value.ToString();
                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static bool OnlyWithNumberQueue
        {
            get { return (bool)_optionFile.Element("UserConfig").Element("OnlyWithNumberQueue"); }
            set
            {
                _optionFile.Element("UserConfig").Element("OnlyWithNumberQueue").Value = value.ToString();
                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static KolejkaWezwanych KolejkaWezwanych
        {
            get
            {
                XElement XKol = _optionFile.Element("UserConfig").Element("KolejkaWezwan");
                KolejkaWezwanych kol = new KolejkaWezwanych
                {
                    Header = new Kafelek
                    {
                        ColorFrom = XKol.Element("Header").Element("ColorFrom").Value,
                        ColorTo = XKol.Element("Header").Element("ColorTo").Value,
                        Height = byte.Parse(XKol.Element("Header").Element("Height").Value),
                        Font = new Models.Font
                        {
                            FontSize = byte.Parse(XKol.Element("Header").Element("Font").Element("Size").Value)
                        },
                        HasShadow = (bool)XKol.Element("Header").Element("HasShadow"),
                        BoxShadow = new BoxShadow
                        {
                            HorizontalOffset = byte.Parse(XKol.Element("Header").Element("BoxShadow").Element("HorizontalOffset").Value),
                            VerticalOffset = byte.Parse(XKol.Element("Header").Element("BoxShadow").Element("VerticalOffset").Value),
                            Blur = byte.Parse(XKol.Element("Header").Element("BoxShadow").Element("Blur").Value),
                            Spread = byte.Parse(XKol.Element("Header").Element("BoxShadow").Element("Spread").Value),
                            Color = Color.BlueViolet
                        }
                    },
                    NumberColorFrom = XKol.Element("NumberColorFrom").Value,
                    NumberColorTo = XKol.Element("NumberColorTo").Value,
                    NumerPacjentaFontSize = byte.Parse(XKol.Element("NumerPacjentaFontSize").Value),
                    GabinetFontSize = byte.Parse(XKol.Element("GabinetFontSize").Value)
                };
                return kol;
            }
            set
            {
                XElement XKol = _optionFile.Element("UserConfig").Element("KolejkaWezwan");
                KolejkaWezwanych kol = value;

                XKol.Element("Header").Element("ColorFrom").Value = FormatujKolor(kol.Header.ColorFrom);
                XKol.Element("Header").Element("ColorTo").Value = FormatujKolor(kol.Header.ColorTo);
                XKol.Element("Header").Element("Height").Value = kol.Header.Height.ToString();
                XKol.Element("Header").Element("Font").Element("Size").Value = kol.Header.Font.FontSize.ToString();

                XKol.Element("Header").Element("HasShadow").Value = kol.Header.HasShadow.ToString();
                XKol.Element("Header").Element("BoxShadow").Element("HorizontalOffset").Value = kol.Header.BoxShadow.HorizontalOffset.ToString();
                XKol.Element("Header").Element("BoxShadow").Element("VerticalOffset").Value = kol.Header.BoxShadow.VerticalOffset.ToString();
                XKol.Element("Header").Element("BoxShadow").Element("Blur").Value = kol.Header.BoxShadow.Blur.ToString();
                XKol.Element("Header").Element("BoxShadow").Element("Spread").Value = kol.Header.BoxShadow.Spread.ToString();


                XKol.Element("NumberColorFrom").Value = FormatujKolor(kol.NumberColorFrom);
                XKol.Element("NumberColorTo").Value = FormatujKolor(kol.NumberColorTo);
                XKol.Element("NumerPacjentaFontSize").Value = kol.NumerPacjentaFontSize.ToString();
                XKol.Element("GabinetFontSize").Value = kol.GabinetFontSize.ToString();

                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        private static string FormatujKolor(string kolor)
        {
            Regex template = new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$");
            if (!kolor.Contains('#'))
            {
                kolor = "#" + kolor;
            }
            if (template.IsMatch(kolor))
            {
                return kolor;
            }
            else
                return "";
        }

        public static OpcjeModel GetOpcjeModel()
        {
            OpcjeModel model = new OpcjeModel();
            model.DatabaseConnString.ConnectionString = DatabaseConnectionString;
            model.BackgroundColor = BackgroundColor;
            model.BackgroundImg = BackgroundImage;
            model.BackgroundImgOpacity = BackgroundImageOpacity;
            model.BackgroundBlur = BackgroundBlur;
            model.OnlyWithNumberQueue = OnlyWithNumberQueue;
            model.WezwaniPacjenci = WezwaniPacjenci;
            model.KolejkaWezwanych = KolejkaWezwanych;
            return model;
        }
    }
}