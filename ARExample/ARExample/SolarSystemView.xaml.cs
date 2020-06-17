namespace ARExample
{
    public partial class SolarSistemView
    {
        public SolarSistemView()
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

        private void btSolarSistem_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.DrawSolarSystem?.Invoke();
        }
    }
}
