using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene("Playground");
    }

    public void LoadGame(Scene scene, int saveSlot)
    {
        SaveSystem.LoadGame(saveSlot);
        SceneManager.LoadScene(scene.buildIndex.ToString());
    }

    public void MainMenu()
    {
        SaveSystem.SaveGame(GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>(), 
            GameObject.Find("GameManager").GetComponent<GameManager>().SaveSlot);
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
