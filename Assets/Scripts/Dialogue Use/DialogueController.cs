using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using SDS.CharacterStats;
using SDS.DialogueSystem.SO;
using SDS.DialogueSystem.Enums;
using System.Collections.Generic;
//using SDS.Inventory;

// <summary>
// Napisane przez sharashino
// 
// Controller odpowiadający za wyświetlanie okna z dialogiem
// </summary>
namespace SDS.DialogueSystem.Actions
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

       // [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private GameObject dialogueUI = default;

        [Header("NPC Texts")]
        [SerializeField] private TMP_Text NPCName = default;
        [SerializeField] private TMP_Text NPCAnswer = default;

        [Header("Talkers Images")]
        [SerializeField] private Image playerFaceImage = default;
        [SerializeField] private GameObject playerImageGO = default;
        [SerializeField] private Image NPCFaceImage = default;
        [SerializeField] private GameObject NPCImageGO = default;

        [Header("Answer Buttons")]
        [SerializeField] private Button answerButton1 = default;
        [SerializeField] private TMP_Text answerButtonText1 = default;
        [Space]
        [SerializeField] private Button answerButton2 = default;
        [SerializeField] private TMP_Text answerButtonText2 = default;
        [Space]
        [SerializeField] private Button answerButton3 = default;
        [SerializeField] private TMP_Text answerButtonText3 = default;
        [Space]
        [SerializeField] private Button answerButton4 = default;
        [SerializeField] private TMP_Text answerButtonText4 = default;

        public List<Button> answerButtons = new List<Button>();
        public List<TMP_Text> answerButtonsTexts = new List<TMP_Text>();
        private int itemCheckNodeCount = 0;
        private int statCheckNodeCount = 0;
      //  public InventoryClass inventoryClass;
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            
            //inventoryClass = InventoryClass.Instance;
            AddButtonsToList();
            ShowDialogueUI(false);
        }

        public void ShowDialogueUI(bool show)
        {
            dialogueUI.SetActive(show);
        }

        public void SetText(string newName, string newTextBox)
        {
            NPCName.text = newName;
            NPCAnswer.text = newTextBox;
        }

        public void SetImage(Sprite playerImage, Sprite npcImage)
        {
            playerImageGO.SetActive(true);
            NPCImageGO.SetActive(true);
            playerFaceImage.sprite = playerImage;
            NPCFaceImage.sprite = npcImage;
        }

        //Setting up buttons to show them in dialogue options
        public void SetButtons(List<string> texts, List<UnityAction> unityActions, List<StatCheckNodeData> statCheckNodeDatas, List<ItemCheckNodeData> itemCheckNodeDatas)
        {
            answerButtons.ForEach(button => button.gameObject.SetActive(false));
            UnityAction statCheck = null;
            UnityAction getItemCheck = null;
            UnityAction giveItemCheck = null;

            statCheck = () =>
            {
                Debug.Log("Twoje statystyki są zbyt słabe, podszlifuj swoje umiejętności...");
            };

            getItemCheck = () =>
            {
                Debug.Log("Nie możesz dostać tego itemu, wyrzucam go na podłoge!");
            };
            
            giveItemCheck = () =>
            {
                Debug.Log("Nie możesz oddać przedmiotów, bo ich nie posiadasz!");
            };
            
            if (itemCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < itemCheckNodeDatas.Count; i++)
                {
                    int itemToCheckIndex = itemCheckNodeDatas.IndexOf(itemCheckNodeDatas[i]); 
                    
                    /*
                    switch (itemCheckNodeDatas[i].ItemCheckType)
                    {
                        case ItemCheckNodeType.GetItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            answerButtonsTexts[i].text = "[Otrzymaj " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GetItem:
                            answerButtonsTexts[i].text = "[Otrzymaj " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            answerButtonsTexts[i].text = "[Oddaj " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem:
                            answerButtonsTexts[i].text = "[Oddaj " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                    }
                    */
                    
                    answerButtons[i].gameObject.SetActive(true);
                    answerButtons[i].onClick = new Button.ButtonClickedEvent();
                    
                    if (itemCheckNodeDatas[i].ItemCheckType == ItemCheckNodeType.GetItem)
                    {
                        /*
                        if(inventoryClass.CheckFreeSpaceForAllSlots(itemCheckNodeDatas[i].NodeItem.itemWidth, itemCheckNodeDatas[i].NodeItem.itemHeight))
                        {
                            answerButtons[i].onClick.AddListener(delegate { ValidateItemCheck(itemCheckNodeDatas[itemToCheckIndex]); });
                            answerButtons[i].onClick.AddListener(unityActions[i]);
                        }
                        else
                        {
                            answerButtons[i].onClick.AddListener(getItemCheck);
                            answerButtons[i].onClick.AddListener(unityActions[i]);
                        }
                        */
                    }
                    else if(itemCheckNodeDatas[i].ItemCheckType == ItemCheckNodeType.GiveItem)
                    {
                        /*
                        if (inventoryClass.CheckForItem(itemCheckNodeDatas[i].NodeItem, itemCheckNodeDatas[i].ItemCheckValue, false))
                        {
                            answerButtons[i].onClick.AddListener(delegate { ValidateItemCheck(itemCheckNodeDatas[itemToCheckIndex]); });
                            answerButtons[i].onClick.AddListener(unityActions[i]);
                        }
                        else
                        {
                            answerButtons[i].onClick.AddListener(giveItemCheck);
                        }
                        */
                    }
                }
                
                itemCheckNodeCount = itemCheckNodeDatas.Count;
            }
            
            if (statCheckNodeDatas.Count > 0)
            {
                /*
                for (int i = 0; i < statCheckNodeDatas.Count; i++)
                {
                    
                    int playerValue = AddStatCheckPlayerValues(statCheckNodeDatas[i]);

                    answerButtonsTexts[i + itemCheckNodeCount].text = "[" + statCheckNodeDatas[i].StatCheckType + " " + playerValue + "/" + statCheckNodeDatas[i].StatCheckValue + "] " + texts[i + itemCheckNodeCount];
                    answerButtons[i + itemCheckNodeCount].gameObject.SetActive(true);
                    answerButtons[i + itemCheckNodeCount].onClick = new Button.ButtonClickedEvent();
                    answerButtons[i + itemCheckNodeCount].onClick.AddListener(ValidateStatCheck(statCheckNodeDatas[i]) ? unityActions[i] : statCheck);
                }
                */

                statCheckNodeCount = statCheckNodeDatas.Count;
            }   
            
            for (int i = statCheckNodeCount + itemCheckNodeCount; i < texts.Count; i++)
            {
                answerButtonsTexts[i].text = texts[i];
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].onClick = new Button.ButtonClickedEvent();
                answerButtons[i].onClick.AddListener(unityActions[i]);
            }

            statCheckNodeCount = 0;
            itemCheckNodeCount = 0;
        }

        public bool ValidateItemCheck(ItemCheckNodeData itemCheckNodeData)
        {
            /*
            switch (itemCheckNodeData.ItemCheckType)
            {
                case ItemCheckNodeType.GetItem:
                {
                    if (itemCheckNodeData.ItemCheckValue > 1)
                        Debug.Log("Otrzymałeś - " + itemCheckNodeData.ItemCheckValue + " " + itemCheckNodeData.NodeItem);
                    else
                        Debug.Log("Otrzymałeś - " + itemCheckNodeData.NodeItem);
                    
                    return true;
                }
                case ItemCheckNodeType.GiveItem:
                {
                    //inventoryClass.CheckForItem(itemCheckNodeData.NodeItem, itemCheckNodeData.ItemCheckValue, true);
                    
                    if (itemCheckNodeData.ItemCheckValue > 1)
                        Debug.Log("Oddałeś - " + itemCheckNodeData.ItemCheckValue + " " + itemCheckNodeData.NodeItem);
                    else
                        Debug.Log("Oddałeś - " + itemCheckNodeData.NodeItem);
                    
                    return true;
                }
                default:
                    break;
            }
            */
            
            return false;
        }

        /*
        private bool ValidateStatCheck(StatCheckNodeData statCheckNodeData)
        {
            
            switch (statCheckNodeData.StatCheckType)
            {
                case StatCheckType.Exp:
                    return statCheckNodeData.StatCheckValue < playerStats.ExperiencePoints;
                case StatCheckType.Level:
                    return statCheckNodeData.StatCheckValue < playerStats.Level;
                case StatCheckType.Health:
                    return statCheckNodeData.StatCheckValue < playerStats.Health.BaseValue;
                case StatCheckType.Mana:
                    return statCheckNodeData.StatCheckValue < playerStats.Mana.BaseValue;
                case StatCheckType.Armor:
                    return statCheckNodeData.StatCheckValue < playerStats.Armor.BaseValue;
                case StatCheckType.Damage:
                    return statCheckNodeData.StatCheckValue < playerStats.Damage.BaseValue;
                case StatCheckType.Strenght:
                    return statCheckNodeData.StatCheckValue < playerStats.Strength.BaseValue;
                case StatCheckType.Agility:
                    return statCheckNodeData.StatCheckValue < playerStats.Agility.BaseValue;
                case StatCheckType.Intelligence:
                    return statCheckNodeData.StatCheckValue < playerStats.Intelligence.BaseValue;
                case StatCheckType.Vitality:
                    return statCheckNodeData.StatCheckValue < playerStats.Vitality.BaseValue;
                case StatCheckType.Luck:
                    return statCheckNodeData.StatCheckValue < playerStats.Luck.BaseValue;
                default:
                    return false;
            }
        }
        */
        
        /*
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
                case StatCheckType.Strenght:
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
                    return playerStats.Luck.BaseValue;
            }
        }
        */
        
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



