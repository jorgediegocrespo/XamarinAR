[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArIkeaView), typeof(ARExample.iOS.Renderers.ArIkeaViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using ARExample.Controls;
    using ARKit;
    using CoreAnimation;
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

            UIPinchGestureRecognizer pichGestureRecognizer = new UIPinchGestureRecognizer(HandlerPick);
            sceneView.AddGestureRecognizer(pichGestureRecognizer);

            UILongPressGestureRecognizer longPressRecognizer = new UILongPressGestureRecognizer(HandlerLongPress);
            sceneView.AddGestureRecognizer(longPressRecognizer);
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
                    SCNNode node = tableScene.RootNode.FindChildNode("table", false);
                    CenterPivot(node);
                    return node;
            }
        }

        private void CenterPivot(SCNNode node)
        {
            var min = new SCNVector3();
            var max = new SCNVector3();
            if (node.GetBoundingBox(ref min, ref max))
            {
                node.Pivot = new SCNMatrix4(CATransform3D.MakeTranslation(
                    min.X + (max.X - min.X) / 2,
                    min.Y + (max.Y - min.Y) / 2,
                    min.Z + (max.Z - min.Z) / 2));
            }
        }

        private void HandlerPick(UIPinchGestureRecognizer sender)
        {
            ARSCNView pinchScene = sender.View as ARSCNView;
            CoreGraphics.CGPoint pinchLocation = sender.LocationInView(pinchScene);
            SCNHitTestResult[] hitTest = pinchScene.HitTest(pinchLocation, new SCNHitTestOptions());

            if (hitTest?.Any() != true)
                return;

            SCNNode node = hitTest.First().Node;
            SCNAction pinchAction = SCNAction.ScaleBy(sender.Scale, 0);
            node.RunAction(pinchAction);
            sender.Scale = 1.0f;
        }

        private void HandlerLongPress(UILongPressGestureRecognizer sender)
        {
            ARSCNView longPressScene = sender.View as ARSCNView;
            CoreGraphics.CGPoint longPressLocation = sender.LocationInView(longPressScene);
            SCNHitTestResult[] hitTest = longPressScene.HitTest(longPressLocation, new SCNHitTestOptions());

            if (hitTest?.Any() != true)
                return;

            SCNNode node = hitTest.First().Node;

            if (sender.State == UIGestureRecognizerState.Began)
            {
                SCNAction rotateAction = SCNAction.RotateBy(0, ConvertDegreesToRadians(360f), 0, 1f);
                SCNAction rotateForever = SCNAction.RepeatActionForever(rotateAction);
                node.RunAction(rotateForever);
            }
            else if (sender.State == UIGestureRecognizerState.Ended)
            {
                node.RemoveAllActions();
            }
        }

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}