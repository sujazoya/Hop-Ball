using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManagerRoad : MonoBehaviour
{
    public static MenuManagerRoad Instance;
    public bool GameState;
    public GameObject menuElement;
    //Transform currentStages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        menuElement.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        //currentStages= PlayerManager.Instance.currentStages;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartTheGame()
    {
        GameState = true;
        menuElement.SetActive(false);
        //PlayerManager.Instance.Jump(currentStages);
    }
    public void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene)
        Application.LoadLevel(Application.loadedLevel);
    }
}
