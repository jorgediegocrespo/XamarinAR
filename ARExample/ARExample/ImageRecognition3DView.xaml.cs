using Xamarin.Forms;

namespace ARExample
{
    public partial class ImageRecognition3DView : ContentPage
    {
        public ImageRecognition3DView()
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
    }
}