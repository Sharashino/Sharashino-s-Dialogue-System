using UnityEngine;

// Main class for things player can interact with (currently with `E` key), all things that player can interact with should derive from this class
namespace SIS.Actions.Interaction
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractionZone interactionZone;   //Reference to InteractionZone that checks if player is in range to interact
        [SerializeField] private bool isInteracting;    //Player interaction checker

        public InteractionZone InteractionZone { get => interactionZone; set => interactionZone = value; }

        public void Awake()
        {
            interactionZone = GetComponentInChildren<InteractionZone>();
        }

        public void Update()
        {
            if (interactionZone.IsInRange)
            {
                if (Input.GetKeyDown(KeyCode.E))    //If player is in range and presses E - start interaction
                {
                    isInteracting = true;
                    Interact();
                }
            }
            else if(!interactionZone.IsInRange && isInteracting)    //If player is out of range but still interacting - cancel interaction
            {
                isInteracting = false;
                StopInteract();
            }
        }
        
        public virtual void Interact() // Overwritable void for interaction
        {

        }

        public virtual void StopInteract() // Overwritable void for stopping interaction
        {

        }
    }
}