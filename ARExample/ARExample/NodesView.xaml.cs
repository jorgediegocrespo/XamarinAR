namespace ARExample
{
    public partial class NodesView
    {
        public NodesView()
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

        private void BtAddBox_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddBox?.Invoke();
        }

        private void btReset_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.ResetSession?.Invoke();
        }

        private void btAddPath_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddPath?.Invoke();
        }

        private void btAddHouse_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddHouse?.Invoke();
        }

        private void btAddSphere_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddSphere?.Invoke();
        }

        private void btAddTorus_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddTorus?.Invoke();
        }

        private void btAddTube_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddTube?.Invoke();
        }

        private void btAddCone_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddCone?.Invoke();
        }

        private void btAddCylinder_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddCylinder?.Invoke();
        }

        private void btAddPyramid_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddPyramid?.Invoke();
        }

        private void btAddPlane_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddPlane?.Invoke();
        }

        private void btAddCapsule_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddCapsule?.Invoke();
        }

        private void btAddAddRelativeNodes_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddRelativeNodes?.Invoke();
        }
    }
}
