using UnityEngine;
using SIS.Items.Enums;
using UnityEngine.Events;

// Main class for Items
namespace SIS.Items
{
    public class Item : MonoBehaviour
    {
        [System.Serializable] public class OnUseEvent : UnityEvent { }  // Unity event for using item
        [System.Serializable] public class OnPickupEvent : UnityEvent { }   // Unity event for picking up item
        
        [SerializeField] public OnUseEvent onUseEvent;
        [SerializeField] public OnPickupEvent onPickupEvent;
        
        [Header("Base item info")]
        public int itemID;
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon; //reference to this item icon that can shown in inventory
        public Sprite inventoryItemSprite;  //reference to this item image that can be shown in equipment 
        public ItemTypes itemType;

        [Range(1, 10)]
        public int itemWidth = 1, itemHeight = 1;   // Item size if you want to have different item sizes and grid inventory

        [Header("Stack options")]
        public bool isStackable;    // If item can be stackable e.g torches, potions

        [Range(1, 100)]
        public int itemMaxStackSize = 1;    // Max stack size

        [Range(1, 100)]
        public int itemStackSize = 1;   // Stack size counter
    }
}
