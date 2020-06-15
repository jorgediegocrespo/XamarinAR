using ARKit;
using Foundation;
using SceneKit;

namespace ARExample.iOS.Renderers
{
    public class CustomArScnViewDelegate : ARSCNViewDelegate
    { 
        private readonly ARSCNView sceneView;

        private SCNVector3 currentPositionOfCamera;

        public CustomArScnViewDelegate(ARSCNView sceneView)
        {
            this.sceneView = sceneView;
        }

        public SCNVector3 CurrentPositionOfCamera
        {
            get => currentPositionOfCamera;
            private set
            {
                currentPositionOfCamera = value;
            }
        }

        [Export("renderer:willRenderScene:atTime:")]
        public override void WillRenderScene(SceneKit.ISCNSceneRenderer renderer, SceneKit.SCNScene scene, double timeInSeconds)
        {
            SCNNode pointOfView = sceneView.PointOfView;
            if (pointOfView == null)
                return;

            SCNMatrix4 transform = pointOfView.Transform;
            SCNVector3 orientation = new SCNVector3(-transform.M31, -transform.M32, -transform.M33);
            SCNVector3 location = new SCNVector3(transform.M41, transform.M42, transform.M43);
            CurrentPositionOfCamera = orientation + location;
        }
    }
}
