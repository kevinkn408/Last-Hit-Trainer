using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Core;

namespace RPG.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        bool hasPressed;

        // Start is called before the first frame update
        void Start()
        {
            hasPressed = false;
        }

        private void Awake()
        {
            playerController.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !hasPressed)
            {
                hasPressed = true;
                GetComponent<Animator>().SetTrigger("OnPress");
                GetComponent<SpawnerController>().TurnOnSpawns();
                GetComponent<AudioSource>().Play();
                Destroy(gameObject, 1f);
            }
        }

        public void GrantControl()
        {
            playerController.enabled = true;
        }

    }

}