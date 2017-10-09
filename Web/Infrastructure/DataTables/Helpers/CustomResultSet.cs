using System;
using System.Collections.Generic;

namespace Web.Infrastructure.DataTables.Helpers
{
    [Serializable]
    public class CustomResultSet<T>
    {
        int _RecordCount = -1;
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }

        public int CurrentPage = -1;
        public int PageSize = -1;

        public int iTotalRecords => _RecordCount;
        public int iTotalDisplayRecords => _RecordCount;
        public string sEcho { get; set; }

        public IList<T> aaData = null;
    }
}
