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

    public UnitData unit;
    public SkillData skill;
    public DungeonData dungeon;
    public PlayerData player;

    public SkinData skin;

    public TextData text;
    
    private void Awake()
    {
       LoadDataProcess(0);    
    }

    public void LoadDataProcess(int i)
    {
        switch(i)
        {
            //0. 첫씬일 경우 기본데이타(리소스) 삽입. 
            //      아닐경우 본인 삭제.
            case 0 :
                if(_instance != null)
                {
                    Destroy(this.gameObject);
                    LoadDataProcess(2);
                    return;
                }
                
                skin.LoadResource();
                text.setTextData();
                MonsterData.Instance.InsertData();
                dungeon.SetData();

                LoadDataProcess(1);
                break;
            
            //1. 이전 플레이가 존재하는지 검사 후 xml 로드
            //   (존재하지 않을 경우, 새 데이터를 만듦.)
            case 1 : 
                xmlDoc = new XmlDocument();

                if(System.IO.File.Exists("Assets/Resources/XML/PlayerData.xml"))
                {
                    LoadXml("PlayerData", false);
                    Debug.Log("기존데이타 로드");
                }
                else
                {
                    LoadXml("FirstData", true);
                    CreateData();
                    Debug.Log("신규데이타 생성");
                }

                _instance = this;
                DontDestroyOnLoad(this);
                break;

            //2. World에서 Load된(혹은 있떤) 데이터에 맞게 씬구성(GameController)
            //   1과정 종료 혹은 0과정 후(기존데이터 존재) 로드됨.
            case 2 :
                GameController.Instance.ConnectData();
                break;
            default :
                break;
        }
    }
    #region  새로운 데이타(던전, 용병 등)를 생성하는 부분

    private void CreateData()
    {
        dungeon.DungeonMake();
        unit.CreateRandomKnight(3);
        LoadDataProcess(2);
    }

    #endregion

    #region 기존 데이타 로드
    public void LoadXml(string temp, bool isNew)
    {
        TextAsset textAsset = Resources.Load("XML/"+ temp) as TextAsset;
        xmlDoc.LoadXml(textAsset.text);
        
        //1. PlayerData
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        player.Day = System.Convert.ToInt32(node.SelectSingleNode("Day").InnerText);
        player.Gold = System.Convert.ToInt32(node.SelectSingleNode("Gold").InnerText);
        player.Stress = System.Convert.ToInt32(node.SelectSingleNode("Stress").InnerText);


        //2. DungeonProgress;
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/DungeonInfo/Dungeon");
        int index = 0;
        
        foreach (XmlNode _node in nodes)
        {
            DungeonProgress d = new DungeonProgress(
                dungeon.dungeons[System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText)],
                System.Convert.ToBoolean(_node.SelectSingleNode("IsClear").InnerText),
                System.Convert.ToDouble(_node.SelectSingleNode("Saturation").InnerText),
                System.Convert.ToDouble(_node.SelectSingleNode("SearchP").InnerText)
            );
            dungeon.dungeon_Progress[index++] = d;
        }

        //3-1. Unit - Knight
        nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Knight");
        foreach (XmlNode _node in nodes)
        {
            Knight k = new Knight(
                 System.Convert.ToInt32(_node.SelectSingleNode("Num").InnerText),
                _node.SelectSingleNode("Name").InnerText, 
                System.Convert.ToInt32(_node.SelectSingleNode("Job").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Level").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Exper").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Skill").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Favor").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Skin").InnerText),
                System.Convert.ToBoolean(_node.SelectSingleNode("IsAlive").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Hp").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Stress").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Uni").InnerText)
            );
            unit.knights.Add(k);
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
        nodes = xmlDoc.SelectNodes("PlayerData/Info/RandomKnightInfo/Knight");
        index = 0;
        foreach (XmlNode _node in nodes)
        {
            unit.randomKnightList[index++] = new RandomKnight
            (
                _node.SelectSingleNode("Name").InnerText,
                System.Convert.ToInt32(_node.SelectSingleNode("Job").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Level").InnerText),
                System.Convert.ToBoolean(_node.SelectSingleNode("Employ").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Skin").InnerText)
            );
        
        }

        //4-2. GameTurn - PartyEvent

        //4-3. GameTurn - KnightEvent

        if(!isNew) LoadDataProcess(2);
    }

    #endregion
   

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
        XmlElement num = xmlDoc.CreateElement("Num");
        num.InnerText = System.Convert.ToString(k.num);
        child.AppendChild(num);

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

    public void ResetData()
    {
        System.IO.File.Delete("Assets/Resources/XML/PlayerData.xml");
        GameController.Instance.LoadScene(0);
        Destroy(this.gameObject);
    }

}