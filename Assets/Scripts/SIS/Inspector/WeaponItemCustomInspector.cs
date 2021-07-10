using SIS.Items;
using SIS.Items.Enums;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SIS.Inventory.Editors
{
    [CustomEditor(typeof(WeaponItem))]
    public class WeaponItemCustomInspector : Editor
    {
        private WeaponItem weaponItem;  // Reference to WeaponItem we're displaying

        public override void OnInspectorGUI()
        {
            weaponItem = target as WeaponItem;  // Telling unity to target this object as WeaponItem 
            
            if(weaponItem) DrawGeneralItem(); // Drawing all weapon fields
            
            var buttonStyle = new GUIStyle(GUI.skin.button);    // Style for our save changes button
            buttonStyle.normal.textColor = Color.red;
            buttonStyle.fontStyle = FontStyle.Bold;
            
            if (GUILayout.Button("Save changes", buttonStyle))
            {
                EditorUtility.SetDirty(weaponItem); // Marking this weapon item as dirty to ensure all changes are registered
                EditorSceneManager.MarkSceneDirty(weaponItem.gameObject.scene);   // Marking the scene as modified so it takes our changes
            }
        }

        public void DrawGeneralItem() 
        {
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Basic Weapon Item Settings", EditorStyles.boldLabel);
            weaponItem.itemName = EditorGUILayout.TextField("Weapon name:", weaponItem.itemName);   // Text field for weapon name
            weaponItem.itemDescription = EditorGUILayout.TextField("Weapon description:", weaponItem.itemDescription);  // Text field for weapon description
            weaponItem.itemIcon = (Sprite)EditorGUILayout.ObjectField("Weapon icon:", weaponItem.itemIcon, typeof(Sprite), false); // Object field of type sprite for weapon icon
            weaponItem.inventoryItemSprite = (Sprite)EditorGUILayout.ObjectField("Inventory icon:", weaponItem.inventoryItemSprite, typeof(Sprite), false); // Object field of type sprite for weapon inventory image
            weaponItem.itemID = EditorGUILayout.IntField("Weapon ID:", weaponItem.itemID);  // Int field for weapon ID
            
            if (GUILayout.Button("Generate random weapon ID"))    // Generating random ID for weapon
            {
                weaponItem.itemID = Random.Range(0, int.MaxValue);
            }
            
            GUILayout.Label("Inventory Weapon Settings", EditorStyles.boldLabel);
            weaponItem.itemWidth = EditorGUILayout.IntField("Inventory Width:", weaponItem.itemWidth);    // Int field for inventory weapon width
            weaponItem.itemHeight = EditorGUILayout.IntField("Inventory Height:", weaponItem.itemHeight); // Int field for inventory weapon height
            weaponItem.isStackable = EditorGUILayout.Toggle("Stackable", weaponItem.isStackable);   // Toggle for weapon stackable

            if (weaponItem.isStackable)
            {
                weaponItem.itemMaxStackSize = EditorGUILayout.IntSlider("Max stack size", weaponItem.itemMaxStackSize, 1, 100); // Int field for weapon max stack size
            }
            
            GUILayout.EndVertical();

            GUILayout.Label("Detailed Weapon Item Settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            weaponItem.weaponType = (WeaponTypes)EditorGUILayout.EnumPopup("Weapon type:", weaponItem.weaponType);  // Enum popup field for weapon type
            weaponItem.weaponDamage = EditorGUILayout.IntField("Weapon damage:", weaponItem.weaponDamage);  // Int field for weapon damage
            weaponItem.weaponLevel = EditorGUILayout.IntField("Weapon level:", weaponItem.weaponLevel);  // Int field for weapon level
            GUILayout.EndVertical();
        }
    }
}
