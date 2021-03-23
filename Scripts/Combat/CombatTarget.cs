using UnityEngine;
using RPG.Attributes;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (callingController.GetComponent<Fighter>().CanAttack(gameObject) == false)
            {
                return false; 
            }

            if (Input.GetMouseButtonDown(0))
            {
                //callingController.GetComponent<Fighter>().StopAttack();
                if (!GetComponent<Health>().IsDead())
                {
                    callingController.GetComponent<Fighter>().Attack(gameObject);
                    callingController.GetComponent<Fighter>().TriggerAttack();
                }

            }
            return true;
        }
    }
}