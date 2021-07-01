using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using SDS.DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;

// <summary>
// Napisane przez sharashino
// 
// Skrypt odpowiadający za rysowanie nodów w GraphView
// </summary>
namespace SDS.DialogueSystem.Editor
{
    public class DialogueGraphView : GraphView
    {
        private string styleSheetsName = "GraphViewStyleSheet";
        private DialogueEditorWindow editorWindow;
        private NodeSearchWindow searchWindow;

        public DialogueGraphView(DialogueEditorWindow newEditorWindow)
        {
            editorWindow = newEditorWindow;

            StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(styleSheetsName);
            styleSheets.Add(tmpStyleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            Port startPortView = startPort;

            ports.ForEach((port) =>
            {
                Port portView = port;

                if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        public void LanguageReload()
        {
            List<DialogueNode> dialogueNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                dialogueNode.ReloadLanguage();
            }
        }

        public StartNode CreateStartNode(Vector2 pos)
        {
            StartNode tmp = new StartNode(pos, editorWindow, this);

            return tmp;
        }

        public EndNode CreateEndNode(Vector2 pos)
        {
            EndNode tmp = new EndNode(pos, editorWindow, this);

            return tmp;
        }

        public EventNode CreateEventNode(Vector2 pos)
        {
            EventNode tmp = new EventNode(pos, editorWindow, this);

            return tmp;
        }

        public StatCheckNode CreateStatCheckNode(Vector2 pos)
        {
            StatCheckNode tmp = new StatCheckNode(pos, editorWindow, this);

            return tmp;
        }

        public ItemCheckNode CreateItemCheckNode(Vector2 pos)
        {
            ItemCheckNode tmp = new ItemCheckNode(pos, editorWindow, this);

            return tmp;
        }

        public DialogueNode CreateDialogueNode(Vector2 pos)
        {
            DialogueNode tmp = new DialogueNode(pos, editorWindow, this);

            return tmp;
        }
    }
}

