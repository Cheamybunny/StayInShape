using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    [SerializeField] PlayerDataSO player;
    [SerializeField] SaveManagerSO saveManager;
    [SerializeField] ShopUIEvents shopUIEvents;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("new shop manager");
            instance = this;
            saveManager.Load();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    private void Update()
    {
        shopUIEvents.SetChilliStockValue(player.GetChilliCrop());
        shopUIEvents.SetEggplantStockValue(player.GetEggplantCrop());
        shopUIEvents.SetLoofaStockValue(player.GetLoofaCrop());
        shopUIEvents.SetSweetPotatoStockValue(player.GetSweetPotatoCrop());
        shopUIEvents.SetPapayaValue(player.GetPapayaCrop());
        shopUIEvents.SetKalamansiStockValue(player.GetKalamansiCrop());
    }

    public void AddChilli()
    {
        if(player.GetWater() >= 3)
        {
            player.SetChilliCrop(1);
            player.SetWater(player.GetWater() - 3);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public void AddEggplant()
    {
        if (player.GetChilliCrop() >= 3)
        {
            player.SetChilliCrop(-3);
            player.SetEggplantCrop(1);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public void AddLoofa()
    {
        if (player.GetChilliCrop() >= 3 && player.GetEggplantCrop() >= 3 && Mathf.Floor(player.GetExp() / 1000) >= 2)
        {
            player.SetChilliCrop(-3);
            player.SetEggplantCrop(-3);
            player.SetLoofaCrop(1);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public void AddSweetPotato()
    {
        if(player.GetChilliCrop() >= 1 && player.GetEggplantCrop() >= 3 && player.GetLoofaCrop() >= 4 && Mathf.Floor(player.GetExp() / 1000) >= 3)
        {
            player.SetChilliCrop(-1);
            player.SetEggplantCrop(-3);
            player.SetLoofaCrop(-4);
            player.SetSweetPotatoCrop(1);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public void AddPapaya()
    {
        if(player.GetLoofaCrop() >= 3 && player.GetSweetPotatoCrop() >= 3 && player.GetKalamansiCrop() >= 5 && Mathf.Floor(player.GetExp() / 1000) >= 5)
        {
            player.SetLoofaCrop(-3);
            player.SetSweetPotatoCrop(-3);
            player.SetKalamansiCrop(-5);
            player.SetPapayaCrop(1);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public void AddKalamansi()
    {
        if(player.GetEggplantCrop() >= 2 && player.GetLoofaCrop() >= 2 && player.GetSweetPotatoCrop() >= 4 && Mathf.Floor(player.GetExp() / 1000) >= 4)
        {
            player.SetEggplantCrop(-2);
            player.SetLoofaCrop(-2);
            player.SetSweetPotatoCrop(-4);
            player.SetKalamansiCrop(1);
            saveManager.Save();
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
        }
    }

    public bool CanBuyFlower1()
    {

        if(player.GetLastHeldItem() != -1)
        {
            shopUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
            return false;
        }
        else if(player.GetChilliCrop() >= 2 && player.GetEggplantCrop() >= 2 && Mathf.Floor(player.GetExp() / 1000) >= 1)
        {
            player.SetChilliCrop(-2);
            player.SetEggplantCrop(-2);
            player.SetLastHeldItem(10);
            saveManager.Save();
            return true;
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
            return false;
        }
    }

    public bool CanBuyFlower2()
    {
        if (player.GetLastHeldItem() != -1)
        {
            shopUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
            return false;
        }
        else if (player.GetChilliCrop() >= 2 && player.GetEggplantCrop() >= 1 && player.GetLoofaCrop() >= 1 && Mathf.Floor(player.GetExp() / 1000) >= 2)
        {
            player.SetChilliCrop(-2);
            player.SetEggplantCrop(-1);
            player.SetLoofaCrop(-1);
            player.SetLastHeldItem(11);
            saveManager.Save();
            return true;
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
            return false;
        }
    }

    public bool CanBuySunflower()
    {
        if (player.GetLastHeldItem() != -1)
        {
            shopUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
            return false;
        }
        else if (player.GetEggplantCrop() >= 3 && player.GetLoofaCrop() >= 3 
            && player.GetSweetPotatoCrop() >= 3 && player.GetLoofaCrop() >= 1 && Mathf.Floor(player.GetExp() / 1000) >= 3)
        {
            player.SetChilliCrop(-2);
            player.SetEggplantCrop(-1);
            player.SetLoofaCrop(-1);
            player.SetLastHeldItem(12);
            saveManager.Save();
            return true;
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
            return false;
        }


    }

    public bool CanBuyChicken()
    {
        if(player.GetLastHeldItem() != -1)
        {
            shopUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
            return false;
        }
        else if(player.GetSweetPotatoCrop() >= 4 && player.GetKalamansiCrop() >= 4 && Mathf.Floor(player.GetExp() / 1000) >= 4)
        {
            player.SetSweetPotatoCrop(-4);
            player.SetKalamansiCrop(-4);
            player.SetLastHeldItem(13);
            saveManager.Save();
            return true;
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
            return false;
        }
    }

    public bool CanBuyRadio()
    {
        if (player.GetLastHeldItem() != -1)
        {
            shopUIEvents.ThrowError(ErrorManager.instance.HoldingDecorationError());
            return false;
        }
        else if (player.GetChilliCrop() >= 3 
            && player.GetEggplantCrop() >= 3
            && player.GetLoofaCrop() >= 3
            && player.GetSweetPotatoCrop() >= 3
            && player.GetKalamansiCrop() >= 3
            && player.GetPapayaCrop() >= 3
            && Mathf.Floor(player.GetExp() / 1000) >= 5)
        {
            player.SetChilliCrop(-3);
            player.SetEggplantCrop(-3);
            player.SetLoofaCrop(-3);
            player.SetSweetPotatoCrop(-3);
            player.SetKalamansiCrop(-3);
            player.SetPapayaCrop(-3);
            player.SetLastHeldItem(14);
            saveManager.Save();
            return true;
        }
        else
        {
            shopUIEvents.ThrowError(ErrorManager.instance.CurrencyError());
            return false;
        }
    }
}
