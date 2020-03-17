using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyCensus.DataServices.Interfaces
{
    public interface ISaveAndLoad
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        void DelFile(string filename);
        string LoadLocalText(string filename);
    }
}




