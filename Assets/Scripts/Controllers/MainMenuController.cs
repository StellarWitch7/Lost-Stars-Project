using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private GameObject _mainMenu;
    private GameObject _loadMenu;
    private GameObject _coopMenu;
    private GameObject _optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenu = GameObject.Find("MainMenu");
        _loadMenu = GameObject.Find("LoadMenu");
        _coopMenu = GameObject.Find("CoopMenu");
        _optionsMenu = GameObject.Find("OptionsMenu");

        OpenMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenu()
    {
        _mainMenu.SetActive(true);
        _loadMenu.SetActive(false);
        _coopMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void OpenLoadMenu()
    {
        _mainMenu.SetActive(false);
        _loadMenu.SetActive(true);
        _coopMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void OpenCoopMenu()
    {
        _mainMenu.SetActive(false);
        _loadMenu.SetActive(false);
        _coopMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        _mainMenu.SetActive(false);
        _loadMenu.SetActive(false);
        _coopMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }
}
