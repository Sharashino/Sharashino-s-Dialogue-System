using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace SDS.DialogueSystem.Nodes
{
    public class StatCheckNode : BaseNode
    {
        private StatCheckType statCheckType = StatCheckType.Luck;
        private string statCheckValue;
        private EnumField statCheckField;
        private TextField statCheckValueField;

        public StatCheckType CheckType { get => statCheckType; set => statCheckType = value; }
        public string StatCheckValue { get => statCheckValue; set => statCheckValue = value; }

        public StatCheckNode()
        {
           
        }

        public StatCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("StatCheckNodeStyleSheet");
            styleSheets.Add(styleSheet); 
            
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Stat Check";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            statCheckValueField = new TextField();
            statCheckValueField.RegisterValueChangedCallback(value =>
            {
                statCheckValue = value.newValue;
            });
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            mainContainer.Add(statCheckValueField);

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

        public override void LoadValueInToField()
        {
            statCheckValueField.SetValueWithoutNotify(statCheckValue);
            statCheckField.SetValueWithoutNotify(statCheckType);
        }
    }
}

