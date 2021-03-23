using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Stats
{
    public class ScoreKeeper : MonoBehaviour
    {
        [SerializeField] int lastHitScore = 0;
        [SerializeField] UnityEvent onScore;
        Spawner[] spawners = null;

        private void Awake()
        {
            spawners = FindObjectsOfType<Spawner>();
        }
        //void Start()
        //{
        //    foreach(Spawner spawner in spawners)
        //    {
        //        spawner.onSpawn += FindSpawns;
        //    }
        //}

        //void FindSpawns()
        //{
        //    GameObject enemy = GameObject.FindWithTag("Enemy");

        //    if (enemy != null) 
        //    {
        //        enemy.GetComponent<Health>().onDie.AddListener(() => OnMiss());
        //    }
        //}

        // Update is called once per frame
        void Update()
        {

        }

        void OnScore()
        {
            print("score");
        }

        void OnMiss()
        {
            print("miss");
        }

        public void UpdateScore()
        {
            lastHitScore += 1;
            print(lastHitScore);
        }

        public void ResetScore()
        {
            lastHitScore = 0;
            print(lastHitScore);
        }
    }
}
