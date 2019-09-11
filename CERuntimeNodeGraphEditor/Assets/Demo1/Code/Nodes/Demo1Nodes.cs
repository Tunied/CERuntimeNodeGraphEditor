using CERuntimeNodeGraph.Code.GUI.RenderObject;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Nodes
{
    public class Demo1Nodes : RNG_NodeBase
    {
        public Color defaultOutlineColor;
        public Color onSelectedOutlineColor;

        public Image outline;

        protected override void Initialize()
        {
            base.Initialize();
            OnNodeSelectedChange(IsNodeSelected);
        }

        protected override void OnNodeSelectedChange(bool _isNowSelected)
        {
            base.OnNodeSelectedChange(_isNowSelected);
            outline.color = _isNowSelected ? onSelectedOutlineColor : defaultOutlineColor;
        }
    }
}