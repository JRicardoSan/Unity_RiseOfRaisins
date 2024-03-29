/* Main Menu Manager Class
 *
 * Created by J Ricardo San
 * GitHub: @JRicardoSan
 * E-mail address: jricardosan.tech@gmail.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMgr : MonoBehaviour
{
    
    // For PLAY button
    public void PlayGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
    }

    // For QUIT button
    public void QuitGame()
    {
        Application.Quit();
    }

}
