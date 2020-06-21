using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArVehicularPhysicsView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
    }
}
