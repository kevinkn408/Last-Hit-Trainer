using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Core
{
    public class Pooler : MonoBehaviour
    {
        [SerializeField] GameObject objectToPool = null;
        [SerializeField] int poolSize;
        [SerializeField] bool isCharacter;
        public List<GameObject> pooledObjects = new List<GameObject>();

        private void Start()
        {
            CreatePool();
        }
        void CreatePool()
        {
            if (objectToPool == null) return;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledObject = Instantiate(objectToPool, transform);
                pooledObject.transform.parent = this.transform;
                if (isCharacter)
                {
                    pooledObject.GetComponent<Health>().onDie.AddListener((GameObject unitKilled, GameObject instigator) => ReturnToPool(pooledObject, 2));
                }
                pooledObjects.Add(pooledObject);
                pooledObject.SetActive(false);
            }
        }
        public List<GameObject> GetPooledObjects()
        {
            return pooledObjects;
        }


        public GameObject GetPooledObject()
        {
            GameObject nextObject = null;
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    nextObject = pooledObjects[i];
                    nextObject.SetActive(true);
                    return nextObject;
                }
            }
            return null;
            
        }

        public void ReturnToPool(GameObject returnObject, float delay)
        {
            StartCoroutine(ReturnDelay(returnObject, delay));
            //get reference to object 
            //return object to pool
        }

        IEnumerator ReturnDelay(GameObject returnObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            returnObject.SetActive(false);
            returnObject.transform.position = this.transform.position;
        }
    }

}