
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyCensus.Controls.DialogKit.Views;
using Rg.Plugins.Popup.Extensions;

namespace MyCensus.Controls.DialogKit
{
    public class DialogKit : IDialogKit
    {
        static readonly Color[] defaultColors = { Color.Blue, Color.Green, Color.Red, Color.Yellow, Color.Orange, Color.Purple, Color.Brown };
        public Task<Color?> GetColorAsync(string title, string message, params Color[] colors)
        {
            var cts = new TaskCompletionSource<Color?>();

            if (colors == null || colors.Length == 0) colors = defaultColors;
            var _dialogView = new ColorPickerView(title, message, colors);
            _dialogView.Picked += (s, e) =>
            {
                cts.SetResult(e);
                PopupNavigation.Instance.PopAsync();
            };

            PopupNavigation.Instance.PushAsync(new Rg.Plugins.Popup.Pages.PopupPage
            {
                Content = _dialogView
            });

            return cts.Task;
        }

        /// <summary>
        /// new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="keyboard"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<string> GetInputTextAsync(string title, string message, Keyboard keyboard = null, string selected = "", params string[] options)
        {

            if (keyboard == null) keyboard = Keyboard.Default;
            var cts = new TaskCompletionSource<string>();
            var _dialogView = new MyCensus.Controls.DialogKit.Views.InputView(title, message, keyboard, selected, options);
            _dialogView.FocusEntry();
            _dialogView.Picked += (s, o) =>
            {
                if (!string.IsNullOrEmpty(o))
                    cts.SetResult(_dialogView.SelectedText + "为" + o + " 箱/年");
                PopupNavigation.Instance.PopAsync();
            };
            PopupNavigation.Instance.PushAsync(_dialogView);
            return cts.Task;
        }

        public Task<string[]> GetCheckboxResultAsync(string title, string message, params string[] options)
        {
            var tcs = new TaskCompletionSource<string[]>();
            var _dialogView = new MyCensus.Controls.DialogKit.Views.CheckBoxView(title, message, options);
            _dialogView.Completed += (s, e) => { tcs.SetResult(e == null ? null : _dialogView.SelectedValues.ToArray()); PopupNavigation.Instance.PopAsync(); };
            PopupNavigation.Instance.PushAsync(new PopupPage { Content = _dialogView });

            return tcs.Task;
        }
        public Task<int[]> GetCheckboxResultAsync(string title, string message, Dictionary<int, string> options)
        {
            var tcs = new TaskCompletionSource<int[]>();
            var _dialogView = new MyCensus.Controls.DialogKit.Views.CheckBoxView(title, message, options);
            _dialogView.Completed += (s, e) => { tcs.SetResult(e?.ToArray()); PopupNavigation.Instance.PopAsync(); };
            PopupNavigation.Instance.PushAsync(new PopupPage { Content = _dialogView });

            return tcs.Task;
        }
        public Task<string> GetRadioButtonResultAsync(string title, string message, params string[] options)
        {
            var tcs = new TaskCompletionSource<string>();
            var _dialogView = new MyCensus.Controls.DialogKit.Views.RadioButtonView(title, message, options);
            _dialogView.Completed += (s, e) => 
            {
                tcs.SetResult(_dialogView.SelectedText);
                PopupNavigation.Instance.PopAsync();
            };
            PopupNavigation.Instance.PushAsync(new PopupPage { Content = _dialogView });
            return tcs.Task;
        }
        /// <summary>
        /// Pushes a pop-up page which includes radio buttons for selection
        /// </summary>
        /// <param name="title">Title to be shown to user</param>
        /// <param name="message">Message to be shown to user</param>
        /// <param name="selectionSource">Ask options from a Collection</param>
        /// <param name="displayMember">Which property will be shown of object in collection</param>
        public Task<T> GetRadioButtonResultAsync<T>(string title, string message, IEnumerable<T> selectionSource, string displayMember)
        {
            var tcs = new TaskCompletionSource<T>();
            var _dialogView = new MyCensus.Controls.DialogKit.Views.RadioButtonView(title, message, (IEnumerable<object>)selectionSource, displayMember);
            _dialogView.Completed += (s, e) => { tcs.SetResult((T)_dialogView.SelectecItem); PopupNavigation.Instance.PopAsync(); };
            PopupNavigation.Instance.PushAsync(new PopupPage { Content = _dialogView });
            return tcs.Task;
        }
    }
}
