using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class GoldSpawner : MonoBehaviour
{
    [SerializeField] float offSetY = 1.4f;
    Pooler pooler;

    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    public void Spawn(Transform killedUnit, int score)
    {
        var goldEffect = pooler.GetPooledObject();
        goldEffect.GetComponent<GoldCounter>().SetParticleAmount(score);
        goldEffect.transform.position = killedUnit.transform.position + new Vector3(0, offSetY, 0);
        pooler.ReturnToPool(goldEffect, 2f);
    }
}
