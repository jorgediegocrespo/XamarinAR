using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class PlaneNode : SCNNode
    {
        public PlaneNode(float width, float length, UIColor color, float cornerRadious, float opacity)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(width, length, color, cornerRadious),
                Opacity = opacity
            };

            AddChildNode(rootNode);
        }

        private SCNGeometry CreateGeometry(float width, float length, UIColor color, float cornerRadious)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;
            material.DoubleSided = true;

            SCNPlane geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };
            geometry.CornerRadius = cornerRadious;

            return geometry;
        }
    }
}
