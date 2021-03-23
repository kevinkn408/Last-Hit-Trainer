using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreGround = null;

        void Update()
        {

            if(Mathf.Approximately(healthComponent.GetHealthFraction(), 0))
            {
                StartCoroutine(DisableHealthBar());
            }
            foreGround.localScale = new Vector3(healthComponent.GetHealthFraction(), 1, 1);
        }

        private IEnumerator DisableHealthBar()
        {
            yield return new WaitForSeconds(0.01f);
            GetComponentInChildren<Canvas>().enabled = false;
        }

        private void OnEnable()
        {
            GetComponentInChildren<Canvas>().enabled = true;

        }
    }

}