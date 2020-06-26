[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArVirtualPortalView), typeof(ARExample.iOS.Renderers.ArVirtualPortalViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System.ComponentModel;
    using System.Linq;
    using ARExample.Controls;
    using ARKit;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArVirtualPortalViewRenderer : ViewRenderer<ArVirtualPortalView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        internal bool PlaneDetected { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<ArVirtualPortalView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.RunSession = null;
                e.OldElement.PauseSession = null;
            }

            if (e.NewElement != null)
            {
                sceneView = new ARSCNView()
                {
                    AutoenablesDefaultLighting = true,
                    DebugOptions = ARSCNDebugOptions.ShowWorldOrigin,
                };

                sceneView.Frame = Bounds;

                e.NewElement.RunSession = RunSession;
                e.NewElement.PauseSession = PauseSession;

                SetNativeControl(sceneView);
                AddGestureRecognizers();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void RunSession()
        {
            config?.Dispose();
            sceneView?.Delegate?.Dispose();

            config = new ARWorldTrackingConfiguration();
            sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin;
            config.PlaneDetection = ARPlaneDetection.Horizontal;
            sceneView.Session.Run(config, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
            sceneView.Delegate = new ArPlaneScnViewDelegate(sceneView);

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        private void AddGestureRecognizers()
        {
            UITapGestureRecognizer tapGestureRecognizer = new UITapGestureRecognizer(HandleTap);
            sceneView.AddGestureRecognizer(tapGestureRecognizer);
        }

        private void HandleTap(UITapGestureRecognizer sender)
        {
            ARSCNView tapScene = sender.View as ARSCNView;
            if (tapScene == null)
                return;

            CoreGraphics.CGPoint tapLocation = sender.LocationInView(tapScene);
            ARHitTestResult[] hitTest = tapScene.HitTest(tapLocation, ARHitTestResultType.ExistingPlaneUsingExtent);
            if (hitTest?.Any() != true)
                return;

            ShowPortal(hitTest.FirstOrDefault());
        }

        private void ShowPortal(ARHitTestResult hitTestResult)
        {
            //let portalScene = SCNScene(named: "Portal.scnassets/Portal.scn")
            //let portalNode = portalScene!.rootNode.childNode(withName: "Portal", recursively: false)!
            //let transform = hitTestResult.worldTransform
            //let planeXposition = transform.columns.3.x
            //let planeYposition = transform.columns.3.y
            //let planeZposition = transform.columns.3.z
            //portalNode.position = SCNVector3(planeXposition, planeYposition, planeZposition)
            //self.sceneView.scene.rootNode.addChildNode(portalNode)
            //self.addPlane(nodeName: "roof", portalNode: portalNode, imageName: "top")
            //self.addPlane(nodeName: "floor", portalNode: portalNode, imageName: "bottom")
            //self.addWalls(nodeName: "backWall", portalNode: portalNode, imageName: "back")
            //self.addWalls(nodeName: "sideWallA", portalNode: portalNode, imageName: "sideA")
            //self.addWalls(nodeName: "sideWallB", portalNode: portalNode, imageName: "sideB")
            //self.addWalls(nodeName: "sideDoorA", portalNode: portalNode, imageName: "sideDoorA")
            //self.addWalls(nodeName: "sideDoorB", portalNode: portalNode, imageName: "sideDoorB")
        }

        private void AddWalls(string nodeName, SCNNode portalNode, string imageName)
        {
            //let child = portalNode.childNode(withName: nodeName, recursively: true)
            //child?.geometry?.firstMaterial?.diffuse.contents = UIImage(named: "Portal.scnassets/\(imageName).png")
            //child?.renderingOrder = 200
            //if let mask = child?.childNode(withName: "mask", recursively: false) {
            //        mask.geometry?.firstMaterial?.transparency = 0.000001
            //}
        }

        private void AddPlane(string nodeName, SCNNode portalNode, string imageName)
        {
            //let child = portalNode.childNode(withName: nodeName, recursively: true)
            //child?.geometry?.firstMaterial?.diffuse.contents = UIImage(named: "Portal.scnassets/\(imageName).png")
            //child?.renderingOrder = 200
        }
    }
}
