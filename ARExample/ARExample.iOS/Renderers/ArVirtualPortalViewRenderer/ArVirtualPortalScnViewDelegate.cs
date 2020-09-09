using ARKit;
using Foundation;
using SceneKit;

namespace ARExample.iOS.Renderers
{
    public class ArVirtualPortalScnViewDelegate : ARSCNViewDelegate
    {
        private readonly ArVirtualPortalViewRenderer arVirtualPortalViewRenderer;

        public ArVirtualPortalScnViewDelegate(ArVirtualPortalViewRenderer arVirtualPortalViewRenderer)
        {
            this.arVirtualPortalViewRenderer = arVirtualPortalViewRenderer;
        }

        [Export("renderer:didAddNode:forAnchor:")]
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor)
                arVirtualPortalViewRenderer.PlaneDetected = true;
            else
                arVirtualPortalViewRenderer.PlaneDetected = false;
        }
    }
}