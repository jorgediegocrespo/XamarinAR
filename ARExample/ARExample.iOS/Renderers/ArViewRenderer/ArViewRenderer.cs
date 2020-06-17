[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArView), typeof(ARExample.iOS.Renderers.ArViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using ARExample.Controls;
    using ARKit;
    using CoreGraphics;
    using Foundation;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArViewRenderer : ViewRenderer<ArView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<ArView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            sceneView = new ARSCNView()
            {
                AutoenablesDefaultLighting = true,
                DebugOptions = ARSCNDebugOptions.ShowWorldOrigin,
                ShowsStatistics = true
            };

            sceneView.Frame = Bounds;

            Element.RunSession = RunSession;
            Element.PauseSession = PauseSession;
            Element.ResetSession = ResetSession;
            Element.AddBox = AddBox;
            Element.AddPath = AddPath;
            Element.AddHouse = AddHouse;

            SetNativeControl(sceneView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (touches.AnyObject is UITouch touch)
            {
                CGPoint point = touch.LocationInView(this.sceneView);

                SCNHitTestOptions hitTestOptions = new SCNHitTestOptions();

                SCNHitTestResult[] hits = sceneView.HitTest(point, hitTestOptions);
                SCNHitTestResult hit = hits.FirstOrDefault();

                if (hit == null)
                    return;

                SCNNode node = hit.Node;

                if (node == null)
                    return;

                node.RemoveFromParentNode();
            }
        }

        private void RunSession()
        {
            config?.Dispose();
            sceneView?.Delegate?.Dispose();

            config = new ARWorldTrackingConfiguration();
            sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin;
            sceneView.Session.Run(config, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
            sceneView.Delegate = new CustomArScnViewDelegate(sceneView);
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        private void ResetSession()
        {
            PauseSession();
            RemoveAllNodes();
            RunSession();
        }

        private void RemoveAllNodes()
        {
            while (sceneView.Scene.RootNode.ChildNodes.Length > 0)
            {
                sceneView.Scene.RootNode.ChildNodes[0].RemoveFromParentNode();
            }
        }

        private void AddBox()
        {
            SCNMaterial material = new SCNMaterial();
            material.Specular.Contents = UIColor.White; //Color del reflejo
            material.Diffuse.Contents = UIColor.Blue; //Color del objeto

            SCNNode boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.1f, 0.1f, 0.1f, 0);
            boxNode.Geometry.Materials = new SCNMaterial[] { material }; 
            boxNode.Position = new SCNVector3(0, 0, -0.3f);

            sceneView.Scene.RootNode.AddChildNode(boxNode);
        }

        private void AddPath()
        {
            SCNMaterial material = new SCNMaterial();
            material.Specular.Contents = UIColor.White; //Color del reflejo
            material.Diffuse.Contents = UIColor.Blue; //Color del objeto

            var path = new UIBezierPath();
            path.MoveTo(new CGPoint(0, 0));
            path.AddLineTo(new CGPoint(0, 0.2));
            path.AddLineTo(new CGPoint(0.2, 0.3));
            path.AddLineTo(new CGPoint(0.4, 0.2));
            path.AddLineTo(new CGPoint(0.4, 0));
            var shape = SCNShape.Create(path, 0.2f);

            SCNNode node = new SCNNode();
            node.Geometry = shape;
            node.Geometry.Materials = new SCNMaterial[] { material };
            node.Position = new SCNVector3(0, 0, -0.7f);

            sceneView.Scene.RootNode.AddChildNode(node);
        }

        private void AddHouse()
        {
            var pyramidNode = new SCNNode();
            pyramidNode.Geometry = SCNPyramid.Create(0.1f, 0.1f, 0.1f);
            pyramidNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Red;
            pyramidNode.Position = new SCNVector3(0, 0, -0.2f);
            sceneView.Scene.RootNode.AddChildNode(pyramidNode);

            var boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.1f, 0.1f, 0.1f, 0);
            boxNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Blue;
            boxNode.Position = new SCNVector3(0, -0.05f, 0);
            pyramidNode.AddChildNode(boxNode);

            var door = new SCNNode();
            door.Geometry = SCNPlane.Create(0.03f, 0.06f);
            door.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Brown;
            door.Position = new SCNVector3(0, -0.02f, 0.051f);
            boxNode.AddChildNode(door);

            var window1 = new SCNNode();
            window1.Geometry = SCNPlane.Create(0.02f, 0.02f);
            window1.Geometry.FirstMaterial.Diffuse.Contents = UIColor.White;
            window1.Position = new SCNVector3(0.03f, 0.025f, 0.051f);
            boxNode.AddChildNode(window1);

            var window2 = new SCNNode();
            window2.Geometry = SCNPlane.Create(0.02f, 0.02f);
            window2.Geometry.FirstMaterial.Diffuse.Contents = UIColor.White;
            window2.Position = new SCNVector3(-0.03f, 0.025f, 0.051f);
            boxNode.AddChildNode(window2);
        }

        private void ShowDistinctNodes()
        {
            var size = 0.05f;
            var material = new SCNMaterial();
            material.Diffuse.Contents = UIColor.Red;

            var boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(size, size, size, 0);
            boxNode.Geometry.Materials = new SCNMaterial[] { material };
            boxNode.Position = new SCNVector3(0, 0, 0);

            var sphereNode = new SCNNode();
            sphereNode.Geometry = SCNSphere.Create(size / 2);
            sphereNode.Geometry.Materials = new SCNMaterial[] { material };
            sphereNode.Position = new SCNVector3(0.1f, 0, 0);

            var torusNode = new SCNNode();
            torusNode.Geometry = SCNTorus.Create(size / 2, size / 6);
            torusNode.Geometry.Materials = new SCNMaterial[] { material };
            torusNode.Position = new SCNVector3(0.1f, 0, 0);

            var tubeNode = new SCNNode();
            tubeNode.Geometry = SCNTube.Create(size / 2, (size / 2), size * 2);
            tubeNode.Geometry.Materials = new SCNMaterial[] { material };
            tubeNode.Position = new SCNVector3(0.1f, 0, 0);

            var coneNode = new SCNNode();
            coneNode.Geometry = SCNCone.Create(0.01f, (size / 2), size * 2);
            coneNode.Geometry.Materials = new SCNMaterial[] { material };
            coneNode.Position = new SCNVector3(0.1f, 0, 0);

            var cylinderNode = new SCNNode();
            cylinderNode.Geometry = SCNCylinder.Create(size / 2, size);
            cylinderNode.Geometry.Materials = new SCNMaterial[] { material };
            cylinderNode.Position = new SCNVector3(01f, 0, 0);

            var pyramidNode = new SCNNode();
            pyramidNode.Geometry = SCNPyramid.Create(size, size, size);
            pyramidNode.Geometry.Materials = new SCNMaterial[] { material };
            pyramidNode.Position = new SCNVector3(0.1f, 0, 0);

            var planeNode = new SCNNode();
            planeNode.Geometry = SCNPlane.Create(size, size);
            planeNode.Geometry.Materials = new SCNMaterial[] { material };
            planeNode.Position = new SCNVector3(0.1f, 0, 0);

            var capsuleNode = new SCNNode();
            capsuleNode.Geometry = SCNCapsule.Create(0.1f, 0.3f);
            capsuleNode.Geometry.Materials = new SCNMaterial[] { material };
            capsuleNode.Position = new SCNVector3(0.1f, 0, 0);


            sceneView.Scene.RootNode.AddChildNode(boxNode); //Se coloca boxNode en una posición absoluta respecto a rootNode
            boxNode.AddChildNode(sphereNode); //Se coloca sphereNode en una posición relativa a boxNode

            sphereNode.AddChildNode(torusNode);
            torusNode.AddChildNode(tubeNode);
            tubeNode.AddChildNode(coneNode);
            coneNode.AddChildNode(cylinderNode);
            cylinderNode.AddChildNode(pyramidNode);
            pyramidNode.AddChildNode(planeNode);
            planeNode.AddChildNode(capsuleNode);
        }

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}
