using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpawnerController : MonoBehaviour
    {
        [SerializeField] Spawner[] spawners;
        // Start is called before the first frame update
        void Awake()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TurnOnSpawns()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.enabled = true;
            }
        }
    }

}