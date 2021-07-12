using UnityEngine;

// Auxiliary class for Trinket Items
namespace SIS.Items
{
    public class TrinketItem : Item
    {
        [Header("Trinket item details")] 
        public int healthBuff;
        public int staminaBuff;
        public int manaBuff;
    }
}
