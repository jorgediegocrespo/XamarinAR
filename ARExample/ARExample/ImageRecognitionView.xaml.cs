using Xamarin.Forms;

namespace ARExample
{
    public partial class ImageRecognitionView : ContentPage
    {
        public ImageRecognitionView()
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
