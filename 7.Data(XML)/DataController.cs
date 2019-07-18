using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class DataController : MonoBehaviour
{

    private static DataController _instance = null;

    public static DataController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(DataController)) as DataController;

                if(_instance == null)
                {
                    Debug.LogError("There's no active DataController object");
                }
            }
            return _instance;
        }
    }
    public static void InstanceUpdata()
    {
        _instance = null;
    }
    

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    
    }

    public UnitData unit;
    public SkillData skill;
    public DungeonData dungeon;
    public MonsterData monster;
    public PlayerData player;

    public SkinData skin;

    public TextData text;

    public void Test_InsertData()
    {
        //데이타 삽입
        unit.Test_InsertData();
        skill.Test_InsertData();
        monster.Test_InsertData();
            
        text.setTextData();
    }



    public void LoadResource()
    {
        TextAsset textAsset = Resources.Load("XML/DungeonData") as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        //1. Dungeon
        XmlNodeList nodes = xmlDoc.SelectNodes("/DungeonData/Dungeon");

        dungeon.ResizeDungeon(nodes.Count);
        foreach (XmlNode _node in nodes)
        {
            int num = System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText);

            dungeon.dungeons[num] = new Dungeon(
                num,
                _node.SelectSingleNode("Name").InnerText,
                System.Convert.ToInt32(_node.SelectSingleNode("Level").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Monster").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Dangers").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Evnets").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Gold").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Exper").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Before").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Next").InnerText),
                System.Convert.ToBoolean(_node.SelectSingleNode("IsClear").InnerText)
            );
            //dungeon.dungeons.Add(d);
        }
        
    }
    public void LoadXml(string temp)
    {
        TextAsset textAsset = Resources.Load("XML/"+ temp) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        
        //1. PlayerData
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        player.Day = System.Convert.ToInt32(node.SelectSingleNode("Day").InnerText);
        player.Gold = System.Convert.ToInt32(node.SelectSingleNode("Gold").InnerText);
        


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
                CodeBox.StringSplit(_node.SelectSingleNode("Skill").InnerText),
                IntAsBool(CodeBox.StringSplit(_node.SelectSingleNode("UseSkill").InnerText)),
                System.Convert.ToInt32(_node.SelectSingleNode("Point").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Favor").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Stress").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Skin").InnerText)
            );
            unit.knights.Add(k);
            unit.SetSkin(num); //스킨적용 함수.

        }

        //3. Party
        nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Party");
        foreach (XmlNode _node in nodes)
        {
            int num = System.Convert.ToInt32(_node.SelectSingleNode("DungeonNum").InnerText);
            //각 기사의 정보 로드.
            XmlNodeList nodes_k = xmlDoc.SelectNodes("PlayerData/KnightInfo/Party/Knight");
            int size = nodes_k.Count;
            int [] Narr = new int[size];
            int [] Harr = new int[size];
            int [] Sarr = new int[size];
            int i = 0;
            foreach (XmlNode __node in nodes_k)
            {   //kngihtNum / hp / stress 기록
                Narr[i] = System.Convert.ToInt32(__node.SelectSingleNode("Num").InnerText);
                Harr[i] = System.Convert.ToInt32(__node.SelectSingleNode("Hp").InnerText);
                Sarr[i++] = System.Convert.ToInt32(__node.SelectSingleNode("Stress").InnerText);
            }
            unit.partys.Add(new Party(num, Narr, Harr, Sarr, 
                 System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText)));
            ;
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
