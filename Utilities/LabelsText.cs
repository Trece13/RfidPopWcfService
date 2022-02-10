using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Xml.Linq;

namespace Utilities
{
    public class LabelsText
    {
        private static LogGenerator log = new LogGenerator();
        private static MethodBase method;
        private static string strError = string.Empty;

        public string readStatement(string Module, string idiom, string labelName)
        {
            var nombreLabel = labelName;
            method = MethodBase.GetCurrentMethod();
            string resultado = string.Empty;
            strError = string.Empty;
            string recursoXML = resolveUrlAbsolute(ConfigurationManager.AppSettings["fileLabelsForms"].ToString());

            // Validar parametros de entrada
            if (string.IsNullOrEmpty(Module) || string.IsNullOrEmpty(nombreLabel) || string.IsNullOrEmpty(idiom))
            { strError = "Valores no válidos para los parametros Module y operationName"; }

            if (!string.IsNullOrEmpty(strError)) { throw new System.ArgumentException(strError); }

            try
            {
                XElement xmlModulos = XElement.Load(recursoXML);
                XElement module = null;

                XElement result = xmlModulos.Element("Modules")
                                            .Elements("Module")
                                            .Single(x => x.Attribute("name").Value == Module)
                                            .Elements("Idiom")
                                            .Single(x => x.Attribute("name").Value == idiom);


                module = result.Elements("label").FirstOrDefault(x => x.Attribute("name").Value == nombreLabel);

                if (module == null) { throw new System.ArgumentException("module " + Module + " for " + nombreLabel + " return null on query"); }

                resultado = (string)module.Elements("stringMessage").FirstOrDefault();

                xmlModulos = null;
            }
            catch (Exception ex)
            {
                strError = "Failed to read statement. Try again or contact your administrator ";
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
            }
            return resultado.Trim();
        }

        public string resolveUrlAbsolute(string searchPath = null)
        {
            if (string.IsNullOrEmpty(searchPath))
                return searchPath;

            if (System.Web.HttpContext.Current == null)
                return searchPath;

            // Verificar la cadena que llega sobre le metodo para arreglarla en dado caso
            if (searchPath.StartsWith("/"))
                searchPath = searchPath.Insert(0, "~");

            if (!searchPath.StartsWith("~/"))
                searchPath = searchPath.Insert(0, "~/");

            var url = System.Web.HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, System.Web.VirtualPathUtility.ToAbsolute(searchPath));

        }
    }
}
