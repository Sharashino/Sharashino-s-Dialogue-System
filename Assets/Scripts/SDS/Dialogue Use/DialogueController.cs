using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SDS.CharacterStats;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using System.Collections.Generic;

// Controller responsible for showing the dialogue box and filling dialogue choices
namespace SDS.DialogueSystem.Actions
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; } // Making a singleton out of this class

        [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private GameObject dialogueUI = default;   // Our Dialogue UI 

        [Header("NPC Texts")]
        [SerializeField] private TMP_Text NPCName = default;    // NPC name text
        [SerializeField] private TMP_Text NPCAnswer = default;  // NPC dialogue text

        [Header("Talkers Images")]
        [SerializeField] private Image playerFaceImage = default;   // Player image
        [SerializeField] private GameObject playerImageGO = default;    // Player image object
        [SerializeField] private Image NPCFaceImage = default;  // NPC image
        [SerializeField] private GameObject NPCImageGO = default;   // Npc object image

        [Header("Answer Buttons")]
        [SerializeField] private Button answerButton1 = default;    // Dialogue option 1 button 
        [SerializeField] private TMP_Text answerButtonText1 = default;  // Dialogue option 1 button text 
        [Space]
        [SerializeField] private Button answerButton2 = default;    // Dialogue option 2 button
        [SerializeField] private TMP_Text answerButtonText2 = default; // Dialogue option 2 button text
        [Space]
        [SerializeField] private Button answerButton3 = default;    // Dialogue option 3 button
        [SerializeField] private TMP_Text answerButtonText3 = default;  // Dialogue option 3 button text 
        [Space]
        [SerializeField] private Button answerButton4 = default;    // Dialogue option 4 button
        [SerializeField] private TMP_Text answerButtonText4 = default;  // Dialogue option 4 button text 

        public List<Button> answerButtons = new List<Button>(); // List of buttons which is dialogue answers
        public List<TMP_Text> answerButtonsTexts = new List<TMP_Text>();    // List of texts to these buttons
        
        // Check nodes present in the passed dialogue 
        private int itemCheckNodeCount = 0; 
        private int statCheckNodeCount = 0;
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            
            AddButtonsToList(); 
            ShowDialogueUI(false);  // Hiding the UI on awake
        }

        // Showing/Hiding dialogue UI
        public void ShowDialogueUI(bool show)   
        {
            dialogueUI.SetActive(show);
        }

        // Setting NPCs dialogue and name texts 
        public void SetText(string newName, string newTextBox)  
        {
            NPCName.text = newName;
            NPCAnswer.text = newTextBox;
        }

        // Setting player and npc images
        public void SetImage(Sprite playerImage, Sprite npcImage)
        {
            playerImageGO.SetActive(true);
            NPCImageGO.SetActive(true);
            playerFaceImage.sprite = playerImage;
            NPCFaceImage.sprite = npcImage;
        }

        // Filling up buttons to show them in dialogue options
        public void SetButtons(List<string> texts, List<UnityAction> unityActions, List<StatCheckNodeData> statCheckNodeDatas, List<ItemCheckNodeData> itemCheckNodeDatas)
        {
            answerButtons.ForEach(button => button.gameObject.SetActive(false));
            UnityAction failStatCheck = null;   
            UnityAction failGetItemCheck = null;
            UnityAction failGiveItemCheck = null;

            // If you won't pass checks these messages will show up
            failStatCheck = () =>
            {
                Debug.Log("You're too weak, improve your skills...");
            };

            failGetItemCheck = () =>
            {
                Debug.Log("You can't get this item! Throwing it on the floor");
            };
            
            failGiveItemCheck = () =>
            {
                Debug.Log("You cannot return items because you don't have them!");
            };
            
            // If item check nodes are present in this dialogue
            if (itemCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < itemCheckNodeDatas.Count; i++)
                {
                    int itemToCheckIndex = itemCheckNodeDatas.IndexOf(itemCheckNodeDatas[i]);   // Getting index of item check to validate it properly 
                    
                    // Based on Item Check Type we're displaying different text in dialogue choice
                    switch (itemCheckNodeDatas[i].ItemCheckType)
                    {
                        // If item check quantity is less than 2 we're not showing the quantity in dialogue option
                        case ItemCheckNodeType.GetItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            answerButtonsTexts[i].text = "[Receive " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GetItem:
                            answerButtonsTexts[i].text = "[Receive " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            answerButtonsTexts[i].text = "[Return " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem:
                            answerButtonsTexts[i].text = "[Return " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                    }
                    
                    answerButtons[i].gameObject.SetActive(true);    // Showing the dialogue choice we just configured
                    answerButtons[i].onClick = new Button.ButtonClickedEvent(); // Adding new event to dialogue choice
                    
                    if (itemCheckNodeDatas[i].ItemCheckType == ItemCheckNodeType.GetItem)
                    {
                        // If player passes get item check validation
                        if(ValidateItemCheck(itemCheckNodeDatas[i]))
                        {
                            answerButtons[i].onClick.AddListener(unityActions[i]);  // Player passes get item check, gets to the next dialogue sequence
                            
                            if (itemCheckNodeDatas[i].ItemCheckValue > 1)
                                Debug.Log("You received - " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem);
                            else
                                Debug.Log("You received  - " + itemCheckNodeDatas[i].NodeItem);
                        }
                        else
                        {
                            answerButtons[i].onClick.AddListener(failGetItemCheck); // Player fails give item check, gets to the next dialogue sequence, item is dropped on the floor
                            answerButtons[i].onClick.AddListener(unityActions[i]);  // Gets to the next dialogue
                        }
                        
                    }
                    else if(itemCheckNodeDatas[i].ItemCheckType == ItemCheckNodeType.GiveItem)
                    {
                        // If player passes give item check validation
                        if (ValidateItemCheck(itemCheckNodeDatas[i]))
                        {
                            answerButtons[i].onClick.AddListener(unityActions[i]);  // Player passes give item check, gets to the next dialogue sequence
                            
                            if (itemCheckNodeDatas[i].ItemCheckValue > 1)
                                Debug.Log("You returned - " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem);
                            else
                                Debug.Log("You returned - " + itemCheckNodeDatas[i].NodeItem);
                        }
                        else
                        {
                            answerButtons[i].onClick.AddListener(failGiveItemCheck);    // Player fails item check, cant get further
                        }
                    }
                }
                
                itemCheckNodeCount = itemCheckNodeDatas.Count;  // How many Item Check Nodes are in this dialogue
            }
            
            // If stat check nodes are present in this dialogue 
            if (statCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < statCheckNodeDatas.Count; i++)
                {
                    int playerStatValue = AddStatCheckPlayerValues(statCheckNodeDatas[i]);  // Adding right player value to the Stat Check we're processing 

                    answerButtonsTexts[i + itemCheckNodeCount].text = "[" + statCheckNodeDatas[i].StatCheckType + " " + playerStatValue + "/" + statCheckNodeDatas[i].StatCheckValue + "] " + texts[i + itemCheckNodeCount];
                    answerButtons[i + itemCheckNodeCount].gameObject.SetActive(true);
                    answerButtons[i + itemCheckNodeCount].onClick = new Button.ButtonClickedEvent();
                    answerButtons[i + itemCheckNodeCount].onClick.AddListener(ValidateStatCheck(statCheckNodeDatas[i]) ? unityActions[i] : failStatCheck);
                }

                statCheckNodeCount = statCheckNodeDatas.Count;  // How many Stat Check Nodes are in this dialogue
            }   
            
            // Processing left dialogue choices
            for (int i = statCheckNodeCount + itemCheckNodeCount; i < texts.Count; i++)
            {
                answerButtonsTexts[i].text = texts[i];
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].onClick = new Button.ButtonClickedEvent();
                answerButtons[i].onClick.AddListener(unityActions[i]);
            }

            // Resetting both counters after processing check nodes
            statCheckNodeCount = 0; 
            itemCheckNodeCount = 0;
        }

        // Validating if player can get or receive item
        public bool ValidateItemCheck(ItemCheckNodeData itemCheckNodeData)
        {
            switch (itemCheckNodeData.ItemCheckType)
            {
                // You will always pass GetItem check since NPC can drop the item on the floor if you dont have space in your inventory 
                case ItemCheckNodeType.GetItem:
                {
                    /*
                    *  Here you should place code that is checking players inventory space to fit specific item
                    *  itemCheckNodeData.NodeItem - Item we're checking
                    *  itemCheckNodeData.NodeItem. - Quantity of item we're checking
                    *  itemCheckNodeData.NodeItem.itemWidth - Item Width we're checking
                    *  itemCheckNodeData.NodeItem.itemWidth - Item Height we're checking
                    *
                    *  This bool returns true now to make Get Item Check work
                    */
                    return true;
                }
                case ItemCheckNodeType.GiveItem:
                {
                    /*
                     *  Here you should place code that is checking players inventory for specific item
                     *  itemCheckNodeData.NodeItem - Item we're checking
                     *  itemCheckNodeData.ItemCheckValue - Quantity of item we're checking
                     *
                     *  This bool returns true now to make Give Item Check work
                     */
                    return true;
                }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        // Validating if player can pass Stat Check
        private bool ValidateStatCheck(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.StatCheckType)
            {
                case StatCheckType.Exp:
                    return statCheckNodeData.StatCheckValue <= playerStats.ExperiencePoints;
                case StatCheckType.Level:
                    return statCheckNodeData.StatCheckValue <= playerStats.Level;
                case StatCheckType.Health:
                    return statCheckNodeData.StatCheckValue <= playerStats.Health.BaseValue;
                case StatCheckType.Mana:
                    return statCheckNodeData.StatCheckValue <= playerStats.Mana.BaseValue;
                case StatCheckType.Armor:
                    return statCheckNodeData.StatCheckValue <= playerStats.Armor.BaseValue;
                case StatCheckType.Damage:
                    return statCheckNodeData.StatCheckValue <= playerStats.Damage.BaseValue;
                case StatCheckType.Strength:
                    return statCheckNodeData.StatCheckValue <= playerStats.Strength.BaseValue;
                case StatCheckType.Agility:
                    return statCheckNodeData.StatCheckValue <= playerStats.Agility.BaseValue;
                case StatCheckType.Intelligence:
                    return statCheckNodeData.StatCheckValue <= playerStats.Intelligence.BaseValue;
                case StatCheckType.Vitality:
                    return statCheckNodeData.StatCheckValue <= playerStats.Vitality.BaseValue;
                case StatCheckType.Luck:
                    return statCheckNodeData.StatCheckValue <= playerStats.Luck.BaseValue;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
        
        // Returning players value of stat we're checking
        private int AddStatCheckPlayerValues(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.StatCheckType)
            {
                case StatCheckType.Exp:
                    return playerStats.Level;
                case StatCheckType.Level:
                    return playerStats.Level;
                case StatCheckType.Health:
                    return playerStats.Health.BaseValue;
                case StatCheckType.Mana:
                    return playerStats.Mana.BaseValue;
                case StatCheckType.Armor:
                    return playerStats.Armor.BaseValue;
                case StatCheckType.Damage:
                    return playerStats.Damage.BaseValue;
                case StatCheckType.Strength:
                    return playerStats.Strength.BaseValue;
                case StatCheckType.Agility:
                    return playerStats.Agility.BaseValue;
                case StatCheckType.Intelligence:
                    return playerStats.Intelligence.BaseValue;
                case StatCheckType.Vitality:
                    return playerStats.Vitality.BaseValue;
                case StatCheckType.Luck:
                    return playerStats.Luck.BaseValue;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
        
        // Adding dialogue choices buttons to list
        private void AddButtonsToList()
        {
            answerButtons.Add(answerButton1);
            answerButtons.Add(answerButton2);
            answerButtons.Add(answerButton3);
            answerButtons.Add(answerButton4);

            answerButtonsTexts.Add(answerButtonText1);
            answerButtonsTexts.Add(answerButtonText2);
            answerButtonsTexts.Add(answerButtonText3);
            answerButtonsTexts.Add(answerButtonText4);
        }
    }
}



