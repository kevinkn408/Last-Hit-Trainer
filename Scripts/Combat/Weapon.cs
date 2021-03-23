using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] UnityEvent onHit;

        public void OnHit()
        {
            onHit?.Invoke();
        }
    }

}