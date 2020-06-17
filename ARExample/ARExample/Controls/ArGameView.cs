using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArGameView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
        public Action PlayGame { get; set; }
        public Action ResetGame { get; set; }
    }
}
