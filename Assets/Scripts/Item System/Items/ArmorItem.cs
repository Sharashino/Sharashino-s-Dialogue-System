using UnityEngine;
using SIS.Items.Enums;

// Auxiliary class for Armor Items
namespace SIS.Items
{
    public class ArmorItem : Item
    {
        [Header("Armor item details")]
        public ArmorTypes armorType;    //reference to our WeaponTypes enum
        public int armorValue;
        public int armorLevel;
    }
}
