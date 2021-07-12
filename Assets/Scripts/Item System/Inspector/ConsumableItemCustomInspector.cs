using SIS.Items;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SIS.Inventory.Editors
{
    [CustomEditor(typeof(ConsumableItem))]
    public class ConsumableItemCustomInspector : Editor
    {
        private ConsumableItem consumableItem;  // Reference to ConsumableItem we're displaying

        public override void OnInspectorGUI()
        {
            consumableItem = target as ConsumableItem;  // Telling unity to target this object as ConsumableItem 
            
            if(consumableItem) DrawGeneralItem(); // Drawing all consumable fields
            
            var buttonStyle = new GUIStyle(GUI.skin.button);    // Style for our save changes button
            buttonStyle.normal.textColor = Color.red;
            buttonStyle.fontStyle = FontStyle.Bold;
            
            if (GUILayout.Button("Save changes", buttonStyle))
            {
                EditorUtility.SetDirty(consumableItem); // Marking this consumable item as dirty to ensure all changes are registered
                EditorSceneManager.MarkSceneDirty(consumableItem.gameObject.scene);   // Marking the scene as modified so it takes our changes
            }
        }

        public void DrawGeneralItem()
        {
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Basic Trinket Item Settings", EditorStyles.boldLabel);
            consumableItem.itemName = EditorGUILayout.TextField("Consumable Name:", consumableItem.itemName);   // Text field for consumable name
            consumableItem.itemDescription = EditorGUILayout.TextField("Consumable Description:", consumableItem.itemDescription);  // Text field for consumable description
            consumableItem.itemIcon = (Sprite)EditorGUILayout.ObjectField("Consumable Icon:", consumableItem.itemIcon, typeof(Sprite), false); // Object field of type sprite for consumable icon
            consumableItem.inventoryItemSprite = (Sprite)EditorGUILayout.ObjectField("Consumable Inventory Icon:", consumableItem.inventoryItemSprite, typeof(Sprite), false); // Object field of type sprite for consumable inventory image
            consumableItem.itemID = EditorGUILayout.IntField("Consumable ID:", consumableItem.itemID);  // Int field for consumable ID
            
            if (GUILayout.Button("Generate random consumable ID"))    // Generating random ID for consumable
            {
                consumableItem.itemID = Random.Range(0, int.MaxValue);
            }
            
            GUILayout.Label("Inventory Consumable Settings:", EditorStyles.boldLabel);
            consumableItem.itemWidth = EditorGUILayout.IntField("Inventory Width:", consumableItem.itemWidth);    // Int field for inventory consumable width
            consumableItem.itemHeight = EditorGUILayout.IntField("Inventory Height:", consumableItem.itemHeight); // Int field for inventory consumable height
            consumableItem.isStackable = EditorGUILayout.Toggle("Stackable:", consumableItem.isStackable);   // Toggle for consumable stackable

            if (consumableItem.isStackable)
            {
                consumableItem.itemMaxStackSize = EditorGUILayout.IntSlider("Max Stack Size:", consumableItem.itemMaxStackSize, 1, 100); // Int field for consumable max stack size
            }
            
            GUILayout.EndVertical();

            GUILayout.Label("Detailed Consumable Item Settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            consumableItem.healingPower = EditorGUILayout.IntField("Consumable Healing Power:", consumableItem.healingPower);  // Int field for consumable healing power
            
            GUILayout.EndVertical();
        }
    }
}
