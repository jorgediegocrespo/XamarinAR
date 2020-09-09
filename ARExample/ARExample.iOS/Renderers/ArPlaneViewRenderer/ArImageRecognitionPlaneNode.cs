using System;
using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ArImageRecognitionPlaneNode : SCNNode
    {
        public ArImageRecognitionPlaneNode(nfloat width, nfloat length, SCNVector3 position, UIColor colour)
        {
            SCNNode rootNode = new SCNNode
            {
                Geometry = CreateGeometry(width, length, colour),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(nfloat width, nfloat length, UIColor colour)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = colour;
            material.DoubleSided = false;

            SCNPlane geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
