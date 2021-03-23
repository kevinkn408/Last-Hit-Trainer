using UnityEngine;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        private void BuildLookUp()
        {
            if (lookupTable != null) return;
            //if dictionary hasn't been built, proceed to build one

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            //declaring the value of lookupTable with dictionary constructor;

            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                //for each of the progressionCharacterClass in array characterClasses

                Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();
                //create a dictionary containing <key Stat> with <value of type array of floats>

                foreach(ProgressionStat progressionStat in progressionCharacterClass.stats)
                {
                    //for each one of the progressionStat in the array of stats in progressionCharacterClass.stats

                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                    //setting the <key stat> to be that of each [0]array value in progressionStat.levels;
                }
                lookupTable[progressionCharacterClass.characterClass] = statLookupTable;
                //each <key, value> pair of Dictionary statLookupTable will now be the <key CharacterClass> of lookupTable 

            }
        }

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            if(levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1];
        }

        public int GetLevel(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

    }

}