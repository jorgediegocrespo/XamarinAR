[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArVehicularPhysicsView), typeof(ARExample.iOS.Renderers.ArVehicularPhysicsViewRenderer))]
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

    public class ArVehicularPhysicsViewRenderer : ViewRenderer<ArVehicularPhysicsView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<ArVehicularPhysicsView> e)
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
                //AddGestureRecognizers();
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
    }
}
