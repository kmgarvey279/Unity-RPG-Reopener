using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip select;
    [SerializeField] protected SignalSender onFadeIn;
    [SerializeField] protected SignalSender onFadeOut;

    public void OnEnable()
    {
        StartCoroutine(EnterTitleCo());
    }

    private IEnumerator EnterTitleCo()
    {
        yield return new WaitForEndOfFrame();

        MusicManager.Instance.PlayClip(bgm);
        InputManager.Instance.ChangeActionMap(ActionMapType.UI);

        onFadeIn.Raise();
    }

    public IEnumerator ExitTitleCo(string sceneName)
    {
        SoundFXManager.Instance.PlayClip(select, 0.5f);

        yield return new WaitForEndOfFrame();

        onFadeOut.Raise();
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SceneSwapManager.Instance.ChangeSceneCo(sceneName));
    }
}
