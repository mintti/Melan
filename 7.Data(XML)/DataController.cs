using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class DataController : MonoBehaviour
{
    public UnitData unit;
    public SkillData skill;
    public DungeonData dungeon;
    public EventData event_;
    public MonsterData monster;
    public PlayerData player;

    public TextData text;


    public void Test_InsertData()
    {
        unit.Test_InsertData();
        skill.Test_InsertData();
        dungeon.Test_ConnectData();
       // event_.Test_InsertData();
        monster.Test_InsertData();
        
        text.setTextData();
    }

    void Test_OutPutData()
    {
        //event_.Test_OutputData();
    }

    //xml Text

    public void LoadXml()
    {
        TextAsset textAsset = Resources.Load("XML/SecondData") as TextAsset;
        Debug.Log(textAsset);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        
        
        //1. PlayerData
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
 
        int g = System.Convert.ToInt32(node.SelectSingleNode("Gold").InnerText);
        int d = System.Convert.ToInt32(node.SelectSingleNode("Day").InnerText);

        player.SetData(d, g);


        //2. KnightData
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Knight");
        foreach (XmlNode _node in nodes)
        {
            int num = System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText);
            Knight k = new Knight(
                num,
                _node.SelectSingleNode("Name").InnerText, 
                System.Convert.ToInt32(_node.SelectSingleNode("Job").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Level").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Exper").InnerText),
                CodeBox.StringAsIntArr(_node.SelectSingleNode("Skill").InnerText),
                IntAsBool(CodeBox.StringAsIntArr(_node.SelectSingleNode("UseSkill").InnerText)),
                System.Convert.ToInt32(_node.SelectSingleNode("Point").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Favor").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Stress").InnerText),
                CodeBox.StringAsIntArr(_node.SelectSingleNode("Skin").InnerText)
            );
            unit.knights.Add(k);
            unit.SetSkin(num); //스킨적용 함수.

        }

    }

    bool[] IntAsBool(int[] _arr)
    {
        bool[] arr = new bool[_arr.Length];

        for(int i = 0; i<_arr.Length; i++)
        {
            arr[i] = _arr[i] == 0 ? false : true;
        }
        return arr;
    }
    
    //덮어쓰기
    /* 변경된 데이터만 추가 할 수 있게 할까 ? 
    public void SaveOverlapXml()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Character");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList nodes = xmlDoc.SelectNodes("CharacterInfo/Character");
        XmlNode character = nodes[0];

        character.SelectSingleNode("Name").InnerText = "wergia";
        character.SelectSingleNode("Level").InnerText = "5";
        character.SelectSingleNode("Experience").InnerText = "180";

        xmlDoc.Save("./Assets/Resources/Character.xml");

    }
    */
}
