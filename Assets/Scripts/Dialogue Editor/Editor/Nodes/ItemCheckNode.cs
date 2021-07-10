using System;
using UnityEngine;
using SDS.Items;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Auxiliary class for creating DialogueNodes
namespace SDS.DialogueSystem.Nodes
{
    public class ItemCheckNode : BaseNode
    {
        private Item nodeItem;  // Reference to Item in this ItemCheckNode
        private ItemCheckNodeType itemCheckType;    // ItemCheck type (Give Item | Get Item)
        private string itemCheckValue;  //  How many items should player Give/Get

        // Fields instantiated in this ItemCheckNode
        private EnumField itemCheckField;
        private ObjectField itemField;
        private TextField itemCheckValueField;

        public Item NodeItem { get => nodeItem; set => nodeItem = value; }
        public string ItemCheckValue { get => itemCheckValue; set => itemCheckValue = value; }
        public ItemCheckNodeType ItemCheckNodeType { get => itemCheckType; set => itemCheckType = value; }

        // Initializing as empty
        public ItemCheckNode()
        {
            
        }

        // Spawning ItemCheckNode
        public ItemCheckNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("ItemNodeStyleSheet");
            styleSheets.Add(styleSheet);
            
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Item Check";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            
            // ItemCheck type field
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
            
            // ItemCheck value field
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
            
            // ItemCheck item field
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
            
            mainContainer.Add(itemField);
        }
        
        // Saving values into ItemCheckNode fields
        public override void LoadValueInToField()
        {
            itemCheckValueField.SetValueWithoutNotify(itemCheckValue);
            itemCheckField.SetValueWithoutNotify(itemCheckType);
            itemField.SetValueWithoutNotify(nodeItem);
        }
    }
}

