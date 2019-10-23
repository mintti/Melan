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

        xmlDoc = new XmlDocument();

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
                CodeBox.StringSplit(_node.SelectSingleNode("Skill").InnerText),
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

    /*
    모든 행동에 따른 TextAsset을 저장한다.
    */
    public XmlDocument xmlDoc; //첫 데이터 로드 시 지정됨.
    
    #region COMMON
    public void ConnetctedXmlDoc(string text)
    {
        if(text == "first")
        {

        }
        else if(text == "continue")
        {
            
        }
    }
    public void SaveOverlapXml(string info)
    {
        xmlDoc.Save("./Assets/Resources/XML/PlayerData.xml");
        Debug.Log( info + " Save Complete.");
    }
    #endregion
    
    #region PLAYER
    public void UpdateGold()
    {
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        node.SelectSingleNode("Gold").InnerText =  System.Convert.ToString(player.Gold);

        SaveOverlapXml("골드업데이트");
    }

    public void UpdateDay()
    {
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        node.SelectSingleNode("Day").InnerText =  System.Convert.ToString(player.Day);

        SaveOverlapXml("Day업데이트");
    }

    public void UpdateStress()
    {
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        node.SelectSingleNode("Stress").InnerText =  System.Convert.ToString(player.Stress);

        SaveOverlapXml("Player Stress 업데이트");
    }

    #endregion

    #region WORLD
    public void SaveDungeon(int[] array)
    {
        XmlNode main = xmlDoc.SelectSingleNode("PlayerData");
        
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "DungeonInfo", string.Empty);
        main.AppendChild(root);

        for(int i = 0 ; i < 8 ; i ++)
        {
            XmlNode child = xmlDoc.CreateNode(XmlNodeType.Element, "Dungeon", string.Empty);
            root.AppendChild(child);

            XmlElement num = xmlDoc.CreateElement("Num");
            num.InnerText = System.Convert.ToString(array[i]);
            child.AppendChild(num);

            XmlElement isClear = xmlDoc.CreateElement("IsClear");
            isClear.InnerText = "false";
            child.AppendChild(isClear);

            XmlElement saturation = xmlDoc.CreateElement("Saturation");
            saturation.InnerText = "0";
            child.AppendChild(saturation);

            XmlElement search = xmlDoc.CreateElement("SearchP");
            search.InnerText = "0";
            child.AppendChild(search);
        }

        SaveOverlapXml("던전생성");
    }

    public void UpdateDungeon(int num)
    {
        DungeonProgress d = DungeonData.Instance.dungeon_Progress[num];
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/DungeonInfo/Dungeon");
        XmlNode dungeon = nodes[num];

        dungeon.SelectSingleNode("IsClear").InnerText = System.Convert.ToString(d.isClear);
        dungeon.SelectSingleNode("Saturation").InnerText = System.Convert.ToString(d.Saturation);
        dungeon.SelectSingleNode("SearchP").InnerText = System.Convert.ToString(d.SearchP);

        SaveOverlapXml("던전업데이트");
    }
    #endregion

    #region ADMIN
    //UnitData - Employment(). 기사 고용에 해당. 
    public void AddKnight(Knight k)
    {
        XmlNode root = xmlDoc.SelectSingleNode("PlayerData/KnightInfo");
        
        XmlNode child = xmlDoc.CreateNode(XmlNodeType.Element, "Knight", string.Empty);
        root.AppendChild(child);

        // * 기본적인 인포
        XmlElement name = xmlDoc.CreateElement("Name");
        name.InnerText = k.name;
        child.AppendChild(name);

        XmlElement job = xmlDoc.CreateElement("Job");
        job.InnerText = System.Convert.ToString(k.job);
        child.AppendChild(job);

        XmlElement level = xmlDoc.CreateElement("Level");
        level.InnerText = System.Convert.ToString(k.level);
        child.AppendChild(level);

        XmlElement exper = xmlDoc.CreateElement("Exper");
        exper.InnerText = System.Convert.ToString(k.exper);
        child.AppendChild(exper);

        XmlElement skill = xmlDoc.CreateElement("Skill");
        skill.InnerText = IntArrayToString(k.skill);
        child.AppendChild(skill);

        XmlElement favor = xmlDoc.CreateElement("Favor");
        favor.InnerText = System.Convert.ToString(k.favor);
        child.AppendChild(favor);

        XmlElement day = xmlDoc.CreateElement("Day");
        day.InnerText = System.Convert.ToString(k.day);
        child.AppendChild(day);

        XmlElement skin = xmlDoc.CreateElement("Skin");
        skin.InnerText = IntArrayToString(k.skinArr);;
        child.AppendChild(skin);

        // * 전투관련 정보
        XmlElement isAlive = xmlDoc.CreateElement("IsAlive");
        isAlive.InnerText = System.Convert.ToString(k.isAlive);
        child.AppendChild(isAlive);

        XmlElement hp = xmlDoc.CreateElement("Hp");
        hp.InnerText = System.Convert.ToString(k.hp);
        child.AppendChild(hp);

        XmlElement stress = xmlDoc.CreateElement("Stress");
        stress.InnerText = System.Convert.ToString(k.stress);
        child.AppendChild(stress);

        XmlElement uni = xmlDoc.CreateElement("Uni");
        uni.InnerText = IntArrayToString(k.uni);;
        child.AppendChild(uni);

        SaveOverlapXml("새로운 기사!");
    }

    private string IntArrayToString(int[] array)
    {
        if(array == null)
            return "";

        string text = "";
        for(int i = 0 ; i < array.Length; i++)
            text+= array[i] + ",";
        text = text.Substring(0, text.Length -1);
        
        return text;
    }


    public void UpdateKnight(Knight k, string type)
    {
        switch(type)
        {
            case "" :
                break;
            default :
                break;
        }
        SaveOverlapXml("기사업데이트(" + type + ")");
    }

    public void AddParty()
    {

    }
    public void UpdateParty()
    {

    }

    public void AddRandomKnight(RandomKnight[] k)
    {
        XmlNode root = xmlDoc.SelectSingleNode("PlayerData/Info/RandomKnightInfo");
        
        for(int i = 0 ; i < k.Length; i ++)
        {   
            XmlNode child = xmlDoc.CreateNode(XmlNodeType.Element, "Knight", string.Empty);
            root.AppendChild(child);

            // * 기본적인 인포
            XmlElement name = xmlDoc.CreateElement("Name");
            name.InnerText = k[i].name;
            child.AppendChild(name);

            XmlElement job = xmlDoc.CreateElement("Job");
            job.InnerText = System.Convert.ToString(k[i].job);
            child.AppendChild(job);

            XmlElement level = xmlDoc.CreateElement("Level");
            level.InnerText = System.Convert.ToString(k[i].level);
            child.AppendChild(level);
            
            XmlElement employ = xmlDoc.CreateElement("Employ");
            employ.InnerText = "false";
            child.AppendChild(employ);

            XmlElement skin = xmlDoc.CreateElement("Skin");
            skin.InnerText = IntArrayToString(k[i].skinNum);;
            child.AppendChild(skin);
        }

        SaveOverlapXml("고용기사생성");
    }

    //고용정보 업데이트(대부분)
    public void UpdateRandomKnight(int index)
    {
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/Info/RandomKnightInfo/Knight");
        XmlNode node = nodes[index];

        node.SelectSingleNode("Employ").InnerText = "true";
        SaveOverlapXml("고용정보 업데이트");
    }


    #endregion

    #region SYSTEM

    #endregion
}