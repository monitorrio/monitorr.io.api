using System;
using System.Collections.Generic;

namespace Web.Infrastructure.DataTables
{
    [Serializable]
    public class ResultSet<T>
    {
        int _RecordCount = -1;
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }

        public int CurrentPage = -1;
        public int PageSize = -1;
        public IList<T> Items = null;

        public int iTotalRecords { get { return _RecordCount; } }

        public int iTotalDisplayRecords { get { return _RecordCount; } }
        public string sEcho { get; set; }
        public IList<T> aaData { get { return Items; } }
    }
}
