using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class CubeNode : SCNNode
    {
        public CubeNode(float size, UIColor color, float opacity)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(size, color),
                Opacity = opacity
            };

            AddChildNode(rootNode);
        }

        private SCNGeometry CreateGeometry(float size, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNBox geometry = SCNBox.Create(size, size, size, 0);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }

}
