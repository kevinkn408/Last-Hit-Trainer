using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.VFX
{
    public class Freezer : MonoBehaviour
    {
        [SerializeField] float freezeAmount = 1f;
        Coroutine freeze = null;
    // Start is called before the first frame update
        public void StartFreeze()
        {
            freeze = StartCoroutine(Freeze());
        }

        private IEnumerator Freeze()
        {
            if(freeze == null)
            {
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(freezeAmount);
                Time.timeScale = 1f;
                freeze = null;
            }

        }
    }

}