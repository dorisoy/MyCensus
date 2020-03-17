using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MyCensus.DataServices.Interfaces;
using MyCensus.Droid.Services;

//[assembly: Dependency(typeof(SaveAndLoadService))]
namespace MyCensus.Droid.Services
{
    public class SaveAndLoadService : ISaveAndLoad
    {
        public void SaveText(string filename, string text)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                System.IO.File.WriteAllText(filePath, text);
            }
            catch (Exception) { }
        }


        public string LoadText(string filename)
        {
            try { 
             var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
            }
            catch (Exception ) {
                return "";
            }
        }


        public void DelFile(string filename)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                System.IO.File.Delete(filePath);
            }
            catch (Exception ) { }
        }


        public string LoadLocalText(string filename)
        {
            //Assets.Open ("read_asset.txt"))
            //Assets.Open("read_asset.txt")
            //Environment.SpecialFolder
            //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);
            return System.IO.File.ReadAllText(filePath);
        }

    }
}

