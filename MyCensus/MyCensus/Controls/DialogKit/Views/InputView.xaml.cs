using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Controls.DialogKit.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InputView : PopupPage
    {
        //public InputView()
        //{
        //    InitializeComponent();
        //}


        #region Property

        public new event PropertyChangedEventHandler PropertyChanged;
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private bool _Isvalid;
        private string _ValidMessage;

        public bool Isvalid
        {
            get { return _Isvalid; }
            set
            {
                this.SetProperty(ref _Isvalid, value);
            }
        }

        public string ValidMessage
        {
            get { return _ValidMessage; }
            set
            {
                this.SetProperty(ref _ValidMessage, value);
            }
        }


        public InputView(string title, string message, Keyboard keyboard = null, string selected = "", params string[] options)
        {
            InitializeComponent();

            this.BindingContext = new {
                Title = title,
                Message = message,
                Isvalid,
                ValidMessage,
            };
            txtInput.Keyboard = keyboard;
            for (int i = 0; i < options.Length; i++)
            {
                slContent.Children.Add(new RadioButton(options[i], selected == options[i]));
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }



        //public InputView(string title, string message, IEnumerable<object> values, string displayMember)
        //{
        //    InitializeComponent();
        //    this.BindingContext = new {
        //        Title = title,
        //        Message = message,
        //        Isvalid,
        //        ValidMessage
        //    };

        //    foreach (var item in values)
        //    {
        //        slContent.Children.Add(new RadioButton(item, displayMember));
        //    }
        //}

        public event EventHandler Completed;
        public string SelectedText
        {
            get
            {
                foreach (var item in slContent.Children)
                {
                    if (item is RadioButton && (item as RadioButton).IsChecked)
                        return (item as RadioButton).Text;
                }
                return null;
            }
        }
        public int SelectedIndex { get => slContent.SelectedIndex; }
        public object SelectecItem { get => slContent.SelectedItem; }

        public event EventHandler<string> Picked;
        public void FocusEntry()
        {
            txtInput.Focus();
        }
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            txtInput_valid.IsVisible = false;
            int saleVal = 0;
            Isvalid = false;
            if (!int.TryParse(txtInput.Text, out saleVal))
            {
                Isvalid = true;
                ValidMessage = "销量必须是数字";
                txtInput_valid.Text = "销量必须是数字";
                txtInput_valid.IsVisible = true;
                return;
            }
            else
            {
                txtInput_valid.IsVisible = false;
            }
            Picked?.Invoke(this, txtInput.Text);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            Picked?.Invoke(this, null);
        }
    }
}