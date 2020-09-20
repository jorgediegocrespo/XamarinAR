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
            sceneView.Delegate = new ArVirtualPortalScnViewDelegate(this);

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

            if (!PlaneDetected)
                return;

            CoreGraphics.CGPoint tapLocation = sender.LocationInView(tapScene);
            ARHitTestResult[] hitTest = tapScene.HitTest(tapLocation, ARHitTestResultType.ExistingPlaneUsingExtent);
            if (hitTest?.Any() != true)
                return;

            ShowPortal(hitTest.FirstOrDefault());
        }

        private void ShowPortal(ARHitTestResult hitTestResult)
        {
            SCNScene portalScene = SCNScene.FromFile("art.scnassets/Portal.scn");
            SCNNode portalNode = portalScene.RootNode.FindChildNode("Portal", false);
            OpenTK.NMatrix4 transform = hitTestResult.WorldTransform;

            float planeXposition = transform.Column3.X;
            float planeYposition = transform.Column3.Y;
            float planeZposition = transform.Column3.Z;

            portalNode.Position = new SCNVector3(planeXposition, planeYposition, planeZposition);
            sceneView.Scene.RootNode.AddChildNode(portalNode);

            AddPlane("roof", portalNode, "portalTop.png");
            AddPlane("floor", portalNode, "portalBottom.png");
            AddWalls("backWall", portalNode, "portalBack.png");
            AddWalls("sideWallA", portalNode, "portalSideA.png");
            AddWalls("sideWallB", portalNode, "portalSideB.png");
            AddWalls("sideDoorA", portalNode, "portalSideDoorA.png");
            AddWalls("sideDoorB", portalNode, "portalSideDoorB.png");
        }

        private void AddWalls(string nodeName, SCNNode portalNode, string imageName)
        {
            SCNNode child = portalNode.FindChildNode(nodeName, true);
            child.Geometry.FirstMaterial.Diffuse.Contents = new UIImage(imageName);
            child.RenderingOrder = 200;
            SCNNode mask = child.FindChildNode("mask", false);
            if (mask != null)
            {
                mask.Geometry.FirstMaterial.Transparency = 0.000001f;
            }
        }

        private void AddPlane(string nodeName, SCNNode portalNode, string imageName)
        {
            SCNNode child = portalNode.FindChildNode(nodeName, true);
            child.Geometry.FirstMaterial.Diffuse.Contents = new UIImage(imageName);
            child.RenderingOrder = 200;
        }
    }
}
