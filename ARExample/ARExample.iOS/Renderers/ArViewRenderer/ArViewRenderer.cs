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
            config = new ARWorldTrackingConfiguration();
            sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin;
            sceneView.Session.Run(config, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;

            //sceneView.Session.Run(new ARWorldTrackingConfiguration
            //{
            //    AutoFocusEnabled = true,
            //    PlaneDetection = ARPlaneDetection.Horizontal,
            //    LightEstimationEnabled = true,
            //    WorldAlignment = ARWorldAlignment.GravityAndHeading
            //}, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
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
            material.Diffuse.Contents = UIColor.Red;

            SCNNode boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.1f, 0.1f, 0.1f, 0);
            boxNode.Geometry.Materials = new SCNMaterial[] { material };
            boxNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            boxNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Blue; //Color del objeto
            boxNode.Position = new SCNVector3(0, 0, -0.3f);

            sceneView.Scene.RootNode.AddChildNode(boxNode);
        }



















        private void ShowRotatedCube()
        {
            CubeNode cubeNode = new CubeNode(0.05f, UIColor.Red, 0);
            cubeNode.Position = new SCNVector3(0, 0, 0);
            cubeNode.Rotation = new SCNVector4(0, 0, 1, ConvertDegreesToRadians(45));
            sceneView.Scene.RootNode.AddChildNode(cubeNode);
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
            torusNode.Position = new SCNVector3(0.2f, 0, 0);

            var tubeNode = new SCNNode();
            tubeNode.Geometry = SCNTube.Create(size / 2, (size / 2), size * 2);
            tubeNode.Geometry.Materials = new SCNMaterial[] { material };
            tubeNode.Position = new SCNVector3(0.3f, 0, 0);

            var coneNode = new SCNNode();
            coneNode.Geometry = SCNCone.Create(0.01f, (size / 2), size * 2);
            coneNode.Geometry.Materials = new SCNMaterial[] { material };
            coneNode.Position = new SCNVector3(0.4f, 0, 0);

            var cylinderNode = new SCNNode();
            cylinderNode.Geometry = SCNCylinder.Create(size / 2, size);
            cylinderNode.Geometry.Materials = new SCNMaterial[] { material };
            cylinderNode.Position = new SCNVector3(0.5f, 0, 0);

            var pyramidNode = new SCNNode();
            pyramidNode.Geometry = SCNPyramid.Create(size, size, size);
            pyramidNode.Geometry.Materials = new SCNMaterial[] { material };
            pyramidNode.Position = new SCNVector3(0.6f, 0, 0);

            var planeNode = new SCNNode();
            planeNode.Geometry = SCNPlane.Create(size, size);
            planeNode.Geometry.Materials = new SCNMaterial[] { material };
            planeNode.Position = new SCNVector3(0.7f, 0, 0);

            sceneView.Scene.RootNode.AddChildNode(boxNode);
            sceneView.Scene.RootNode.AddChildNode(sphereNode);
            sceneView.Scene.RootNode.AddChildNode(torusNode);
            sceneView.Scene.RootNode.AddChildNode(tubeNode);
            sceneView.Scene.RootNode.AddChildNode(coneNode);
            sceneView.Scene.RootNode.AddChildNode(cylinderNode);
            sceneView.Scene.RootNode.AddChildNode(pyramidNode);
            sceneView.Scene.RootNode.AddChildNode(planeNode);
        }

        private void ShowCustomNodes()
        {
            //Add a cube
            CubeNode cubeNode = new CubeNode(0.05f, UIColor.Red, 0);
            cubeNode.Position = new SCNVector3(0, 0, 0);
            cubeNode.Rotation = new SCNVector4(0, 0, 1, ConvertDegreesToRadians(45));
            sceneView.Scene.RootNode.AddChildNode(cubeNode);

            //Add a sphere
            SphereNode sphereNode = new SphereNode(0.03f, UIColor.White, 0.7f);
            sphereNode.Position = new SCNVector3(0.1f, 0, 0);
            sceneView.Scene.RootNode.AddChildNode(sphereNode);

            //Text
            TextNode textNode = new TextNode("Hello Universe", UIColor.Orange, 0.5f);
            textNode.Position = new SCNVector3(0, -0.6f, 0);
            sceneView.Scene.RootNode.AddChildNode(textNode);

            //Image
            ImageNode imageNode = new ImageNode("Monkey1.jpg", 0, 0.6f);
            imageNode.Position = new SCNVector3(0, 0, 0);
            sceneView.Scene.RootNode.AddChildNode(imageNode);
        }

        private void ShowNodesInAllSides()
        {
            var size = 0.05f;
            var distanceAway = 1f;

            // Front
            SphereNode cubeNodeFront = new SphereNode(size, UIColor.Yellow, 1);
            cubeNodeFront.Position = new SCNVector3(distanceAway, 0, 0);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeFront);

            // Back
            SphereNode cubeNodeBack = new SphereNode(size, UIColor.Blue, 0.9f);
            cubeNodeBack.Position = new SCNVector3(-distanceAway, 0, 0);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeBack);

            // Right
            SphereNode cubeNodeRight = new SphereNode(size, UIColor.Red, 0.8f);
            cubeNodeRight.Position = new SCNVector3(0, 0, distanceAway);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeRight);

            // Left
            SphereNode cubeNodeLeft = new SphereNode(size, UIColor.Green, 0.7f);
            cubeNodeLeft.Position = new SCNVector3(0, 0, -distanceAway);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeLeft);

            // Above
            SphereNode cubeNodeAbove = new SphereNode(size, UIColor.Orange, 0.6f);
            cubeNodeAbove.Position = new SCNVector3(0, distanceAway, 0);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeAbove);

            // Below
            SphereNode cubeNodeBelow = new SphereNode(size, UIColor.Purple, 0.5f);
            cubeNodeBelow.Position = new SCNVector3(0, -distanceAway, 0);
            sceneView.Scene.RootNode.AddChildNode(cubeNodeBelow);
        }

        

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}
