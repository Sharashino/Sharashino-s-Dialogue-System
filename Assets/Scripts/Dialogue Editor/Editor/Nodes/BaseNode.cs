using UnityEngine;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Main class every node relies on
namespace SDS.DialogueSystem.Nodes
{
    public class BaseNode : Node
    {
        protected string nodeGuid;  // Unique node guid for easier connection
        protected DialogueGraphView graphView;  // Graph view node is getting displayed on
        protected DialogueEditorWindow editorWindow;    // Editor Window used to display GraphView
        protected Vector2 defaultNodeSize = new Vector2(200, 250);  // Default node size 

        public string NodeGuid { get => nodeGuid; set => nodeGuid = value; }
        
        // Initializing BaseNode with .css
        public BaseNode()
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
            styleSheets.Add(styleSheet);
        }

        // Adding output port to node, for possible connections
        public void AddOutputPort(string outputPortName, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = outputPortName;

            outputContainer.Add(outputPort);
        }

        // Adding input port to node, for possible connections
        public void AddInputPort(string inputPortName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = inputPortName;

            inputContainer.Add(inputPort);
        }

        // Instantiating basic node port to enable connections 
        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }
        
        // Loading node values into its fields
        public virtual void LoadValueInToField()
        {

        }
    }
}
