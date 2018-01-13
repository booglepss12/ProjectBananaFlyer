using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Invoke("LoadNextScene", 2f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
