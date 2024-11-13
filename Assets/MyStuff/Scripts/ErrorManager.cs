using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    public static ErrorManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); //Destroy duplicates
        }
    }

    public string NoSeedsError(string plantName)
    {
        return "\n\nOops! Unfortunately, \nyou do not \nhave any " + plantName + " Stock.\nGo to the shop to buy more!";
    }

    public string HoldingDecorationError()
    {
        return "\n\nYou are holding a \ndecoration. Place it in your garden \nbefore proceeding";
    }

    public string CropWitheredError()
    {
        return "\n\nUnfortunately, this crop has withered.\nEquip a trowel and remove it!";
    }

    public string NoResourcesError(string type)
    {
        return "\n\nOops! Unfortunately, you do not \nhave any " + type + ".\nPlay games to get more!";
    }

    public string CurrencyError()
    {
        return "\n\nOops! Unfortunately, \nyou cannot afford this \nor your level is not high enough.\nHarvest more crops!";
    }
}
