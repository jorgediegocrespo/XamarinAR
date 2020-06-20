[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArIkeaView), typeof(ARExample.iOS.Renderers.ArIkeaViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using ARExample.Controls;
    using ARKit;
    using OpenTK;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArIkeaViewRenderer : ViewRenderer<ArIkeaView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<ArIkeaView> e)
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

                var tapGestureRecognizer = new UITapGestureRecognizer(HandleTap);
                sceneView.AddGestureRecognizer(tapGestureRecognizer);
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

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        private void HandleTap(UITapGestureRecognizer sender)
        {
            ARSCNView tapScene = sender.View as ARSCNView;
            CoreGraphics.CGPoint tapLocation = sender.LocationInView(tapScene);
            ARHitTestResult[] hitTest = tapScene.HitTest(tapLocation, ARHitTestResultType.ExistingPlaneUsingExtent);
            if (hitTest?.Any() != true)
                return;

            AddSelectedItem(hitTest.FirstOrDefault());
        }

        private void AddSelectedItem(ARHitTestResult hitTestResult)
        {
            SCNNode node = GetSelectedNode();
            NMatrix4 transform = hitTestResult.WorldTransform;
            Vector4 thirdColumn = transform.Column3;
            node.Position = new SCNVector3(thirdColumn.X, thirdColumn.Y, thirdColumn.Z);

            sceneView.Scene.RootNode.AddChildNode(node);
        }

        private SCNNode GetSelectedNode()
        {
            switch (Element.SelectedItem)
            {
                case IkeaItem.Cup:
                    SCNScene cupScene = SCNScene.FromFile("art.scnassets/cup.scn");
                    return cupScene.RootNode.FindChildNode("cup", false);
                case IkeaItem.Vase:
                    SCNScene vaseScene = SCNScene.FromFile("art.scnassets/vase.scn");
                    return vaseScene.RootNode.FindChildNode("vase", false);
                case IkeaItem.Boxing:
                    SCNScene boxingScene = SCNScene.FromFile("art.scnassets/boxing.scn");
                    return boxingScene.RootNode.FindChildNode("boxing", false);
                case IkeaItem.Table:
                default:
                    SCNScene tableScene = SCNScene.FromFile("art.scnassets/table.scn");
                    return tableScene.RootNode.FindChildNode("table", false);
            }
        }
    }
}