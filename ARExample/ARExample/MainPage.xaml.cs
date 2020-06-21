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

        private async void btNodes_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NodesView());
        }

        private async void btSolarSistem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SolarSistemView());
        }

        private async void btGame_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new GameView());
        }

        private async void btPlane_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PlaneView());
        }

        private async void btIkea_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new IkeaView());
        }

        private async void btVehicularPhysics_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new VehicularPhysicsView());
        }
    }
}
