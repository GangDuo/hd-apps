using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CenterFee.Domain
{
    internal class MappingForTamamura : AbstractMapping
    {
        protected override List<Tuple<object, int, int, object>> Create(DataRow record, string supplierName)
        {
            return new List<Tuple<object, int, int, object>>()
            {
                // シート名、行番号、列番号、設定する値
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 3, 8, record[Entity.Literal.SupplierCodeField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 10, 1, supplierName),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 18, 3, record[Entity.Literal.DPaymentAmountField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 22, 7, record[Entity.Literal.Field03]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 24, 7, record[Entity.Literal.Field05]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 25, 7, record[Entity.Literal.Field06]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 26, 6, record[Entity.Literal.Field07]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 26, 7, record[Entity.Literal.FeeField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 27, 7, record[Entity.Literal.CobaltCenterFeeAmountField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 28, 7, record[Entity.Literal.EdiField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 29, 7, record[Entity.Literal.DepositAtTamamuraField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 29, 6, record[Entity.Literal.MaximumUsedSectionField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 30, 7, record[Entity.Literal.OthersField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 43, 4, record[Entity.Literal.Field10]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 43, 6, record[Entity.Literal.BankTransferFeeField]),
                Tuple.Create<object, int, int, object>(Entity.Literal.SheetName, 43, 7, record[Entity.Literal.DPaymentAmountField]),
            };
        }
    }
}
