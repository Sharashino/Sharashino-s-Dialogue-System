using System;
using UnityEngine;
//using SDS.Items;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace SDS.DialogueSystem.Nodes
{
    public class ItemCheckNode : BaseNode
    {
        //private Item nodeItem;
        private ItemCheckNodeType itemCheckType;
        private string itemCheckValue;

        private EnumField itemCheckField;
        private ObjectField itemField;
        private TextField itemCheckValueField;

        //public Item NodeItem { get => nodeItem; set => nodeItem = value; }
        public string ItemCheckValue { get => itemCheckValue; set => itemCheckValue = value; }
        public ItemCheckNodeType ItemCheckNodeType { get => itemCheckType; set => itemCheckType = value; }

        public ItemCheckNode()
        {
            
        }

        public ItemCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("ItemNodeStyleSheet");
            styleSheets.Add(styleSheet);

            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Item Check";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);

            itemCheckField = new EnumField()
            {
                value = itemCheckType
            };

            itemCheckField.Init(itemCheckType);

            itemCheckField.RegisterValueChangedCallback((value) =>
            {
                itemCheckType = (ItemCheckNodeType)value.newValue;
            });
            itemCheckField.SetValueWithoutNotify(itemCheckType);

            mainContainer.Add(itemCheckField);

            itemCheckValueField = new TextField()
            {
                label = "Item count"
            };

            itemCheckValueField.RegisterValueChangedCallback(value =>
            {
                itemCheckValue = value.newValue;
            });
            itemCheckValueField.SetValueWithoutNotify(itemCheckValue);
            mainContainer.Add(itemCheckValueField);
            
            /*
            itemField = new ObjectField()
            {
                objectType = typeof(Item),
                allowSceneObjects = true,
                
                value = nodeItem,
            };

            itemField.RegisterValueChangedCallback(value =>
            {
                nodeItem = itemField.value as Item;
            });

            itemField.SetValueWithoutNotify(nodeItem);
            */
            mainContainer.Add(itemField);
        }

        public override void LoadValueInToField()
        {
            itemCheckValueField.SetValueWithoutNotify(itemCheckValue);
            itemCheckField.SetValueWithoutNotify(itemCheckType);
            //itemField.SetValueWithoutNotify(nodeItem);
        }
    }
}

