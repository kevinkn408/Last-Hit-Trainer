using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] Health currentTarget;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        public Health GetTarget { get { return currentTarget; } }

        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        float timeSinceLastAttack = Mathf.Infinity;
        //so player can attack immediately
      

        void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        // Start is called before the first frame update
        void Start()
        {
            currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (currentTarget == null) return;
            if (currentTarget.IsDead()) 
            { 
                currentTarget = null;
                return;
            }
            if (!GetIsInRange(currentTarget.transform))
            {
                GetComponent<Mover>().MoveTo(currentTarget.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(currentTarget.transform))
            {   
                return false; 
            }
            return true;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            currentTarget = combatTarget.GetComponent<Health>();
        }


        private void AttackBehavior()
        {
            transform.LookAt(currentTarget.transform);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                //this will trigger (Hit() animation event;
                //timeSinceLastAttack = 0f;
            }
        }

        public void TriggerAttack()
        {
            if (GetIsInRange(currentTarget.transform))
            {
                timeSinceLastAttack = 0f;
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
            }
            
        }


        void Hit()
        //THIS GETS CALLED ON Animation Event
        //THIS GETS CALLED ON Animation Event
        //THIS GETS CALLED ON Animation Event
        {

            if (currentTarget == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget, this.gameObject, damage);
            }
            else
            {
                currentTarget.TakeDamage(gameObject, damage);
            }

        }

        //I think this was used because animation for bow uses Shoot() and can't be changed (read only)
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.WeaponRange();
        }
      
        public void Cancel()
        {
            StopAttack();
            currentTarget = null;
            GetComponent<Mover>().Cancel();
        }

        public void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            //if (currentWeapon.value == null)
            //{
            //    Debug.Log($"{name} does not have a weapon equipped in CaptureState()");
            //}
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }
    }

}