using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Text healthDisplay = null;
        Health health;
        void Awake()
        {
            health = FindObjectOfType<Player>().GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            healthDisplay.text = String.Format("{0:0}/{1:0}",health.GetHealthPoints(), health.GetMaximumHealthPoints());
        }
    }
}
