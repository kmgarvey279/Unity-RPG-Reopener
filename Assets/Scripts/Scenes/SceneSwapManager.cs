using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;

    [Header("Load Bar")]
    [SerializeField] private GameObject loadContainer;
    [SerializeField] private SliderBar loadBar;
    private bool isLoading;
    private float loadProgress;

    public static UnityEvent onLoadComplete = new UnityEvent();

    [Header("Battle Scene")]
    [SerializeField] private SceneField battleScene;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //loadContainer.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public IEnumerator ChangeSceneCo(string sceneName)
    {
        Debug.Log("changing current scene");

        //loadContainer.SetActive(true);
        //loadBar.SetCurrentValue(0);

        AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        //asyncLoadOperation.allowSceneActivation = false;
        //isLoading = true;

        //while (asyncLoadOperation.progress < 0.9f)
        //{
            //float progress = Mathf.Clamp01(asyncLoadOperation.progress / 0.9f);
            //loadProgress = progress;

            //yield return new WaitForEndOfFrame();
            yield return null;
        //}

        //loadProgress = 1f;
        //while (isLoading)
        //{
        //    yield return null;
        //}
        
        asyncLoadOperation.allowSceneActivation = true;
    }

    private void OnSceneChanged(Scene scene1, Scene scene2)
    {
        //loadContainer.SetActive(false);
        onLoadComplete.Invoke();
    }
    //private void Update()
    //{
    //    if (isLoading)
    //    {
    //        float currentValue = loadBar.GetCurrentValue();
    //        float valueToDisplay = Mathf.MoveTowards(currentValue, loadProgress, 3f * Time.deltaTime);
    //        loadBar.SetCurrentValue(valueToDisplay);
    //        if (loadBar.GetCurrentValue() == 1f)
    //        {
    //            isLoading = false;
    //        }
    //    }
    //}
}
