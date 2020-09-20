using System;
using Xamarin.Forms;

namespace ARExample.Controls
{
    public class ArImageRecognitionView : View
    {
        public bool Model3D { get; set;}
        public Action RunSession { get; set; }
        public Action PauseSession { get; set; }
    }
}
