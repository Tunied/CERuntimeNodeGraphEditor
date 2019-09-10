using CERuntimeNodeGraph.Code.Core.DragDropLogic;
using CERuntimeNodeGraph.Code.GUI.RenderObject;
using DefaultNamespace;
using UnityEngine;

namespace CERuntimeNodeGraph.Code
{
    public static class RNG_ClassWrap
    {
        //=================
        //== Data
        //=================
        public class DataRoot
        {
            public RNG_DataGUI GUI = new RNG_DataGUI();
            public RNG_DataGraph Graph = new RNG_DataGraph();
        }

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
        }

        public class SubPortalLogicRoot
        {
            public RNG_PortalDragToLinkLogic Link = new RNG_PortalDragToLinkLogic();
        }

        public class SubPortalLinkLogicRoot
        {
            public RNG_DataPortalLink Delete = new RNG_DataPortalLink();
        }

        public class SubGraphLogicRoot
        {
            public RNG_GraphDragDropLogicFacade DragDrop = new RNG_GraphDragDropLogicFacade();
            public RNG_GraphZoomInOutLogic Zoom = new RNG_GraphZoomInOutLogic();
        }
    }
}