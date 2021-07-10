using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using SDS.DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;

// Script responsible for spawning nodes in graph view
namespace SDS.DialogueSystem.Editor
{
    public class DialogueGraphView : GraphView
    {
        private string styleSheetsName = "GraphViewStyleSheet"; // This window .css
        private DialogueEditorWindow editorWindow;  // Editor window
        private NodeSearchWindow searchWindow;  // Search window
    
        // Spawning graph view
        public DialogueGraphView(DialogueEditorWindow newEditorWindow)
        {
            editorWindow = newEditorWindow;
            
            // Adding and loading this editor .css
            StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(styleSheetsName);
            styleSheets.Add(tmpStyleSheet);
            
            // Adding zoom to the editor
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());  // Ability to drag content
            this.AddManipulator(new SelectionDragger());    // Ability to drag selection
            this.AddManipulator(new RectangleSelector());   // Adding rectangle selector
            this.AddManipulator(new FreehandSelector());    // Adding keyboard selection

            // Adding grid to background
            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();
        }
        
        // Adding search window for nodes
        private void AddSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }
        
        // Reloading language on different language selection
        public void LanguageReload()
        {
            List<DialogueNode> dialogueNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                dialogueNode.ReloadLanguage();
            }
        }

        // Spawning StartNode
        public StartNode CreateStartNode(Vector2 pos)
        {
            StartNode tmp = new StartNode(pos, editorWindow, this);

            return tmp;
        }
        
        // Spawning EndNode
        public EndNode CreateEndNode(Vector2 pos)
        {
            EndNode tmp = new EndNode(pos, editorWindow, this);

            return tmp;
        }

        // Spawning EventNode
        public EventNode CreateEventNode(Vector2 pos)
        {
            EventNode tmp = new EventNode(pos, editorWindow, this);

            return tmp;
        }

        // Spawning StatCheckNode
        public StatCheckNode CreateStatCheckNode(Vector2 pos)
        {
            StatCheckNode tmp = new StatCheckNode(pos, editorWindow, this);

            return tmp;
        }

        // Spawning ItemCheckNode
        public ItemCheckNode CreateItemCheckNode(Vector2 pos)
        {
            ItemCheckNode tmp = new ItemCheckNode(pos, editorWindow, this);

            return tmp;
        }
        
        // Spawning DialogueNode
        public DialogueNode CreateDialogueNode(Vector2 pos)
        {
            DialogueNode tmp = new DialogueNode(pos, editorWindow, this);

            return tmp;
        }
    }
}

