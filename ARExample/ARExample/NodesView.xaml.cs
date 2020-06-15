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
            ArScene.AddBox();
        }

        private void btReset_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.ResetSession();
        }

        private void btAddPath_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddPath();
        }

        private void btAddHouse_Clicked(System.Object sender, System.EventArgs e)
        {
            ArScene.AddHouse();
        }
    }
}
