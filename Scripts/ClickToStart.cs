using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(currentScene + 1);
        }
    }
}
