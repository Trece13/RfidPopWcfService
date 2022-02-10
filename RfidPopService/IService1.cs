using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace RfidPopService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string ProWhcol133(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);

        [OperationContract]
        bool ProWhcol133Ora(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC);

        [OperationContract]
        bool Update133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [OperationContract]
        DataTable SelectWhcol133Oss(string RFID, string EVNT);

        [OperationContract]
        DataTable SelectWhcol131Ora(string PAID);

        [OperationContract]
        DataTable SelectTicol011McnoOra(string MCNO, string STAT);

        [OperationContract]
        bool InsertTicol080Ora(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG);

        [OperationContract]
        DataTable SelectWhcol133Evnt(string RFID, string EVNT,string PROC);

        [OperationContract]
        bool UpdateWhcol131(string PAID, string QTYA, string STAT);

        [OperationContract]
        void InitProcRfid(string RFID, string EVNT,string LOGN, string PROC);

        [OperationContract]
        void InitProcRfid022(string RFID, string EVNT, string LOGN, string PROC);

        [OperationContract]
        void InitProcRfid025(string RFID, string EVNT, string LOGN, string PROC);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
