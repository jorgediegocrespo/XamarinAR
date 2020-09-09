using System;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ArImageRecognitionScnViewDelegate : ARSCNViewDelegate
    {
        [Export("renderer:didAddNode:forAnchor:")]
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARImageAnchor imageAnchor = anchor as ARImageAnchor;
            if (imageAnchor == null)
                return;

            ArImageRecognitionPlaneNode imagePlaneNode = CreateImagePlaneNode(imageAnchor);
            node.AddChildNode(imagePlaneNode);
        }

        [Export("renderer:didUpdateNode:forAnchor:")]
        public override void DidRemoveNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARImageAnchor imageAnchor = anchor as ARImageAnchor;
            if (imageAnchor == null)
                return;

            foreach (SCNNode chilNode in node.ChildNodes)
                chilNode.RemoveFromParentNode();

            ArImageRecognitionPlaneNode imagePlaneNode = CreateImagePlaneNode(imageAnchor);
            node.AddChildNode(imagePlaneNode);
        }

        [Export("renderer:didRemoveNode:forAnchor:")]
        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARImageAnchor imageAnchor = anchor as ARImageAnchor;
            if (imageAnchor == null)
                return;

            foreach (SCNNode chilNode in node.ChildNodes)
                chilNode.RemoveFromParentNode();
        }

        private ArImageRecognitionPlaneNode CreateImagePlaneNode(ARImageAnchor imageAnchor)
        {
            ARReferenceImage detectedImage = imageAnchor.ReferenceImage;

            nfloat width = detectedImage.PhysicalSize.Width;
            nfloat length = detectedImage.PhysicalSize.Height;
            ArImageRecognitionPlaneNode imagePlaneNode = new ArImageRecognitionPlaneNode(width, length, new SCNVector3(0, 0, 0), UIColor.Blue);

            float angle = (float)(-Math.PI / 2);
            imagePlaneNode.EulerAngles = new SCNVector3(angle, 0, 0);

            return imagePlaneNode;
        }
    }
}
