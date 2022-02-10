using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Entities
{
    public class Ent_ParametrosDAL
    {
        public string Name { get; set; }
        public DbType Type { get; set; }
        public object Value { get; set; }
        public ParameterDirection ParDirection { get; set; }

        public static bool AgregaParametro(ref List<Ent_ParametrosDAL> lstParametros, string Name, DbType Type, object Value,
                                           ParameterDirection ParDirection = ParameterDirection.Input)
        {
            try
            {
                Ent_ParametrosDAL pDal = new Ent_ParametrosDAL();
                pDal.Name = Name;
                pDal.Type = Type;
                pDal.Value = Value;
                pDal.ParDirection = ParDirection;
                lstParametros.Add(pDal);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
