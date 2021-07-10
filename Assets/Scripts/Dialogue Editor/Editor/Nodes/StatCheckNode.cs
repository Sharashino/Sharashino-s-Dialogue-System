using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Auxiliary class for creating StatCheckNodes
namespace SDS.DialogueSystem.Nodes
{
    public class StatCheckNode : BaseNode
    {
        private StatCheckType statCheckType = StatCheckType.Luck;   // Stat check type in this StatCheckNode
        private string statCheckValue;  // Stat check value in this StatCheckNode (Strength - 12)
        
        // Fields instantiated in this StatCheckNode
        private EnumField statCheckField;
        private TextField statCheckValueField;

        public StatCheckType CheckType { get => statCheckType; set => statCheckType = value; }
        public string StatCheckValue { get => statCheckValue; set => statCheckValue = value; }

        // Initializing StatCheckNode with .css
        public StatCheckNode()
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("StatCheckNodeStyleSheet");
            styleSheets.Add(styleSheet); 
        }

        // Spawning StatCheckNode
        public StatCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Stat Check";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            
            // StatCheck value field            
            statCheckValueField = new TextField();
            statCheckValueField.RegisterValueChangedCallback(value =>
            {
                statCheckValue = value.newValue;
            });
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            mainContainer.Add(statCheckValueField);
            
            // StatCheck type field
            statCheckField = new EnumField()
            {
                value = statCheckType
            };

            statCheckField.Init(statCheckType);

            statCheckField.RegisterValueChangedCallback((value) =>
            {
                statCheckType = (StatCheckType)value.newValue;
            });
            statCheckField.SetValueWithoutNotify(statCheckType);

            mainContainer.Add(statCheckField);
        }
        
        // Saving values into StatCheckNode fields
        public override void LoadValueInToField()
        {
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            statCheckField.SetValueWithoutNotify(statCheckType);
        }
    }
}

