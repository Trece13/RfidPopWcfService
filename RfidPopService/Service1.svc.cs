using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using Utilities;
using System.Threading.Tasks;
using Utilities;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using Utilities;
using System.Web.Configuration;

namespace RfidPopService
{
    public class Service1 : IService1
    {
        private static MethodBase method;
        private static Recursos recursos = new Recursos();
        private static Dictionary<string, object> parametersOut = new Dictionary<string, object>();
        private static Dictionary<string, object> paramList = new Dictionary<string, object>();
        private static String env = " ";
        private static String owner = " ";
        private static string tabla = "x";
        private static LogGenerator log = new LogGenerator();
        string strSentencia = string.Empty; 

        public string ProWhcol133(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU)
        {
            try
            {
                string query = string.Format(@"SELECT TOP 1 [PAID],[RFID],[EVNT],[ORNO],[DATE],[LOGN],[PROC]
                                    FROM [RFID_RV].[dbo].[whcol133] 
                                    WHERE [PAID] = ''
                                    AND [PROC]='No' 
                                    AND [EVNT] 
                                    LIKE '%{0}%' 
                                    ORDER BY [DATE] DESC", EVNT);

                DataTable dtSS = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(query);
                if (dtSS.Rows.Count > 0)
                {

                    dtSS.Rows[0]["RFID"].ToString();
                    dtSS.Rows[0]["EVNT"].ToString();
                    string query2 = "UPDATE [RFID_RV].[dbo].[whcol133] set " +
                                    "[PAID]='" + PAID + "'," +
                                    "[ORNO]='" + ORNO + "'," +
                                    "[PROC]='Si' " +
                                    "WHERE RFID = '" + dtSS.Rows[0]["RFID"].ToString().Trim() + "'" +
                                    "AND [PROC] ='No'";
                    DataTable dtSS2 = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(query2);
                    ProWhcol133Ora(PAID, dtSS.Rows[0]["RFID"].ToString(), dtSS.Rows[0]["EVNT"].ToString(), ORNO, dtSS.Rows[0]["LOGN"].ToString(), "Si");
                    return "ejecucion Ok" + strSentencia;
                }
                else
                {
                    return "Rfids no disponibles";
                }
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia + " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
        }

        public bool ProWhcol133Ora(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC)
        {
            try
            {
                method = MethodBase.GetCurrentMethod();
                paramList = new Dictionary<string, object>();
                paramList.Add(":T$PAID", PAID);
                paramList.Add(":T$EVNT", EVNT);
                paramList.Add(":T$RFID", RFID);
                paramList.Add(":T$ORNO", ORNO);
                paramList.Add(":T$LOGN", LOGN);
                paramList.Add(":T$PROC", PROC == "Si" ? "1" : "0");

                strSentencia = "INSERT INTO BAAN.TWHCOL133140 VALUES('" + PAID.Trim() + "','" + RFID.Trim() + "','" + EVNT.Trim() + "','" + ORNO + "',(SYSDATE +5/24),'" + LOGN.Trim() + "','1','0','0')";
                log.Write(strSentencia,
                   AppDomain.CurrentDomain.FriendlyName,
                   MethodBase.GetCurrentMethod().Name,
                   false);
                Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return true;
        }

        public bool InsertTicol080Ora(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG)
        {
            bool res = false;
            try
            {
                strSentencia = "INSERT INTO BAAN.TTICOL080140 VALUES ('" + ORNO + "', '" + PONO + "', '" + ITEM + "', '" + CWAR + "', '" + QUNE + "', '" + LOGN + "', sysdate+5/24, '" + PROC + "', '" + CLOT + "', '0', '0', sysdate+5/24, '" + PICK + "', '" + OORG + "')";
                log.Write(strSentencia,AppDomain.CurrentDomain.FriendlyName,MethodBase.GetCurrentMethod().Name,false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool InsertTticol020(string PDNO, string MITM, string DSCA, string QTDL, string CUNI, string DATE, string MESS, string USER, string REFCNTD, string REDCNTU)
        {

            bool res = false;
            try
            {
                string strSentencia = "INSERT INTO BAAN.Tticol020140 VALUES('" + PDNO + "','" + MITM + "','" + DSCA + "','" + QTDL + "','" + CUNI + "',sysdate+5/24,'" + MESS + "','" + USER + "','" + REFCNTD + "','" + REDCNTU + "')";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool InsertTticol022(string PDNO, string SQNB, string PROC, string LOGN, string DATE, string MITM, string QTDL, string CUNI, string LOG1, string DATC, string QTD1, string PRO1, string LOG2, string DATU, string QTD2, string PRO2, string LOCA, string NORP, string DLRP, string DELE, string LOGD, string DATD, string REFCNTD, string REFCNTU)
        {

            bool res = false;
            try
            {
                string strSentencia = "INSERT INTO BAAN.Tticol022140 VALUES('" + PDNO + "','" + SQNB + "', '" + PROC + "', '" + LOGN + "',sysdate+5/24, '" + MITM + "', '" + QTDL + "', '" + CUNI + "', '" + LOG1 + "',sysdate+5/24, '" + QTD1 + "', '" + PRO1 + "', '" + LOG2 + "',sysdate+5/24, '" + QTD2 + "', '" + PRO2 + "', '" + LOCA + "', '" + NORP + "',sysdate+5/24, '" + DELE + "', '" + LOGD + "',sysdate+5/24, '" + REFCNTD + "', '" + REFCNTU + "')";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool InsertTticol222(string PDNO, string SQNB, string DRPT, string URPT, string ACQT, string CWAF, string CWAT, string ACLO, string REFCNTD, string REFCNTU, string ALLO)
        {
            bool res = false;
            try
            {
                string strSentencia = "INSERT INTO BAAN.Tticol222140 VALUES('" + PDNO + "', '" + SQNB + "', sysdate+5/24, '" + URPT + "', '" + ACQT + "', '" + CWAF + "', '" + CWAT + "', '" + ACLO + "', '" + REFCNTD + "', '" + REFCNTU + "', '0')";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool InsertTticol025(string PDNO, string SQNB, string MITM, string DSCA, string QTDL, string CUNI, string DATE, string MESS, string USER, string REFCNTD, string REFCNTU)
        {

            bool res = false;
            try
            {
                string strSentencia = "INSERT INTO BAAN.TTICOL025140 VALUES('" + PDNO + "', NVL((SELECT MAX(T$SQNB)+1 SQNB FROM BAAN.TTICOL025140),'1'), '" + MITM + "', '" + DSCA + "', '" + QTDL + "', '" + CUNI + "', SYSDATE+5/24, ' ',' ', '" + REFCNTD + "', '" + REFCNTU + "')";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool Update133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU)
        {
            try
            {
                strSentencia = "UPDATE [RFID_RV].[dbo].[whcol133] set " +
                                "[PAID]='" + PAID + "'," +
                                "[ORNO]='" + ORNO + "'," +
                                "[PROC]='Si' " +
                                "WHERE RFID = '" + RFID.Trim() + "'" +
                                "AND EVNT like '" + EVNT.Trim() + "%'" +
                                "AND [PROC] ='No'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DataTable dtSS2 = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(strSentencia);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return true;
        }

        public bool UpdateWhcol131(string PAID, string QTYA, string STAT)
        {
            bool res = false;
            try
            {
                strSentencia = "UPDATE BAAN.TWHCOL131140 SET T$QTYA = '" + QTYA + "', T$STAT = '" + STAT + "' WHERE T$PAID = '" + PAID + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,

                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool UpdateTticol022(string SQNB, string DATC, string DELE, string QTD1, string PRO1)
        {

            bool res = false;
            try
            {
                strSentencia = "UPDATE BAAN.TTICOL022140 SET T$DATU=sysdate+5/24,T$QTD1='" + QTD1 + "',T$DELE='" + DELE + "',T$PRO1='" + PRO1 + "' WHERE T$SQNB = '" + SQNB + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);                
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool UpdateTticol222(string SQNB, string CWAT, string ACLO)
        {

            bool res = false;
            try
            {
                strSentencia = "UPDATE  BAAN.TTICOL222140 SET T$CWAT='" + CWAT + "',T$ACLO='" + ACLO + "' WHERE T$SQNB = '" + SQNB + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool UpdateQuneTicol80Ora(string ORNO, string PONO, string CLOT, string QUNE)
        {
            bool res = false;
            try
            {
                strSentencia = "UPDATE BAAN.TTICOL080140 set T$QUNE = T$QUNE + '" + QUNE.Trim() + "' where T$ORNO = '" + ORNO + "' AND T$PONO = '" + PONO + "' AND T$CLOT = '" + CLOT + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);                
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public bool insertTicol083(string ORNOO, string ORNO, string POST, string ADVS, string ITEM, string QTYP, string UNIT, string CWAR, string MCNO, string DATE, string PRIO, string PICK, string PAID, string LOCA, string USER, string STAT)
        {
            bool res = false;
            try
            {
                strSentencia = "INSERT INTO BAAN.TTICOL083140 VALUES ('" + ORNOO + "','" + ORNO + "','" + POST + "','" + ADVS + "','" + ITEM + "','" + QTYP + "','" + UNIT + "','" + CWAR + "','" + MCNO + "',sysdate +5/24 ,'" + PRIO + "','" + PICK + "','" + PAID + "','" + LOCA + "','" + USER + "','" + STAT + "','0','0')";

                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCrud("Text", strSentencia, ref parametersOut, null, false);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString() + " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }


        
        public bool Updtwhcol133RfidSS(string PAID, string RFID)
        {
            try
            {
                strSentencia = "UPDATE [RFID_RV].[dbo].[whcol133] set " +
                                "RFID = '" + RFID.Trim() + "'" +
                                "WHERE [PAID]='" + PAID + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DataTable dtSS2 = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(strSentencia);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return true;
        }

        public DataTable SelectWhcol133Oss(string RFID, string EVNT)
        {
            DataTable dtss = new DataTable();
            try
            {
                strSentencia = string.Format(@"SELECT TOP 1 [PAID],[RFID],[EVNT],[ORNO],[DATE],[LOGN],[PROC]
                                    FROM [RFID_RV].[dbo].[whcol133] 
                                    WHERE [PAID] = ''
                                    AND [RFID] = '{0}'
                                    AND [PROC]='No' 
                                    AND [EVNT] 
                                    LIKE '%{1}%' 
                                    ORDER BY [DATE] DESC", RFID, EVNT);
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                dtss = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(strSentencia);
                dtss.TableName = "aaaa";
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return dtss;
        }

        public DataTable SelectWhcol133OPaidAssing(string RFID, string EVNT)
        {
            DataTable dtss = new DataTable();
            try
            {
                strSentencia = string.Format(@"SELECT TOP 1 [PAID],[RFID],[EVNT],[ORNO],[DATE],[LOGN],[PROC]
                                    FROM [RFID_RV].[dbo].[whcol133] 
                                    WHERE [PAID] != ''
                                    AND [RFID] = '{0}'
                                    AND [EVNT] 
                                    LIKE '%{1}%' 
                                    ORDER BY [DATE] DESC", RFID, EVNT);
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                dtss = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(strSentencia);
                dtss.TableName = "aaaa";
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString() + " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return dtss;
        }

        public DataTable SelectWhcol131Ora(string PAID)
        {
            DataTable DTORA = new DataTable();
            try
            {
                string strSentencia = "SELECT * FROM BAAN.TWHCOL131140 WHERE T$PAID = '" + PAID.Trim() + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DTORA = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return DTORA;
        }

        public DataTable SelectTicol011McnoOra(string MCNO, string STAT)
        {
            DataTable DT011 = new DataTable();
            try
            {
                string strSentencia = "SELECT * FROM BAAN.TTICOL011140 WHERE T$MCNO = '" + MCNO + "' AND T$STAT = '" + STAT + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DT011 = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return DT011;
        }

        public DataTable SelectTicst001PdnoOra(string PDNO, string SITM)
        {
            DataTable DT001 = new DataTable();
            try
            {
                string strSentencia = "SELECT * FROM BAAN.TTICST001140 WHERE T$PDNO = '" + PDNO + "' AND T$SITM LIKE '%" + SITM + "%'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DT001 = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return DT001;
        }

        public DataTable SelectWhcol133Evnt(string RFID, string EVNT, string PROC = "Si")
        {
            DataTable dtss = new DataTable();
            try
            {
                strSentencia = string.Format(@"SELECT TOP 1 [PAID],[RFID],[EVNT],[ORNO],[DATE],[LOGN],[PROC]
                                    FROM [RFID_RV].[dbo].[whcol133] 
                                    WHERE [RFID] = trim('{0}')
                                    AND [PROC]= '{2}' 
                                    AND [EVNT] 
                                    LIKE '%{1}%' 
                                    ORDER BY [DATE] DESC", RFID, EVNT, PROC);
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                dtss = Dal.BaseDALSS.BaseDalSS.EjecutarCrud(strSentencia);
                dtss.TableName = "whcol133";
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return dtss;
        }

        public DataTable SelectTcibd001(string ITEM)
        {

            DataTable res = new DataTable();
            try
            {
                string strSentencia = "select * from BAAN.TTCIBD001140 WHERE TRIM(T$ITEM) = '" + ITEM.Trim() + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public DataTable SelectTicol022InnerTicol222(string SQNB)
        {

            DataTable res = new DataTable();
            try
            {
                string strSentencia = "SELECT * FROM BAAN.TTICOL022140 COL022 INNER JOIN BAAN.TTICOL222140 COL222 ON COL222.T$SQNB = COL022.T$SQNB WHERE COL022.T$SQNB = '" + SQNB + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);                
                res = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public DataTable MaxSecuenceTticol022(string PDNO)
        {

            DataTable res = new DataTable();
            try
            {
                string strSentencia = "SELECT T$SQNB AS SQNB, SYSDATE AS FECHA, T$DELE AS DELE FROM baan.tticol022140 WHERE T$PDNO = '" + PDNO + "' AND T$SQNB = (SELECT MAX(T$SQNB) FROM baan.tticol022140 WHERE T$PDNO = '" + PDNO + "' AND REGEXP_LIKE(SUBSTR(T$SQNB,11,1), '[^A-Za-z]'))";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);               
                res = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public DataTable SelectTisfc001(string PDNO)
        {

            DataTable res = new DataTable();
            try
            {
                string strSentencia = "SELECT * FROM BAAN.TTISFC001140 WHERE T$PDNO ='" + PDNO + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public DataTable SelecTwhwmd300(string CWAR)
        {

            DataTable res = new DataTable();
            try
            {
                string strSentencia = string.Format(@"SELECT T$LOCA
                                    FROM BAAN.TWHWMD300140
                                    WHERE T$CWAR = '{0}'
                                    AND T$PRIO = (SELECT MIN(T$PRIO)
                                    FROM BAAN.TWHWMD300140
                                    WHERE T$CWAR = '{0}')", CWAR);
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                res = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return res;
        }

        public DataTable VerifyTicol80Ora(string ORNO, string PONO, string CLOT)
        {
            DataTable DT080 = new DataTable();
            try
            {
                strSentencia = "SELECT * FROM BAAN.TTICOL080140 WHERE T$ORNO = '" + ORNO + "' AND T$PONO = '" + PONO + "' AND T$CLOT = '" + CLOT.Trim() + "'";
                log.Write(strSentencia, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                DT080 = Dal.BaseDAL.BaseDal.EjecutarCons("Text", strSentencia, ref parametersOut, null, true);
            }
            catch (Exception e)
            {
                log.Write(e.Message.ToString()+ " " + strSentencia,
                    AppDomain.CurrentDomain.FriendlyName,
                    MethodBase.GetCurrentMethod().Name,
                    true);
                throw new Exception("¡ERROR!", e);
            }
            return DT080;
        }

        public void InitProcRfid(string RFID, string EVNT, string LOGN, string PROC)
        {
            try
            {
                DataTable Dt133 = SelectWhcol133Evnt(RFID.Trim(), "VA Dock");
                if (Dt133.Rows.Count > 0)
                {
                    log.Write("Dt133.Rows.Count = " + Dt133.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                    DataTable Dt131 = SelectWhcol131Ora(Dt133.Rows[0]["PAID"].ToString());
                    if (Dt131.Rows.Count > 0)
                    {
                        log.Write("Dt131.Rows.Count = " + Dt131.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        DataTable Dt011 = SelectTicol011McnoOra(EVNT.Substring(0, 4), "2");
                        if (Dt011.Rows.Count > 0)
                        {
                            log.Write("Dt011.Rows.Count = " + Dt011.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                            DataTable Dt001 = SelectTicst001PdnoOra(Dt011.Rows[0]["T$PDNO"].ToString(), Dt131.Rows[0]["T$ITEM"].ToString());
                            if (Dt001.Rows.Count > 0)
                            {
                                log.Write("Dt001.Rows.Count = " + Dt001.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                DataTable dt080 = VerifyTicol80Ora(Dt011.Rows[0]["T$PDNO"].ToString(), Dt001.Rows[0]["T$PONO"].ToString(), Dt131.Rows[0]["T$CLOT"].ToString());
                                if (dt080.Rows.Count > 0)
                                {
                                    log.Write("t080.Rows.Count > 0 == true", AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                    log.Write("dt080.Rows.Count = " + dt080.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                    bool upd080 = UpdateQuneTicol80Ora(Dt011.Rows[0]["T$PDNO"].ToString(), Dt001.Rows[0]["T$PONO"].ToString(), Dt131.Rows[0]["T$CLOT"].ToString(), Dt131.Rows[0]["T$QTYA"].ToString());
                                    if (upd080)
                                    {
                                        log.Write("upd080 == " + upd080, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                        bool upd133ss = Update133ss(Dt131.Rows[0]["T$PAID"].ToString(), RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), "", "", "", "", "");
                                        if (upd133ss)
                                        {
                                            log.Write("upd133ss == " + upd133ss, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                            bool upd133Ora = ProWhcol133Ora(Dt131.Rows[0]["T$PAID"].ToString(), RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), LOGN, "1");
                                            if (upd133Ora)
                                            {
                                                log.Write("upd133Ora == " + upd133Ora, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                bool upd131 = UpdateWhcol131(Dt131.Rows[0]["T$PAID"].ToString(), "0", "9");
                                                log.Write("upd131 == " + upd131, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                if (upd131)
                                                {
                                                    bool ins083 = insertTicol083("4", Dt011.Rows[0]["T$PDNO"].ToString(), Dt001.Rows[0]["T$PONO"].ToString(), "0", Dt131.Rows[0]["T$ITEM"].ToString(), "1", "UN", Dt001.Rows[0]["T$CWAR"].ToString(), "TE09", "", "0", "2", Dt133.Rows[0]["PAID"].ToString(), " ", " ", "4");
                                                    log.Write("ins083 == " + ins083, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    log.Write("t080.Rows.Count > 0 == false", AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                    bool ins080 = InsertTicol080Ora(Dt011.Rows[0]["T$PDNO"].ToString(), Dt001.Rows[0]["T$PONO"].ToString(), Dt001.Rows[0]["T$SITM"].ToString(), Dt001.Rows[0]["T$CWAR"].ToString(), Dt131.Rows[0]["T$QTYA"].ToString(), " ", " ", "2", Dt131.Rows[0]["T$CLOT"].ToString(), "0", "0", " ", "2", "4");
                                    if (ins080)
                                    {
                                        log.Write("ins080 == " + ins080, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                        bool upd133ss = Update133ss(Dt131.Rows[0]["T$PAID"].ToString(), RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), "", "", "", "", "");
                                        if (upd133ss)
                                        {
                                            log.Write("upd133ss == " + upd133ss, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                            bool upd133Ora = ProWhcol133Ora(Dt131.Rows[0]["T$PAID"].ToString(), RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), LOGN, "1");
                                            if (upd133Ora)
                                            {
                                                log.Write("upd133Ora == " + upd133Ora, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                bool upd131 = UpdateWhcol131(Dt131.Rows[0]["T$PAID"].ToString(), "0", "9");
                                                log.Write("upd131 == " + upd131, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                if (upd131)
                                                {
                                                    bool ins083 = insertTicol083("4", Dt011.Rows[0]["T$PDNO"].ToString(), Dt001.Rows[0]["T$PONO"].ToString(), "0", Dt131.Rows[0]["T$ITEM"].ToString(), "1", "UN", Dt001.Rows[0]["T$CWAR"].ToString(), "TE09", "", "0", "2", Dt133.Rows[0]["PAID"].ToString(), " ", " ", "4");
                                                    log.Write("ins083 == " + ins083, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Write(string.Format("¡ERROR! ", e.Message), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, true);
            }


        }

        public void InitProcRfid022(string RFID, string EVNT, string LOGN, string PROC)
        {
            try
            {
                DataTable Dt011 = SelectTicol011McnoOra(EVNT.Substring(0, 4), "2");
                DataTable Dt133 = SelectWhcol133Evnt(RFID.Trim(), EVNT.Trim(), "No");
                if (Dt011.Rows.Count > 0)
                {
                    log.Write("upd131 == " + Dt011.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                    DataTable Dt022Max = MaxSecuenceTticol022(Dt011.Rows[0]["T$PDNO"].ToString().Trim());
                    DataTable Dtsfc001 = SelectTisfc001(Dt011.Rows[0]["T$PDNO"].ToString().Trim());

                    string NewMaxSec = string.Empty;
                    if (Dt022Max.Rows.Count > 0)
                    {
                        log.Write(string.Format("Dt022Max.Rows.Count > 0 == True"), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        log.Write("Dt022Max == " + Dt022Max.Rows.Count, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        string strMaxSec = Dt022Max.Rows[0]["SQNB"].ToString().Trim().Substring(Dt022Max.Rows[0]["SQNB"].ToString().Trim().IndexOf('-') + 1);
                        int MaxSec = Convert.ToInt32(strMaxSec) + 1;
                        log.Write("strMaxSec == " + strMaxSec + "MaxSec == " + MaxSec, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        if (MaxSec < 10)
                        {
                            NewMaxSec = "00" + (Convert.ToInt32(strMaxSec) + 1).ToString();
                        }
                        else if (MaxSec < 100)
                        {
                            NewMaxSec = "0" + (Convert.ToInt32(strMaxSec) + 1).ToString();
                        }
                        else if (MaxSec < 1000)
                        {
                            NewMaxSec = (Convert.ToInt32(strMaxSec) + 1).ToString();
                        }
                        log.Write("NewMaxSec == " + NewMaxSec + "MaxSec == " + MaxSec, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        bool I020 = InsertTticol020(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dtsfc001.Rows[0]["T$MITM"].ToString(), SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$DSCA"].ToString(), "1", SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$CUNI"].ToString(), "DATE", " ", " ", "0", "0");
                        bool I022 = InsertTticol022(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSec, "1", " ", "DATE", Dtsfc001.Rows[0]["T$MITM"].ToString(), "1", SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$CUNI"].ToString(), " ", "DATC", "0", "0", " ", "01/01/70", "0", "2", " ", "1", "01/01/70", "2", " ", "01/01/70", "0", "0");
                        bool I222 = InsertTticol222(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSec, "DRPT", " ", "1", Dtsfc001.Rows[0]["T$CWAR"].ToString(), " ", " ", "0", "0", " ");
                        log.Write(string.Format("I020 =={0};I022=={1};I222=={2}:", I020, I022, I222) + MaxSec, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        if (I020 && I022 && I222)
                        {
                            bool upd133ss = Update133ss(Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSec, RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), "", "", "", "", "");
                            if (upd133ss)
                            {
                                log.Write("upd133ss == " + upd133ss, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                bool upd133Ora = ProWhcol133Ora(Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSec, RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), LOGN, "1");
                                log.Write("upd133Ora == " + upd133Ora, AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                            }
                        }
                    }
                    else
                    {
                        log.Write(string.Format("Dt022Max.Rows.Count > 0 == True"), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        string NewMaxSecZero = "001";
                        log.Write(string.Format("NewMaxSecZero == {0}" + NewMaxSecZero), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        bool I020 = InsertTticol020(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dtsfc001.Rows[0]["T$MITM"].ToString(), SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$DSCA"].ToString(), "1", SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$CUNI"].ToString(), "DATE", " ", " ", "0", "0");
                        bool I022 = InsertTticol022(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSecZero, "1", " ", "DATE", Dtsfc001.Rows[0]["T$MITM"].ToString(), "1", SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$CUNI"].ToString(), " ", "DATC", "0", "0", " ", "01/01/70", "0", "2", " ", "1", "01/01/70", "2", " ", "01/01/70", "0", "0");
                        bool I222 = InsertTticol222(Dt011.Rows[0]["T$PDNO"].ToString().Trim(), Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSecZero, "DRPT", " ", "1", Dtsfc001.Rows[0]["T$CWAR"].ToString(), " ", " ", "0", "0", " ");
                        log.Write(string.Format("I020 =={0};I022=={1};I222=={2}:", I020, I022, I222), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        if (I020 && I022 && I222)
                        {
                            bool upd133ss = Update133ss(Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSecZero, RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), "", "", "", "", "");

                            if (upd133ss)
                            {
                                log.Write(string.Format("upd133ss == {0};", upd133ss), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                bool upd133Ora = ProWhcol133Ora(Dt011.Rows[0]["T$PDNO"].ToString().Trim() + "-" + NewMaxSecZero, RFID, EVNT, Dt011.Rows[0]["T$PDNO"].ToString(), LOGN, "1");
                                log.Write(string.Format("upd133Ora == {0};", upd133Ora), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Write(string.Format("¡ERROR! ", e.Message), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, true);
            }

        }

        public void InitProcRfid025(string RFID, string EVNT, string LOGN, string PROC, string PRINT = "")
        {
            try
            {
                DataTable col133 = SelectWhcol133Evnt(RFID, "TE10 Outbound");
                if (col133.Rows.Count > 0)
                {
                    log.Write(string.Format("col133.Rows.Count == {0};", col133.Rows.Count), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                    DataTable Dtsfc001 = SelectTisfc001(col133.Rows[0]["ORNO"].ToString());
                    bool i025 = InsertTticol025(col133.Rows[0]["ORNO"].ToString(), col133.Rows[0]["PAID"].ToString(), Dtsfc001.Rows[0]["T$MITM"].ToString(), SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$DSCA"].ToString(), "1", SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$CUNI"].ToString(), "DATE", "", "", "0", "0");
                    if (i025)
                    {
                        log.Write(string.Format("i025 == {0};", i025), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                        bool upd022 = UpdateTticol022(col133.Rows[0]["PAID"].ToString(), "DATC", "7", "1", "1");
                        if (upd022)
                        {
                            log.Write(string.Format("upd022 == {0};", upd022), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                            bool upd222 = UpdateTticol222(col133.Rows[0]["PAID"].ToString(), Dtsfc001.Rows[0]["T$CWAR"].ToString(), SelecTwhwmd300(Dtsfc001.Rows[0]["T$CWAR"].ToString()).Rows[0]["T$LOCA"].ToString());
                            if (upd222)
                            {
                                log.Write(string.Format("upd222 == {0};", upd222), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                bool upd133 = Update133ss(col133.Rows[0]["PAID"].ToString(), RFID,EVNT, col133.Rows[0]["ORNO"].ToString(), "", "", "", "", "");
                                if (upd133)
                                {
                                    log.Write(string.Format("upd133 == {0};", upd133), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                    bool upd133Ora = ProWhcol133Ora(col133.Rows[0]["PAID"].ToString(), RFID, EVNT, col133.Rows[0]["ORNO"].ToString(), LOGN, "1");
                                    if (upd133Ora)
                                    {
                                        log.Write(string.Format("upd133Ora == {0};", upd133), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://SUSAVGABART01:82/Integration/webServiceIntegration/Execute");
                                        httpWebRequest.ContentType = "application/json";
                                        httpWebRequest.Method = "POST";

                                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                        {
                                            string json = "{\"BarTenderLabel\": \"Label_Announce.btw\"," +
                                                "\"PrinterName\": \"" + (PRINT == string.Empty? WebConfigurationManager.AppSettings["CustomPrint"].ToString():PRINT) + "\"," +
                                                "\"DSCA\": \"" + SelectTcibd001(Dtsfc001.Rows[0]["T$MITM"].ToString()).Rows[0]["T$DSCA"].ToString() + "\"," +
                                                "\"PAID\": \"" + col133.Rows[0]["PAID"].ToString() + "\"," +
                                                "\"ORNO\": \"" + col133.Rows[0]["ORNO"].ToString() + "\"," +
                                                "\"ITEM\": \"" + Dtsfc001.Rows[0]["T$MITM"].ToString() + "\"," +
                                                "\"MCNO\": \"TE10\"," +
                                                "\"QTY\": \"1\"," +
                                                "\"UN\": \"CJ\"," +
                                                "\"DATE\": \"" + DateTime.Now.ToString() + "\"," +
                                                "\"USER\": \"INTERFACE\"}";

                                            streamWriter.Write(json);
                                            log.Write(string.Format(" json : {0};", json), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);
                                        }
                                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                        {
                                            var result = streamReader.ReadToEnd();
                                            log.Write(string.Format(" http request bartender service: result == {0};", result), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, false);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Write(string.Format("¡ERROR! ", e.Message), AppDomain.CurrentDomain.FriendlyName, MethodBase.GetCurrentMethod().Name, true);
            }
        }

    }
}


