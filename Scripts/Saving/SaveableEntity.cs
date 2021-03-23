using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using RPG.Core;

namespace RPG.Saving
{
    [System.Serializable]
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueID = "";
        static Dictionary<string, SaveableEntity> globalLookUp = new Dictionary<string, SaveableEntity>();
        // Start is called before the first frame update
        public string GetUniqueModifier()
        {
            return uniqueID;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueID");

            if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            globalLookUp[property.stringValue] = this;
        }
#endif
        private bool IsUnique(string candidate)
        {
            if (!globalLookUp.ContainsKey(candidate))
            {
                return true;
            }
            if(globalLookUp[candidate] == this)
            {
                return true;
            }
            if(globalLookUp[candidate] == null)
            {
                globalLookUp.Remove(candidate);
                return true;
            }
            if(globalLookUp[candidate].GetUniqueModifier() != candidate)
            {
                return true;
            }
            return false;
        }
    }
}