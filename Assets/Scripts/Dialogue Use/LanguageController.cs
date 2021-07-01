using UnityEngine;
using SDS.DialogueSystem.Enums;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt odpowiadający za wersję językową gry
/// </summary>
namespace SDS.DialogueSystem.Actions
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private LanguageType language;

        public static LanguageController Instance { get; private set; }
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
