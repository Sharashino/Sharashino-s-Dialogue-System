using UnityEngine;
using SDS.CharacterStats.Stats;

// <summary>
// Napisane przez Sharashino
// 
// Skrypt definiujący statystyki postaci 
// </summary>
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
      
        public void AddToLevel(int value)
        {
            level += value;
        }
        public void SetExperiencePoints(int value)
        {
            experiencePoints += value;
        }
        
        #endregion
        
        private void Start()
        {
            MaxHealth = Health.BaseValue;
        }

        private void Update()
        {
            if (health.BaseValue <= 0)
            {
                CharacterDie();
            }
        }

        protected virtual void Heal(int healAmount)
        {
            if(MaxHealth + healAmount > Health.BaseValue)
            {
                Health.AddModifier(5);
            }
            else
            {
                MaxHealth += healAmount;
            }
        }

        protected virtual void TakeDamage(int damageAmount)
        {
            //logic for damage reduction goes here
            damageAmount -= Armor.BaseValue;
            damageAmount = Mathf.Clamp(damageAmount, 0, int.MaxValue);

            MaxHealth -= damageAmount;

            print(transform.name + " takes " + damageAmount + " damage");
            if (MaxHealth <= 0)
            {
                CharacterDie();
            }
        }

        protected virtual void CharacterDie()
        {
            //Die lol
            //Overwritten
            print(transform.name + " died");
        }   
    }
}



