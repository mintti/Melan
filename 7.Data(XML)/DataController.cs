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

    
    private void Awake()
    {
        //첫 씬이 아닌경우 본인 삭제.
        if(_instance != null)
        {
            Debug.Log("DataController - Awake() : _instance not null. this scenes script Destroy");
            Destroy(this.gameObject);
            return;
        }
        
        //첫 씬인 경우 데이터 삽입.(게임 시작)
        
        Test_InsertData();

        //데이터가 존재하는지 검사
        string _Filestr = "Resources/XML/SecondData.xml";
        System.IO.FileInfo fi = new System.IO.FileInfo(_Filestr);

        if(fi.Exists)
        {
            LoadXml("SecondData");
        }
        else
        {
            LoadXml("FirstData");
            CreateData();
        }

        _instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("DataController - Awake() : _instance is null. insert script");
    }


    public UnitData unit;
    public SkillData skill;
    public DungeonData dungeon;
    public PlayerData player;

    public SkinData skin;

    public TextData text;

    

    public void Test_InsertData()
    {
        //데이타 삽입
        skin.LoadResource();
        unit.Test_InsertData();
        MonsterData.Instance.InsertData();
        dungeon.SetData();
        text.setTextData();
    }

    #region  새로운 데이타(던전, 용병 등)를 생성하는 부분

    private void CreateData()
    {
        dungeon.DungeonMake();
        unit.CreateRandomKnight(3);
    }

    #endregion

    #region 새로운 XML 데이타를 생성하는 부분

    public void CreateKnightXml(Knight k)
    {
        
    }
    #endregion

    #region 기존 데이타 로드
    public void LoadXml(string temp)
    {
        TextAsset textAsset = Resources.Load("XML/"+ temp) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        
        //1. PlayerData
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        //player.Stress = System.Convert.ToInt32(node.SelectSingleNode("Stress").InnerText);
        player.Day = System.Convert.ToInt32(node.SelectSingleNode("Day").InnerText);
        player.Gold = System.Convert.ToInt32(node.SelectSingleNode("Gold").InnerText);
        
        //2. DungeonProgress;
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/DungeonProgress/Dungeon");
        int index = 0;
        foreach (XmlNode _node in nodes)
        {
            dungeon.dungeon_Progress[index].SetData(
                dungeon.dungeons[System.Convert.ToInt32(node.SelectSingleNode("Num").InnerText)],
                System.Convert.ToBoolean(node.SelectSingleNode("isClear").InnerText),
                System.Convert.ToDouble(node.SelectSingleNode("Saturation").InnerText),
                System.Convert.ToDouble(node.SelectSingleNode("SearchPoint").InnerText)
            );
        }

        //3-1. Unit - Knight
        nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Knight");
        foreach (XmlNode _node in nodes)
        {
            int num = System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText);
            Knight k = new Knight(
                num,
                _node.SelectSingleNode("Name").InnerText, 
                System.Convert.ToInt32(_node.SelectSingleNode("Job").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Level").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Exper").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("UseSkill").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Point").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Favor").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Hp").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Stress").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Skin").InnerText)
            );
            unit.knights.Add(k);
            unit.SetSkin(num); //스킨적용 함수.

        }

        //3-2. Unit - Party
        /*
            Party - LoadParty(int _d, KnightState[] _knights, int _day)
            Xml  :: ks (K)
        */
        
        nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Party");
        foreach (XmlNode _node in nodes)
        {
            int dNum = System.Convert.ToInt32(_node.SelectSingleNode("DungeonNum").InnerText);
            //각 기사의 정보 로드.
            XmlNodeList nodes_ks = xmlDoc.SelectNodes("PlayerData/KnightInfo/Party/KnightState");
            int size = nodes_ks.Count;
            KnightState[] ks = new KnightState[size];
            int[] karr = new int[size];
            index = 0;
            foreach (XmlNode __node in nodes_ks)
            {
                karr[index] = System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText);
                Knight k = unit.knights[karr[index]];
                ks[index++] = new KnightState(k);    
            }
            int day = System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText);
            
            Party p = new Party();
            p.LoadParty(dNum, karr, ks, day);
        }

        //4-1. GameTurn - (고용등록된)Knight

        //4-2. GameTurn - PartyEvent

        //4-3. GameTurn - KnightEvent
    }

    #endregion
   


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

        #region 노드 생성 참고
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        //루트노드생성    
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "PlayerInfo" , string.Empty);
        xmlDoc.AppendChild(root);

        //1. PlayerData
        XmlNode player = xmlDoc.CreateNode(XmlNodeType.Element, "Player" , string.Empty);
        root.AppendChild(player);
        XmlElement day = xmlDoc.CreateElement("Day");
        day.InnerText = "1";
        player.AppendChild(day);
        XmlElement gold = xmlDoc.CreateElement("Gold");
        gold.InnerText = "0";
        player.AppendChild(gold);
        XmlElement stress = xmlDoc.CreateElement("Stress");
        stress.InnerText = "0";
        player.AppendChild(stress);

        //2. DungeonProgress
        XmlNode dp = xmlDoc.CreateNode(XmlNodeType.Element, "DungeonProgress", string.Empty);
        root.AppendChild(dp); 

        #endregion;
        xmlDoc.Save("./Assets/Resources/Character.xml");


    }
    */

    bool[] IntAsBool(int[] _arr)
    {
        bool[] arr = new bool[_arr.Length];

        for(int i = 0; i<_arr.Length; i++)
        {
            arr[i] = _arr[i] == 0 ? false : true;
        }
        return arr;
    }
}
