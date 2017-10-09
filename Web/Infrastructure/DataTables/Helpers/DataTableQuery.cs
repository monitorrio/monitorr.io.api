using System;
using System.Collections.Generic;

namespace Web.Infrastructure.DataTables.Helpers
{
    [Serializable]
    public class DataTableQuery
    {
        public DataTableQuery() { }
        public DataTableQuery(List<DataTableKeyValuePair> data)
        {
            if (data.Exists(x => x.name == "sEcho"))
                sEcho = data.Find(x => x.name == "sEcho").value;
            if (data.Exists(x => x.name == "iColumns"))
                iColumns = int.Parse(data.Find(x => x.name == "iColumns").value);
            if (data.Exists(x => x.name == "sColumns"))
                sColumns = data.Find(x => x.name == "sColumns").value;
            if (data.Exists(x => x.name == "iDisplayStart"))
                iDisplayStart = int.Parse(data.Find(x => x.name == "iDisplayStart").value);
            if (data.Exists(x => x.name == "iDisplayLength"))
                iDisplayLength = int.Parse(data.Find(x => x.name == "iDisplayLength").value);
            if (data.Exists(x => x.name == "totalRecords"))
                totalRecords = int.Parse(data.Find(x => x.name == "totalRecords").value);
            if (data.Exists(x => x.name == "sSearch"))
                SearchTerm = data.Find(x => x.name == "sSearch").value;
        }

        public string sEcho { get; set; }
        public int iColumns { get; set; }
        public string sColumns { get; set; }
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public string SearchTerm { get; set; }
        public int totalRecords { get; set; }
        public int PageIndex => (iDisplayStart >= iDisplayLength) ? (int)Math.Floor((double)(iDisplayStart / iDisplayLength)) : 0;
        public int PageSize => (totalRecords > 0 && ((totalRecords - ((PageIndex + 1) * iDisplayLength)) < iDisplayLength)) ? (totalRecords - (PageIndex * iDisplayLength)) : iDisplayLength;

        public List<DTColumn> columns { get; set; }
        public List<DTOrder> order { get; set; }
    }

    public class DTOrder
    {
        /// <summary>
        /// Column to which ordering should be applied. This is an index reference to the columns array of information that is also submitted to the server.
        /// </summary>
        public int column { get; set; }
        /// <summary>
        /// Ordering direction for this column.
        /// </summary>
        public SortDirection dir { get; set; }
    }
    /// <summary>
    /// Column Array 
    /// </summary>
    public class DTColumn
    {
        /// <summary>
        /// Column's data source, as defined by columns.data Option within DataTables.
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// Column's name, as defined by columns.name Option within DataTables.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Flag to indicate if this column is searchable (true) or not (false). This is controlled by the columns.searchable Option within Datatables.
        /// </summary>
        public bool searchable { get; set; }
        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false). This is controlled by the columns.searchable Option within Datatables.
        /// </summary>
        public bool orderable { get; set; }      


    }

    public enum SortDirection
    {
        asc, desc
    }
}
