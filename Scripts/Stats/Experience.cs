using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        public event Action onExperienceGained;

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
            //print("I gained " + experience);
            experiencePoints += experience;
            onExperienceGained();
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public float GetExperiencePoints()
        {
            return experiencePoints;
        }
    }
}
