using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        [SerializeField] IAction currentAction;
        // Start is called before the first frame update
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            //if player is already in the same action, do nothing

            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}