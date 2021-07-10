using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Auxiliary class for creating EventNodes
namespace SDS.DialogueSystem.Nodes
{
    public class EventNode : BaseNode
    {
        private DialogueEventSO dialogueEvent;  // Reference to event happening in this EventNode
        public DialogueEventSO DialogueEvent { get => dialogueEvent; set => dialogueEvent = value; }

        private ObjectField objectField; // Fields instantiated in this EventNode
        
        // Initializing EventNode with .css
        public EventNode()
        {
            // Adding and loading this node .css from /Resources
            StyleSheet styleSheet = Resources.Load<StyleSheet>("EventNodeStyleSheet");
            styleSheets.Add(styleSheet);
        }
        
        // Spawning EventNode
        public EventNode(Vector2 position, DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            title = "Event";
            SetPosition(new Rect(position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            
            // Dialogue Event field 
            objectField = new ObjectField()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = dialogueEvent,
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                dialogueEvent = objectField.value as DialogueEventSO;
            });

            objectField.SetValueWithoutNotify(dialogueEvent);
            mainContainer.Add(objectField);
        }
        
        // Saving values into EventNode field
        public override void LoadValueInToField()
        {
            objectField.SetValueWithoutNotify(dialogueEvent);
        }
    }   
}
