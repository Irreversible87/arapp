/*
 * PauseMenu script
 * 
 * simple script that implements a application pause menu
 * to navigate through the menus from within the running 
 * application
 * 
 * Coder: Lars Pastoor
 * Date: 22/04/2020
 * Version: 0.1
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // init pause menu game object
    public GameObject pauseMenuUI;

    // Updates every frame
    private void Update()
    {
        // checks if there was a touch input and if it began
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Pause();
        }
        // checks if a second touch input is detected
        else if (Input.GetTouch(1).phase == TouchPhase.Began)
        {
            Resume();
        }
        
    }

    /*
     * Method to activate the application
     * pause menu.
     * 
     * @return void
     * 
     */
    void Pause()
    {
        pauseMenuUI.SetActive(true);
    }
    /*
    * Method to deactivate the application
    * pause menu.
    * 
    * @return void
    * 
    */
    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
    }
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
    /*
    * Method to navigate to the library
    * menu from within the application
    * menu
    * 
    * @return void
    * 
    */
    public void OpenLib()
    {
        Debug.Log("Open Library...");
    }
}
