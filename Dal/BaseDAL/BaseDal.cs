using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
//using Oracle.DataAccess.Client;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Reflection;
using Entities;


namespace Dal.BaseDAL
{
    public class BaseDal
    {

        private static OracleConnection conexionBD;
        //private static Utilidades.Seguimiento log = new Seguimiento();
        private static StackTrace stackTrace = new StackTrace();

        private static MethodBase method = MethodBase.GetCurrentMethod();
        private static string metodo = method.Name;
        
        private static Dictionary<string, string> connectionCaption = new Dictionary<string, string>() { { "cnxServiceWEB", "cnxServiceWEB" },
                                                                                                        {"cnxWEBInterfaz","cnxWEBInterfaz"}};
        static BaseDal()
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Manejo de Conexiones

        public static bool creaConexion()
        {
            string cadenaConexion = string.Empty;
            cadenaConexion = ConfigurationManager.ConnectionStrings[connectionCaption["cnxWEBInterfaz"]].ConnectionString;
            method = MethodBase.GetCurrentMethod();

            System.Diagnostics.Trace.TraceWarning(cadenaConexion);
            try
            {
                conexionBD = new OracleConnection(cadenaConexion);
                conexionBD.Open();
            }
            catch (OracleException ex)
            {
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
            }

            if (conexionBD != null && conexionBD.State == System.Data.ConnectionState.Open)
                return true;
            else
                return false;
        }


        public static void CerrarConexiones(ref OracleCommand cmd, ref OracleTransaction tra, bool blnCommit = false, bool blnRollback = false)
        {
            //Verificar transacción...
            if (tra != null)
            {
                if (tra.Connection != null)
                {
                    OracleConnection conn = tra.Connection;

                    if (tra.Connection.State != ConnectionState.Closed)
                    {
                        if (blnCommit)
                            tra.Commit();
                        if (blnRollback)
                            tra.Rollback();

                        conn.Close();
                        conn.Dispose();
                    }
                }
                tra.Dispose();
                tra = null;
            }

            //Verificar si hay comando...
            if (cmd != null)
            {
                //Verificar si tiene conexión...
                if (cmd.Connection != null)
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        //Cerrarlo...
                        cmd.Connection.Close();
                        cmd.Dispose();
                    }
                }
            }
        }


        public static void CerrarConexionesCmd(ref OracleCommand cmd, bool blnCommit = false, bool blnRollback = false)
        {
            OracleTransaction tra = null;
            CerrarConexiones(ref cmd, ref tra, blnCommit, blnRollback);
        }
        #endregion

        #region Sentencias a Ejecutar

        public static OracleDataReader ListaRegistrosSimples(string tipoComandoSql, string strQuery, ref string strError, bool blnPRetorno = true, string strNombreCursor = null, string Aplicacion = null)
        {
            method = MethodBase.GetCurrentMethod();
            OracleDataReader Idr = null;
            OracleCommand cmd = new OracleCommand(); ;
            try
            {
                if (creaConexion())
                {
                    cmd.Connection = conexionBD;
                    if (tipoComandoSql.Trim().ToUpperInvariant().Equals("STOREDPROCEDURE"))
                        cmd.CommandType = CommandType.StoredProcedure;
                    else if (tipoComandoSql.Trim().ToUpperInvariant().Equals("TEXT"))
                        cmd.CommandType = CommandType.Text;
                    strError = strQuery;
                    System.Diagnostics.Trace.TraceWarning(strQuery);

                    cmd.CommandText = strQuery;
                    Idr = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                CerrarConexionesCmd(ref cmd);

                if (Idr != null && !Idr.IsClosed)
                {
                    Idr.Close();
                    Idr.Dispose();
                }
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                throw ex;
            }
            return Idr;

        }

        public static OracleDataReader ListaRegistrosParametros(string tipoSentencia, string strQuery, ref string strError, string Aplicacion,
                                                               List<Ent_ParametrosDAL> lstParameters = null,
                                                               bool blnPRetorno = true)
        {
            method = MethodBase.GetCurrentMethod();
            OracleDataReader Idr = null;
            OracleCommand cmd = new OracleCommand();
            try
            {
                if (creaConexion())
                {
                    cmd.Connection = conexionBD;
                    CargarParEntCmd(tipoSentencia, ref cmd, false, false, strQuery, lstParameters);
                    Idr = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {

                CerrarConexionesCmd(ref cmd);
                if (Idr != null && !Idr.IsClosed)
                {
                    Idr.Close();
                    Idr.Dispose();
                }
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                //throw new Exception(strError += "| " + ex.InnerException.ToString());
            }
            return Idr;

        }

        public static bool EjecutarCrud(string tipoSentencia, string strQuery, ref Dictionary<string, object> lstParametersOut,
                                        List<Ent_ParametrosDAL> lstParameters = null, bool blnUsarPRetorno = true)
        {
            method = MethodBase.GetCurrentMethod();

            int retorno = -1;
            bool resultado = false;
            if (creaConexion())
            {
                OracleCommand cmd = conexionBD.CreateCommand(); // new OracleCommand();
                OracleTransaction tran;
                try
                {
                    //Carga parámetros de entrada para el cmd...

                    tran = conexionBD.BeginTransaction(IsolationLevel.ReadCommitted);
                    CargarParEntCmd(tipoSentencia, ref cmd, false, false, strQuery, lstParameters);

                    cmd.Transaction = tran;

                    //cmd.Connection = conexionBD;

                    retorno = cmd.ExecuteNonQuery();

                    //Carga y retorna los parámetros de salida...
                    if (blnUsarPRetorno)
                    {
                        if (cmd.Parameters["@p_Int_Resultado"].Value.ToString() != string.Empty)
                        { retorno = Convert.ToInt32(cmd.Parameters["@p_Int_Resultado"].Value); }
                    }
                    if (retorno > 0)
                    {
                        tran.Commit();
                        resultado = true;
                    }
                }
                catch (Exception ex)
                {
                    //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                    throw ex;
                }
                finally
                {
                    CerrarConexionesCmd(ref cmd);
                }
            }
            return resultado;
        }


        public static DataTable EjecutarCons(string tipoSentencia, string strQuery, ref Dictionary<string, object> lstParametersOut,
                                        List<Ent_ParametrosDAL> lstParameters = null, bool blnUsarPRetorno = true)
        {
            method = MethodBase.GetCurrentMethod();
            DataTable resultado = new DataTable();
            if (creaConexion())
            {
                OracleCommand cmd = new OracleCommand();

                try
                {
                    //Carga parámetros de entrada para el cmd...
                    CargarParEntCmd(tipoSentencia, ref cmd, false, false, strQuery, lstParameters);
                    cmd.Connection = conexionBD;
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(resultado); //. = (DataTable)cmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    //log.escribirError(ex.Message + " [" + strQuery + "]", stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                    throw ex;
                }
                finally
                {
                    CerrarConexionesCmd(ref cmd);
                }
            }
            return resultado;
        }



        protected static void CargarParEntCmd(string tipoSentencia, ref OracleCommand cmd, bool blnUsarCursor, bool blnUsarPRetorno,
                                           string strQuery, List<Ent_ParametrosDAL> lstParameters)
        {
            if (tipoSentencia.Trim().ToUpperInvariant().Equals("TEXT")) { cmd.CommandType = CommandType.Text; }
            else if (tipoSentencia.Trim().ToUpperInvariant().Equals("STOREDPROCEDURE")) { cmd.CommandType = CommandType.StoredProcedure; }
            else if (tipoSentencia.Trim().ToUpperInvariant().Equals("NORETORNO")) { cmd.CommandType = CommandType.Text; }

            cmd.CommandText = strQuery;

            agregaParametrosCommand(ref lstParameters, ref cmd);
        }


        protected static void agregaParametrosCommand(ref List<Ent_ParametrosDAL> lstParameters, ref OracleCommand cmd)
        {
            method = MethodBase.GetCurrentMethod();
            ParameterDirection parSalida = ParameterDirection.Output;
            ParameterDirection parEntrada = ParameterDirection.Input;

            try
            {
                //Agregar parámetros de cliente...
                if (lstParameters != null)
                {
                    foreach (Ent_ParametrosDAL par in lstParameters)
                    {
                        //Verificar si es parámetro de entrada...
                        if (par.ParDirection == parEntrada)
                        {
                            cmd.Parameters.Add(par.Name, GetDbType(par.Type)).Value = par.Value;
                        }
                        else if (par.ParDirection == parSalida)
                        {
                            //Verificar si es string para incluir la longitud...
                            if (par.Type == DbType.String)
                            {
                                //cmd.Parameters.Add(par.Name, UTILIDADES.DataBase.GetDbType(par.Type), 1000).Direction = parSalida;
                            }
                            else
                            {
                                cmd.Parameters.Add(par.Name, GetDbType(par.Type)).Direction = parSalida;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name); 
            }
        }

        public static OracleType GetDbType(object o)
        {
            method = MethodBase.GetCurrentMethod();
            try
            {
                if (o.ToString().Equals(System.Data.DbType.String.ToString())) return OracleType.VarChar;
                // if (o.ToString().Equals(System.Data.DbType.DateTime.ToString())) return DbType.DateTime;
                if (o.ToString().Equals(System.Data.DbType.Int64.ToString())) return OracleType.Int32;
                if (o.ToString().Equals(System.Data.DbType.Int32.ToString())) return OracleType.Int32;
                if (o.ToString().Equals(System.Data.DbType.Int16.ToString())) return OracleType.Int16;
                if (o.ToString().Equals(System.Data.DbType.Decimal.ToString())) return OracleType.Number;
                if (o.ToString().Equals(System.Data.DbType.Byte.ToString())) return OracleType.Byte;
                if (o.ToString().Equals(System.Data.DbType.Double.ToString())) return OracleType.Double;

            }
            catch (Exception e)
            {
                //log.escribirError(e.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                throw e;
            }

            return OracleType.VarChar;
        }

        
        
        
        // En caso de usar con DataAccess Client de Oracle, descomentariar
        //public static OracleDbType GetDbType(object o)
        //{
        //    try
        //    {
        //        if (o.ToString().Equals(System.Data.DbType.String.ToString())) return Oracle.DataAccess.Client.OracleDbType.Varchar2;
        //        // if (o.ToString().Equals(System.Data.DbType.DateTime.ToString())) return DbType.DateTime;
        //        if (o.ToString().Equals(System.Data.DbType.Int64.ToString())) return OracleDbType.Int64;
        //        if (o.ToString().Equals(System.Data.DbType.Int32.ToString())) return OracleDbType.Int32;
        //        if (o.ToString().Equals(System.Data.DbType.Int16.ToString())) return OracleDbType.Int16;
        //        //if (o.ToString().Equals(System.Data.DbType.Byte.ToString())) return DbType.Byte;
        //        //if (o.ToString().Equals(System.Data.DbType.Double.ToString())) return DbType.Double;
        //        //if (o.ToString().Equals(System.Data.DbType.Decimal.ToString())) return DbType.Decimal;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return OracleDbType.Varchar2;
        //}


        #endregion
    }

}

