using System;
using UnityEngine;
using UnityEngine.Events;
using SDS.NPC.Interaction;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using System.Collections.Generic;

// NPC Script responsible for reading data from ScriptableObject and putting them into dialogue window (DialogueController)
namespace SDS.DialogueSystem.Actions
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private DialogueController dialogueController = default;  // Reference to a controller that places dialogue data into UI
        [SerializeField] private AudioSource audioSource = default; // NPC audio source used to play npcs voice 
        
        private DialogueNodeData currentDialogueNodeData;   // Currently running dialogue node
        private DialogueNodeData lastDialogueNodeData;  // Last running dialogue node

        private List<StatCheckNodeData> statCheckNodeDatas = new List<StatCheckNodeData>(); // List of stat check nodes in this dialogue
        private List<ItemCheckNodeData> itemCheckNodeDatas = new List<ItemCheckNodeData>(); // List of item check nodes in this dialogue
        public bool isTalking = false; // Is the player talking to npc?

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();  // Getting the reference on awake
        }

        public void StartDialogue(DialogueContainerSO dialogueContainer)    // Starting the dialogue on interaction with NPC
        {
            currentDialogue = dialogueContainer;    // Current dialogue is the one passed in interaction
            
            if(isTalking != true)   // If player isn't talking already we can run dialogue 
            {
                isTalking = true;   // Setting the bool to true because player is talking now
                CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));  // Every dialogue starts with Start Node so we're getting the first item in StartNodeData to properly run the dialogue sequence 
                dialogueController.ShowDialogueUI(true);    // Showing the dialogue UI
            }
        }

        public void EndDialogue()   // Ending the dialogue
        {
            isTalking = false;  // Not talking anymore 
            dialogueController.ShowDialogueUI(false);   // Hiding the dialogue UI
            GetComponent<NPCInteraction>().StopInteract();  // Stopping the interaction
        }

        private void CheckNodeType(BaseNodeData baseNodeData)   // Checking node type based on its data
        {
            switch (baseNodeData)
            {
                case StartNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EventNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EndNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case StatCheckNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case ItemCheckNodeData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
        
        private void RunNode(StartNodeData nodeData)    // Running start node
        {
            CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));
        }

        private void RunNode(DialogueNodeData nodeData) // Running dialogue node
        {
            // If the current dialogue node is different than coming dialogue node
            if (currentDialogueNodeData != nodeData)    
            {
                lastDialogueNodeData = currentDialogueNodeData; // Last dialogue node is the current node
                currentDialogueNodeData = nodeData; // Current dialogue node is the coming node 
            }
            
            dialogueController.SetText(nodeData.Name, nodeData.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType); // Setting NPC name text and its dialogue text
            dialogueController.SetImage(nodeData.playerSprite, nodeData.npcSprite); // Setting NPC and Player images to show in dialogue UI
            
            MakeButtons(nodeData.DialogueNodePorts);    // Spawning dialogue buttons based on possible dialogue options in Dialogue Node

            audioSource.clip = nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType; // Getting the right NPC audio clip to play
            audioSource.Play(); // Playing found clip
        }
        
        private void RunNode(EventNodeData nodeData)    // Running event node
        {
            nodeData.DialogueEventSO.RunEvent();    // Running event in node

            CheckNodeType(GetNextNode(nodeData));   // Checking and getting the next node based on this node 
        }

        private void RunNode(StatCheckNodeData nodeData)    // Running stat check node 
        {
            statCheckNodeDatas.Add(nodeData);   // Adding stat check node to its list

            CheckNodeType(GetNextNode(nodeData));   // Checking and getting the next node based on this node 
        }

        private void RunNode(ItemCheckNodeData nodeData)    // Running item check node
        {
            itemCheckNodeDatas.Add(nodeData);   // Adding item check node to its list

            CheckNodeType(GetNextNode(nodeData));   // Checking and getting the next node based on this node 
        }

        private void RunNode(EndNodeData nodeData)  // Running end node 
        {
            switch (nodeData.EndNodeType)   // Different behaviour based on end node type
            {
                case EndNodeType.End:
                    dialogueController.ShowDialogueUI(false); // Ending the dialogue, hiding UI
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid)); // Repeating current dialogue node
                    break;
                case EndNodeType.Goback:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));    // Repeating last dialogue node
                    break;
                case EndNodeType.RetrunToStart:
                    CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));  // Starting from the beginning 
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
            EndDialogue();
        }

        private void MakeButtons(List<DialogueNodePort> nodePorts)  // Spawning dialogue buttons based on possible dialogue options in Dialogue Node
        {
            List<string> texts = new List<string>();    // Creating a list of text fields in this dialogue 
            List<UnityAction> unityActions = new List<UnityAction>();   // Creating a list of unity actions for each dialogue option

            foreach (DialogueNodePort nodePort in nodePorts)    // For each dialogue option
            {
                texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);  // Adding right texts based on the language game is set to 
                
                UnityAction tempAciton = null;  // Creating new unity action
                tempAciton += () => // Every action
                {
                    // Clear earlier check nodes
                    statCheckNodeDatas.Clear();
                    itemCheckNodeDatas.Clear();

                    audioSource.Stop(); // Stopping the npcs voice 
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));   // Checking and getting the next node based on this node
                };
                unityActions.Add(tempAciton);   // Adding this action to list
            }

            dialogueController.SetButtons(texts, unityActions, statCheckNodeDatas, itemCheckNodeDatas); // Passing all data to fill buttons with values
        }
    }
}