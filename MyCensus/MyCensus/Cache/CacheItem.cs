using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyCensus.Cache
{
    public class CacheItem
    {
        [PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
