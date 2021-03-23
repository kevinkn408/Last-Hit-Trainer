using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;
using System.Collections;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercent = 0;
        [SerializeField] bool isInvulnerable = false;
        [SerializeField] public TakeDamageEvent onTakeDamage;
        [SerializeField] public DieEvent onDie;
        //public Action onDieOverride;
        

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        [System.Serializable]
        public class DieEvent : UnityEvent<GameObject, GameObject> { }


        LazyValue<float> healthPoints;
        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);

            GetComponent<CapsuleCollider>().enabled = true;

            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            isDead = false;
            //healthPoints = new LazyValue<float>(GetInitialHealth);

            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercent / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (!isInvulnerable) 
            { 
                healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); 
            }
/*            print(gameObject.name + "took damage of " + damage);
*/            
            if(healthPoints.value <= 0)
            {
                onDie?.Invoke(this.gameObject, instigator);
                //onDieOverride?.Invoke();
                AwardEXP(instigator);
                Die();
            }
            else
            {
                onTakeDamage?.Invoke(damage);
            }
        }


        private void AwardEXP(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaximumHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPercentage()
        {
            return 100 * GetHealthFraction();
            //divide current healthPoints with total healthpoints of that level.
        }

        public float GetHealthFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Die()
        {
            print("died with " + healthPoints.value);
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        public void Heal(float amountToHeal)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amountToHeal, GetMaximumHealthPoints());

            //healthPoints.value += amountToHeal;
            //if(healthPoints.value > GetMaximumHealthPoints())
            //{
            //    healthPoints.value = GetMaximumHealthPoints();
            //}
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if(healthPoints.value <= 0)
            {
                Die();
            }
        }
    }

}