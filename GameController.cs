using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public WorldController world;
    public AdminController admin;

    void Start()
    {
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSetting()
    {
        world.Click();
        admin.MakeKinghtList();
    }
}
