using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMGR : MonoBehaviour {

    [SerializeField]
    protected GameObject mainMenuObject;
    [SerializeField]
    protected GameObject statMenuObject;
    [SerializeField]
    protected GameObject shopMenuObject;
    [SerializeField]
    protected GameObject hatObject;
    [SerializeField]
    protected GameObject crownObject;
    [SerializeField]
    protected GameObject wingObject;
    [SerializeField]
    protected GameObject glassesObject;
    	
    public void UseButton(string buttonName)
    {
        switch (buttonName)
        {
            case "Shop":
                Shop();
                break;
            case "Stats":
                Stats();
                break;
            case "Main Menu":
                MainMenu();
                break;
            case "Wings":
                Wings();
                break;
            case "Crown":
                Crown();
                break;
            case "Hat":
                Hat();
                break;
            case "Glasses":
                Glasses();
                break;
        }
    }

    #region Button methods
    void Stats()
    {
        print("Stats");
        mainMenuObject.SetActive(false);
        shopMenuObject.SetActive(false);
        statMenuObject.SetActive(true);
    }

    void MainMenu()
    {
        print("MainMenu");
        shopMenuObject.SetActive(false);
        statMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);

    }

    void Shop()
    {
        print("Shop");
        mainMenuObject.SetActive(false);
        statMenuObject.SetActive(false);
        shopMenuObject.SetActive(true);

    }

    void Wings()
    {
        if (wingObject.activeInHierarchy)
            wingObject.SetActive(false);
        else
            wingObject.SetActive(true);
    }

    void Crown()
    {
        if (crownObject.activeInHierarchy)
            crownObject.SetActive(false);
        else
            crownObject.SetActive(true);
    }

    void Hat()
    {
        if (hatObject.activeInHierarchy)
            hatObject.SetActive(false);
        else
            hatObject.SetActive(true);
    }

    void Glasses()
    {
        if (glassesObject.activeInHierarchy)
            glassesObject.SetActive(false);
        else
            glassesObject.SetActive(true);
    }
    #endregion
}
