using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Debug
{
    public class Debug_SceneReset : MonoBehaviour
    {
        public void ResetScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}