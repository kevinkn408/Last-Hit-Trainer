using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class SelectionEffectWorld : MonoBehaviour
    {
        [SerializeField] PlayerController player;
        [SerializeField] Fighter playerFighter;
        [SerializeField] GameObject selectionMarker;

        void Update()
        {
            if (playerFighter.GetTarget != null)
            {
                selectionMarker.SetActive(true);
                GameObject target = playerFighter.GetTarget.gameObject;
                selectionMarker.transform.position = target.transform.position;
            }
            else
            {
                selectionMarker.SetActive(false);
            }
        }
    }
}