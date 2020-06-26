using System;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;

namespace ARExample.iOS.Renderers
{
    public class ArVehicularPhysicsScnViewDelegate : ARSCNViewDelegate
    {
        private readonly ArVehicularPhysicsViewRenderer arVehicularPhysicsViewRenderer;

        public ArVehicularPhysicsScnViewDelegate(ArVehicularPhysicsViewRenderer arVehicularPhysicsViewRenderer)
        {
            this.arVehicularPhysicsViewRenderer = arVehicularPhysicsViewRenderer;
        }

        [Export("renderer:didAddNode:forAnchor:")]
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARPlaneAnchor planeAnchor = anchor as ARPlaneAnchor;
            if (planeAnchor == null)
                return;

            SCNNode concreteNode = CreateConcreteNode(planeAnchor);
            node.AddChildNode(concreteNode);
        }

        [Export("renderer:didUpdateNode:forAnchor:")]
        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            ARPlaneAnchor planeAnchor = anchor as ARPlaneAnchor;
            if (planeAnchor == null)
                return;

            foreach (SCNNode chilNode in node.ChildNodes)
                chilNode.RemoveFromParentNode();

            SCNNode concreteNode = CreateConcreteNode(planeAnchor);
            node.AddChildNode(concreteNode);
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

        public override void DidSimulatePhysics(ISCNSceneRenderer renderer, double timeInSeconds)
        {
            if (arVehicularPhysicsViewRenderer.PhysicsVehicle != null)
            {
                System.Diagnostics.Debug.WriteLine(arVehicularPhysicsViewRenderer.Orientation);
                arVehicularPhysicsViewRenderer.PhysicsVehicle.SetSteeringAngle(arVehicularPhysicsViewRenderer.Orientation, 0);
                arVehicularPhysicsViewRenderer.PhysicsVehicle.SetSteeringAngle(arVehicularPhysicsViewRenderer.Orientation, 1);

                arVehicularPhysicsViewRenderer.PhysicsVehicle.ApplyEngineForce(arVehicularPhysicsViewRenderer.Speed, 2);
                arVehicularPhysicsViewRenderer.PhysicsVehicle.ApplyEngineForce(arVehicularPhysicsViewRenderer.Speed, 3);
            }
        }

        private SCNNode CreateConcreteNode(ARPlaneAnchor planeAnchor)
        {
            SCNNode concreteNode = new SCNNode();
            concreteNode.Geometry = SCNPlane.Create(planeAnchor.Extent.X, planeAnchor.Extent.Z);
            concreteNode.Geometry.FirstMaterial.Diffuse.Contents = new UIImage("Concrete.png");
            concreteNode.Geometry.FirstMaterial.DoubleSided = true;
            concreteNode.Position = new SCNVector3(planeAnchor.Center.X, planeAnchor.Center.Y, planeAnchor.Center.Z);
            concreteNode.EulerAngles = new SCNVector3(ConvertDegreesToRadians(90), 0, 0);

            //To support other objects
            SCNPhysicsBody staticBody = SCNPhysicsBody.CreateStaticBody();
            concreteNode.PhysicsBody = staticBody;

            return concreteNode;
        }

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}