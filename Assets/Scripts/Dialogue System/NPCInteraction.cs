using UnityEngine;
using System.Linq;
using SDS.DialogueSystem.SO;
using SIS.Actions.Interaction;
using SDS.DialogueSystem.Actions;
using System.Collections.Generic;

// Script used to interact with npcs, it is responsible for holding dialogues and running them in the right order
namespace SDS.NPC.Interaction
{
    public class NPCInteraction : Interactable
    {
        [SerializeField] private List<DialogueContainerSO> NPCDialogues = new List<DialogueContainerSO>();  // List of dialogues this npc can conduct
        private int interactionCounter = 0; // This int holds value of interactions made with this npc
        
        private DialogueTalk dialogueTalk; // Reference to script responsible for running dialogue

        private new void Awake()
        {
            dialogueTalk = GetComponent<DialogueTalk>();    // Getting the reference on awake
        }

        public override void Interact() // Overwritable void running when player interacts with npc
        {
            // If statement responsible for running right dialogue from the list
            // Last dialogue on the list will run constantly after earlier dialogues were finished 
            if (interactionCounter >= NPCDialogues.Count)   
            {
                dialogueTalk.StartDialogue(NPCDialogues.Last());   // Running last dialogue if earlier dialogues were finished or theres only one
            }
            else
            {
                dialogueTalk.StartDialogue(NPCDialogues[interactionCounter]); // Running dialogue with index of interactionCounter
                interactionCounter++;   // Adding up to interaction counter after running dialogue
            }
        }

        public override void StopInteract() // Running after ending dialogue
        {
            Debug.Log("Finished talking with " + gameObject.name);
        }
    }
}
