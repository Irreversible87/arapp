using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    public menu currentMenu;

    public void Start()
    {
        ShowMenu(currentMenu);
    }

    public void ShowMenu(menu menu) 
    {
        if (currentMenu != null)
            currentMenu.isOpen = false;

        currentMenu = menu;
        currentMenu.isOpen = true;
    }
}
