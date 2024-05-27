using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetupManager : MonoBehaviour
{
    [SerializeField] protected GameObject content;
    [SerializeField] protected OverworldData overworldData;
    [SerializeField] protected string startingActionMap;
    //[SerializeField] protected ScreenTransition screenTransition;
    [SerializeField] protected SignalSender onFadeIn;
    [SerializeField] protected SignalSender onFadeOut;

    //protected void OnEnable()
    //{
    //    SceneSwapManager.onLoadComplete.AddListener(OnLoadComplete);
    //}

    //protected void OnDisable()
    //{
    //    SceneSwapManager.onLoadComplete.RemoveListener(OnLoadComplete);
    //}

    public void OnEnable()
    {
        StartCoroutine(EnterSceneCo());
    }

    protected void OnLoadComplete()
    {
        Debug.Log("Load complete!");
        StartCoroutine(EnterSceneCo());
    }

    public IEnumerator EnterSceneCo()
    {
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(SetupSceneCo());

        onFadeIn.Raise();
        yield return new WaitForSeconds(1f);

        StartScene();
    }

    public IEnumerator OnExitSceneCo(string sceneName, int entryPointIndex = 0)
    {
        SaveManager.Instance.SessionData.SetNextSceneEntryPoint(entryPointIndex);

        Debug.Log("exiting current scene");
        InputManager.Instance.LockInput();

        onFadeOut.Raise();
        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(BreakdownSceneCo());

        StartCoroutine(SceneSwapManager.Instance.ChangeSceneCo(sceneName));
    }

    public virtual IEnumerator SetupSceneCo()
    {
        yield return null;
    }

    public virtual void StartScene()
    {
        InputManager.Instance.ChangeActionMap(startingActionMap);
        InputManager.Instance.UnlockInput();
    }

    public virtual IEnumerator BreakdownSceneCo()
    {
        yield return null;
    }
}
