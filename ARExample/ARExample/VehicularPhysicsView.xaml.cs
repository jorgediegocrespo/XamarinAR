namespace ARExample
{
    public partial class VehicularPhysicsView
    {
        public VehicularPhysicsView()
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

        private void BtAddCar_Clicked(object sender, System.EventArgs e)
        {
            ArScene.AddCar?.Invoke();
        }

        private void BtGoLeft_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.GoLeft?.Invoke();
        }

        private void BtGoRight_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.GoRight?.Invoke();
        }

        private void BtGoAhead_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.GoAhead?.Invoke();
        }

        private void BtGoBack_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.GoBack?.Invoke();
        }
    }
}
