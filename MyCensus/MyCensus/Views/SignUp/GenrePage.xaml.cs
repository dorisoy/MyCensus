using MyCensus.Animations;
using MyCensus.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class GenrePage : ContentView
    {
        public GenrePage()
        {
            InitializeComponent();

            var sunAnimation = this.Resources["SunAnimation"] as RotateToAnimation;

            if (sunAnimation != null && Device.RuntimePlatform != Device.UWP)
            {
                Sun.Animate(sunAnimation);
            }

            var cloud1Animation = this.Resources["Cloud1Animation"] as StoryBoard;

            if (cloud1Animation != null)
            {
                Cloud1.Animate(cloud1Animation);
            }

            var cloud2Animation = this.Resources["Cloud2Animation"] as StoryBoard;

            if (cloud2Animation != null)
            {
                Cloud2.Animate(cloud2Animation);
            }
        }
    }
}