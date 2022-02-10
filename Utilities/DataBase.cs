using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Utilities
{

    /// <summary>
    /// Date: 28-05-2016
    /// Class Name: DataBase
    /// Author: Edwing Loaiza
    /// Description: Contiene funciones relativas a base de datos...
    /// </summary>
    public static class DataBase
    {

  
        #region Utilidades arquitectura compleja

        public static object ConvertValueToType(object value, Type targetType)
        {
            if (value != DBNull.Value && value != null && value.ToString() != "")
            {
                Type valueType = value.GetType();
                if (targetType == valueType || targetType.IsAssignableFrom(valueType))
                    return value;
                else
                {
                    Type targetTypeTemp = targetType;
                    if (targetTypeTemp.IsNullable())
                    {
                        if (value.ToString().Trim().Length == 0)
                            return null;
                        targetTypeTemp = Nullable.GetUnderlyingType(targetType);
                        if (targetTypeTemp == valueType || targetTypeTemp.IsAssignableFrom(valueType))
                            return value;
                    }
                    if (targetTypeTemp.IsEnum)
                        return Enum.Parse(targetTypeTemp, value.ToString(), true);
                    else
                    {
                        try { return Convert.ChangeType(value, targetTypeTemp); }
                        catch { return Convert.ChangeType(value, targetTypeTemp, CultureInfo.InvariantCulture); }
                    }
                }
            }
            else
                return null;
        }

        public static T ConvertValueToType<T>(object value)
        {
            object returnValue = ConvertValueToType(value, typeof(T));
            if (returnValue == null)
                return default(T);
            return (T)returnValue;
        }

        public static string TrimToFieldLength(object value, int maxLength)
        {
            if (value != null)
            {
                string valueString = value.ToString().Trim();
                return valueString.Length > maxLength ? valueString.Substring(0, maxLength).Trim() : valueString;
            }
            else
                return null;
        }

        public static object EvaluateDBNullValue(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            else if (obj.ToString().Trim().Length == 0)
                return DBNull.Value;
            else if (obj is string)
                return obj.ToString().Trim();
            else return obj;
        }

        public static object EvaluateDBNullValue(object obj, int maxLength)
        {
            if (obj == null)
                return DBNull.Value;
            else if (obj.ToString().Trim().Length == 0)
                return DBNull.Value;
            else if (obj is string)
                return TrimToFieldLength_eval(obj, maxLength);
            else return obj;
        }

        public static string TrimToFieldLength_eval(object value, int maxLength)
        {
            if (value != null)
            {
                string valueString = value.ToString().Trim();
                return valueString.Length > maxLength ? valueString.Substring(0, maxLength).Trim() : valueString;
            }
            else
                return null;
        }
           
        #region extensiones 
        public static bool IsNullable(this Type t)
        {
            return t.Name.ToLower() == "nullable`1";
        }
        #endregion

        #endregion

    }
}
