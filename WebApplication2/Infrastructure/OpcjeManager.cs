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

        public static WzywanyNumer WzywanyNumer
        {
            get
            {
                XElement XNum = _optionFile.Element("UserConfig").Element("SummonNumber");
                //XElement XBoxShadowColor = XNum.Element("BoxShadow").Element("Color");
                WzywanyNumer num = new WzywanyNumer
                {
                    ColorFrom = XNum.Element("ColorFrom").Value,
                    ColorTo = XNum.Element("ColorTo").Value,
                    Height = byte.Parse(XNum.Element("Height").Value),
                    FontColor = XNum.Element("FontColor").Value,
                    NumberFont = new Models.Font
                    {
                        FontSize = byte.Parse(XNum.Element("NumberFont").Element("Size").Value)
                    },
                    TimeFont = new Models.Font
                    {
                        FontSize = byte.Parse(XNum.Element("TimeFont").Element("Size").Value)
                    },
                    SurgeryFont = new Models.Font
                    {
                        FontSize = byte.Parse(XNum.Element("SurgeryFont").Element("Size").Value)
                    },
                    CzasWyswietlania = int.Parse(XNum.Element("DisplayTime").Value)
                    //HasShadow = (bool)XNum.Element("HasShadow"),
                    //BoxShadow = new BoxShadow
                    //{
                    //    HorizontalOffset = byte.Parse(XNum.Element("BoxShadow").Element("HorizontalOffset").Value),
                    //    VerticalOffset = byte.Parse(XNum.Element("BoxShadow").Element("VerticalOffset").Value),
                    //    Blur = byte.Parse(XNum.Element("BoxShadow").Element("Blur").Value),
                    //    Spread = byte.Parse(XNum.Element("BoxShadow").Element("Spread").Value),
                    //    Color = XBoxShadowColor.Element("Name").Value,
                    //    ColorAlpha = byte.Parse(XBoxShadowColor.Element("Alpha").Value)

                    //}
                };
                return num;
            }
            set
            {
                XElement XNum = _optionFile.Element("UserConfig").Element("SummonNumber");
                WzywanyNumer num = value;

                #region SetNumerek
                XNum.Element("ColorFrom").Value = FormatujKolor(num.ColorFrom);
                XNum.Element("ColorTo").Value = FormatujKolor(num.ColorTo);
                XNum.Element("Height").Value = num.Height.ToString();
                XNum.Element("FontColor").Value = FormatujKolor(num.FontColor);
                XNum.Element("NumberFont").Element("Size").Value = num.NumberFont.FontSize.ToString();
                XNum.Element("TimeFont").Element("Size").Value = num.TimeFont.FontSize.ToString();
                XNum.Element("SurgeryFont").Element("Size").Value = num.SurgeryFont.FontSize.ToString();
                XNum.Element("DisplayTime").Value = num.CzasWyswietlania.ToString();
                //XNum.Element("HasShadow").Value = num.HasShadow.ToString();
                //XNum.Element("BoxShadow").Element("HorizontalOffset").Value = num.HasShadow ? num.BoxShadow.HorizontalOffset.ToString() : "0";
                //XNum.Element("BoxShadow").Element("VerticalOffset").Value = num.HasShadow ? num.BoxShadow.VerticalOffset.ToString() : "0";
                //XNum.Element("BoxShadow").Element("Blur").Value = num.HasShadow ? num.BoxShadow.Blur.ToString() : "0";
                //XNum.Element("BoxShadow").Element("Spread").Value = num.HasShadow ? num.BoxShadow.Spread.ToString() : "0";
                //XNum.Element("BoxShadow").Element("Color").Element("Alpha").Value = num.HasShadow ? num.BoxShadow.ColorAlpha.ToString() : "0";
                //XNum.Element("BoxShadow").Element("Color").Element("Name").Value = num.HasShadow ? num.BoxShadow.Color.ToString() : "#000000";

                #endregion

                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static bool IsSummonSound
        {
            get { return (bool)_optionFile.Element("UserConfig").Element("SummonSound").Element("IsSummonSound"); }
            set
            {
                _optionFile.Element("UserConfig").Element("SummonSound").Element("IsSummonSound").Value = value.ToString();
                _optionFile.Save(Paths.UserConfigPath);
            }
        }

        public static string SummonSound
        {
            get { return (string)_optionFile.Element("UserConfig").Element("SummonSound").Element("FileName"); }
            set { _optionFile.Element("UserConfig").Element("SummonSound").Element("FileName").Value = value; _optionFile.Save(Paths.UserConfigPath); }
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
                XElement xHeadColor = XKol.Element("Header").Element("BoxShadow").Element("Color");
                XElement xNumColor = XKol.Element("Numerek").Element("BoxShadow").Element("Color");
                KolejkaWezwanych kol = new KolejkaWezwanych
                {
                    Header = new KafelekNaglowka
                    {
                        ColorFrom = XKol.Element("Header").Element("ColorFrom").Value,
                        ColorTo = XKol.Element("Header").Element("ColorTo").Value,
                        Height = byte.Parse(XKol.Element("Header").Element("Height").Value),
                        FontColor = XKol.Element("Header").Element("FontColor").Value,
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
                            Color = xHeadColor.Element("Name").Value,
                            ColorAlpha = byte.Parse(xHeadColor.Element("Alpha").Value)
                        }
                    },
                    Numerek = new KafelekNumerka
                    {
                        ColorFrom = XKol.Element("Numerek").Element("ColorFrom").Value,
                        ColorTo = XKol.Element("Numerek").Element("ColorTo").Value,
                        Height = byte.Parse(XKol.Element("Numerek").Element("Height").Value),
                        FontColor = XKol.Element("Numerek").Element("FontColor").Value,
                        NumerPacjentaFont = new Models.Font
                        {
                            FontSize = byte.Parse(XKol.Element("Numerek").Element("NumerPacjentaFont").Element("Size").Value)
                        },
                        TimeFont = new Models.Font
                        {
                            FontSize = byte.Parse(XKol.Element("Numerek").Element("TimeFont").Element("Size").Value)
                        },
                        GabinetFont = new Models.Font
                        {
                            FontSize = byte.Parse(XKol.Element("Numerek").Element("GabinetFont").Element("Size").Value)
                        },
                        HasShadow = (bool)XKol.Element("Numerek").Element("HasShadow"),
                        BoxShadow = new BoxShadow
                        {
                            HorizontalOffset = byte.Parse(XKol.Element("Numerek").Element("BoxShadow").Element("HorizontalOffset").Value),
                            VerticalOffset = byte.Parse(XKol.Element("Numerek").Element("BoxShadow").Element("VerticalOffset").Value),
                            Blur = byte.Parse(XKol.Element("Numerek").Element("BoxShadow").Element("Blur").Value),
                            Spread = byte.Parse(XKol.Element("Numerek").Element("BoxShadow").Element("Spread").Value),
                            Color = xNumColor.Element("Name").Value,
                            ColorAlpha = byte.Parse(xNumColor.Element("Alpha").Value)

                        }
                    }
                };
                return kol;
            }
            set
            {
                XElement XKol = _optionFile.Element("UserConfig").Element("KolejkaWezwan");
                KolejkaWezwanych kol = value;

                #region SetHeader

                XKol.Element("Header").Element("ColorFrom").Value = FormatujKolor(kol.Header.ColorFrom);
                XKol.Element("Header").Element("ColorTo").Value = FormatujKolor(kol.Header.ColorTo);
                XKol.Element("Header").Element("Height").Value = kol.Header.Height.ToString();
                XKol.Element("Header").Element("FontColor").Value = FormatujKolor(kol.Header.FontColor);
                XKol.Element("Header").Element("Font").Element("Size").Value = kol.Header.Font.FontSize.ToString();
                XKol.Element("Header").Element("HasShadow").Value = kol.Header.HasShadow.ToString();
                XKol.Element("Header").Element("BoxShadow").Element("HorizontalOffset").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.HorizontalOffset.ToString() : "0";
                XKol.Element("Header").Element("BoxShadow").Element("VerticalOffset").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.VerticalOffset.ToString() : "0";
                XKol.Element("Header").Element("BoxShadow").Element("Blur").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.Blur.ToString() : "0";
                XKol.Element("Header").Element("BoxShadow").Element("Spread").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.Spread.ToString() : "0";
                XKol.Element("Header").Element("BoxShadow").Element("Color").Element("Alpha").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.ColorAlpha.ToString() : "0";
                XKol.Element("Header").Element("BoxShadow").Element("Color").Element("Name").Value = kol.Header.HasShadow ? kol.Header.BoxShadow.Color.ToString() : "#000000";
                #endregion

                #region SetNumerek
                XKol.Element("Numerek").Element("ColorFrom").Value = FormatujKolor(kol.Numerek.ColorFrom);
                XKol.Element("Numerek").Element("ColorTo").Value = FormatujKolor(kol.Numerek.ColorTo);
                XKol.Element("Numerek").Element("Height").Value = kol.Numerek.Height.ToString();
                XKol.Element("Numerek").Element("FontColor").Value = FormatujKolor(kol.Numerek.FontColor);
                XKol.Element("Numerek").Element("NumerPacjentaFont").Element("Size").Value = kol.Numerek.NumerPacjentaFont.FontSize.ToString();
                XKol.Element("Numerek").Element("TimeFont").Element("Size").Value = kol.Numerek.TimeFont.FontSize.ToString();
                XKol.Element("Numerek").Element("GabinetFont").Element("Size").Value = kol.Numerek.GabinetFont.FontSize.ToString();
                XKol.Element("Numerek").Element("HasShadow").Value = kol.Numerek.HasShadow.ToString();
                XKol.Element("Numerek").Element("BoxShadow").Element("HorizontalOffset").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.HorizontalOffset.ToString() : "0";
                XKol.Element("Numerek").Element("BoxShadow").Element("VerticalOffset").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.VerticalOffset.ToString() : "0";
                XKol.Element("Numerek").Element("BoxShadow").Element("Blur").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.Blur.ToString() : "0";
                XKol.Element("Numerek").Element("BoxShadow").Element("Spread").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.Spread.ToString() : "0";
                XKol.Element("Numerek").Element("BoxShadow").Element("Color").Element("Alpha").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.ColorAlpha.ToString() : "0";
                XKol.Element("Numerek").Element("BoxShadow").Element("Color").Element("Name").Value = kol.Numerek.HasShadow ? kol.Numerek.BoxShadow.Color.ToString() : "#000000";

                #endregion

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
            model.IsSummonSound = IsSummonSound;
            model.WzywanyNumer = WzywanyNumer;
            model.SummonSound = SummonSound;
            model.WezwaniPacjenci = WezwaniPacjenci;
            model.KolejkaWezwanych = KolejkaWezwanych;
            return model;
        }
    }
}