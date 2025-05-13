using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BettingUIManager : MonoBehaviour
{
    public TMP_Dropdown betTypeDropdown;
    public GameObject ColorOption;
    public GameObject NumberOption;

    void Start()
    {
        betTypeDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        OnDropdownValueChanged(betTypeDropdown.value); // Initialize correct panel
    }

    void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0: // Bet by Color
                ColorOption.SetActive(true);
                NumberOption.SetActive(false);
                break;
            case 1: // Bet by Number
                ColorOption.SetActive(false);
                NumberOption.SetActive(true);
                break;
        }
    }
}

