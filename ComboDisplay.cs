using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Stats
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] Text comboDisplay = null;
        [SerializeField] TextMeshProUGUI comboDisplayTMP = null;
        LastHitManager lastHitManager;

        void Awake()
        {
            lastHitManager = FindObjectOfType<LastHitManager>();
        }

        void Update()
        {
            if (comboDisplayTMP != null)
            {
                UI_DisplayCombo();
            }
            else
            {
                comboDisplay.text = lastHitManager.score.ToString();
            }
        }

        private void UI_DisplayCombo()
        {
            comboDisplayTMP.text = lastHitManager.score.ToString();
            if (lastHitManager.score > 1)
            {
                comboDisplayTMP.enabled = true;
            }
            else
            {
                comboDisplayTMP.enabled = false;
            }
        }
    }
}