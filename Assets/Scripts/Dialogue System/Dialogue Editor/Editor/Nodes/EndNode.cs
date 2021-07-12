using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Auxiliary class for creating EndNodes
namespace SDS.DialogueSystem.Nodes
{
    public class EndNode : BaseNode
    {
        private EndNodeType endNodeType = EndNodeType.End;  // Type of this EndNode
        public EndNodeType EndNodeType { get => endNodeType; set => endNodeType = value; }

        private EnumField enumField; // Enum field instantiated in this EndNode

        // Initializing as empty
        public EndNode()
        {
            
        }

        public EndNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("EndNodeStyleSheet");
            styleSheets.Add(styleSheet);
            
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "End";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            
            // Creating enum field
            enumField = new EnumField()
            {
                value = endNodeType
            };

            enumField.Init(endNodeType);

            enumField.RegisterValueChangedCallback((value) =>
            {
                endNodeType = (EndNodeType)value.newValue;
            });
            enumField.SetValueWithoutNotify(endNodeType);

            mainContainer.Add(enumField);
        }
        
        // Loading value into EndNode field
        public override void LoadValueInToField()
        {
            enumField.SetValueWithoutNotify(endNodeType);
        }
    }
}