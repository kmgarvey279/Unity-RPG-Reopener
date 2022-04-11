using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string previousName = "";

    private void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Debug.Log("Changed from " + current.name + "to " + next.name);
        previousName = current.name;
    }

    public void ChangeScene(string newSceneName)
    {
        Debug.Log("Changing to " + newSceneName);
        SceneManager.LoadScene(newSceneName);

        Scene scene = SceneManager.GetSceneByName(newSceneName);
        SceneManager.SetActiveScene(scene);
    }
}
