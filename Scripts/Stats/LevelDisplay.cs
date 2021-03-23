using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Text levelDisplay = null;
        BaseStats baseStats;

        void Awake()
        {
            baseStats = FindObjectOfType<Player>().GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {
            levelDisplay.text = String.Format("{0}", baseStats.GetLevel().ToString());
        }
    }
}
