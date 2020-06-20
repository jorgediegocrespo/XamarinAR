using System.Collections.Generic;
using ARExample.Controls;

namespace ARExample
{
    public partial class IkeaView
    {
        public IkeaView()
        {
            InitializeComponent();
            PkItemToAdd.ItemsSource = new List<string> { "Cup", "Vase", "Boxing", "Table" };
            PkItemToAdd.SelectedIndex = 0;
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

        private void PkItemToAdd_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            switch (PkItemToAdd.SelectedIndex)
            {
                case 0:
                    ArScene.SelectedItem = IkeaItem.Cup;
                    break;
                case 1:
                    ArScene.SelectedItem = IkeaItem.Vase;
                    break;
                case 2:
                    ArScene.SelectedItem = IkeaItem.Boxing;
                    break;
                case 3:
                default:
                    ArScene.SelectedItem = IkeaItem.Table;
                    break;
            }
        }
    }
}
