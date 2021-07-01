using UnityEngine;
//using SDS.Items;
using SDS.DialogueSystem.SO;
//using SDS.Inventory;

/// <summary>
/// Napisane przez sharashino
/// 
/// Event w dialogu odpowiadający za otrzymanie itemu
/// </summary>
namespace SDS.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName ="Dialogue/New OpenShop Event", fileName = "OpenShop Event")]
    public class EventGetItemShop : DialogueEventSO
    {
        //[SerializeField] private Item itemToGet;
        public override void RunEvent()
        {
            base.RunEvent();
            GetItem();
        }

        private void GetItem()
        {
            //InventoryClass.Instance.AddItem(itemToGet);
        }
    }
}

