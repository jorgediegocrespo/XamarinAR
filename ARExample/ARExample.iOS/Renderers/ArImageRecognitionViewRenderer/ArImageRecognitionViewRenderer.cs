[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArImageRecognitionView), typeof(ARExample.iOS.Renderers.ArImageRecognitionViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using ARExample.Controls;
    using ARKit;
    using Xamarin.Forms.Platform.iOS;

    public class ArImageRecognitionViewRenderer : ViewRenderer<ArImageRecognitionView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<ArImageRecognitionView> e)
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
            }
        }

        private void RunSession()
        {
            config?.Dispose();
            sceneView?.Delegate?.Dispose();

            sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin;

            config = new ARWorldTrackingConfiguration();
            config.AutoFocusEnabled = true;
            config.PlaneDetection = ARPlaneDetection.Horizontal | ARPlaneDetection.Vertical;
            config.LightEstimationEnabled = true;
            config.WorldAlignment = ARWorldAlignment.GravityAndHeading;
            config.DetectionImages = ARReferenceImage.GetReferenceImagesInGroup("AR Resources", null); ;
            config.MaximumNumberOfTrackedImages = 1;

            sceneView.Session.Run(config, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            sceneView.Delegate = new ArImageRecognitionScnViewDelegate();

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }
    }
}
