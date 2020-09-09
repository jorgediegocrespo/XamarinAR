[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArGameView), typeof(ARExample.iOS.Renderers.ArGameViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using ARExample.Controls;
    using ARKit;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArGameViewRenderer : ViewRenderer<ArGameView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;
        private readonly Random random = new Random();
        private bool isHandlingTap = false;

        protected override void OnElementChanged(ElementChangedEventArgs<ArGameView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.RunSession = null;
                e.OldElement.PauseSession = null;
                e.OldElement.PlayGame = null;
                e.OldElement.ResetGame = null;
            }

            if (e.NewElement != null)
            {
                sceneView = new ARSCNView()
                {
                    AutoenablesDefaultLighting = true
                };

                sceneView.Frame = Bounds;

                e.NewElement.RunSession = RunSession;
                e.NewElement.PauseSession = PauseSession;
                e.NewElement.PlayGame = PlayGame;
                e.NewElement.ResetGame = ResetGame;

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
            sceneView.Session.Run(config, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        private void PlayGame()
        {
            AddNode();
        }

        private void ResetGame()
        { }

        private void AddNode()
        {
            if (sceneView.Scene.RootNode.FindChildNode("Jellyfish", true) != null)
                return;

            //TODO 5.2 Añadiendo modelos 3D
            SCNScene jellyfishScn = SCNScene.FromFile("art.scnassets/Jellyfish");
            SCNNode jellyfishNode = jellyfishScn.RootNode.FindChildNode("Jellyfish", false);


            double x = (double)random.Next(-1000, 1000) / 1000d;
            float xf = Convert.ToSingle(x);

            double y = (double)random.Next(-1000, 1000) / 1000d;
            float yf = Convert.ToSingle(x);

            double z = (double)random.Next(-1000, 1000) / 1000d;
            float zf = Convert.ToSingle(x);

            jellyfishNode.Position = new SCNVector3(xf, yf, zf);
            sceneView.Scene.RootNode.AddChildNode(jellyfishNode);
        }

        private async void HandleTap(UITapGestureRecognizer sender)
        {
            if (isHandlingTap)
                return;

            try
            {
                isHandlingTap = true;
                SCNView sceneViewTappedOn = sender.View as SCNView;
                CoreGraphics.CGPoint touchCoordinates = sender.LocationInView(sceneViewTappedOn);
                SCNHitTestResult[] hitTest = sceneViewTappedOn.HitTest(touchCoordinates, new SCNHitTestOptions());

                if (!hitTest.Any())
                    return;

                SCNNode pressedNode = hitTest.FirstOrDefault().Node;
                await AnimateNode(pressedNode);
                RemoveAllNodes();
                AddNode();
            }
            finally
            {
                isHandlingTap = false;
            }
        }

        private async Task AnimateNode(SCNNode node)
        {
            var sourcePosition = new SCNVector3(node.PresentationNode.Position.X, node.PresentationNode.Position.Y, node.PresentationNode.Position.Z);
            var targetPosition = new SCNVector3(node.PresentationNode.Position.X - 0.2f, node.PresentationNode.Position.Y - 0.2f, node.PresentationNode.Position.Z - 0.2f);

            for (int i = 0; i < 3; i++)
            {
                SCNAction toTarget = SCNAction.MoveTo(targetPosition, 0.2f);
                node.RunAction(toTarget);
                await Task.Delay(200);

                SCNAction toSource = SCNAction.MoveTo(sourcePosition, 0.2f);
                node.RunAction(toSource);
                await Task.Delay(200);
            }            
        }

        private void RemoveAllNodes()
        {
            while (sceneView.Scene.RootNode.ChildNodes.Length > 0)
            {
                sceneView.Scene.RootNode.ChildNodes[0].RemoveFromParentNode();
            }
        }
    }
}
