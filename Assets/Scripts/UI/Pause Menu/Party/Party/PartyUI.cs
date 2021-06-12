using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SelectedCharacter
{
    Claire, 
    Mutiny,
    Blaine,
    Shad,
    Lucy
}

public class PartyUI : MonoBehaviour
{
    public GameObject firstButton;
    public SelectedCharacter selectedCharacter;

    private void Start()
    {
        selectedCharacter = SelectedCharacter.Claire;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ChangeSelectedCharacter(int enumIndex)
    {
        selectedCharacter = (SelectedCharacter)enumIndex;
    }
}
