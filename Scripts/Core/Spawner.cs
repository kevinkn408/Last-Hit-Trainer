using System.Collections;
using UnityEngine;
using RPG.Control;
using System;
using RPG.Attributes;

namespace RPG.Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] float timeBetweenSpawns = 3f;
        [SerializeField] PatrolPath path = null;
        [SerializeField] Transform pathStart = null;
        [SerializeField] bool spawnOnDie = false;

        Pooler pooler;
        public Action onSpawn;
        WaitForSeconds delay = new WaitForSeconds(17f);

        GameObject emptyA;
        GameObject emptyB;

        void Start()
        {
            //Spawn(emptyA, emptyB);
            StartCoroutine(SpawnUnits());
            var units = pooler.GetPooledObjects();
            foreach(GameObject unit in units)
            {
                unit.GetComponent<Health>().onDie.AddListener(FindObjectOfType<LastHitManager>().CheckLastWhoLastHit);
                if (spawnOnDie)
                {
                    unit.GetComponent<Health>().onDie.AddListener(Spawn);
                }
            }
            
        }

        void Awake()
        {
            pooler = GetComponent<Pooler>();
            if (pathStart == null)
            {
                pathStart = this.transform;
            }
        }

        IEnumerator SpawnUnits()
        {
            while (true)
            {
                Spawn(emptyA,emptyB);
                yield return delay;
            }
        }

        private void Spawn(GameObject a, GameObject b)
        {
            var spawnObject = pooler.GetPooledObject();
            if(spawnObject !=null)
            {
                spawnObject.transform.position = this.transform.position;
                if (path != null)
                {
                    spawnObject.GetComponent<AIController>().AssignPath(path);
                }
            }  
        }
    }
}