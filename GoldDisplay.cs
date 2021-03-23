using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace RPG.Stats
{
    public class GoldDisplay : MonoBehaviour
    {
        [SerializeField] Text goldDisplay = null;
        [SerializeField] TextMeshProUGUI goldDisplayTMP = null;
        LastHitManager lastHitManager;

        // Start is called before the first frame update
        void Awake()
        {
            lastHitManager = FindObjectOfType<LastHitManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (goldDisplayTMP != null)
            {
                goldDisplayTMP.text = lastHitManager.currency.ToString();
            }
            else
            {
                goldDisplay.text = lastHitManager.currency.ToString();
            }
        }
    }
}