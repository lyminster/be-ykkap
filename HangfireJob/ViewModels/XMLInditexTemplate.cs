using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireJob.ViewModels
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ITX_CLOSE_EXPORT
    {

        private ITX_CLOSE_EXPORTSTORE_INFO sTORE_INFOField;

        private ITX_CLOSE_EXPORTTICKET[] vALID_TICKETSField;

        private ITX_CLOSE_EXPORTLINE[] sALE_LINESField;

        private ITX_CLOSE_EXPORTMEDIA[] mEDIA_LINESField;

        private object cUSTOMERSField;

        private ITX_CLOSE_EXPORTVOIDED_TICKETS vOIDED_TICKETSField;

        private ITX_CLOSE_EXPORTTRANSACTION[] tRANSACTIONSField;

        private ITX_CLOSE_EXPORTWARNING[] wARNINGSField;

        /// <remarks/>
        public ITX_CLOSE_EXPORTSTORE_INFO STORE_INFO
        {
            get
            {
                return this.sTORE_INFOField;
            }
            set
            {
                this.sTORE_INFOField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TICKET", IsNullable = false)]
        public ITX_CLOSE_EXPORTTICKET[] VALID_TICKETS
        {
            get
            {
                return this.vALID_TICKETSField;
            }
            set
            {
                this.vALID_TICKETSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("LINE", IsNullable = false)]
        public ITX_CLOSE_EXPORTLINE[] SALE_LINES
        {
            get
            {
                return this.sALE_LINESField;
            }
            set
            {
                this.sALE_LINESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("MEDIA", IsNullable = false)]
        public ITX_CLOSE_EXPORTMEDIA[] MEDIA_LINES
        {
            get
            {
                return this.mEDIA_LINESField;
            }
            set
            {
                this.mEDIA_LINESField = value;
            }
        }

        /// <remarks/>
        public object CUSTOMERS
        {
            get
            {
                return this.cUSTOMERSField;
            }
            set
            {
                this.cUSTOMERSField = value;
            }
        }

        /// <remarks/>
        public ITX_CLOSE_EXPORTVOIDED_TICKETS VOIDED_TICKETS
        {
            get
            {
                return this.vOIDED_TICKETSField;
            }
            set
            {
                this.vOIDED_TICKETSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TRANSACTION", IsNullable = false)]
        public ITX_CLOSE_EXPORTTRANSACTION[] TRANSACTIONS
        {
            get
            {
                return this.tRANSACTIONSField;
            }
            set
            {
                this.tRANSACTIONSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("WARNING", IsNullable = false)]
        public ITX_CLOSE_EXPORTWARNING[] WARNINGS
        {
            get
            {
                return this.wARNINGSField;
            }
            set
            {
                this.wARNINGSField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTSTORE_INFO
    {

        private ushort storeIdField;

        private string companyNameField;

        private ulong fiscalIdentifierField;

        private System.DateTime sessionDateField;

        private uint dateFromField;

        private uint timeFromField;

        private uint dateToField;

        private uint timeToField;

        private string versionField;

        /// <remarks/>
        public ushort storeId
        {
            get
            {
                return this.storeIdField;
            }
            set
            {
                this.storeIdField = value;
            }
        }

        /// <remarks/>
        public string companyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        public ulong fiscalIdentifier
        {
            get
            {
                return this.fiscalIdentifierField;
            }
            set
            {
                this.fiscalIdentifierField = value;
            }
        }

        /// <remarks/>
        public System.DateTime sessionDate
        {
            get
            {
                return this.sessionDateField;
            }
            set
            {
                this.sessionDateField = value;
            }
        }

        /// <remarks/>
        public uint dateFrom
        {
            get
            {
                return this.dateFromField;
            }
            set
            {
                this.dateFromField = value;
            }
        }

        /// <remarks/>
        public uint timeFrom
        {
            get
            {
                return this.timeFromField;
            }
            set
            {
                this.timeFromField = value;
            }
        }

        /// <remarks/>
        public uint dateTo
        {
            get
            {
                return this.dateToField;
            }
            set
            {
                this.dateToField = value;
            }
        }

        /// <remarks/>
        public uint timeTo
        {
            get
            {
                return this.timeToField;
            }
            set
            {
                this.timeToField = value;
            }
        }

        /// <remarks/>
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTTICKET
    {

        private uint dateField;

        private uint timeField;

        private uint operatorIdField;

        private decimal totalSaleField;

        private decimal totalNetField;

        private byte isVoidTicketField;

        private uint employeeIdField;

        private byte fiscalprinterIdField;

        private byte operationTypeGroupField;

        private byte roundingErrorField;

        private ITX_CLOSE_EXPORTTICKETTICKET_DATA[] tICKET_DATA_LISTField;

        private ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LIST tICKET_COUNTER_LISTField;

        private ushort sTOREIDField;

        private byte pOSNUMBERField;

        private uint oPERATIONNUMBERField;

        private uint oPERATIONTYPEField;

        private string dOCUMENTUUIDField;

        private ushort tICKETNUMBERField;

        /// <remarks/>
        public uint date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public uint time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public uint operatorId
        {
            get
            {
                return this.operatorIdField;
            }
            set
            {
                this.operatorIdField = value;
            }
        }

        /// <remarks/>
        public decimal totalSale
        {
            get
            {
                return this.totalSaleField;
            }
            set
            {
                this.totalSaleField = value;
            }
        }

        /// <remarks/>
        public decimal totalNet
        {
            get
            {
                return this.totalNetField;
            }
            set
            {
                this.totalNetField = value;
            }
        }

        /// <remarks/>
        public byte isVoidTicket
        {
            get
            {
                return this.isVoidTicketField;
            }
            set
            {
                this.isVoidTicketField = value;
            }
        }

        /// <remarks/>
        public uint employeeId
        {
            get
            {
                return this.employeeIdField;
            }
            set
            {
                this.employeeIdField = value;
            }
        }

        /// <remarks/>
        public byte fiscalprinterId
        {
            get
            {
                return this.fiscalprinterIdField;
            }
            set
            {
                this.fiscalprinterIdField = value;
            }
        }

        /// <remarks/>
        public byte operationTypeGroup
        {
            get
            {
                return this.operationTypeGroupField;
            }
            set
            {
                this.operationTypeGroupField = value;
            }
        }

        /// <remarks/>
        public byte roundingError
        {
            get
            {
                return this.roundingErrorField;
            }
            set
            {
                this.roundingErrorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TICKET_DATA", IsNullable = false)]
        public ITX_CLOSE_EXPORTTICKETTICKET_DATA[] TICKET_DATA_LIST
        {
            get
            {
                return this.tICKET_DATA_LISTField;
            }
            set
            {
                this.tICKET_DATA_LISTField = value;
            }
        }

        /// <remarks/>
        public ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LIST TICKET_COUNTER_LIST
        {
            get
            {
                return this.tICKET_COUNTER_LISTField;
            }
            set
            {
                this.tICKET_COUNTER_LISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort STOREID
        {
            get
            {
                return this.sTOREIDField;
            }
            set
            {
                this.sTOREIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte POSNUMBER
        {
            get
            {
                return this.pOSNUMBERField;
            }
            set
            {
                this.pOSNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONNUMBER
        {
            get
            {
                return this.oPERATIONNUMBERField;
            }
            set
            {
                this.oPERATIONNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONTYPE
        {
            get
            {
                return this.oPERATIONTYPEField;
            }
            set
            {
                this.oPERATIONTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DOCUMENTUUID
        {
            get
            {
                return this.dOCUMENTUUIDField;
            }
            set
            {
                this.dOCUMENTUUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort TICKETNUMBER
        {
            get
            {
                return this.tICKETNUMBERField;
            }
            set
            {
                this.tICKETNUMBERField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTTICKETTICKET_DATA
    {

        private ushort idField;

        private string dESCRIPTIONField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DESCRIPTION
        {
            get
            {
                return this.dESCRIPTIONField;
            }
            set
            {
                this.dESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LIST
    {

        private ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LISTTICKET_COUNTER tICKET_COUNTERField;

        /// <remarks/>
        public ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LISTTICKET_COUNTER TICKET_COUNTER
        {
            get
            {
                return this.tICKET_COUNTERField;
            }
            set
            {
                this.tICKET_COUNTERField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTTICKETTICKET_COUNTER_LISTTICKET_COUNTER
    {

        private object tICKET_COUNTER_DATA_LISTField;

        private ushort vALUEField;

        private uint idField;

        private string nAMEField;

        /// <remarks/>
        public object TICKET_COUNTER_DATA_LIST
        {
            get
            {
                return this.tICKET_COUNTER_DATA_LISTField;
            }
            set
            {
                this.tICKET_COUNTER_DATA_LISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort VALUE
        {
            get
            {
                return this.vALUEField;
            }
            set
            {
                this.vALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NAME
        {
            get
            {
                return this.nAMEField;
            }
            set
            {
                this.nAMEField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINE
    {

        private uint dateField;

        private uint timeField;

        private uint operatorIdField;

        private byte lineNumberField;

        private string barcodeField;

        private ushort campaignYearField;

        private bool campaignYearFieldSpecified;

        private byte campaignField;

        private bool campaignFieldSpecified;

        private string descriptionField;

        private byte familyCodeField;

        private bool familyCodeFieldSpecified;

        private ushort subFamilyCodeField;

        private bool subFamilyCodeFieldSpecified;

        private byte periodField;

        private byte departmentIdField;

        private sbyte quantityField;

        private decimal orgPriceField;

        private decimal priceField;

        private uint employeeIdField;

        private byte isVoidLineField;

        private byte operationTypeGroupField;

        private string lineTypeField;

        private uint controlCodeField;

        private bool controlCodeFieldSpecified;

        private ITX_CLOSE_EXPORTLINEOriginalData originalDataField;

        private ITX_CLOSE_EXPORTLINELINE_TAX_LIST lINE_TAX_LISTField;

        private ITX_CLOSE_EXPORTLINELINE_DATA[] lINE_DATA_LISTField;

        private ITX_CLOSE_EXPORTLINEPROMOTION_LIST pROMOTION_LISTField;

        private ushort sTOREIDField;

        private byte pOSNUMBERField;

        private uint oPERATIONNUMBERField;

        private uint oPERATIONTYPEField;

        private ushort tICKETNUMBERField;

        /// <remarks/>
        public uint date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public uint time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public uint operatorId
        {
            get
            {
                return this.operatorIdField;
            }
            set
            {
                this.operatorIdField = value;
            }
        }

        /// <remarks/>
        public byte lineNumber
        {
            get
            {
                return this.lineNumberField;
            }
            set
            {
                this.lineNumberField = value;
            }
        }

        /// <remarks/>
        public string barcode
        {
            get
            {
                return this.barcodeField;
            }
            set
            {
                this.barcodeField = value;
            }
        }

        /// <remarks/>
        public ushort campaignYear
        {
            get
            {
                return this.campaignYearField;
            }
            set
            {
                this.campaignYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool campaignYearSpecified
        {
            get
            {
                return this.campaignYearFieldSpecified;
            }
            set
            {
                this.campaignYearFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte campaign
        {
            get
            {
                return this.campaignField;
            }
            set
            {
                this.campaignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool campaignSpecified
        {
            get
            {
                return this.campaignFieldSpecified;
            }
            set
            {
                this.campaignFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public byte familyCode
        {
            get
            {
                return this.familyCodeField;
            }
            set
            {
                this.familyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool familyCodeSpecified
        {
            get
            {
                return this.familyCodeFieldSpecified;
            }
            set
            {
                this.familyCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ushort subFamilyCode
        {
            get
            {
                return this.subFamilyCodeField;
            }
            set
            {
                this.subFamilyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool subFamilyCodeSpecified
        {
            get
            {
                return this.subFamilyCodeFieldSpecified;
            }
            set
            {
                this.subFamilyCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }

        /// <remarks/>
        public byte departmentId
        {
            get
            {
                return this.departmentIdField;
            }
            set
            {
                this.departmentIdField = value;
            }
        }

        /// <remarks/>
        public sbyte quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public decimal orgPrice
        {
            get
            {
                return this.orgPriceField;
            }
            set
            {
                this.orgPriceField = value;
            }
        }

        /// <remarks/>
        public decimal price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public uint employeeId
        {
            get
            {
                return this.employeeIdField;
            }
            set
            {
                this.employeeIdField = value;
            }
        }

        /// <remarks/>
        public byte isVoidLine
        {
            get
            {
                return this.isVoidLineField;
            }
            set
            {
                this.isVoidLineField = value;
            }
        }

        /// <remarks/>
        public byte operationTypeGroup
        {
            get
            {
                return this.operationTypeGroupField;
            }
            set
            {
                this.operationTypeGroupField = value;
            }
        }

        /// <remarks/>
        public string lineType
        {
            get
            {
                return this.lineTypeField;
            }
            set
            {
                this.lineTypeField = value;
            }
        }

        /// <remarks/>
        public uint controlCode
        {
            get
            {
                return this.controlCodeField;
            }
            set
            {
                this.controlCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool controlCodeSpecified
        {
            get
            {
                return this.controlCodeFieldSpecified;
            }
            set
            {
                this.controlCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ITX_CLOSE_EXPORTLINEOriginalData originalData
        {
            get
            {
                return this.originalDataField;
            }
            set
            {
                this.originalDataField = value;
            }
        }

        /// <remarks/>
        public ITX_CLOSE_EXPORTLINELINE_TAX_LIST LINE_TAX_LIST
        {
            get
            {
                return this.lINE_TAX_LISTField;
            }
            set
            {
                this.lINE_TAX_LISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("LINE_DATA", IsNullable = false)]
        public ITX_CLOSE_EXPORTLINELINE_DATA[] LINE_DATA_LIST
        {
            get
            {
                return this.lINE_DATA_LISTField;
            }
            set
            {
                this.lINE_DATA_LISTField = value;
            }
        }

        /// <remarks/>
        public ITX_CLOSE_EXPORTLINEPROMOTION_LIST PROMOTION_LIST
        {
            get
            {
                return this.pROMOTION_LISTField;
            }
            set
            {
                this.pROMOTION_LISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort STOREID
        {
            get
            {
                return this.sTOREIDField;
            }
            set
            {
                this.sTOREIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte POSNUMBER
        {
            get
            {
                return this.pOSNUMBERField;
            }
            set
            {
                this.pOSNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONNUMBER
        {
            get
            {
                return this.oPERATIONNUMBERField;
            }
            set
            {
                this.oPERATIONNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONTYPE
        {
            get
            {
                return this.oPERATIONTYPEField;
            }
            set
            {
                this.oPERATIONTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort TICKETNUMBER
        {
            get
            {
                return this.tICKETNUMBERField;
            }
            set
            {
                this.tICKETNUMBERField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINEOriginalData
    {

        private ushort storeIdField;

        private bool storeIdFieldSpecified;

        private byte posNumberField;

        private bool posNumberFieldSpecified;

        private uint operationNumberField;

        private bool operationNumberFieldSpecified;

        private uint ticketNumberField;

        private bool ticketNumberFieldSpecified;

        private uint dateField;

        private bool dateFieldSpecified;

        private ITX_CLOSE_EXPORTLINEOriginalDataLINE_DATA[] lIST_ORIGINAL_DATADATAField;

        /// <remarks/>
        public ushort storeId
        {
            get
            {
                return this.storeIdField;
            }
            set
            {
                this.storeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool storeIdSpecified
        {
            get
            {
                return this.storeIdFieldSpecified;
            }
            set
            {
                this.storeIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte posNumber
        {
            get
            {
                return this.posNumberField;
            }
            set
            {
                this.posNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool posNumberSpecified
        {
            get
            {
                return this.posNumberFieldSpecified;
            }
            set
            {
                this.posNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint operationNumber
        {
            get
            {
                return this.operationNumberField;
            }
            set
            {
                this.operationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operationNumberSpecified
        {
            get
            {
                return this.operationNumberFieldSpecified;
            }
            set
            {
                this.operationNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint ticketNumber
        {
            get
            {
                return this.ticketNumberField;
            }
            set
            {
                this.ticketNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ticketNumberSpecified
        {
            get
            {
                return this.ticketNumberFieldSpecified;
            }
            set
            {
                this.ticketNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateSpecified
        {
            get
            {
                return this.dateFieldSpecified;
            }
            set
            {
                this.dateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("LINE_DATA", IsNullable = false)]
        public ITX_CLOSE_EXPORTLINEOriginalDataLINE_DATA[] LIST_ORIGINAL_DATADATA
        {
            get
            {
                return this.lIST_ORIGINAL_DATADATAField;
            }
            set
            {
                this.lIST_ORIGINAL_DATADATAField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINEOriginalDataLINE_DATA
    {

        private ushort idField;

        private string dESCRIPTIONField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DESCRIPTION
        {
            get
            {
                return this.dESCRIPTIONField;
            }
            set
            {
                this.dESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINELINE_TAX_LIST
    {

        private ITX_CLOSE_EXPORTLINELINE_TAX_LISTLINE_TAX lINE_TAXField;

        /// <remarks/>
        public ITX_CLOSE_EXPORTLINELINE_TAX_LISTLINE_TAX LINE_TAX
        {
            get
            {
                return this.lINE_TAXField;
            }
            set
            {
                this.lINE_TAXField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINELINE_TAX_LISTLINE_TAX
    {

        private decimal taxPercentField;

        private byte taxExemptField;

        private uint idTaxAuthorityField;

        private uint idTaxGroupField;

        private byte idTaxRuleField;

        /// <remarks/>
        public decimal taxPercent
        {
            get
            {
                return this.taxPercentField;
            }
            set
            {
                this.taxPercentField = value;
            }
        }

        /// <remarks/>
        public byte taxExempt
        {
            get
            {
                return this.taxExemptField;
            }
            set
            {
                this.taxExemptField = value;
            }
        }

        /// <remarks/>
        public uint idTaxAuthority
        {
            get
            {
                return this.idTaxAuthorityField;
            }
            set
            {
                this.idTaxAuthorityField = value;
            }
        }

        /// <remarks/>
        public uint idTaxGroup
        {
            get
            {
                return this.idTaxGroupField;
            }
            set
            {
                this.idTaxGroupField = value;
            }
        }

        /// <remarks/>
        public byte idTaxRule
        {
            get
            {
                return this.idTaxRuleField;
            }
            set
            {
                this.idTaxRuleField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINELINE_DATA
    {

        private ushort idField;

        private string dESCRIPTIONField;

        private ulong valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DESCRIPTION
        {
            get
            {
                return this.dESCRIPTIONField;
            }
            set
            {
                this.dESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public ulong Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINEPROMOTION_LIST
    {

        private ITX_CLOSE_EXPORTLINEPROMOTION_LISTPROMOTION pROMOTIONField;

        /// <remarks/>
        public ITX_CLOSE_EXPORTLINEPROMOTION_LISTPROMOTION PROMOTION
        {
            get
            {
                return this.pROMOTIONField;
            }
            set
            {
                this.pROMOTIONField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTLINEPROMOTION_LISTPROMOTION
    {

        private byte idField;

        private byte typeField;

        private string nameField;

        private byte quantityField;

        private decimal percentField;

        private decimal unitAmountField;

        private decimal totalAmountField;

        /// <remarks/>
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public byte type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public byte quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public decimal percent
        {
            get
            {
                return this.percentField;
            }
            set
            {
                this.percentField = value;
            }
        }

        /// <remarks/>
        public decimal unitAmount
        {
            get
            {
                return this.unitAmountField;
            }
            set
            {
                this.unitAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTMEDIA
    {

        private uint dateField;

        private uint timeField;

        private decimal paidField;

        private decimal returnedField;

        private uint paymentMethodField;

        private object mEDIA_DATA_LISTField;

        private ushort sTOREIDField;

        private byte pOSNUMBERField;

        private uint oPERATIONNUMBERField;

        private uint oPERATIONTYPEField;

        private ushort tICKETNUMBERField;

        /// <remarks/>
        public uint date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public uint time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public decimal paid
        {
            get
            {
                return this.paidField;
            }
            set
            {
                this.paidField = value;
            }
        }

        /// <remarks/>
        public decimal returned
        {
            get
            {
                return this.returnedField;
            }
            set
            {
                this.returnedField = value;
            }
        }

        /// <remarks/>
        public uint paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

        /// <remarks/>
        public object MEDIA_DATA_LIST
        {
            get
            {
                return this.mEDIA_DATA_LISTField;
            }
            set
            {
                this.mEDIA_DATA_LISTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort STOREID
        {
            get
            {
                return this.sTOREIDField;
            }
            set
            {
                this.sTOREIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte POSNUMBER
        {
            get
            {
                return this.pOSNUMBERField;
            }
            set
            {
                this.pOSNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONNUMBER
        {
            get
            {
                return this.oPERATIONNUMBERField;
            }
            set
            {
                this.oPERATIONNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONTYPE
        {
            get
            {
                return this.oPERATIONTYPEField;
            }
            set
            {
                this.oPERATIONTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort TICKETNUMBER
        {
            get
            {
                return this.tICKETNUMBERField;
            }
            set
            {
                this.tICKETNUMBERField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTVOIDED_TICKETS
    {

        private ITX_CLOSE_EXPORTVOIDED_TICKETSTICKET_VOID tICKET_VOIDField;

        /// <remarks/>
        public ITX_CLOSE_EXPORTVOIDED_TICKETSTICKET_VOID TICKET_VOID
        {
            get
            {
                return this.tICKET_VOIDField;
            }
            set
            {
                this.tICKET_VOIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTVOIDED_TICKETSTICKET_VOID
    {

        private uint timeField;

        private uint operatorIdField;

        private uint voidedoperationNumberField;

        private byte voidedPosNumberField;

        private ushort voidedstoreIdField;

        private string originalUIDField;

        private ushort sTOREIDField;

        private byte pOSNUMBERField;

        private uint oPERATIONNUMBERField;

        private uint oPERATIONTYPEField;

        private string dOCUMENTUUIDField;

        private ushort tICKETNUMBERField;

        /// <remarks/>
        public uint time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public uint operatorId
        {
            get
            {
                return this.operatorIdField;
            }
            set
            {
                this.operatorIdField = value;
            }
        }

        /// <remarks/>
        public uint voidedoperationNumber
        {
            get
            {
                return this.voidedoperationNumberField;
            }
            set
            {
                this.voidedoperationNumberField = value;
            }
        }

        /// <remarks/>
        public byte voidedPosNumber
        {
            get
            {
                return this.voidedPosNumberField;
            }
            set
            {
                this.voidedPosNumberField = value;
            }
        }

        /// <remarks/>
        public ushort voidedstoreId
        {
            get
            {
                return this.voidedstoreIdField;
            }
            set
            {
                this.voidedstoreIdField = value;
            }
        }

        /// <remarks/>
        public string originalUID
        {
            get
            {
                return this.originalUIDField;
            }
            set
            {
                this.originalUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort STOREID
        {
            get
            {
                return this.sTOREIDField;
            }
            set
            {
                this.sTOREIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte POSNUMBER
        {
            get
            {
                return this.pOSNUMBERField;
            }
            set
            {
                this.pOSNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONNUMBER
        {
            get
            {
                return this.oPERATIONNUMBERField;
            }
            set
            {
                this.oPERATIONNUMBERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint OPERATIONTYPE
        {
            get
            {
                return this.oPERATIONTYPEField;
            }
            set
            {
                this.oPERATIONTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DOCUMENTUUID
        {
            get
            {
                return this.dOCUMENTUUIDField;
            }
            set
            {
                this.dOCUMENTUUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort TICKETNUMBER
        {
            get
            {
                return this.tICKETNUMBERField;
            }
            set
            {
                this.tICKETNUMBERField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTTRANSACTION
    {

        private ulong codeField;

        private string descriptionField;

        private decimal debitField;

        private decimal creditField;

        private uint auxValueField;

        private string auxValue2Field;

        private byte taxPercentField;

        private bool taxPercentFieldSpecified;

        private uint employeeIdField;

        private bool employeeIdFieldSpecified;

        private string universalIdField;

        private string txTypeField;

        /// <remarks/>
        public ulong code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public decimal debit
        {
            get
            {
                return this.debitField;
            }
            set
            {
                this.debitField = value;
            }
        }

        /// <remarks/>
        public decimal credit
        {
            get
            {
                return this.creditField;
            }
            set
            {
                this.creditField = value;
            }
        }

        /// <remarks/>
        public uint auxValue
        {
            get
            {
                return this.auxValueField;
            }
            set
            {
                this.auxValueField = value;
            }
        }

        /// <remarks/>
        public string auxValue2
        {
            get
            {
                return this.auxValue2Field;
            }
            set
            {
                this.auxValue2Field = value;
            }
        }

        /// <remarks/>
        public byte taxPercent
        {
            get
            {
                return this.taxPercentField;
            }
            set
            {
                this.taxPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxPercentSpecified
        {
            get
            {
                return this.taxPercentFieldSpecified;
            }
            set
            {
                this.taxPercentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint employeeId
        {
            get
            {
                return this.employeeIdField;
            }
            set
            {
                this.employeeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool employeeIdSpecified
        {
            get
            {
                return this.employeeIdFieldSpecified;
            }
            set
            {
                this.employeeIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string universalId
        {
            get
            {
                return this.universalIdField;
            }
            set
            {
                this.universalIdField = value;
            }
        }

        /// <remarks/>
        public string txType
        {
            get
            {
                return this.txTypeField;
            }
            set
            {
                this.txTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ITX_CLOSE_EXPORTWARNING
    {

        private string warningTypeField;

        private string warningMessageField;

        private byte posNumberField;

        private byte refoperationNumberField;

        /// <remarks/>
        public string warningType
        {
            get
            {
                return this.warningTypeField;
            }
            set
            {
                this.warningTypeField = value;
            }
        }

        /// <remarks/>
        public string warningMessage
        {
            get
            {
                return this.warningMessageField;
            }
            set
            {
                this.warningMessageField = value;
            }
        }

        /// <remarks/>
        public byte posNumber
        {
            get
            {
                return this.posNumberField;
            }
            set
            {
                this.posNumberField = value;
            }
        }

        /// <remarks/>
        public byte refoperationNumber
        {
            get
            {
                return this.refoperationNumberField;
            }
            set
            {
                this.refoperationNumberField = value;
            }
        }
    }

}