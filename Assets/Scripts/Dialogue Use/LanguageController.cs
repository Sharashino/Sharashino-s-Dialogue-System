using UnityEngine;
using SDS.DialogueSystem.Enums;

// Script responsible for game language
// Every dialogue is running language matching this script
namespace SDS.DialogueSystem.Actions
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private LanguageType language; // Our language

        public static LanguageController Instance { get; private set; } // Singleton for easy reference
        public LanguageType Language { get => language; set => language = value; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
