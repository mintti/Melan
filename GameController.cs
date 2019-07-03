using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public DataController data;
    public WorldController world;
    public AdminController admin;

    public static GameObject gameInstance = null;
    public static GameObject unitInstance = null;

    void Start()
    {
        if(gameInstance!=null)
        {
            Destroy(gameInstance);
        }

        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSetting()
    {
        world.Click();
    }

    public void Test_LoadScene()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(data);
        SceneManager.LoadScene("2.Battle");
    }
}
