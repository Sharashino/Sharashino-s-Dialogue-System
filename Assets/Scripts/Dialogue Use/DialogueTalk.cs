using UnityEngine;
using UnityEngine.Events;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using System.Collections.Generic;
//using SDS.NPC.Interaction;

// <summary>
// Napisane przez sharashino
// 
// Skrypt odpowiadający za odczytywanie danych z ScriptableObjectu i wrzucanie ich do okna z dialogiem (DialogueController)
// </summary>
namespace SDS.DialogueSystem.Actions
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private DialogueController dialogueControler = default;
        [SerializeField] private AudioSource audioSource = default;
        
        private DialogueNodeData currentDialogueNodeData;
        private DialogueNodeData lastDialogueNodeData;

        private List<StatCheckNodeData> statCheckNodeDatas = new List<StatCheckNodeData>();
        private List<ItemCheckNodeData> itemCheckNodeDatas = new List<ItemCheckNodeData>();
        private bool isTalking = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();  
        }

        public void StartDialogue(DialogueContainerSO dialogueContainer)
        {
            currentDialogue = dialogueContainer;
            
            if(isTalking != true)
            {
                isTalking = true;
                CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));
                dialogueControler.ShowDialogueUI(true);
            }
        }

        public void EndDialogue()
        {
            isTalking = false;
            dialogueControler.ShowDialogueUI(false);
            //GetComponent<NPCInteraction>().StopInteract();
        }

        private void CheckNodeType(BaseNodeData baseNodeData)
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
                    break;
            }
        }
        
        private void RunNode(StartNodeData nodeData)
        {
            CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));
        }

        private void RunNode(DialogueNodeData nodeData)
        {
            if (currentDialogueNodeData != nodeData)
            {
                lastDialogueNodeData = currentDialogueNodeData;
                currentDialogueNodeData = nodeData;
            }

            dialogueControler.SetText(nodeData.Name, nodeData.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
            dialogueControler.SetImage(nodeData.playerSprite, nodeData.npcSprite);
            
            MakeButtons(nodeData.DialogueNodePorts);

            audioSource.clip = nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
            audioSource.Play();
        }
        
        private void RunNode(EventNodeData nodeData)
        {
            nodeData.DialogueEventSO.RunEvent();

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(StatCheckNodeData nodeData)
        {
            statCheckNodeDatas.Add(nodeData);

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(ItemCheckNodeData nodeData)
        {
            itemCheckNodeDatas.Add(nodeData);

            CheckNodeType(GetNextNode(nodeData));
        }

        private void RunNode(EndNodeData nodeData)
        {
            switch (nodeData.EndNodeType)
            {
                case EndNodeType.End:
                    dialogueControler.ShowDialogueUI(false);
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.Goback:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.RetrunToStart:
                    CheckNodeType(GetNextNode(currentDialogue.StartNodeDatas[0]));
                    break;
            }

            EndDialogue();
        }

        private void MakeButtons(List<DialogueNodePort> nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in nodePorts)
            {
                texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
                
                UnityAction tempAciton = null;
                tempAciton += () =>
                {
                    statCheckNodeDatas.Clear();
                    itemCheckNodeDatas.Clear();

                    audioSource.Stop();
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAciton);
            }

            dialogueControler.SetButtons(texts, unityActions, statCheckNodeDatas, itemCheckNodeDatas);
        }
    }
}