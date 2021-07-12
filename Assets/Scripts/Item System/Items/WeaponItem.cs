using UnityEngine;
using SIS.Items.Enums;

// Auxiliary class for Weapon Items
namespace SIS.Items
{
    public class WeaponItem : Item
    {
        [Header("Weapon item details")]
        public WeaponTypes weaponType;  //reference to our WeaponTypes enum
        public int weaponDamage;
        public int weaponLevel;
    }
}
