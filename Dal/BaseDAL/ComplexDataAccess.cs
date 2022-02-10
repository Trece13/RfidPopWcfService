using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Diagnostics;
using System.Reflection;

namespace Dal.BaseDAL
{
    public abstract class ComplexDataAccess
    {
        #region Protected members

        //private static Seguimiento log = new Seguimiento();
        private static StackTrace stackTrace = new StackTrace();

        private static MethodBase method;
        //private static string metodo;

        /// <summary>
        /// Properties Dictionary to map from database columns
        /// </summary>
        protected Dictionary<MemberInfo, string> _PropertiesToColumnsNames;

        /// <summary>
        /// Current object type working
        /// </summary>
        protected Type _currentType;

        /// <summary>
        /// Variable to indicate if working with inheritance
        /// </summary>
        protected bool WorkWithInheredMembers { get { return true; } }

        /// <summary>
        /// Delegates to search criteria.
        /// </summary>
        /// <param name="objMemberInfo">The obj member info.</param>
        /// <param name="objSearch">The obj search.</param>
        /// <returns></returns>
        private bool DelegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            object[] attributes = objMemberInfo.GetCustomAttributes(typeof(ColumnAttribute), WorkWithInheredMembers);
            if (attributes.Length > 0)
            {
                ColumnAttribute dbItemInfo = (ColumnAttribute)attributes[0];
                if (!_PropertiesToColumnsNames.ContainsKey(objMemberInfo))
                    _PropertiesToColumnsNames.Add(objMemberInfo, dbItemInfo.Name);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Finds the column names.
        /// </summary>
        /// <param name="type">The type.</param>
        protected void FindColumnNames(Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            if (!WorkWithInheredMembers)
                bindingFlags = bindingFlags | BindingFlags.DeclaredOnly;
            _PropertiesToColumnsNames = new Dictionary<MemberInfo, string>();
            type.FindMembers(MemberTypes.Property, bindingFlags, DelegateToSearchCriteria, null);

        }
                /// <summary>
        /// Converts the type of the value to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        protected object ConvertValueToType(object value, Type targetType)
        {
            return Utilities.DataBase.ConvertValueToType(value, targetType);
        }


        /// <summary>
        /// Maps from data reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="closeReader">if set to <c>true</c> [close reader].</param>
        /// <returns></returns>
        protected List<T> MapFromDataReader<T>(ref System.Data.SqlClient.SqlDataReader reader, bool closeReader)
        {
            method = MethodBase.GetCurrentMethod();
            try
            {
                Type newType = typeof(T);
                if (_currentType != newType)
                {
                    _currentType = newType;
                    FindColumnNames(_currentType);
                }

                List<T> returnList = new List<T>();
                object[] values = new object[reader.FieldCount];
                Dictionary<string, int> columnsIds = new Dictionary<string, int>(reader.FieldCount);
                List<string> schemaFieldsName = null;
                while (reader.Read())
                {
                    reader.GetValues(values);
                    T objTarget = Activator.CreateInstance<T>();
                    if (schemaFieldsName == null)
                        using (DataTable dtSchema = reader.GetSchemaTable())
                        {
                            schemaFieldsName = dtSchema.AsEnumerable().Select(x => x.Field<string>("ColumnName").ToUpper()).ToList();
                        }
                    foreach (MemberInfo member in _PropertiesToColumnsNames.Keys)
                    {
                        string columnName = _PropertiesToColumnsNames[member];
                        if (schemaFieldsName.Contains(columnName.ToUpper()))
                        {
                            int columnId = 0;
                            if (columnsIds.ContainsKey(columnName))
                                columnId = columnsIds[columnName];
                            else
                            {
                                columnId = reader.GetOrdinal(columnName);
                                columnsIds.Add(columnName, columnId);
                            }
                            PropertyInfo pMember = (PropertyInfo)member;
                            object value;
                            try
                            { value = ConvertValueToType(values[columnId], pMember.PropertyType); }
                            catch { value = values[columnId]; }
                            pMember.SetValue(objTarget, value, null);
                        }
                    }
                    returnList.Add(objTarget);
                }
                if (closeReader)
                    reader.Dispose();
                return returnList.Count > 0 ? returnList : null;
            }
            catch (Exception ex)
            {
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name);
                throw new Exception(ex.InnerException.ToString());
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
            }
        }

        protected List<T> MapFromDataReader<T>(DataTable lista, bool closeReader)
        {
            method = MethodBase.GetCurrentMethod();
            try
            {
                Type newType = typeof(T);
                if (_currentType != newType)
                {
                    _currentType = newType;
                    FindColumnNames(_currentType);
                }

                List<T> returnList = new List<T>();
                object[] values = new object[lista.Columns.Count];
                Dictionary<string, int> columnsIds = new Dictionary<string, int>(lista.Columns.Count);
                
                List<string> schemaFieldsName = null;
                schemaFieldsName = lista.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToUpper()).ToList();  

                foreach (DataRow reg in lista.Rows)
                {
                    values = reg.ItemArray;
                    T objTarget = Activator.CreateInstance<T>();
                   
                    foreach (MemberInfo member in _PropertiesToColumnsNames.Keys)
                    {
                        string columnName = _PropertiesToColumnsNames[member];
                        if (schemaFieldsName.Contains(columnName.ToUpper()))
                        {
                            int columnId = 0;
                            if (columnsIds.ContainsKey(columnName))
                                columnId = columnsIds[columnName];
                            else
                            {
                                columnId = reg.Table.Columns[columnName].Ordinal;
                                columnsIds.Add(columnName, columnId);
                            }

                            PropertyInfo pMember = (PropertyInfo)member;
                            object value;
                            try
                            {
                                value = ConvertValueToType(values[columnId], pMember.PropertyType); }
                            catch
                            {
                                value = values[columnId];
                            }
                            pMember.SetValue(objTarget, value, null);
                        }
                    }
                    returnList.Add(objTarget);

                }
                return returnList.Count > 0 ? returnList : null;
            }
            catch (Exception ex)
            {
                //log.escribirError(ex.Message, stackTrace.GetFrame(1).GetMethod().Name, method.Name, method.ReflectedType.Name); 
                throw new Exception(ex.InnerException.ToString());
            }
        }

 
        protected List<T> MapFromDataReader<T>(ref System.Data.SqlClient.SqlDataReader reader)
        {
            return MapFromDataReader<T>(ref reader, true);
        }

        protected List<T> MapFromDataReader<T>(DataTable reader)
        {
            return MapFromDataReader<T>(reader, true);
        }
        #endregion
    }
}
