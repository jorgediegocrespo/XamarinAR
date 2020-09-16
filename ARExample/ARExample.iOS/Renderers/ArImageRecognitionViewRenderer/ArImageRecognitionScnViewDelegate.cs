using System;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ArImageRecognitionScnViewDelegate : ARSCNViewDelegate
    {
        private bool model3D;

        public ArImageRecognitionScnViewDelegate(bool model3D)
        {
            this.model3D = model3D;
        }

        [Export("renderer:didAddNode:forAnchor:")]
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARImageAnchor imageAnchor = anchor as ARImageAnchor;
            if (imageAnchor == null)
                return;

            ARReferenceImage detectedImage = imageAnchor.ReferenceImage;
            if (detectedImage.Name != "AR_DevsDNA_Card")
                return;

            node.AddChildNode(model3D ? (SCNNode)CreateImagePlaneNode(detectedImage) : Create3DNode());
        }

        private ArImageRecognitionPlaneNode CreateImagePlaneNode(ARReferenceImage detectedImage)
        {
            nfloat width = detectedImage.PhysicalSize.Width;
            nfloat length = detectedImage.PhysicalSize.Height;
            ArImageRecognitionPlaneNode imagePlaneNode = new ArImageRecognitionPlaneNode(width, length, new SCNVector3(0, 0, 0), UIColor.Blue);

            float angle = (float)(-Math.PI / 2);
            imagePlaneNode.EulerAngles = new SCNVector3(angle, 0, 0);

            return imagePlaneNode;
        }

        public SCNNode Create3DNode()
        {
            SCNScene jellyfishScn = SCNScene.FromFile("art.scnassets/Jellyfish");
            SCNNode jellyfishNode = jellyfishScn.RootNode.FindChildNode("Jellyfish", false);

            jellyfishNode.Scale = new SCNVector3(0.019f, 0.019f, 0.019f);
            jellyfishNode.Position = new SCNVector3(0, 0, 0);
            float angle = (float)(-Math.PI / 2);
            jellyfishNode.EulerAngles = new SCNVector3(angle, angle, 0);

            // Animate the opacity to 100% over 0.75 seconds
            jellyfishNode.Opacity = 0;
            jellyfishNode.RunAction(SCNAction.FadeIn(0.75));

            return jellyfishNode;
        }
    }
}
