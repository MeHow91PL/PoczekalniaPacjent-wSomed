using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
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
                string colorCode = value;
                Regex template = new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$");
                if (!colorCode.Contains('#'))
                {
                    colorCode = "#" + colorCode;
                }
                if (template.IsMatch(colorCode))
                {
                    _optionFile.Element("UserConfig").Element("BackgroundColor").Element("Color").Value = colorCode;
                    _optionFile.Save(Paths.UserConfigPath);
                }
                else
                    throw new Exception("Kolor podany w złym formacie!");
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



        public static OpcjeModel GetOpcjeModel()
        {
            OpcjeModel model = new OpcjeModel();
            model.DatabaseConnString.ConnectionString = DatabaseConnectionString;
            model.BackgroundColor = BackgroundColor;
            model.BackgroundImg = BackgroundImage;
            model.BackgroundImgOpacity = BackgroundImageOpacity;
            return model;
        }
    }
}