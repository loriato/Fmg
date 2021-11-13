using System;
using System.Collections.Generic;

namespace Europa.Development
{
    public class JExcelOptions
    {
        public List<List<Header>> NestedHeaders { get; set; }
        public List<Column> Columns { get; set; }
        public bool AllowInsertRow { get; set; }
        public bool AllowInsertColumn { get; set; }
        public bool AllowDeleteColumn { get; set; }
        public bool AllowDeleteRow { get; set; }
        public bool AllowRenameColumn { get; set; }
        public bool AllowComments { get; set; }
        public bool AllowExport { get; set; }
        public bool ColumnSorting { get; set; }
        public bool ColumnDrag { get; set; }
        public bool ColumnResize { get; set; }
        public bool RowResize { get; set; }
        public bool RowDrag { get; set; }
        public bool Editable { get; set; }
        public bool AllowManualInsertRow { get; set; }
        public bool AllowManualInsertColumn { get; set; }
        public String Url { get; set; }

        public List<List<Object>> Data { get; set; }
    }
}