using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using SDS.DialogueSystem.Nodes;
using UnityEditor.Experimental.GraphView;

// <summary>
// Napisane przez sharashino
// 
// Skrypty wyszukiwarki nodów w GraphView
// </summary>
namespace SDS.DialogueSystem.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueEditorWindow editorWindow;
        private DialogueGraphView graphView;

        private Texture2D pic;

        public void Configure(DialogueEditorWindow newEditorWindow, DialogueGraphView newGraphView)
        {
            editorWindow = newEditorWindow;
            graphView = newGraphView;

            pic = new Texture2D(1, 1);
            pic.SetPixel(0, 0, new Color(0, 0, 0, 0));
            pic.Apply();
        }


        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Dialogue Nodes"),1),

                AddNodeSearch("Start Node",new StartNode()),
                AddNodeSearch("Dialogue Node",new DialogueNode()),
                AddNodeSearch("Event Node",new EventNode()),
                AddNodeSearch("Stat Check Node",new StatCheckNode()),
                AddNodeSearch("Item Node", new ItemCheckNode()),
                AddNodeSearch("End Node",new EndNode()),
            };

            return tree;
        }

        private SearchTreeEntry AddNodeSearch(string nodeName, BaseNode baseNode)
        {
            SearchTreeEntry tempEntry = new SearchTreeEntry(new GUIContent(nodeName, pic))
            {
                level = 2,
                userData = baseNode
            };

            return tempEntry;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo
                (
                editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position
                );

            Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

            return CheckForNodeType(searchTreeEntry, graphMousePosition);
        }

        private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 pos)
        {
            switch (searchTreeEntry.userData)
            {
                case StartNode startNode:
                    graphView.AddElement(graphView.CreateStartNode(pos));
                    return true;
                case DialogueNode dialogueNode:
                    graphView.AddElement(graphView.CreateDialogueNode(pos));
                    return true;
                case EventNode eventNode:
                    graphView.AddElement(graphView.CreateEventNode(pos));
                    return true;
                case ItemCheckNode eventNode:
                    graphView.AddElement(graphView.CreateItemCheckNode(pos));
                    return true;
                case StatCheckNode statCheckNode:
                    graphView.AddElement(graphView.CreateStatCheckNode(pos));
                    return true;
                case EndNode endNode:
                    graphView.AddElement(graphView.CreateEndNode(pos));
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}