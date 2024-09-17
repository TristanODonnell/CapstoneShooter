using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Button weaponsButton;
    public Button passiveButton;
    public Button abilityButton;
    public Button skillTreeButton;
    public Button grenadesandAmmoButton;

    public GameObject weaponSelectPanel;
    public GameObject passiveSelectPanel;
    public GameObject abilitySelectPanel;
    public GameObject skillTreeSelectPanel;
    public GameObject grenadesAndAmmoPanel;

    public void TogglePage(GameObject page)
    {
        // Set all pages to inactive
        weaponSelectPanel.SetActive(false);
        passiveSelectPanel.SetActive(false);
        abilitySelectPanel.SetActive(false);
        skillTreeSelectPanel.SetActive(false);
        grenadesAndAmmoPanel.SetActive(false);

        // Set the clicked page to active
        page.SetActive(true);
    }


    public void OnWeaponsButtonClick()
{
    TogglePage(weaponSelectPanel);
}

public void OnPassiveButtonClick()
{
    TogglePage(passiveSelectPanel);
}

public void OnAbilityButtonClick()
{
    TogglePage(abilitySelectPanel);
}

public void OnSkillTreeButtonClick()
{
    TogglePage(skillTreeSelectPanel);
}

public void OnGrenadesandAmmoButtonClick()
{
    TogglePage(grenadesAndAmmoPanel);
}
}
