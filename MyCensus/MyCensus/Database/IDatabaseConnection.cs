using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyCensus.Database
{
    public interface IDatabaseConnection
    {
        SQLiteAsyncConnection GetConnection(string path);
    }
}
