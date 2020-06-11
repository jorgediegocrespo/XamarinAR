using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ImageNode : SCNNode
    {
        public ImageNode(string fileName, float cornerRadious, float opacity)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(fileName, cornerRadious),
                Position = new SCNVector3(0, 0, 0),
                Opacity = opacity
            };

            AddChildNode(rootNode);
        }

        private SCNGeometry CreateGeometry(string fileName, float cornerRadious)
        {
            UIImage image = UIImage.FromFile(fileName);

            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = image;
            material.DoubleSided = true;

            SCNPlane geometry = SCNPlane.Create(0.1f, 0.1f);
            geometry.Materials = new[] { material };
            geometry.CornerRadius = cornerRadious;

            return geometry;
        }
    }
}
