using SIS.Items;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SIS.Inventory.Editors
{
    [CustomEditor(typeof(Item))]
    public class ItemCustomInspector : Editor
    {
        private Item item;  //  Reference to Item we're displaying

        public override void OnInspectorGUI()
        {
            item = target as Item;  // Telling unity to target this object as Item 
            
            if(item) DrawGeneralItem(); // Drawing all item fields
            
            var style = new GUIStyle(GUI.skin.button);  // Style for our save changes button
            style.normal.textColor = Color.red;
            style.fontStyle = FontStyle.Bold;
            
            if (GUILayout.Button("Save changes?", style))   // Button for saving changes
            {
                EditorUtility.SetDirty(item); // Marking this item as dirty to ensure all changes are registered
                EditorSceneManager.MarkSceneDirty(item.gameObject.scene);   // Marking the scene as modified so it takes our changes
            }
        }

        public void DrawGeneralItem()
        {
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Basic Item Settings", EditorStyles.boldLabel);
            item.itemName = EditorGUILayout.TextField("Item Name:", item.itemName);   // Text field for item name
            item.itemDescription = EditorGUILayout.TextField("Item Description:", item.itemDescription);  // Text field for item description
            item.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon:", item.itemIcon, typeof(Sprite), false); // Object field of type sprite for item icon
            item.inventoryItemSprite = (Sprite)EditorGUILayout.ObjectField("Item Inventory Icon:", item.inventoryItemSprite, typeof(Sprite), false); // Object field of type sprite for item inventory image
            item.itemID = EditorGUILayout.IntField("Item ID", item.itemID);  // Int field for item id

            if (GUILayout.Button("Generate random ID?"))    
            {
                item.itemID = Random.Range(0, int.MaxValue);
            }

            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Inventory Item Settings", EditorStyles.boldLabel);
            item.itemWidth = EditorGUILayout.IntField("Inventory Width", item.itemWidth); // Int field for item inventory width
            item.itemHeight = EditorGUILayout.IntField("Inventory Height", item.itemHeight); // Int field for item inventory height
            GUILayout.EndVertical();
            
            item.isStackable = EditorGUILayout.Toggle("Stackable", item.isStackable); // Toggle for item stackable

            if (item.isStackable)
            {
                item.itemMaxStackSize = EditorGUILayout.IntSlider("Max Stack Size", item.itemMaxStackSize, 1, 100); // Int field for item max stack size
            }
            
            GUILayout.EndVertical();
        }
    }
}
