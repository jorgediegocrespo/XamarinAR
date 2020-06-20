using System;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ArGameScnViewDelegate : ARSCNViewDelegate
    { 
        private readonly ARSCNView sceneView;

        public ArGameScnViewDelegate(ARSCNView sceneView)
        {
            this.sceneView = sceneView;
        }

        [Export("renderer:didAddNode:forAnchor:")]
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARPlaneAnchor planeAnchor = anchor as ARPlaneAnchor;
            if (planeAnchor == null)
                return;

            SCNNode lavaNode = CreateLavaNode(planeAnchor);
            node.AddChildNode(lavaNode);
        }

        [Export("renderer:didUpdateNode:forAnchor:")]
        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARPlaneAnchor planeAnchor = anchor as ARPlaneAnchor;
            if (planeAnchor == null)
                return;

            foreach (SCNNode chilNode in node.ChildNodes)
                chilNode.RemoveFromParentNode();

            SCNNode lavaNode = CreateLavaNode(planeAnchor);
            node.AddChildNode(lavaNode);
        }

        [Export("renderer:didRemoveNode:forAnchor:")]
        public override void DidRemoveNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARPlaneAnchor planeAnchor = anchor as ARPlaneAnchor;
            if (planeAnchor == null)
                return;

            foreach (SCNNode chilNode in node.ChildNodes)
                chilNode.RemoveFromParentNode();
        }

        private SCNNode CreateLavaNode(ARPlaneAnchor planeAnchor)
        {
            SCNNode lavaNode = new SCNNode();
            lavaNode.Geometry = SCNPlane.Create(planeAnchor.Extent.X, planeAnchor.Extent.Z);
            lavaNode.Geometry.FirstMaterial.Diffuse.Contents = new UIImage("Lava.png");
            lavaNode.Geometry.FirstMaterial.DoubleSided = true;
            lavaNode.Position = new SCNVector3(planeAnchor.Center.X, planeAnchor.Center.Y, planeAnchor.Center.Z);
            lavaNode.EulerAngles = new SCNVector3(ConvertDegreesToRadians(90), 0, 0);

            return lavaNode;
        }

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}
