using SIS.Items;
using UnityEngine;

// Use this script on every object you want to pick up
// Has overwritable method from Interactables for changing interaction logic
namespace SIS.Actions.Interaction
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private Item item = default;   // Reference to an item that can be picked up

        public Item Item { get => item; set => item = value; }

        public new void Awake() // Getting item on awake from other script on object
        {
            item = GetComponent<Item>();
        }
        
        public override void Interact() // When player interacted with this object
        {
            base.Interact();
            PickUp();
        }

        private void PickUp()   // Picking up item
        {
            Debug.Log("Picking up item: " + item.name);
            
            /*
             *  Here you should put code that is responsible for
             *  putting items inside of players inventory
             *  'item' variable is designed to be passed to your inventory
             */
        }
    }
}

