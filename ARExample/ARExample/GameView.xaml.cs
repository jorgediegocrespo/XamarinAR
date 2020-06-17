
using Xamarin.Forms;

namespace ARExample
{
    public partial class GameView : ContentPage
    {
        public GameView()
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

        private void btPlay_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.PlayGame?.Invoke();
        }

        private void btReset_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.ResetGame?.Invoke();
        }
    }
}
