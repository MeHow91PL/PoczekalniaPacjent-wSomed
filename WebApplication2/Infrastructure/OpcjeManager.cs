using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                     Attribute("connectionString").Value = value ;
                _webConfigAppFile.Save(Paths.WebConfigApp);
            }
        }

        public static string BackgroundImage
        {
            get { return (string)_optionFile.Element("UserConfig").Element("BackgroundImage").Element("Url"); }
            set { _optionFile.Element("UserConfig").Element("BackgroundImage").Element("Url").Value = value; _optionFile.Save(Paths.UserConfigPath); }
        }


        public static string BackgroundColor
        {
            get {return (string)_optionFile.Element("UserConfig").Element("BackgroundColor").Element("Color"); }
            set { _optionFile.Element("UserConfig").Element("BackgroundColor").Element("Color").Value = value; _optionFile.Save(Paths.UserConfigPath); }
        }


        public static OpcjeModel GetOpcjeModel()
        {
            OpcjeModel model = new OpcjeModel();
            model.DatabaseConnString.ConnectionString = DatabaseConnectionString;
            model.BackgroundColor = BackgroundColor;
            return model;
        }

    }

    
}