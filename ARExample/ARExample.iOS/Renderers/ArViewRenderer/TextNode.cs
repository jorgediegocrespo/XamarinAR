using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class TextNode : SCNNode
    {
        public TextNode(string text, UIColor color, float opacity)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(text, color),
                Position = new SCNVector3(0, 0, 0),
                Opacity = opacity
            };

            AddChildNode(rootNode);
        }

        private SCNGeometry CreateGeometry(string text, UIColor color)
        {
            var geometry = SCNText.Create(text, 0.01f);
            geometry.Font = UIFont.FromName("Courier", 0.5f);
            geometry.Flatness = 0;
            geometry.FirstMaterial.DoubleSided = true;
            geometry.FirstMaterial.Diffuse.Contents = color;
            geometry.FirstMaterial.Specular.Contents = UIColor.Blue;
            return geometry;
        }
    }
}
