using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeButton : MonoBehaviour
{

    public float itemCost;
    public ModifierManager.SkillTreeType skillTreeType;

    public void OnClick()
    {
        ShopManager.singleton.BuySkillTreeTier(this);
    }    

    public void DisableButton()
    {

    }
}
