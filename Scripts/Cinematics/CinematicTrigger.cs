using UnityEngine;
using UnityEngine.Playables;
using System;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasTriggered = false;
        public event Action OnFinish;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !hasTriggered)
            {
                hasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
