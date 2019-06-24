﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataController : MonoBehaviour
{
    public UnitData unit;
    public SkillData skill;
    public DungeonData dungeon;
    public EventData event_;

    public TextData text;

    void Start()
    {
        Test_InsertData();
    }


    void Test_InsertData()
    {
        unit.Test_InsertData();
        skill.Test_InsertData();
        dungeon.Test_ConnectData();
        event_.Test_InsertData();


        text.setTextData();
    }

    void Test_OutPutData()
    {
        event_.Test_OutputData();
    }

}
