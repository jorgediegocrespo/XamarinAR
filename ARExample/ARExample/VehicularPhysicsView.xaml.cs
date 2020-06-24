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
    }
}
