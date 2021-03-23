using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;
using System;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter = null;
        [SerializeField] Text healthDisplay = null;

        // Start is called before the first frame update
        void Awake()
        {
            fighter = FindObjectOfType<Player>().GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if(fighter.GetTarget == null)
            {
                healthDisplay.text = "No Target";
                return;
            }
            Health health = fighter.GetTarget.GetComponent<Health>();
            healthDisplay.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaximumHealthPoints());
        }
    }

}