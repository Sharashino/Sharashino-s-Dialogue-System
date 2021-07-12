using System;
using UnityEngine;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Auxiliary class for creating StartNodes
namespace SDS.DialogueSystem.Nodes
{
    public class StartNode : BaseNode
    {  
        // Initializing as empty
        public StartNode()
        {
            
        }

        // Spawning StartNode
        public StartNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
            styleSheets.Add(styleSheet);
            
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Start";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddOutputPort("Output", Port.Capacity.Single);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

