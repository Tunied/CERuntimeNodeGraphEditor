using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_Init : MonoBehaviour
    {
        public RNG_Demo1_AssetsFactory AssetsFactory;
        public GameObject ContentCanvas;
        public GameObject ControlCanvas;
        public GameObject GUICanvas;


        private void Start()
        {
            RNG.Display.ContentCanvas = ContentCanvas;
            RNG.Display.ControlCanvas = ControlCanvas;
            RNG.Display.GUICanvas = GUICanvas;
            RNG.Assets = AssetsFactory;
        }
    }
}