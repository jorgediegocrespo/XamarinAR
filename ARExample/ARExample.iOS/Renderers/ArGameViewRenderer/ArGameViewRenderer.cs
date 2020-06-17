[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArGameView), typeof(ARExample.iOS.Renderers.ArGameViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System.ComponentModel;
    using System.Diagnostics;
    using ARExample.Controls;
    using ARKit;
    using ObjCRuntime;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArGameViewRenderer : ViewRenderer<ArGameView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

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
                    AutoenablesDefaultLighting = true,
                    //DebugOptions = ARSCNDebugOptions.ShowWorldOrigin,
                    //ShowsStatistics = true
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
            //sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin;
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
            SCNMaterial material = new SCNMaterial();
            material.Specular.Contents = UIColor.White; //Color del reflejo
            material.Diffuse.Contents = UIColor.Blue; //Color del objeto

            SCNNode boxNode = new SCNNode();
            boxNode.Geometry = SCNBox.Create(0.2f, 0.2f, 0.2f, 0);
            boxNode.Geometry.Materials = new SCNMaterial[] { material };
            boxNode.Position = new SCNVector3(0, 0, -1f);

            sceneView.Scene.RootNode.AddChildNode(boxNode);
        }

        private void HandleTap(UITapGestureRecognizer sender)
        {
            SCNView sceneViewTappedOn = sender.View as SCNView;
            CoreGraphics.CGPoint touchCoordinates = sender.LocationInView(sceneViewTappedOn);
            SCNHitTestResult[] hitTest = sceneViewTappedOn.HitTest(touchCoordinates, new SCNHitTestOptions());

            Debug.WriteLine($"Node pressed {hitTest.Length}");
        }
    }
}
