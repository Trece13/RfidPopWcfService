﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RfidPopService.SrvRfidPop {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SrvRfidPop.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ProWhcol133", ReplyAction="http://tempuri.org/IService1/ProWhcol133Response")]
        string ProWhcol133(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ProWhcol133", ReplyAction="http://tempuri.org/IService1/ProWhcol133Response")]
        System.Threading.Tasks.Task<string> ProWhcol133Async(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ProWhcol133Ora", ReplyAction="http://tempuri.org/IService1/ProWhcol133OraResponse")]
        bool ProWhcol133Ora(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ProWhcol133Ora", ReplyAction="http://tempuri.org/IService1/ProWhcol133OraResponse")]
        System.Threading.Tasks.Task<bool> ProWhcol133OraAsync(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Update133ss", ReplyAction="http://tempuri.org/IService1/Update133ssResponse")]
        bool Update133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Update133ss", ReplyAction="http://tempuri.org/IService1/Update133ssResponse")]
        System.Threading.Tasks.Task<bool> Update133ssAsync(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133Oss", ReplyAction="http://tempuri.org/IService1/SelectWhcol133OssResponse")]
        System.Data.DataTable SelectWhcol133Oss(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133Oss", ReplyAction="http://tempuri.org/IService1/SelectWhcol133OssResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133OssAsync(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133ORfidss", ReplyAction="http://tempuri.org/IService1/SelectWhcol133ORfidssResponse")]
        System.Data.DataTable SelectWhcol133ORfidss(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133ORfidss", ReplyAction="http://tempuri.org/IService1/SelectWhcol133ORfidssResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133ORfidssAsync(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol131Ora", ReplyAction="http://tempuri.org/IService1/SelectWhcol131OraResponse")]
        System.Data.DataTable SelectWhcol131Ora(string PAID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol131Ora", ReplyAction="http://tempuri.org/IService1/SelectWhcol131OraResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol131OraAsync(string PAID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectTicol011McnoOra", ReplyAction="http://tempuri.org/IService1/SelectTicol011McnoOraResponse")]
        System.Data.DataTable SelectTicol011McnoOra(string MCNO, string STAT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectTicol011McnoOra", ReplyAction="http://tempuri.org/IService1/SelectTicol011McnoOraResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectTicol011McnoOraAsync(string MCNO, string STAT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InsertTicol080Ora", ReplyAction="http://tempuri.org/IService1/InsertTicol080OraResponse")]
        bool InsertTicol080Ora(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InsertTicol080Ora", ReplyAction="http://tempuri.org/IService1/InsertTicol080OraResponse")]
        System.Threading.Tasks.Task<bool> InsertTicol080OraAsync(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133Evnt", ReplyAction="http://tempuri.org/IService1/SelectWhcol133EvntResponse")]
        System.Data.DataTable SelectWhcol133Evnt(string RFID, string EVNT, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133Evnt", ReplyAction="http://tempuri.org/IService1/SelectWhcol133EvntResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133EvntAsync(string RFID, string EVNT, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UpdateWhcol131", ReplyAction="http://tempuri.org/IService1/UpdateWhcol131Response")]
        bool UpdateWhcol131(string PAID, string QTYA, string STAT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UpdateWhcol131", ReplyAction="http://tempuri.org/IService1/UpdateWhcol131Response")]
        System.Threading.Tasks.Task<bool> UpdateWhcol131Async(string PAID, string QTYA, string STAT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid", ReplyAction="http://tempuri.org/IService1/InitProcRfidResponse")]
        void InitProcRfid(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid", ReplyAction="http://tempuri.org/IService1/InitProcRfidResponse")]
        System.Threading.Tasks.Task InitProcRfidAsync(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid022", ReplyAction="http://tempuri.org/IService1/InitProcRfid022Response")]
        void InitProcRfid022(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid022", ReplyAction="http://tempuri.org/IService1/InitProcRfid022Response")]
        System.Threading.Tasks.Task InitProcRfid022Async(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid025", ReplyAction="http://tempuri.org/IService1/InitProcRfid025Response")]
        void InitProcRfid025(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/InitProcRfid025", ReplyAction="http://tempuri.org/IService1/InitProcRfid025Response")]
        System.Threading.Tasks.Task InitProcRfid025Async(string RFID, string EVNT, string LOGN, string PROC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Updtwhcol133RfidSS", ReplyAction="http://tempuri.org/IService1/Updtwhcol133RfidSSResponse")]
        bool Updtwhcol133RfidSS(string PAID, string RFID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Updtwhcol133RfidSS", ReplyAction="http://tempuri.org/IService1/Updtwhcol133RfidSSResponse")]
        System.Threading.Tasks.Task<bool> Updtwhcol133RfidSSAsync(string PAID, string RFID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133OPaidAssing", ReplyAction="http://tempuri.org/IService1/SelectWhcol133OPaidAssingResponse")]
        System.Data.DataTable SelectWhcol133OPaidAssing(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SelectWhcol133OPaidAssing", ReplyAction="http://tempuri.org/IService1/SelectWhcol133OPaidAssingResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133OPaidAssingAsync(string RFID, string EVNT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Insert133ss", ReplyAction="http://tempuri.org/IService1/Insert133ssResponse")]
        bool Insert133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Insert133ss", ReplyAction="http://tempuri.org/IService1/Insert133ssResponse")]
        System.Threading.Tasks.Task<bool> Insert133ssAsync(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : RfidPopService.SrvRfidPop.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<RfidPopService.SrvRfidPop.IService1>, RfidPopService.SrvRfidPop.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string ProWhcol133(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.ProWhcol133(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
        
        public System.Threading.Tasks.Task<string> ProWhcol133Async(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.ProWhcol133Async(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
        
        public bool ProWhcol133Ora(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC) {
            return base.Channel.ProWhcol133Ora(PAID, RFID, EVNT, ORNO, LOGN, PROC);
        }
        
        public System.Threading.Tasks.Task<bool> ProWhcol133OraAsync(string PAID, string RFID, string EVNT, string ORNO, string LOGN, string PROC) {
            return base.Channel.ProWhcol133OraAsync(PAID, RFID, EVNT, ORNO, LOGN, PROC);
        }
        
        public bool Update133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.Update133ss(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
        
        public System.Threading.Tasks.Task<bool> Update133ssAsync(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.Update133ssAsync(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
        
        public System.Data.DataTable SelectWhcol133Oss(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133Oss(RFID, EVNT);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133OssAsync(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133OssAsync(RFID, EVNT);
        }
        
        public System.Data.DataTable SelectWhcol133ORfidss(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133ORfidss(RFID, EVNT);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133ORfidssAsync(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133ORfidssAsync(RFID, EVNT);
        }
        
        public System.Data.DataTable SelectWhcol131Ora(string PAID) {
            return base.Channel.SelectWhcol131Ora(PAID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol131OraAsync(string PAID) {
            return base.Channel.SelectWhcol131OraAsync(PAID);
        }
        
        public System.Data.DataTable SelectTicol011McnoOra(string MCNO, string STAT) {
            return base.Channel.SelectTicol011McnoOra(MCNO, STAT);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectTicol011McnoOraAsync(string MCNO, string STAT) {
            return base.Channel.SelectTicol011McnoOraAsync(MCNO, STAT);
        }
        
        public bool InsertTicol080Ora(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG) {
            return base.Channel.InsertTicol080Ora(ORNO, PONO, ITEM, CWAR, QUNE, LOGN, DATE, PROC, CLOT, REFCNTD, REFCNTU, PDAT, PICK, OORG);
        }
        
        public System.Threading.Tasks.Task<bool> InsertTicol080OraAsync(string ORNO, string PONO, string ITEM, string CWAR, string QUNE, string LOGN, string DATE, string PROC, string CLOT, string REFCNTD, string REFCNTU, string PDAT, string PICK, string OORG) {
            return base.Channel.InsertTicol080OraAsync(ORNO, PONO, ITEM, CWAR, QUNE, LOGN, DATE, PROC, CLOT, REFCNTD, REFCNTU, PDAT, PICK, OORG);
        }
        
        public System.Data.DataTable SelectWhcol133Evnt(string RFID, string EVNT, string PROC) {
            return base.Channel.SelectWhcol133Evnt(RFID, EVNT, PROC);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133EvntAsync(string RFID, string EVNT, string PROC) {
            return base.Channel.SelectWhcol133EvntAsync(RFID, EVNT, PROC);
        }
        
        public bool UpdateWhcol131(string PAID, string QTYA, string STAT) {
            return base.Channel.UpdateWhcol131(PAID, QTYA, STAT);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateWhcol131Async(string PAID, string QTYA, string STAT) {
            return base.Channel.UpdateWhcol131Async(PAID, QTYA, STAT);
        }
        
        public void InitProcRfid(string RFID, string EVNT, string LOGN, string PROC) {
            base.Channel.InitProcRfid(RFID, EVNT, LOGN, PROC);
        }
        
        public System.Threading.Tasks.Task InitProcRfidAsync(string RFID, string EVNT, string LOGN, string PROC) {
            return base.Channel.InitProcRfidAsync(RFID, EVNT, LOGN, PROC);
        }
        
        public void InitProcRfid022(string RFID, string EVNT, string LOGN, string PROC) {
            base.Channel.InitProcRfid022(RFID, EVNT, LOGN, PROC);
        }
        
        public System.Threading.Tasks.Task InitProcRfid022Async(string RFID, string EVNT, string LOGN, string PROC) {
            return base.Channel.InitProcRfid022Async(RFID, EVNT, LOGN, PROC);
        }
        
        public void InitProcRfid025(string RFID, string EVNT, string LOGN, string PROC) {
            base.Channel.InitProcRfid025(RFID, EVNT, LOGN, PROC);
        }
        
        public System.Threading.Tasks.Task InitProcRfid025Async(string RFID, string EVNT, string LOGN, string PROC) {
            return base.Channel.InitProcRfid025Async(RFID, EVNT, LOGN, PROC);
        }
        
        public bool Updtwhcol133RfidSS(string PAID, string RFID) {
            return base.Channel.Updtwhcol133RfidSS(PAID, RFID);
        }
        
        public System.Threading.Tasks.Task<bool> Updtwhcol133RfidSSAsync(string PAID, string RFID) {
            return base.Channel.Updtwhcol133RfidSSAsync(PAID, RFID);
        }
        
        public System.Data.DataTable SelectWhcol133OPaidAssing(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133OPaidAssing(RFID, EVNT);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SelectWhcol133OPaidAssingAsync(string RFID, string EVNT) {
            return base.Channel.SelectWhcol133OPaidAssingAsync(RFID, EVNT);
        }
        
        public bool Insert133ss(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.Insert133ss(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
        
        public System.Threading.Tasks.Task<bool> Insert133ssAsync(string PAID, string RFID, string EVNT, string ORNO, string DATE, string LOGN, string PROC, string REFCNTD, string REFCNTU) {
            return base.Channel.Insert133ssAsync(PAID, RFID, EVNT, ORNO, DATE, LOGN, PROC, REFCNTD, REFCNTU);
        }
    }
}
