using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Diagnostics;


namespace Dal.BaseDAL
{
    public class ComplexDataAccessImplements : Dal.BaseDAL.ComplexDataAccess
    {
        //private static Utilidades.Seguimiento log = new Seguimiento();
        private static StackTrace stackTrace = new StackTrace();
        private static MethodBase method;

    
        public static List<T> MapFromDataReaderI<T>(ref System.Data.SqlClient.SqlDataReader reader, bool closeReader)
        {
            method = MethodBase.GetCurrentMethod();
            List<T> retorno = null;
            try
            {
                ComplexDataAccessImplements cDAI = new ComplexDataAccessImplements();
                retorno = cDAI.MapFromDataReader<T>(ref reader, closeReader);
            }
            catch (Exception ex)
            {
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
            }
            return retorno;
        }

        public static List<T> MapFromDataReaderI<T>(DataTable reader, bool closeReader)
        {
            method = MethodBase.GetCurrentMethod();
            List<T> retorno = null;

            try
            {
                ComplexDataAccessImplements cDAI = new ComplexDataAccessImplements();
                retorno = cDAI.MapFromDataReader<T>(reader, closeReader);
            }
            catch (Exception ex)
            {
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
            }
            return retorno;
        }

    }
}
