using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    //TODO 1.1 ¿Por dónde empezar?
    public class ArView : View
    {
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
        public Action ResetSession { get; set; }

        public Action AddBox { get; set; }
        public Action AddSphere { get; set; }
        public Action AddTorus { get; set; }
        public Action AddTube { get; set; }
        public Action AddCone { get; set; }
        public Action AddCylinder { get; set; }
        public Action AddPyramid { get; set; }
        public Action AddPlane { get; set; }
        public Action AddCapsule { get; set; }

        public Action AddPath { get; set; }
        public Action AddHouse { get; set; }
        public Action AddRelativeNodes { get; set; }
    }
}
