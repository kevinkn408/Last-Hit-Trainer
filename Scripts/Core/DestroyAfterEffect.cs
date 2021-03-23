using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy = null;
    void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            if(objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
