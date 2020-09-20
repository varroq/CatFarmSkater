using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : CatCommunicator
{
    int currentSceneIndex;
    int totalScenes;
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        totalScenes = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Active scene number: " + (currentSceneIndex + 1) + " of " + totalScenes);
        nextScene = currentSceneIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9)|| changeScene)
        {
            changeScene = false;
            loadNextScene();

        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (currentSceneIndex == 0)
            {
                currentSceneIndex = totalScenes - 1;
            }
            else
            {
                currentSceneIndex -= 1;
            }
            SceneManager.LoadScene(currentSceneIndex);
        }   
        if (Input.GetKeyDown(KeyCode.F11))
        {          
            if (currentSceneIndex == totalScenes -1)
            {
                currentSceneIndex = 0;
            }
            else
            {
                currentSceneIndex += 1;
            }
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
    void loadNextScene()
    {
        if(nextScene > totalScenes - 1)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }
}
