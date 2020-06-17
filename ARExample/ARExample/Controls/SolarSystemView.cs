using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class SolarSystemView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
        public Action DrawSolarSystem { get; set; }
    }
}
