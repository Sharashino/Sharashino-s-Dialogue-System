using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SDS.DialogueSystem.SO;
using System.Collections.Generic;
using SDS.DialogueSystem.Nodes;
using SDS.DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;

namespace SDS.DialogueSystem.SaveLoad
{
    public class DialogueSaveAndLoad
    {
        private List<Edge> edges => graphView.edges.ToList();
        private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

        private DialogueGraphView graphView;

        public DialogueSaveAndLoad(DialogueGraphView newGraphView)
        {
            graphView = newGraphView;
        }

        public void Save(DialogueContainerSO dialogueContainerSO)
        {
            SaveEdges(dialogueContainerSO);
            SaveNodes(dialogueContainerSO);

            EditorUtility.SetDirty(dialogueContainerSO);
            AssetDatabase.SaveAssets();
        }

        public void Load(DialogueContainerSO dialogueContainerSO)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSO);
            ConnectNodes(dialogueContainerSO);
        }

        #region Save
        private void SaveEdges(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.NodeLinkDatas.Clear();
            Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
            
            for (int i = 0; i < connectedEdges.Count(); i++)
            {
                BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
                BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

                dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGuid = outputNode.NodeGuid,
                    TargetNodeGuid = inputNode.NodeGuid
                });
            }
        }

        private void SaveNodes(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.DialogueNodeDatas.Clear();
            dialogueContainerSO.EventNodeDatas.Clear();
            dialogueContainerSO.StatCheckNodeDatas.Clear();
            dialogueContainerSO.ItemCheckNodeDatas.Clear();
            dialogueContainerSO.EndNodeDatas.Clear();
            dialogueContainerSO.StartNodeDatas.Clear();

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
                        nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                        nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                    }
                }
            }

            return dialogueNodeData;
        }

        private StartNodeData SaveNodeData(StartNode node)
        {
            StartNodeData nodeData = new StartNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
            };

            return nodeData;
        }

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

        private ItemCheckNodeData SaveNodeData(ItemCheckNode node)
        {
            ItemCheckNodeData nodeData = new ItemCheckNodeData()
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                //NodeItem = node.NodeItem,
                ItemCheckType = node.ItemCheckNodeType,
                ItemCheckValue = int.Parse(node.ItemCheckValue),
            };
         
            return nodeData;
        }

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

        private void ClearGraph()
        {
            edges.ForEach(edge => graphView.RemoveElement(edge));

            foreach (BaseNode node in nodes)
            {
                graphView.RemoveElement(node);
            }
        }

        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            // Start
            foreach (StartNodeData node in dialogueContainer.StartNodeDatas)
            {
                StartNode tempNode = graphView.CreateStartNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;

                graphView.AddElement(tempNode);
            }

            // End Node 
            foreach (EndNodeData node in dialogueContainer.EndNodeDatas)
            {
                EndNode tempNode = graphView.CreateEndNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.EndNodeType = node.EndNodeType;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            // Event Node
            foreach (EventNodeData node in dialogueContainer.EventNodeDatas)
            {
                EventNode tempNode = graphView.CreateEventNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.DialogueEvent = node.DialogueEventSO;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            // Dialogue Node
            foreach (DialogueNodeData node in dialogueContainer.DialogueNodeDatas)
            {
                DialogueNode tempNode = graphView.CreateDialogueNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.NameText = node.Name;
                tempNode.NpcFaceImage = node.npcSprite;
                tempNode.PlayerFaceImage = node.playerSprite;

                foreach (LanguageGeneric<string> languageGeneric in node.TextLanguages)
                {
                    tempNode.Texts.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }

                foreach (LanguageGeneric<AudioClip> languageGeneric in node.AudioClips)
                {
                    tempNode.AudioClips.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }

                foreach (DialogueNodePort nodePort in node.DialogueNodePorts)
                {
                    tempNode.AddChoicePort(tempNode, nodePort);
                }

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            //Stat Check Node
            foreach (StatCheckNodeData node in dialogueContainer.StatCheckNodeDatas)
            {
                StatCheckNode tempNode = graphView.CreateStatCheckNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.StatCheckValue = node.StatCheckValue.ToString();
                tempNode.CheckType = node.StatCheckType;

                tempNode.LoadValueInToField();
                graphView.AddElement(tempNode);
            }

            //Item Check Node
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

        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                List<NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == nodes[i].NodeGuid).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].TargetNodeGuid;
                    BaseNode targetNode = nodes.First(node => node.NodeGuid == targetNodeGuid);

                    if ((nodes[i] is DialogueNode) == false)
                    {
                        LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    }
                }
            }

            List<DialogueNode> dialogueNodes = nodes.FindAll(node => node is DialogueNode).Cast<DialogueNode>().ToList();

            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                foreach (DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
                {
                    if (nodePort.InputGuid != string.Empty)
                    {
                        BaseNode targetNode = nodes.First(Node => Node.NodeGuid == nodePort.InputGuid);
                        LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
                    }
                }
            }
        }

        private void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            graphView.Add(tempEdge);
        }

        #endregion
    }
}

