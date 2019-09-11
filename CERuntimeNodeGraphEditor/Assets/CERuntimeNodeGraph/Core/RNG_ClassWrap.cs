using System.Collections.Generic;
using CERuntimeNodeGraph.Code.Core.DragDropLogic;
using CERuntimeNodeGraph.Code.GUI.RenderObject;
using DefaultNamespace;
using UnityEngine;

namespace CERuntimeNodeGraph.Code
{
    public static class RNG_ClassWrap
    {
        //=================
        //== Logic
        //=================

        public class LogicRoot
        {
            public SubGraphLogicRoot Graph = new SubGraphLogicRoot();
            public SubNodeLogicRoot Node = new SubNodeLogicRoot();
            public SubPortalLogicRoot Portal = new SubPortalLogicRoot();
            public SubPortalLinkLogicRoot PortalLink = new SubPortalLinkLogicRoot();
        }

        public class SubNodeLogicRoot
        {
            public RNG_NodeDeleteSelectedLogic Delete = new RNG_NodeDeleteSelectedLogic();
            public RNG_NodeDragToMoveLogic Move = new RNG_NodeDragToMoveLogic();
            public RNG_NodeRectSelectLogic Select = new RNG_NodeRectSelectLogic();
            public RNG_NodeSearchLogic Search = new RNG_NodeSearchLogic();
        }

        public class SubPortalLogicRoot
        {
            public RNG_PortalDragToLinkLogic Link = new RNG_PortalDragToLinkLogic();
            public RNG_PortalSearchLogic Serach = new RNG_PortalSearchLogic();
        }

        public class SubPortalLinkLogicRoot
        {
            public RNG_PortalLinkDeleteSelectedLogic Delete = new RNG_PortalLinkDeleteSelectedLogic();
            public RNG_PortalLinkSearchLogic Search = new RNG_PortalLinkSearchLogic();
        }

        public class SubGraphLogicRoot
        {
            public RNG_GraphDragDropLogicFacade DragDrop = new RNG_GraphDragDropLogicFacade();
            public RNG_GraphZoomInOutLogic Zoom = new RNG_GraphZoomInOutLogic();
        }

        public class DisplayRoot
        {
            public Transform ContentRoot;
            public Transform TopLaterRoot;
            public Transform GraphLayer;
            public Transform BottomLayerRoot;

            public List<RNG_NodeBase> allNodeList = new List<RNG_NodeBase>();
            public List<RNG_PortalLinkBase> allLinkList = new List<RNG_PortalLinkBase>();

            /// <summary>
            /// 当前正在拖拽的Portal
            /// </summary>
            public RNG_PortalBase nowSelectPortal;

            /// <summary>
            /// 当前拖拽中最终会被连接的Portal
            /// (在OnDrag中触发另外一个Portal的OnPointerEnter并且通过IsCanJoin判断)
            /// </summary>
            public RNG_PortalBase nowWillLinkPortal;
        }
    }
}