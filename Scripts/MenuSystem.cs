using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public int menuID = 0;

    private GameObject[] panels;
    private GameObject mainMenu;
    private GameObject trainingType;
    private GameObject optionsMenu;
    private GameObject helpMenu;

    int currentSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        panels = GameObject.FindGameObjectsWithTag("MenuPanel");

        mainMenu = GameObject.Find("Main Menu");
        trainingType = GameObject.Find("Training type menu");
        optionsMenu = GameObject.Find("Options menu");
        helpMenu = GameObject.Find("Help menu");

        switchToMenu(menuID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchToMenu(int menuID)
    {
        foreach(GameObject panel in panels)
        {

            panel.gameObject.SetActive(false);
            Debug.Log(panel.name);
        }

        switch (menuID)
        {
            case 0:
                mainMenu.SetActive(true);
                break;
            case 1:
                trainingType.SetActive(true);
                break;
            case 2:
                optionsMenu.SetActive(true);
                break;
            case 3:
                helpMenu.SetActive(true);
                break;

        }
    }

    public void ExitProgram()
    {
        Application.Quit();
    }

    public void LoadTrainingField()
    {
        
        SceneManager.LoadScene("Training Field");
        //FindObjectOfType<GameState>().timer = 0;
    }

    public void LoadDome()
    {
        SceneManager.LoadScene("Dome");
    }
}
