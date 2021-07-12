using UnityEngine;
using UnityEditor;
using SIS.Items;
using SIS.Items.Enums;
using SIS.Actions.Interaction;

// Main class used to create custom editor for item creation
namespace SIS.Inventory.Editors
{
    public class ItemEditor : EditorWindow
    {
        private GameObject newItem; // New item we're creating
        private GameObject finalItem;   // Final item we're creating
        private GameObject interactionZone; // Interaction zone we're adding to item
        private int index = 0;  // Index of Item Creation page

        private ItemTypes itemType;

        [MenuItem("Sharashino Tools/Create Item")]
        
        static void Init()  // Creating new EditorWindow based on this ItemEditor
        {
            ItemEditor _editor = (ItemEditor)GetWindow(typeof(ItemEditor)); 
            _editor.titleContent = new GUIContent("Item Creator");
            _editor.Show();
        }
       
        private void OnGUI()    // Displaying base item creation fields
        {
            if (index == 0)
            {
                GUILayout.BeginVertical("HelpBox");

                newItem = (GameObject)EditorGUILayout.ObjectField("Item model: ", newItem, typeof(GameObject), true);   // Field for item object
                interactionZone = (GameObject)EditorGUILayout.ObjectField("Interaction Zone model: ", interactionZone, typeof(GameObject), true);   // Field for interaction zone

                GUILayout.TextArea("Item Type", EditorStyles.boldLabel);
                itemType = (ItemTypes)EditorGUILayout.EnumPopup(itemType, GUILayout.Width(100));    // Pop-up for item type

                if (GUILayout.Button("Next"))
                {
                    if (newItem != null)
                    {
                        finalItem = Instantiate(newItem);

                        switch (itemType)   // Adding correct item script to item we're creating 
                        {
                            case ItemTypes.Weapon:
                            {
                                finalItem.AddComponent<WeaponItem>();
                                break;
                            }
                            case ItemTypes.Armor:
                            {
                                finalItem.AddComponent<ArmorItem>();
                                break;
                            }
                            case ItemTypes.Trinket:
                            {
                                finalItem.AddComponent<TrinketItem>();
                                break;
                            }
                            case ItemTypes.Consumable:
                            {
                                finalItem.AddComponent<ConsumableItem>();
                                break;
                            }
                            case ItemTypes.None:
                            {
                                finalItem.AddComponent<Item>(); // Option for making scrap items
                                break;
                            }
                            default:
                                finalItem.AddComponent<Item>();
                                break;
                        }

                        finalItem.tag = "Item";
                        interactionZone = Instantiate(interactionZone, finalItem.transform, true);
                        ItemPickup itemPickup = finalItem.AddComponent<ItemPickup>();
                        itemPickup.InteractionZone = interactionZone.GetComponent<InteractionZone>();

                        Selection.activeGameObject = finalItem;
                        SceneView.lastActiveSceneView.FrameSelected();

                        index++;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Hey!","You forgot to put item model", "Sorry...");
                    }
                }

                GUILayout.EndVertical();
            }
            else if(index == 1)
            {
                GUILayout.TextArea(newItem.name, EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox");

                if (finalItem != null)
                {
                    Editor newItemEditor;

                    switch (itemType)   // Creating detailed editor based on item we're creating
                    {
                        case ItemTypes.Weapon:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<WeaponItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case ItemTypes.Armor:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<ArmorItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case ItemTypes.Trinket:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<TrinketItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case ItemTypes.Consumable:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<ConsumableItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case ItemTypes.None:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<Item>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("You deleted your item in the middle of creation, try again.", MessageType.Warning, true);
                }

                EditorGUILayout.HelpBox("Don't forget to save new item as prefab by dragging it into project folder.", MessageType.Warning);

                if (GUILayout.Button("Finish item creation"))
                {
                    finalItem.name = finalItem.GetComponent<Item>().itemName + " - Item";  // Setting name of newly created item
                    Close();    //Closing this custom editor when you're done defining fields
                }
                
                GUILayout.EndVertical();
            }
        }
    }
}