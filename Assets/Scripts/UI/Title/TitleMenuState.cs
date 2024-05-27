using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using System.Linq;

public enum TitleMenuStateType
{
    Main,
    Load,
    Options
}

public class TitleMenuState : StateMachine.State
{
    [SerializeField] protected TitleMenuStateType titleMenuStateType;
    [SerializeField] protected GameObject defaultButton;
    protected GameObject lastButton;

    protected List<SignalListenerBase> signalListeners = new List<SignalListenerBase>();

    public virtual void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>();
        id = (int)titleMenuStateType;

        //signal listeners
        signalListeners = GetComponentsInChildren<SignalListenerBase>().ToList();
    }

    public override void OnEnter()
    {
        if (signalListeners.Count > 0)
        {
            foreach (MonoBehaviour script in signalListeners)
            {
                if (script != null)
                    script.enabled = true;
            }
        }
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        if (signalListeners.Count > 0)
        {
            foreach (MonoBehaviour script in signalListeners)
            {
                script.enabled = false;
            }
        }
    }



//    public class TitleMenu : MonoBehaviour
//{
//    [SerializeField] private GameObject defaultButton;
//    [SerializeField] private GameObject lastButton;

//    [SerializeField] private SceneField firstScene;

//    [SerializeField] public GameObject mainMenu;
//    [SerializeField] public TitleMenuLoad loadMenu;

//    private void OnEnable()
//    {
//        SelectCurrentButton();
//    }

//    public void SelectCurrentButton()
//    {
//        EventSystem.current.SetSelectedGameObject(null);
//        if (lastButton != null)
//        {
//            EventSystem.current.SetSelectedGameObject(lastButton);
//            lastButton.GetComponent<SelectableButton>().OnSelect(null);
//        }
//        else if (defaultButton != null)
//        {
//            EventSystem.current.SetSelectedGameObject(defaultButton);
//            defaultButton.GetComponent<SelectableButton>().OnSelect(null);
//        }
//    }

//    public void OnClickNewGame()
//    {
//        Debug.Log("CLicked new game.");
//        SceneSetupManager sceneSetupManager = FindObjectOfType<SceneSetupManager>();
//        if (sceneSetupManager)
//        {
//            StartCoroutine(sceneSetupManager.OnExitSceneCo(firstScene));
//        }
//        else 
//        {
//            Debug.Log("Setup manager not found!");
//        }
//    }

//    public void OnClickLoad()
//    {
//        mainMenu.SetActive(false);
//        loadMenu.Display();
//    }
}
