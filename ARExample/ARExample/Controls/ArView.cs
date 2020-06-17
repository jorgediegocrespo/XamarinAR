using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
        public Action ResetSession { get; set; }
        public Action AddBox { get; set; }
        public Action AddPath { get; set; }
        public Action AddHouse { get; set; }
    }
}
