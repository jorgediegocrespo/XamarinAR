using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArIkeaView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }

        public IkeaItem SelectedItem { get; set; }
    }

    public enum IkeaItem
    {
        Cup,
        Vase,
        Boxing,
        Table
    }
}
