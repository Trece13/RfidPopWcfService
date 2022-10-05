using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


namespace Utilities
{
    public class Recursos
    {
        private static LogGenerator log = new LogGenerator();
        private static StackTrace stackTrace = new StackTrace();
        private static MethodBase method;
        private static string strError = string.Empty;

        /// <summary>
        /// Leer sentencia a ejecutar del archivo de recursos
        /// </summary>
        /// <param name="Module">Nombre de la clase</param>
        /// <param name="operationName">Operacion a Ejecutar</param>
        /// <param name="owner">Propietario o Esquema de la base de datos</param>
        /// <param name="env">Ambiente</param>
        /// <param name="tabla">Nombre de la tabla compuesto (propietario+tabla+ambiente): Valor por defecto Nulo </param>
        /// <param name="paramList">Lista de parametros o condiciones en caso que la sentencia sea una consulta. Valor por defecto: Nulo</param>
        /// <param name="Last">Indicador para recuperar el ultimo registro en caso que exista mas de una operacion con el mismo nombre en el mismo modulo</param>
        /// <returns>Cadena formateada con la sentencia a ejecutar</returns>
        public string readStatement(string Module, string operationName, ref string owner, ref string env, string tabla = null, Dictionary<string, object> paramList = null, int Last = 0)
        {
            method = MethodBase.GetCurrentMethod();
            string resultado = string.Empty;
            strError = string.Empty;
            string recursoXML = ConfigurationManager.AppSettings["fileResourcesSQL"].ToString();
            //string recursoXML = resolveUrlAbsolute(ConfigurationManager.AppSettings["fileResourcesSQL"].ToString());
           
            // Validar parametros de entrada
            if (string.IsNullOrEmpty(Module) || string.IsNullOrEmpty(operationName))
            { strError = "Valores no válidos para los parametros Module y operationName"; }

            if (Last < 0 || Last > 1 ) { strError = "El valor del último parametro debe estar entre cero(0) y uno(1) "; }

            if (!string.IsNullOrEmpty(strError)) { throw new System.ArgumentException(strError); }

            try
            {
                XElement xmlModulos = XElement.Load(recursoXML);
                XElement module = null;
                
                XElement result = xmlModulos.Element("Modules")
                                            .Elements("Module")
                                            .Single(x => x.Attribute("name").Value == Module);

                switch (Last)
                {
                    case 0:
                        module = result.Elements("operationName").FirstOrDefault(x => x.Attribute("name").Value == operationName);
                        break;
                    case 1:
                        module = result.Elements("operationName").LastOrDefault(x => x.Attribute("name").Value == operationName);
                         break;
                    }

                    if (module == null) { throw new System.ArgumentException("module " + Module + " for " + operationName + " return null on query"); }

                    resultado = (string)module.Elements("stringSQL").FirstOrDefault();

                if (paramList != null)
                    resultado = addParametersToStatement(( !string.IsNullOrEmpty(tabla) ? string.Format(resultado, owner, env, tabla) : 
                                                            string.Format(resultado, owner, env)), ref paramList);
                else
                    resultado = !string.IsNullOrEmpty(tabla) ? string.Format(resultado, owner, env, tabla) : string.Format(resultado, owner, env);


                xmlModulos = null;
            }
            catch (Exception ex)
            {
                strError = "Failed to read statement. Try again or contact your administrator ";
                log.Write(strError + " " + ex.Message + " path:" + recursoXML+" Module:"+Module+" operationName:"+operationName, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
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
        
        protected string addParametersToStatement(string statement, ref Dictionary<string, object> paramList)
        {
            if (paramList == null)
                return statement;

            List<string> paramKeys = new List<string>(paramList.Keys);
            
            // recorrer las llaves de la lista y asignarla en la cadena con la sentencia
            foreach (string key in paramKeys)
            {
                if (statement.Contains(key))
                {
                    statement = statement.Replace("[" + key + "]", paramList[key].ToString());
                }
            }

            return statement;
        }

        public void createMachines(string path)
        {
            createTableMachineId(path);
        }

        protected void createTableMachineId(string path)
        {
            DataTable machineRelation = new DataTable();
            DataColumn Machine = new DataColumn("Machine", typeof(string));
            DataColumn MachineId = new DataColumn("MachineId", typeof(string));
            DataColumn Kg = new DataColumn("Kg", typeof(string));
            DataColumn Id = new DataColumn("Id", typeof(string));

            machineRelation.Columns.Add(Machine);
            machineRelation.Columns.Add(MachineId);
            machineRelation.Columns.Add(Kg);
            machineRelation.Columns.Add(Id);

            DataRow row = machineRelation.NewRow();

            //            row["MachineId"] = "
            //Colombia
            row["Machine"] = "LA01"; row["MachineId"] = "XMLCN_GrupoPhoenix14"; row["Kg"] = "1"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "LA12"; row["MachineId"] = "XMLCN_GrupoPhoenix15"; row["Kg"] = "1"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM11"; row["MachineId"] = "XMLCN_GrupoPhoenix11"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM75"; row["MachineId"] = "XMLCN_GrupoPhoenix12"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM50"; row["MachineId"] = "XMLCN_GrupoPhoenix13"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM40"; row["MachineId"] = "XMLCN_GrupoPhoenix16"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "IM18"; row["MachineId"] = "XMLCN_GrupoPhoenix21"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "IM06"; row["MachineId"] = "XMLCN_GrupoPhoenix23"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "MA05"; row["MachineId"] = "XMLCN_GrupoPhoenix22"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "FLG1"; row["MachineId"] = "XMLCN_GrupoPhoenix24"; row["Kg"] = "1"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "WE07"; row["MachineId"] = "XMLCN_GrupoPhoenix25"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "UF10"; row["MachineId"] = "XMLCN_GrupoPhoenix26"; row["Kg"] = "0"; row["Id"] = "Colombia"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            //USA
            row["Machine"] = "OM13"; row["MachineId"] = "XMLCN_GrupoPhoenix33"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM15"; row["MachineId"] = "XMLCN_GrupoPhoenix35"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "OM16"; row["MachineId"] = "XMLCN_GrupoPhoenix36"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "TE01"; row["MachineId"] = "XMLCN_GrupoPhoenix37"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "TE02"; row["MachineId"] = "XMLCN_GrupoPhoenix38"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "TE03"; row["MachineId"] = "XMLCN_GrupoPhoenix39"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "TE04"; row["MachineId"] = "XMLCN_GrupoPhoenix310"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row); row = machineRelation.NewRow();
            row["Machine"] = "TE05"; row["MachineId"] = "XMLCN_GrupoPhoenix311"; row["Kg"] = "0"; row["Id"] = "USA"; machineRelation.Rows.Add(row);

            DataSet ds = new DataSet();
            ds.Tables.Add(machineRelation);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            machineRelation.WriteXml(path, XmlWriteMode.WriteSchema);
        }

        public bool createXml(DataTable records, string path)
        {
            return createXMLFile(records, path);
        }

        protected bool createXMLFile(DataTable records, string path)
        {
            StringBuilder xml = new StringBuilder();
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            string owner = "";
            string env = "";
            string[] xmlColumns;
            DataTable machineId = new DataTable();

            try
            {
                string strSentencia = readStatement("XMLGenerate", "xmlColumns", ref owner, ref env, null, paramList);
                xmlColumns = strSentencia.Split(',');

                string[] columns = (from dc in records.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

                string file = path + ConfigurationManager.AppSettings["pathResourcesSQL"].ToString() + "MachinesId.xml";

                if (!System.IO.File.Exists(file))
                    createTableMachineId(file);

                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(file);

                machineId.ReadXml(reader);
            }
            catch (Exception ex)
            {
                strError = " Error procesando xmlGenerate";
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
                return false;
            }


            xml.Append("<DataInfo>\n");
            string xmlLine = string.Empty;

            foreach (DataRow fila in records.Rows)
            {
                xmlLine = "\t<JobScheduleEdit Action=\"Add\" DefaultJob=\"1\" ";
                for (int i = 0; i < xmlColumns.Count(); i++)
                {
                    try
                    {
                        xmlLine += xmlColumns[i].ToString().Trim().Replace('_', ' ') + "=\"" + fila[xmlColumns[i].Trim()].ToString().Trim() + "\" ";
                    }
                    catch
                    {
                        if (xmlColumns[i].ToString().Trim() == "MachineID")
                        {
                            DataRow[] machineRow = machineId.Select("Machine = '" + fila["t$mcno"].ToString() + "'");
                            xmlLine += xmlColumns[i].ToString().Trim().Replace('_', ' ') + "=\"" + (machineRow.Count() > 0 ? machineRow[0]["MachineId"].ToString().Trim() :
                                                                           "Sin Identificacion") + "\" ";
                        }
                        //if (xmlColumns[i].ToString().Trim() == "FGCycleFactor")
                        //{
                        //    if (fila["UNIDAD"].ToString().Trim() == "CJ")
                        //    {
                        //        decimal unxcj = Convert.ToDecimal(fila["Filter2"].ToString().Trim());
                        //        decimal Ciclo = Convert.ToDecimal(fila["CycleFactor"].ToString().Trim());
                        //        decimal valor = 0;
                        //        if (unxcj > 0 && Ciclo > 0)
                        //            valor = (decimal)Ciclo / (decimal)unxcj;

                        //        xmlLine += xmlColumns[i].ToString().Trim().Replace('_', ' ') + "=\"" + valor.ToString().Replace(".","").Replace(",","") +"\" ";

                        //    }
                        //}

                        //if (xmlColumns[i].ToString().Trim() == "RunspeedExpected")
                        //{
                        //    int cavidades = Convert.ToInt32(fila["T$CAMO"].ToString().Trim());
                        //    decimal unxcj = Convert.ToDecimal(fila["Filter2"].ToString().Trim());
                        //    decimal Ciclo = Convert.ToDecimal(fila["PRTE"].ToString().Trim());

                        //    decimal valor = 0;
                        //    if (cavidades > 0 && Ciclo > 0 && unxcj > 0)
                        //        valor = (decimal)cavidades / (decimal)(Ciclo * unxcj);

                        //    xmlLine += xmlColumns[i].ToString().Trim().Replace('_', ' ') + "=\"" + valor.ToString() + "\" ";
                        //}
                    }
                }
                xmlLine += ("/>" + System.Environment.NewLine);
                xml.Append(xmlLine);
                xmlLine = string.Empty;
            }


            xml.Append("</DataInfo>");
            try
            {
                string fileOutput = ConfigurationManager.AppSettings["fileNameOutput"].ToString();
                File.WriteAllText(fileOutput, xml.ToString());
                return true;
            }
            catch (Exception ex)
            {
                strError = "";
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);

                return false;
            }

        }

        public void DownloadFile(string url, string nameFile, string extension, ref Dictionary<string, bool> dcArchivos, ref string strError)
        {
            Boolean retorno = false;
            string pathFile = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(url))
                    strError = "URL inválida";
                else if (DescargarArchivo(url, nameFile, extension, ref pathFile, ref strError))
                {
                    retorno = true;
                }

            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
                retorno = false;
            }
            dcArchivos.Add(pathFile, retorno);
        }

        protected bool DescargarArchivo(string url, string nameFile, string extension, ref string pathFile, ref string strError)
        {
            bool siDescargo;
            string strUserName = string.Empty, strPassword = string.Empty; 
            //string userImpersonation = string.Empty, passImpersonation = string.Empty, domaImpersonation = string.Empty;
            try
            {
                //WebClient wc = new WebClient();
                strUserName = ConfigurationManager.AppSettings["usershoplogix"].ToString();
                strPassword = ConfigurationManager.AppSettings["passshoplogix"].ToString();
                pathFile = ConfigurationManager.AppSettings["pathDownload"].ToString() + nameFile + extension;

                //userImpersonation = ConfigurationManager.AppSettings["userImpersonation"].ToString();
                //passImpersonation = ConfigurationManager.AppSettings["passImpersonation"].ToString();;
                //domaImpersonation = ConfigurationManager.AppSettings["domaImpersonation"].ToString();;

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    strError = "Las credenciales del servidor remoto son inválidas";
                    siDescargo = false;
                }
                else
                {
                    WebClient wc = new WebClient();
                    CredentialCache crCache = new CredentialCache();
                    crCache.Add(new Uri(url), "Basic", new NetworkCredential(strUserName, strPassword));
                    wc.Credentials = crCache;
                    //wc.UseDefaultCredentials = true;

                    //whusa.Utilidades.ImpersonateManager.ImpersonateUser(domaImpersonation, userImpersonation, passImpersonation);
                    byte[] dataBuffer = wc.DownloadData(url);
                    string textFile = Encoding.ASCII.GetString(dataBuffer);
                    if (File.Exists(pathFile))
                        File.Delete(pathFile);

                    File.WriteAllText(pathFile, textFile);
                    //whusa.Utilidades.ImpersonateManager.StopImpersonation();
                    siDescargo = true;
                }
            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
                siDescargo = false;
            }
            return siDescargo;
        }

        public void DownloadFile2(string url, string nameFile, string extension, ref Dictionary<string, bool> dcArchivos, ref string strError)
        {
            Boolean retorno = false;
            string pathFile = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(url))
                    strError = "URL inválida";
                else if (DescargarArchivo2(url, nameFile, extension, ref pathFile, ref strError))
                {
                    retorno = true;
                }

            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
                retorno = false;
            }
            dcArchivos.Add(pathFile, retorno);
        }
    
        protected bool DescargarArchivo2(string url, string nameFile, string extension, ref string pathFile, ref string strError)
        {
            bool siDescargo;
            string strUserName = string.Empty, strPassword = string.Empty;
            try
            {
                HttpWebRequest http = (HttpWebRequest)WebRequest.Create(url);

                strUserName = ConfigurationManager.AppSettings["usershoplogix"].ToString();
                strPassword = ConfigurationManager.AppSettings["passshoplogix"].ToString();
                pathFile = ConfigurationManager.AppSettings["pathDownload"].ToString() + nameFile + extension;

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    strError = "Las credenciales del servidor remoto son inválidas";
                    siDescargo = false;
                }
                else
                {
                    CredentialCache crCache = new CredentialCache();
                    crCache.Add(new Uri(url), "Basic", new NetworkCredential(strUserName, strPassword));
                    http.Credentials = crCache;
                    //http.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    http.UseDefaultCredentials = true;

                    if (File.Exists(pathFile)) File.Delete(pathFile);

                    //whusa.Utilidades.ImpersonateManager.ImpersonateUser("GrupoPhoenixco.net", "webshoplogix", "P3nt4G1r0$");
                    try
                    {
                        //ServicePointManager.ServerCertificateValidationCallback =
                        //delegate(object s, X509Certificate certificate,
                        //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        //{ return true; };
                        http.UseDefaultCredentials = true;
                        http.PreAuthenticate = true;
                        http.Credentials = CredentialCache.DefaultCredentials;
                        WebResponse response = http.GetResponse();
                        Stream stream = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream);
                        string content = sr.ReadToEnd();

                        File.WriteAllText(pathFile, content);
                        
                    }
                    catch (WebException ex)
                    {
                        strError = ex.InnerException != null ?
                            ex.Message + " (" + ex.InnerException + ")" :
                            ex.Message;
                        return false;
                    }
                    //whusa.Utilidades.ImpersonateManager.StopImpersonation();
                    siDescargo = true;
                }
            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
                //log.escribirError(strError + Console.Out.NewLine + ex.Message, method.Module.Name, method.Name, method.ReflectedType.Name);
                siDescargo = false;
            }
            return siDescargo;
        }

        public string GenerateNewPallet(string Paid, string Separator)
        {
            int IndexSeparator = Paid.IndexOf(Separator);
            string Secuence = Paid.Substring(IndexSeparator + 1);
            string Orno = Paid.Substring(0, IndexSeparator);
            string Complement = SeparatorAlphaNumeric(ref Secuence);
            string NewSecuence = IncrementToSecuence(Secuence);
            string NewSecuenceWithZeros = addZerosSecuence(Secuence, NewSecuence);
            return Orno + Separator + Complement + NewSecuenceWithZeros;
        }

        public string SeparatorAlphaNumeric(ref string Secuence)
        {
            string Complement = string.Empty;
            foreach (char x in Secuence)
            {
                if (!char.IsNumber(x))
                {
                    Complement += x;
                    Secuence = Secuence.Remove(Secuence.IndexOf(x), 1);
                }
            }
            return Complement;
        }

        protected string IncrementToSecuence(string strNumberSecuence)
        {
            return (Convert.ToInt32(strNumberSecuence) + 1).ToString();
        }

        protected string addZerosSecuence(string OldSecuence, string NewSecuence)
        {
            int NumZeros = OldSecuence.Length - NewSecuence.Length;
            for (int i = 0; i < NumZeros; i++)
            {
                NewSecuence = "0" + NewSecuence;
            }
            return NewSecuence;
        }


    }
}
