using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_Init : MonoBehaviour
    {
        public RNG_Demo1_AssetsFactory AssetsFactory;
        public Transform ContentRoot;
        public Transform TopLaterRoot;
        public Transform GraphLayer;
        public Transform BottomLayerRoot;


        private void Start()
        {
            RNG.Display.ContentRoot = ContentRoot;
            RNG.Display.BottomLayerRoot = BottomLayerRoot;
            RNG.Display.TopLaterRoot = TopLaterRoot;
            RNG.Display.GraphLayer = GraphLayer;
            RNG.Assets = AssetsFactory;
        }
    }
}