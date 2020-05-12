/*
 * PauseMenu script
 * 
 * simple script that implements a application pause menu
 * to navigate through the menus from within the running 
 * application
 * 
 * @Author Lars Pastoor
 * @Date 22/04/2020
 * 
 * @Version 1.0
 * --- init
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class inAppMenu : MonoBehaviour
{
    // init pause menu game object
    public UnityEngine.GameObject pauseMenuUI;
    /*
    * Method to return into the main
    * menu via the application pause menu
    * 
    * @return void
    * 
    */
    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
