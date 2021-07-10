using SIS.Items;
using SIS.Items.Enums;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SIS.Inventory.Editors
{
    [CustomEditor(typeof(ArmorItem))]
    public class ArmorItemCustomInspector : Editor
    {
        private ArmorItem armorItem;  // Reference to ArmorItem we're displaying

        public override void OnInspectorGUI()
        {
            armorItem = target as ArmorItem;  // Telling unity to target this object as ArmorItem 
            
            if(armorItem) DrawGeneralItem(); // Drawing all armor fields
            
            var buttonStyle = new GUIStyle(GUI.skin.button);    // Style for our save changes button
            buttonStyle.normal.textColor = Color.red;
            buttonStyle.fontStyle = FontStyle.Bold;
            
            if (GUILayout.Button("Save changes", buttonStyle))
            {
                EditorUtility.SetDirty(armorItem); // Marking this armor item as dirty to ensure all changes are registered
                EditorSceneManager.MarkSceneDirty(armorItem.gameObject.scene);   // Marking the scene as modified so it takes our changes
            }
        }

        public void DrawGeneralItem()
        {
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Basic Armor Item Settings", EditorStyles.boldLabel);
            armorItem.itemName = EditorGUILayout.TextField("Armor name:", armorItem.itemName);   // Text field for armor name
            armorItem.itemDescription = EditorGUILayout.TextField("Armor description:", armorItem.itemDescription);  // Text field for armor description
            armorItem.itemIcon = (Sprite)EditorGUILayout.ObjectField("Armor icon:", armorItem.itemIcon, typeof(Sprite), false); // Object field of type sprite for armor icon
            armorItem.inventoryItemSprite = (Sprite)EditorGUILayout.ObjectField("Inventory icon:", armorItem.inventoryItemSprite, typeof(Sprite), false); // Object field of type sprite for armor inventory image
            armorItem.itemID = EditorGUILayout.IntField("Armor ID:", armorItem.itemID);  // Int field for armor ID
            
            if (GUILayout.Button("Generate random armor ID"))    // Generating random ID for armor
            {
                armorItem.itemID = Random.Range(0, int.MaxValue);
            }
            
            GUILayout.Label("Inventory Armor Settings", EditorStyles.boldLabel);
            armorItem.itemWidth = EditorGUILayout.IntField("Inventory Width:", armorItem.itemWidth);    // Int field for inventory armor width
            armorItem.itemHeight = EditorGUILayout.IntField("Inventory Height:", armorItem.itemHeight); // Int field for inventory armor height
            armorItem.isStackable = EditorGUILayout.Toggle("Stackable", armorItem.isStackable);   // Toggle for armor stackable

            if (armorItem.isStackable)
            {
                armorItem.itemMaxStackSize = EditorGUILayout.IntSlider("Max stack size", armorItem.itemMaxStackSize, 1, 100); // Int field for armor max stack size
            }
            
            GUILayout.EndVertical();

            GUILayout.Label("Detailed Armor Item Settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            armorItem.armorType = (ArmorTypes)EditorGUILayout.EnumPopup("Armor type:", armorItem.armorType);  // Enum popup field for armor type
            armorItem.armorValue = EditorGUILayout.IntField("Armor value:", armorItem.armorValue);  // Int field for armor damage
            armorItem.armorLevel = EditorGUILayout.IntField("Armor level:", armorItem.armorLevel);  // Int field for armor level
            GUILayout.EndVertical();
        }
    }
}
