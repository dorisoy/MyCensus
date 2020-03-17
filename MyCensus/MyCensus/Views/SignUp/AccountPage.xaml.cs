using MyCensus.Animations;
using MyCensus.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class AccountPage : ContentView
    {
        public AccountPage()
        {
            InitializeComponent();

            var sunAnimation = this.Resources["SunAnimation"] as RotateToAnimation;

            if (sunAnimation != null && Device.RuntimePlatform != Device.UWP)
            {
                Sun.Animate(sunAnimation);
            }

            var cloudAnimation = this.Resources["CloudAnimation"] as StoryBoard;

            if (cloudAnimation != null)
            {
                Cloud.Animate(cloudAnimation);
            }
        }
    }
}
