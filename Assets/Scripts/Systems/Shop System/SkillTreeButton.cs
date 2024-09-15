using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeButton : MonoBehaviour
{

    public int itemCost;
    public ModifierManager.SkillTreeType skillTreeType;
    public TextMeshProUGUI costText;
    public void OnClick()
    {
        ShopManager.singleton.BuySkillTreeTier(this);
    }    

    public void DisableButton()
    {
        gameObject.SetActive(false);
    }
    
}
