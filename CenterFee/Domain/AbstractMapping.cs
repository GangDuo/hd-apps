using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CenterFee.Domain
{
    internal interface IExcelMapping
    {
        List<Tuple<object, int, int, object>> Create(DataRow record);
    }

    internal abstract class AbstractMapping : IExcelMapping
    {
        public List<Tuple<object, int, int, object>> Create(DataRow record)
        {
            var tbl = record.Table;
            var supplierName = record[Entity.Literal.SupplierNameField].ToString();
            return Create(record, supplierName);
        }

        protected abstract List<Tuple<object, int, int, object>> Create(DataRow record, string supplierName);
    }
}
