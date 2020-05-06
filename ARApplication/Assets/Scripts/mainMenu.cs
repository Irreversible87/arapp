/*
 * Main menu script
 * 
 * Simple script that invokes the main menu scene
 * at the start of the application.
 * 
 * @Author Lars Pastoor
 * @Date 20/04/2020
 * 
 * @Version 1.0
 * --- init
 * 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void StartApp ()
    {
        // Scenemanger loads the ar main scene with buildIndex 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
