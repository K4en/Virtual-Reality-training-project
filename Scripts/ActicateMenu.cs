using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class ActicateMenu : MonoBehaviour
{

    public bool isPaused = false;

    public GameObject menuPanel;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (isPaused) {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        menuPanel.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        player.GetComponent<FirstPersonController>().enabled = false;
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        menuPanel.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<FirstPersonController>().enabled = true;
        Time.timeScale = 1.0f;
        isPaused = false;
    }
}
