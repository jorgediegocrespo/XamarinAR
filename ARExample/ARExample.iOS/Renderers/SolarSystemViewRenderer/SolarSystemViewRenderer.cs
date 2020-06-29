[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.SolarSystemView), typeof(ARExample.iOS.Renderers.SolarSystemViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System;
    using System.ComponentModel;
    using ARExample.Controls;
    using ARKit;
    using SceneKit;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class SolarSystemViewRenderer : ViewRenderer<SolarSystemView, ARSCNView>
    {
        private ARSCNView sceneView;
        private ARWorldTrackingConfiguration config;

        protected override void OnElementChanged(ElementChangedEventArgs<SolarSystemView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.RunSession = null;
                e.OldElement.PauseSession = null;
                e.OldElement.DrawSolarSystem = null;
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
                e.NewElement.DrawSolarSystem = DrawSolarSystem;

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
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        //TODO 3.1 Añadiendo pieles
        private void DrawSolarSystem()
        {
            SCNNode sun = new SCNNode();
            sun.Geometry = SCNSphere.Create(0.2f);
            sun.Geometry.FirstMaterial.Diffuse.Contents = new UIImage("SunTexture.jpg");
            sun.Position = new SCNVector3(0, 0, -1);
            sceneView.Scene.RootNode.AddChildNode(sun);

            AddPlanet(sun.Position, 0.2f, "EarthTexture.jpg", "EarthSpecular.tif", "EarthClouds.jpg", "EarthNormal.tif", 1.2f, 8, 14);
            AddPlanet(sun.Position, 0.1f, "VenusTexture.jpg", null, "VenusEmission.jpg", null, 0.7f, 6, 10);
        }

        private void AddPlanet(SCNVector3 sunPosition, float radious, string diffuse, string specular, string emission, string normal, float xPosition, double selfRotation, double sunRotation)
        {
            SCNNode parent = new SCNNode();
            parent.Position = sunPosition;
            sceneView.Scene.RootNode.AddChildNode(parent);

            SCNNode planet = new SCNNode();
            planet.Geometry = SCNSphere.Create(radious);
            planet.Geometry.FirstMaterial.Diffuse.Contents = string.IsNullOrWhiteSpace(diffuse) ? null : new UIImage(diffuse);
            planet.Geometry.FirstMaterial.Specular.Contents = string.IsNullOrWhiteSpace(specular) ? null : new UIImage(specular);
            planet.Geometry.FirstMaterial.Emission.Contents = string.IsNullOrWhiteSpace(emission) ? null : new UIImage(emission);
            planet.Geometry.FirstMaterial.Normal.Contents = string.IsNullOrWhiteSpace(normal) ? null : new UIImage(normal);
            planet.Position = new SCNVector3(xPosition, 0, 0);

            //TODO 3.2 Movimientos
            SCNAction selfAction = SCNAction.RotateBy(0, ConvertDegreesToRadians(360), 0, selfRotation);
            SCNAction selfForever = SCNAction.RepeatActionForever(selfAction);
            planet.RunAction(selfForever);

            SCNAction sunAction = SCNAction.RotateBy(0, ConvertDegreesToRadians(360), 0, sunRotation);
            SCNAction sunForever = SCNAction.RepeatActionForever(sunAction);
            parent.RunAction(sunForever);

            parent.AddChildNode(planet);
        }

        private float ConvertDegreesToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}
