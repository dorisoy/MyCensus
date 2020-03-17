using System;
using System.Collections.Generic;
using System.Text;
using MyCensus.Animations.Base;
using Xamarin.Forms;
using MyCensus.Controls.DialogKit;

namespace MyCensus.Triggers
{
    public class NumericValidationTriggerAction : TriggerAction<Entry>
    {
        protected async override void Invoke(Entry entry)
        {
            //double result;
            //bool isValid = Double.TryParse(entry.Text, out result);
            //entry.TextColor = isValid ? Color.Default : Color.Red;

            var result =await CrossDiaglogKit.Current.GetInputTextAsync("年销量", "各品牌档次年销量（年/箱）:");
            if (result != null)
            {
                entry.Text = "400";
            }

        }
    }
}
