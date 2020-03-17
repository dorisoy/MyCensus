using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyCensus.BasePage
{
    public class MyPage : ContentPage
    {
        public static Action EmulateBackPressed;

        private bool AcceptBack;

        protected override bool OnBackButtonPressed()
        {
            if (AcceptBack)
                return false;

            PromptForExit();
            return true;
        }

        private async void PromptForExit()
        {
            if (await DisplayAlert("", "你确定姚推出吗?", "是", "否"))
            {
                AcceptBack = true;
                EmulateBackPressed();
            }
        }
    }
}
