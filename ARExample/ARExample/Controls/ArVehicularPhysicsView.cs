using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArVehicularPhysicsView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
        public Action AddCar { get; set; }

        public Action GoLeft { get; set; }
        public Action GoRight { get; set; }
        public Action GoAhead { get; set; }
        public Action GoBack { get; set; }
    }
}
