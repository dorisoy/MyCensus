using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;
using Xamarin.Forms;
using MyCensus.Database;
using MyCensus.Droid;

[assembly: Dependency(typeof(SQLiteClient))]
namespace MyCensus.Droid
{
    public class SQLiteClient : IDatabaseConnection
    {
        public SQLiteAsyncConnection GetConnection(string filename)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var fullPath = Path.Combine(documentsPath, filename);
            var conn = new SQLiteAsyncConnection(fullPath);
            return conn;
        }
    }
}