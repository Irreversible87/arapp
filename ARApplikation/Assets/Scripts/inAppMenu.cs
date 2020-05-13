/*
 * InAppMenu class
 * 
 * simple class that implements a application pause menu
 * to navigate through the menus from within the running 
 * application.
 * 
 * @Author: Lars Pastoor
 * @Version: 1.0
 * @Date: 22/04/2020
 * 
 * -> init
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class InAppMenu : MonoBehaviour
{
    // init pause menu game object
    public GameObject pauseMenuUI;
    /*
     * LoadMenu() method
     * 
     * method to return into the main
     * menu via the application pause
     * menu.
     */
    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
