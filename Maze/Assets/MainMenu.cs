using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text email;
    private void Start()
    {
        email = GameObject.Find("email").GetComponent<Text>();
        email.GetComponent<Text>().text = PlayerPrefs.GetString("userEmail").ToString();
       
    }
    
    public void Logout()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void exitgame()
    {
        Application.Quit();
    }
   
}
