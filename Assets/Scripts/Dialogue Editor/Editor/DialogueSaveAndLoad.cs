using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using System.Collections.Generic;
using SDS.DialogueSystem.Nodes;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

// Script that saves dialogue into ScriptableObject
namespace SDS.DialogueSystem.SaveLoad
{
    public class DialogueSaveAndLoad
    {
        private List<Edge> edges => graphView.edges.ToList();   // List of every nodes edges
        private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList(); // List of every nodes in dialogue, casting every node to BaseNode

        private DialogueGraphView graphView;    // Graph view of our dialogue

        // Constructor spawning graph view
        public DialogueSaveAndLoad(DialogueGraphView newGraphView)
        {
            graphView = newGraphView;
        }

        // Saving our dialgoue
        public void Save(DialogueContainerSO dialogueContainerSO)
        {
            SaveEdges(dialogueContainerSO); 
            SaveNodes(dialogueContainerSO);

            EditorUtility.SetDirty(dialogueContainerSO);    // Setting editor dirty to accept our changes
            AssetDatabase.SaveAssets();
        }
        
        // Loading dialogue
        public void Load(DialogueContainerSO dialogueContainerSO)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSO);
            ConnectNodes(dialogueContainerSO);
        }

        #region Save
        
        // Saving nodes edges 
        private void SaveEdges(DialogueContainerSO dialogueContainerSO)
        {
            // Clearing all links before saving new ones
            dialogueContainerSO.NodeLinkDatas.Clear();
            
            Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray(); // Connecting edges by finding matching ones
            
            for (int i = 0; i < connectedEdges.Count(); i++)
            {
                BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
                BaseNode inputNode = connectedEdges[i].input.node as BaseNode;
                
                // Adding edges to list
                dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGuid = outputNode.NodeGuid,
                    TargetNodeGuid = inputNode.NodeGuid
                });
            }
        }
        
        // Saving all nodes
        private void SaveNodes(DialogueContainerSO dialogueContainerSO)
        {
            // Clearing all data before saving
            dialogueContainerSO.DialogueNodeDatas.Clear();
            dialogueContainerSO.EventNodeDatas.Clear();
            dialogueContainerSO.StatCheckNodeDatas.Clear();
            dialogueContainerSO.ItemCheckNodeDatas.Clear();
            dialogueContainerSO.EndNodeDatas.Clear();
            dialogueContainerSO.StartNodeDatas.Clear();
            
            // Each node is saved to different list
            nodes.ForEach(node =>
            {
                switch (node)
                {
                    case StartNode startNode:
                        dialogueContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
                        break;
                    case EndNode endNode:
                        dialogueContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
                        break;
                    case EventNode eventNode:
                        dialogueContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
                        break;
                    case DialogueNode dialogueNode:
                        dialogueContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
                        break;
                    case StatCheckNode statCheckNode:
                        dialogueContainerSO.StatCheckNodeDatas.Add(SaveNodeData(statCheckNode));
                        break;
                    case ItemCheckNode itemCheckNode:
                        dialogueContainerSO.ItemCheckNodeDatas.Add(SaveNodeData(itemCheckNode));
                        break;
                    default:
                        break;
                }
            });
        }

        // Saving dialogue node
        private DialogueNodeData SaveNodeData(DialogueNode node)
        {
            DialogueNodeData dialogueNodeData = new DialogueNodeData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                TextLanguages = node.Texts,
                Name = node.NameText,
                AudioClips = node.AudioClips,
                npcSprite = node.NpcFaceImage,
                playerSprite = node.PlayerFaceImage,
                DialogueNodePorts = new List<DialogueNodePort>(node.DialogueNodePorts)
            };

            foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
            {
                nodePort.OutputGuid = string.Empty;
                nodePort.InputGuid = string.Empty;
                
                foreach (Edge edge in edges)
                {
                    if (edge.output == nodePort.MyPort)
                    {
                        // Converting DialogueNode output and input guids to BaseNode for unified loading and saving
                        nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;    
                        nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                    }
                }
            }
            
            return dialogueNodeData;
        }

        // Saving StartNode
        private StartNodeData SaveNodeData(StartNode node)
        {
            StartNodeData nodeData = new StartNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
            };

            return nodeData;
        }

        // Saving End Node
        private EndNodeData SaveNodeData(EndNode node)
        {
            EndNodeData nodeData = new EndNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                EndNodeType = node.EndNodeType
            };

            return nodeData;
        }

        // Saving EventNode
        private EventNodeData SaveNodeData(EventNode node)
        {
            EventNodeData nodeData = new EventNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                DialogueEventSO = node.DialogueEvent
            };

            return nodeData;
        }

        // Saving ItemCheckNode
        private ItemCheckNodeData SaveNodeData(ItemCheckNode node)
        {
            ItemCheckNodeData nodeData = new ItemCheckNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                NodeItem = node.NodeItem,
                ItemCheckType = node.ItemCheckNodeType,
                ItemCheckValue = int.Parse(node.ItemCheckValue),
            };
         
            return nodeData;
        }

        // Saving StatCheckNode
        private StatCheckNodeData SaveNodeData(StatCheckNode node)
        {
            StatCheckNodeData nodeData = new StatCheckNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                StatCheckType = node.CheckType,
                StatCheckValue = int.Parse(node.StatCheckValue)
            };

            return nodeData;
        }


        #endregion

        #region Load

        // Clearing graph before loading new dialogue
        private void ClearGraph()
        {
            edges.ForEach(edge => graphView.RemoveElement(edge));

            foreach (BaseNode node in nodes)
            {
                graphView.RemoveElement(node);
            }
        }

        // Spawning nodes based on ScriptableObject data
        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            // StartNode
            foreach (StartNodeData node in dialogueContainer.StartNodeDatas)
            {
                StartNode tempNode = graphView.CreateStartNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;

                graphView.AddElement(tempNode); 
            }

            // EndNode 
            foreach (EndNodeData node in dialogueContainer.EndNodeDatas)
            {
                EndNode tempNode = graphView.CreateEndNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.EndNodeType = node.EndNodeType;

                tempNode.LoadValueInToField();  // Loading values into node fields
                graphView.AddElement(tempNode); // Adding node to graph view
            }

            // EventNode
            foreach (EventNodeData node in dialogueContainer.EventNodeDatas)
            {
                EventNode tempNode = graphView.CreateEventNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.DialogueEvent = node.DialogueEventSO;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            // DialogueNode
            foreach (DialogueNodeData node in dialogueContainer.DialogueNodeDatas)
            {
                DialogueNode tempNode = graphView.CreateDialogueNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.NameText = node.Name;
                tempNode.NpcFaceImage = node.npcSprite;
                tempNode.PlayerFaceImage = node.playerSprite;

                // Matching language dialogue is set to
                foreach (LanguageGeneric<string> languageGeneric in node.TextLanguages)
                {
                    tempNode.Texts.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            
                // Matching voice clips language dialogue is set to
                foreach (LanguageGeneric<AudioClip> languageGeneric in node.AudioClips)
                {
                    tempNode.AudioClips.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
                
                // Matching dialogue choice ports 
                foreach (DialogueNodePort nodePort in node.DialogueNodePorts)
                {
                    tempNode.AddChoicePort(tempNode, nodePort);
                }

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            // StatCheckNode
            foreach (StatCheckNodeData node in dialogueContainer.StatCheckNodeDatas)
            {
                StatCheckNode tempNode = graphView.CreateStatCheckNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.StatCheckValue = node.StatCheckValue.ToString();
                tempNode.CheckType = node.StatCheckType;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            // ItemCheckNode
            foreach (ItemCheckNodeData node in dialogueContainer.ItemCheckNodeDatas)
            {
                ItemCheckNode tempNode = graphView.CreateItemCheckNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.ItemCheckValue = node.ItemCheckValue.ToString();
                tempNode.ItemCheckNodeType = node.ItemCheckType; 
                tempNode.NodeItem = node.NodeItem;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }
        }
        
        // Connecting nodes 
        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            // Looping through every node
            for (int i = 0; i < nodes.Count; i++)
            {
                List<NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == nodes[i].NodeGuid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].TargetNodeGuid;
                    BaseNode targetNode = nodes.First(node => node.NodeGuid == targetNodeGuid);

                    if ((nodes[i] is DialogueNode) == false)
                    {
                        // Connecting every nodes (besides DialogueNode) based on their input and output from ScriptableObject
                        LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    }
                }
            }

            List<DialogueNode> dialogueNodes = nodes.FindAll(node => node is DialogueNode).Cast<DialogueNode>().ToList();
            
            // Looping through every DialogueNode
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                foreach (DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
                {
                    if (nodePort.InputGuid != string.Empty)
                    {
                        // Connecting DialogueNodes and every other nodes to DialogueNode choices
                        BaseNode targetNode = nodes.First(Node => Node.NodeGuid == nodePort.InputGuid);
                        LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
                    }
                }
            }
        }
        
        // Connecting nodes together
        private void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            // Creating new node edge
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };
            
            // Connecting input/output edges
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            graphView.Add(tempEdge);
        }

        #endregion
    }
}

