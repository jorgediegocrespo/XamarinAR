﻿[assembly: Xamarin.Forms.ExportRenderer(typeof(ARExample.Controls.ArVehicularPhysicsView), typeof(ARExample.iOS.Renderers.ArVehicularPhysicsViewRenderer))]
namespace ARExample.iOS.Renderers
{
    using System.ComponentModel;
    using ARExample.Controls;
    using ARKit;
    using SceneKit;
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
                e.OldElement.AddCar = null;

                e.OldElement.GoLeft = null;
                e.OldElement.GoRight = null;
                e.OldElement.GoAhead = null;
                e.OldElement.GoBack = null;
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
                e.NewElement.AddCar = AddCar;

                e.NewElement.GoLeft = GoLeft;
                e.NewElement.GoRight = GoRight;
                e.NewElement.GoAhead = GoAhead;
                e.NewElement.GoBack = GoBack;

                SetNativeControl(sceneView);
            }
        }

        public SCNPhysicsVehicle PhysicsVehicle { get; private set; }
        public float Orientation { get; private set; }
        public float Speed { get; private set; }

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
            sceneView.Delegate = new ArVehicularPhysicsScnViewDelegate(this);

            //Permite añadir reflejos a los objetos de la escena
            sceneView.AutoenablesDefaultLighting = true;
        }

        private void PauseSession()
        {
            sceneView.Session.Pause();
        }

        private void AddCar()
        {
            SCNNode pointOfView = sceneView.PointOfView;
            if (pointOfView == null)
                return;

            SCNMatrix4 transform = pointOfView.Transform;
            SCNVector3 orientation = new SCNVector3(-transform.M31, -transform.M32, -transform.M33);
            SCNVector3 location = new SCNVector3(transform.M41, transform.M42, transform.M43);
            SCNVector3 currentPositionOfCamera = orientation + location;

            //TODO 4.2 Creando el coche
            SCNScene carScene = SCNScene.FromFile("art.scnassets/CarScene.scn");
            SCNNode carNode = carScene.RootNode.FindChildNode("frame", false);

            SCNNode frontLeftWheel = carNode.FindChildNode("frontLeftParent", false);
            SCNPhysicsVehicleWheel v_frontLeftWheel = SCNPhysicsVehicleWheel.Create(frontLeftWheel);

            SCNNode frontRightWheel = carNode.FindChildNode("frontRightParent", false);
            SCNPhysicsVehicleWheel v_frontRightWheel = SCNPhysicsVehicleWheel.Create(frontRightWheel);

            SCNNode rearLeftWheel = carNode.FindChildNode("rearLeftParent", false);
            SCNPhysicsVehicleWheel v_rearLeftWheel = SCNPhysicsVehicleWheel.Create(rearLeftWheel);

            SCNNode rearRightWheel = carNode.FindChildNode("rearRightParent", false);
            SCNPhysicsVehicleWheel v_rearRightWheel = SCNPhysicsVehicleWheel.Create(rearRightWheel);

            carNode.Position = currentPositionOfCamera;

            SCNPhysicsBody body = SCNPhysicsBody.CreateBody(SCNPhysicsBodyType.Dynamic, SCNPhysicsShape.Create(carNode, keepAsCompound: true));
            carNode.PhysicsBody = body;
            PhysicsVehicle = SCNPhysicsVehicle.Create(carNode.PhysicsBody, new SCNPhysicsVehicleWheel[] { v_frontLeftWheel, v_frontRightWheel, v_rearLeftWheel, v_rearRightWheel });

            sceneView.Scene.PhysicsWorld.AddBehavior(PhysicsVehicle);
            sceneView.Scene.RootNode.AddChildNode(carNode);
        }

        //TODO 4.3 Moviemdo el vehículo
        private void GoLeft()
        {
            Orientation -= 0.1f;
        }

        private void GoRight()
        {
            Orientation += 0.1f;
        }

        private void GoAhead()
        {
            Speed += 1f;
        }

        private void GoBack()
        {
            Speed -= 1f;
        }
    }
}
