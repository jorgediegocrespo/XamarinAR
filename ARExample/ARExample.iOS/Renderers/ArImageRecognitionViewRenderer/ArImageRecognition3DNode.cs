using SceneKit;
using System;

namespace ARExample.iOS.Renderers
{
    public class ArImageRecognition3DNode : SCNNode
    {
        public ArImageRecognition3DNode(nfloat width, nfloat length, SCNVector3 position)
        {
            SCNScene jellyfishScn = SCNScene.FromFile("art.scnassets/Jellyfish");
            SCNNode jellyfishNode = jellyfishScn.RootNode.FindChildNode("Jellyfish", false);
            
            //jellyfishNode.Scale = new SCNVector3();
            jellyfishNode.Position = position;

            AddChildNode(jellyfishNode);
        }
    }
}