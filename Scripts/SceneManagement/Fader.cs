using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        Coroutine currentActiveFade = null;
        CanvasGroup canvasGroup;
        // Start is called before the first frame update
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float fadeTime)
        {
            return Fade(1, fadeTime);
        }

        public Coroutine FadeIn(float fadeTime)
        {
            return Fade(0, fadeTime);
        }

        public Coroutine Fade(float alphaTarget, float fadeTime)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(alphaTarget, fadeTime));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float alphaTarget, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / time);
                yield return null;
            }
            print("something");
        }



    }

}