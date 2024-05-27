using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleMenuStateLoad : TitleMenuState
{
    [SerializeField] private GameObject display;
    [Header("List")]
    [SerializeField] private ScrollableList saveList;

    private void OnEnable()
    {
        InputManager.Instance.OnPressCancel.AddListener(OnCancel);
        Hide();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressCancel.RemoveListener(OnCancel);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Display();
    }

    public override void StateUpdate()
    {
    }


    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);

        SaveData saveData = SaveManager.Instance.SaveData;
        if (saveData == null)
        {
            Debug.LogError("Save Data not found");
            return;
        }

        saveList.CreateList(saveData.Files.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in saveList.SlotList)
        {
            if (scrollableListSlot is ScrollableListSlotSave)
            {
                ScrollableListSlotSave saveSlot = (ScrollableListSlotSave)scrollableListSlot;
                SaveFile saveFile = saveData.Files[slotIndex];

                Debug.Log("Assigning save slot #" + saveFile.FileNum);

                if (saveFile == null)
                {
                    continue;
                }
                saveSlot.AssignSaveFileData(saveFile);

                if (slotIndex == 0)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(saveSlot.gameObject);
                    saveSlot.OnSelect(null);
                }
            }
            slotIndex++;
        }
    }

    private void Hide()
    {
        display.SetActive(false);
    }

    public void OnCancel(bool isPressed)
    {
        lastButton = defaultButton;
        stateMachine.ChangeState((int)TitleMenuStateType.Main);
    }

    public void OnClickSaveFile(GameObject slotObject)
    {
        ScrollableListSlotSave saveSlot = slotObject.GetComponent<ScrollableListSlotSave>();

        if (SaveManager.Instance.LoadSelectedFile(SaveManager.Instance.SaveData.Files[saveSlot.FileNum]))
        {
            string sceneToLoad = SaveManager.Instance.LoadedData.PlayerData.CurrentSceneName;

            if (SceneUtility.GetBuildIndexByScenePath(sceneToLoad) != -1)
            {
                SceneSetupManager sceneSetupManager = FindObjectOfType<SceneSetupManager>();
                if (sceneSetupManager)
                {
                    StartCoroutine(sceneSetupManager.OnExitSceneCo(sceneToLoad));
                }
                else
                {
                    Debug.LogError("Setup manager not found!");
                }
            }
            else
            {
                Debug.LogError("No scene exists under the name " + sceneToLoad);
            }
        }
        else
        {
            Debug.LogError("Unable to load file");
        }
    }
}
