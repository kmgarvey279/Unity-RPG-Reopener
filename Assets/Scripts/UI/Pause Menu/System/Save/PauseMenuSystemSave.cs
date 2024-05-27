using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuSystemSave : MonoBehaviour
{
    private int selectedSlotIndex = 0;
    [SerializeField] private ScrollableList saveSlotList;
    [SerializeField] private GameObject overwritePrompt;

    //private bool loadSelected = false;
    //[SerializeField] private GameObject saveTabBorder;
    //[SerializeField] private GameObject loadTabBorder;

    private void OnEnable()
    {
        DisplayFiles();
    }

    private void DisplayFiles()
    {
        ClearAllData();

        SaveData saveData = SaveManager.Instance.SaveData;
        saveSlotList.CreateList(saveData.Files.Count);

        int slotIndex = 0;
        foreach (ScrollableListSlot scrollableListSlot in saveSlotList.SlotList)
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

    public void OnClickSaveFile(GameObject slotObject)
    {
        Debug.Log("Save Slot was clicked!");

        ScrollableListSlotSave saveSlot = slotObject.GetComponent<ScrollableListSlotSave>();
        if (saveSlot != null && saveSlot.FileNum != 0) 
        {
            if (SaveManager.Instance.SaveData.Files[saveSlot.FileNum].PlayerData != null) 
            {
                selectedSlotIndex = saveSlot.FileNum;
                ToggleOverwritePrompt(true);
            }
            else
            {
                SaveToFile(saveSlot.FileNum);
            }
        }
    }

    private void SaveToFile(int fileNum)
    {
        if (fileNum == 0 || fileNum > 12)
        {
            return;
        }

        if (SaveManager.Instance.SaveLoadedData(fileNum))
        {
            Debug.Log("Data was successfully saved");
            ScrollableListSlotSave slotToUpdate = (ScrollableListSlotSave) saveSlotList.SlotList[fileNum];
            if (slotToUpdate)
            {
                slotToUpdate.AssignSaveFileData(SaveManager.Instance.SaveData.Files[fileNum]);
            }
        }
        else
        {
            Debug.LogError("Unable to save data");
        }
    }

    private void ToggleOverwritePrompt(bool shouldDisplay)
    {
        overwritePrompt.SetActive(shouldDisplay);

        ScrollableListSlotSave saveSlot = (ScrollableListSlotSave)saveSlotList.SlotList[selectedSlotIndex];
        if (saveSlot != null)
        {
            if (!shouldDisplay)
            {
                saveSlot.ToggleGlow(false);

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(saveSlot.gameObject);
                saveSlot.OnSelect(null);

                selectedSlotIndex = 0;
            }
            else
            {
                saveSlot.ToggleGlow(true);
            }
        }
    }

    public void OnClickPromptOption(int selectedOption)
    {
        if (selectedOption == 0)
        {
            OnConfirmSave();
        }
        else
        {
            OnCancelSave();
        }
    }

    public void OnConfirmSave()
    {
        ToggleOverwritePrompt(false);

        SaveToFile(selectedSlotIndex);
    }

    public void OnCancelSave()
    {
        ToggleOverwritePrompt(false);
    }

    private void ClearAllData()
    {
        saveSlotList.ClearList();

        selectedSlotIndex = 0;
        overwritePrompt.SetActive(false);
    }
}
