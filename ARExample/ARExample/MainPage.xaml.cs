using System.ComponentModel;
using Xamarin.Forms;

namespace ARExample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ArScene.RunSession?.Invoke();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ArScene.PauseSession?.Invoke();
        }

        private void btAddButton_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddBox();
        }

        private void btReset_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.ResetSession();
        }
    }
}
