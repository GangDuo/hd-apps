using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace CenterFee.Entity
{
    internal class Literal
    {
        public static readonly string FundingField = "F4";
        public static readonly string Field09 = "F9";// 物流仕入れ小計 = 物流仕入額 - センターフィー
        public static readonly string SupplierCodeField = "コード";
        public static readonly string SupplierNameField = "取引先名";
        public static readonly string Field03 = "総仕入額";
        public static readonly string Field05 = "店舗仕入";
        public static readonly string Field06 = "物流仕入";
        public static readonly string Field07 = "センターフィ　％";
        public static readonly string FeeField = "センターフィ";
        public static readonly string Field10 = "差引仕入額";
        public static readonly string BankTransferFeeField = "振込料";
        public static readonly string CobaltCenterFeeAmountField = "コバルト";
        public static readonly string DepositAtTamamuraField = "区画貸";
        public static readonly string DPaymentAmountField = "振込額";
        public static readonly string PaymentDateField = "F18";
        public static readonly string EdiField = "EDI　0#2％";
        public static readonly string MaximumUsedSectionField = "区画数";
        public static readonly string OthersField = "その他";

        public static readonly string XlsTmpl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "差引明細書.xlsx");
        public static readonly string XlsTmplCobalt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "コバルト用センターフィー.xlsx");
        public static readonly string XlsTmplDeposit = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "区画用センターフィー.xlsx");
        public static readonly string SheetName = "差引明細書";
        public static readonly string DestinationFolder = ConfigurationManager.AppSettings["folder:destination"] ?? Environment.CurrentDirectory;
    }
}
