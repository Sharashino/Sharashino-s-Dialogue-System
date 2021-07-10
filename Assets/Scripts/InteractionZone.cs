using System;
using UnityEngine;

// Class used to check if player is in range to preform an interaction
// Every trigger is based on tag "Player", make sure you put it on your Player object
namespace SIS.Actions.Interaction
{
    public class InteractionZone : MonoBehaviour
    {
        [SerializeField] private bool isInRange; // Bool responsible for holding player range state 
        [SerializeField] private GameObject interactionHelper = default; // Reference to canvas indicating that player can interact
        public bool IsInRange { get => isInRange; set => isInRange = value; }

        private void Awake()
        {
            interactionHelper.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                // If player enters - is in range - show interaction helper, allow interaction
                isInRange = true;
                interactionHelper.SetActive(true);
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // If player stays - is in range
                isInRange = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // If player leaves - is out of range - hide interaction helper, don't allow interaction
                isInRange = false;
                interactionHelper.SetActive(false);
            }
        }
    }
}