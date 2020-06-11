using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class SphereNode : SCNNode
    {
        public SphereNode(float size, UIColor color, float opacity)
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

            SCNSphere geometry = SCNSphere.Create(size);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
