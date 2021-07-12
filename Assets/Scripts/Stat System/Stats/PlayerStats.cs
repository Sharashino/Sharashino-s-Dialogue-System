using UnityEngine;
using SDS.CharacterStats.Stats;

// Script defining player stats 
namespace SDS.CharacterStats
{
    public class PlayerStats : MonoBehaviour
    {
        #region singleton

        public static PlayerStats instance;
        private void Awake()
        {
            instance = this;

        }
        #endregion

        [Header("Experience stats")]
        [SerializeField] private int experiencePoints;
        [SerializeField] private int level;

        [Header("Soft stats")]
        [SerializeField] private int maxHealth;
        [SerializeField] private Stat health;
        [SerializeField] private Stat mana;
        [SerializeField] protected Stat armor;
        [SerializeField] protected Stat damage;

        [Header("Hard stats")]
        [SerializeField] private Stat strength;
        [SerializeField] private Stat agility;
        [SerializeField] private Stat intelligence;
        [SerializeField] private Stat vitality;
        [SerializeField] private Stat luck;
        
        #region Properties
        
        public int ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }
        public int Level { get => level ; set => level = value; }
        public int MaxHealth { get => maxHealth ; set => maxHealth = value; }
        public Stat Health { get => health ; set => health = value; }
        public Stat Mana { get => mana ; set => mana = value; }
        public Stat Armor { get => armor ; set => armor = value; }
        public Stat Damage { get => damage ; set => damage = value; }
        public Stat Strength { get => strength ; set => strength = value; }
        public Stat Agility { get => agility ; set => agility = value; }
        public Stat Intelligence { get => intelligence ; set => intelligence = value; }
        public Stat Vitality { get => vitality ; set => vitality = value; }
        public Stat Luck { get => luck ; set => luck = value; }
      
        // Adding value on level up
        public void AddToLevel(int value)
        {
            level += value;
        }
        
        // Adding value on getting experience points
        public void SetExperiencePoints(int value)
        {
            experiencePoints += value;
        }
        
        #endregion
        
        private void Start()
        {
            MaxHealth = Health.BaseValue;   // Max health is equal to health value on start
        }

        private void Update()
        {
            if (health.BaseValue <= 0)  // If health is 0, player is dead
            {
                CharacterDie();
            }
        }

        protected virtual void CharacterDie()
        {
            print(transform.name + " died");
        }   
    }
}



