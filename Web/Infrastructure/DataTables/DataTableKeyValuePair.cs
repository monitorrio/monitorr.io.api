using System;

namespace Web.Infrastructure.DataTables
{
    [Serializable]
    public class DataTableKeyValuePair
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    [Serializable]
    public class DataTableKeyValuePairObject
    {
        public object name { get; set; }
        public object value { get; set; }
    }
}
