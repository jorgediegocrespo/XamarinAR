[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArView), typeof(ARExample.iOS.Renderers.ArViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System.Linq;
    using ARExample.Controls;
    using ARKit;
    using CoreGraphics;
    using Foundation;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    //TODO 1.2 ¿Por dónde empezar?
    public class ArViewRenderer : ViewRenderer<ArView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<ArView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.RunSession = null;
                e.OldElement.PauseSession = null;
                e.OldElement.ResetSession = null;
                e.OldElement.AddBox = null;
                e.OldElement.AddSphere = null;
                e.OldElement.AddTorus = null;
                e.OldElement.AddTube = null;
                e.OldElement.AddCone = null;
                e.OldElement.AddCylinder = null;
                e.OldElement.AddPyramid = null;
                e.OldElement.AddPlane = null;
                e.OldElement.AddCapsule = null;
                e.OldElement.AddPath = null;
                e.OldElement.AddHouse = null;
                e.OldElement.AddRelativeNodes = null;
            }

            if (e.NewElement != null)
            {
                sceneView = new ARSCNView()
                {
                    AutoenablesDefaultLighting = true,
                    DebugOptions = ARSCNDebugOptions.ShowWorldOrigin,
                    ShowsStatistics = true
                };

                sceneView.Frame = Bounds;

                e.NewElement.RunSession = RunSession;
                e.NewElement.PauseSession = PauseSession;
                e.NewElement.ResetSession = ResetSession;
                e.NewElement.AddBox = AddBox;
                e.NewElement.AddSphere = AddSphere;
                e.NewElement.AddTorus = AddTorus;
                e.NewElement.AddTube = AddTube;
                e.NewElement.AddCone = AddCone;
                e.NewElement.AddCylinder = AddCylinder;
                e.NewElement.AddPyramid = AddPyramid;
                e.NewElement.AddPlane = AddPlane;
                e.NewElement.AddCapsule = AddCapsule;
                e.NewElement.AddPath = AddPath;
                e.NewElement.AddHouse = AddHouse;
                e.NewElement.AddRelativeNodes = AddRelativeNodes;

                SetNativeControl(sceneView);
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
            //sceneView.Delegate = new CustomArScnViewDelegate(sceneView);
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

        //TODO 2.1 Añadiendo elementos
        private void AddBox()
        {
            SCNNode boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.05f, 0.05f, 0.05f, 0);
            boxNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Red; //Color del objeto
            boxNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            boxNode.Position = new SCNVector3(0.1f, 0, 0);

            sceneView.Scene.RootNode.AddChildNode(boxNode);
        }

        private void AddSphere()
        {
            SCNNode sphereNode = new SCNNode();
            sphereNode.Geometry = SCNSphere.Create(0.05f / 2);
            sphereNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Blue; //Color del objeto
            sphereNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            sphereNode.Position = new SCNVector3(0, 0.1f, 0);

            sceneView.Scene.RootNode.AddChildNode(sphereNode);
        }

        private void AddTorus()
        {
            SCNNode torusNode = new SCNNode();
            torusNode.Geometry = SCNTorus.Create(0.05f / 2, 0.05f / 6);
            torusNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Black; //Color del objeto
            torusNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            torusNode.Position = new SCNVector3(-0.1f, 0, 0.1f);

            sceneView.Scene.RootNode.AddChildNode(torusNode);
        }

        private void AddTube()
        {
            SCNNode tubeNode = new SCNNode();
            tubeNode.Geometry = SCNTube.Create(0.05f / 2, (0.05f / 2), 0.05f * 2);
            tubeNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Yellow; //Color del objeto
            tubeNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            tubeNode.Position = new SCNVector3(0, -0.1f, 0);

            sceneView.Scene.RootNode.AddChildNode(tubeNode);
        }

        private void AddCone()
        {
            SCNNode coneNode = new SCNNode();
            coneNode.Geometry = SCNCone.Create(0.01f, (0.05f / 2), 0.05f * 2);
            coneNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.SystemPinkColor; //Color del objeto
            coneNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            coneNode.Position = new SCNVector3(0, 0, -0.1f);

            sceneView.Scene.RootNode.AddChildNode(coneNode);
        }

        private void AddCylinder()
        {
            SCNNode cylinderNode = new SCNNode();
            cylinderNode.Geometry = SCNCylinder.Create(0.05f / 2, 0.05f);
            cylinderNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.SystemPurpleColor; //Color del objeto
            cylinderNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            cylinderNode.Position = new SCNVector3(0.1f, 0.1f, 0);

            sceneView.Scene.RootNode.AddChildNode(cylinderNode);
        }

        private void AddPyramid()
        {
            SCNNode pyramidNode = new SCNNode();
            pyramidNode.Geometry = SCNPyramid.Create(0.05f, 0.05f, 0.05f);
            pyramidNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Magenta; //Color del objeto
            pyramidNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            pyramidNode.Position = new SCNVector3(0.1f, 0, 0.1f);

            sceneView.Scene.RootNode.AddChildNode(pyramidNode);
        }

        private void AddPlane()
        {
            SCNNode planeNode = new SCNNode();
            planeNode.Geometry = SCNPlane.Create(0.05f, 0.05f);
            planeNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Green; //Color del objeto
            planeNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            planeNode.Position = new SCNVector3(0, 0.1f, 0.1f);

            sceneView.Scene.RootNode.AddChildNode(planeNode);
        }

        private void AddCapsule()
        {
            SCNNode capsuleNode = new SCNNode();
            capsuleNode.Geometry = SCNCapsule.Create(0.1f, 0.3f);
            capsuleNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Orange; //Color del objeto
            capsuleNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            capsuleNode.Position = new SCNVector3(-0.1f, -0.1f, 0);

            sceneView.Scene.RootNode.AddChildNode(capsuleNode);
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

        private void AddRelativeNodes()
        {
            SCNNode boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.05f, 0.05f, 0.05f, 0);
            boxNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Red; //Color del objeto
            boxNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            boxNode.Position = new SCNVector3(0.1f, 0, 0);

            SCNNode sphereNode = new SCNNode();
            sphereNode.Geometry = SCNSphere.Create(0.05f / 2);
            sphereNode.Geometry.FirstMaterial.Diffuse.Contents = UIColor.Blue; //Color del objeto
            sphereNode.Geometry.FirstMaterial.Specular.Contents = UIColor.White; //Color del reflejo
            sphereNode.Position = new SCNVector3(0.1f, 0, 0);

            sceneView.Scene.RootNode.AddChildNode(boxNode); //Se coloca boxNode en una posición absoluta respecto a rootNode
            boxNode.AddChildNode(sphereNode); //Se coloca sphereNode en una posición relativa a boxNode
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            //TODO 2.2 Interactuando con elementos de pantalla
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
    }
}
