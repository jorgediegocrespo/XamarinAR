using System.ComponentModel;
using Xamarin.Forms;
using System;

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

        private async void btNodes_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NodesView());
        }

        private async void btSolarSistem_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolarSistemView());
        }

        private async void btGame_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GameView());
        }

        private async void btPlane_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlaneView());
        }

        private async void btIkea_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IkeaView());
        }

        private async void btVehicularPhysics_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VehicularPhysicsView());
        }

        private async void btVirtualPortal_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VirtualPortalView());
        }

        private async void btImageRecognition_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageRecognitionView());
        }

        private async void btImageRecognition3D_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageRecognition3DView());
        }
    }
}
