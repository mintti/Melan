using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;
using System;



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

    
    public string platform;
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
                MonsterData.Instance.InsertData();
                dungeon.SetData();
                SkillData.Instance.SetData();

                LoadDataProcess(1);
                break;
            
            //1. 플랫폼 검사 후.
            //   이전 플레이가 존재하는지 검사 후 xml 로드
            //   (존재하지 않을 경우, 새 데이터를 만듦.)
            case 1 : 
                xmlDoc = new XmlDocument();

                if(Application.platform == RuntimePlatform.Android) LoadDataAndroid();
                else    LoadDataPC();

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

    private void LoadDataPC()
    {
        platform = "pc";

        string file = "PlayerData.xml";
        string filePath = "Assets/StreamingAssets/";
        if(File.Exists(filePath + file))
        {
            TextViewer("파일존재.");
            Debug.Log( "파일잇음.");
            xmlDoc.Load(filePath + file);
            LoadXml(false);
        }
        else
        {
            TextViewer("신규데이타 생성.");
            Debug.Log("신규데이타 생성");
            file = "FirstData.xml";
            xmlDoc.Load(filePath + file);
            LoadXml(true);
        }
    }
    
    private void LoadDataAndroid()
    {
        platform = "android";

        TextViewer("안드로이드로 실행됩니다.");
        
        string filePath = Application.persistentDataPath + "/PlayerData.xml";

        bool isFirst = false;
        if(!File.Exists(filePath))
        {
            TextViewer("FirstData 로드..");

            filePath = "jar:file://" + Application.dataPath + "!/assets/FirstData.xml";
            WWW www = new WWW(filePath);

            while(!www.isDone) {};

            StringReader stringReader = new StringReader(www.text);
            xmlDoc.LoadXml(stringReader.ReadToEnd());
            isFirst = true;
        }
        else
        {
            TextViewer("PlayerData 로딩성공");
            xmlDoc.Load(filePath);
        }

        LoadXml(isFirst);
    }
    
    #region dataSave
    /*
    모든 행동에 따른 TextAsset을 저장한다.
    */
    public XmlDocument xmlDoc; //첫 데이터 로드 시 지정됨.
    public GameObject dataUpdateText;
    public Transform TextPos;
    public void TextViewer(string msg)
    {
        GameObject obj =  CodeBox.AddChildInParent(TextPos, dataUpdateText);
        DataUpdateText dut = obj.GetComponent<DataUpdateText>();

        dut.TextUpdate(msg);
    }
    public void SaveOverlapXml(string info)
    {
        if(platform == "pc")
        {
            xmlDoc.Save("./Assets/StreamingAssets/PlayerData.xml");
        }
        else if(platform == "android")
        {
            string filePath = Application.persistentDataPath + "/PlayerData.xml";
            
            File.WriteAllText( filePath, xmlDoc.OuterXml, Encoding.Default);
        }
        string msg = info + " Save Complete.";

        TextViewer(msg);
        Debug.Log( msg);
    }
    #endregion

    #region  새로운 데이타(던전, 용병 등)를 생성하는 부분

    private void CreateData()
    {
        dungeon.DungeonMake();
        unit.CreateRandomKnight(3);
        LoadDataProcess(2);
    }

    #endregion

    #region 데이타 로드
    public void LoadXml(bool isNew)
    {
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
            
            XmlNode info = _node.SelectSingleNode("Info");
            switch (System.Convert.ToInt32(_node.SelectSingleNode("Type").InnerText))
            {
                case 1 :
                    d.Battle(CodeBox.StringSplit(info.SelectSingleNode("Monsters").InnerText));
                    break;
                default:
                    d.eventType = 0;
                    break;
            }
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
        nodes = xmlDoc.SelectNodes("PlayerData/PartyInfo/Party");
        foreach (XmlNode _node in nodes)
        {
            UnitData.Instance.partys.Add(new Party(
                System.Convert.ToInt32(_node.SelectSingleNode("Dungeon").InnerText),
                CodeBox.StringSplit(_node.SelectSingleNode("Knight").InnerText),
                System.Convert.ToInt32(_node.SelectSingleNode("Day").InnerText)
            ));
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
        nodes = xmlDoc.SelectNodes("PlayerData/EventInfo/Event");
        foreach (XmlNode _node in nodes)
        {
            switch(_node.SelectSingleNode("Type").InnerText)
            {
                case "BATTLE" :

                    break;
                case "다른" :
                    break;
                default : 
                    break;
            }
        }
        //4-3. GameTurn - KnightEvent

        if(isNew) CreateData();
        else LoadDataProcess(2);
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

    
    public void SaveOverlapXml()
    {


        Debug.Log("전체데이터 저장.");
        if(platform == "pc")
        {
            xmlDoc.Save("./Assets/StreamingAssets/PlayerData.xml");
        }
        else if(platform == "android")
        {
            string filePath = Application.persistentDataPath + "/PlayerData.xml";
            
            File.WriteAllText( filePath, xmlDoc.OuterXml, Encoding.Default);
        }
    }
    
    public void SaveXml()
    {
        StartCoroutine("SaveXml_");
    }

    IEnumerator SaveXml_()
    {
        ObjectController.Instance.saveObj.SetActive(true);
        
        if(platform == "pc")
        {
            xmlDoc.Save("./Assets/StreamingAssets/PlayerData.xml");
        }
        else if(platform == "android")
        {
            string filePath = Application.persistentDataPath + "/PlayerData.xml";
            
            File.WriteAllText( filePath, xmlDoc.OuterXml, Encoding.Default);
        }
        
        ObjectController.Instance.saveObj.SetActive(false);
        yield return null;
    }
    
    #region COMMON

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
            saturation.InnerText = "50";
            child.AppendChild(saturation);

            XmlElement search = xmlDoc.CreateElement("SearchP");
            search.InnerText = "0";
            child.AppendChild(search);

            XmlElement type = xmlDoc.CreateElement("Type");
            type.InnerText = "0";
            child.AppendChild(type);

            XmlNode info = xmlDoc.CreateNode(XmlNodeType.Element, "Info", string.Empty);
            child.AppendChild(info);
        }

        SaveOverlapXml("던전생성");
    }

    public void UpdateDungeon(int num)
    {
        DungeonProgress dp = DungeonData.Instance.dungeon_Progress[num];
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/DungeonInfo/Dungeon");
        XmlNode dungeon = nodes[num];

        dungeon.SelectSingleNode("IsClear").InnerText = System.Convert.ToString(dp.isClear);
        dungeon.SelectSingleNode("Saturation").InnerText = System.Convert.ToString(dp.Saturation);
        dungeon.SelectSingleNode("SearchP").InnerText = System.Convert.ToString(dp.SearchP);
        dungeon.SelectSingleNode("Type").InnerText = System.Convert.ToString(dp.eventType);

        dungeon.RemoveChild(dungeon.SelectSingleNode("Info"));
        XmlNode info = xmlDoc.CreateNode(XmlNodeType.Element, "Info", string.Empty);
        dungeon.AppendChild(info);
        
        switch (dp.eventType)
        {
            case 1: //전투 이벤트
                XmlElement monsters = xmlDoc.CreateElement("Monsters");
                monsters.InnerText = IntArrayToString(dp.m);
                info.AppendChild(monsters);
                break;
            default:
                break;
        }

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

    public void AddParty(Party p)
    {
        XmlNode root = xmlDoc.SelectSingleNode("PlayerData/PartyInfo");

        XmlNode child = xmlDoc.CreateNode(XmlNodeType.Element, "Party", string.Empty);
        root.AppendChild(child);

        XmlElement dungeon = xmlDoc.CreateElement("Dungeon");
        dungeon.InnerText = System.Convert.ToString(p.dungeonNum);
        child.AppendChild(dungeon);

        XmlElement knight = xmlDoc.CreateElement("Knight");
        knight.InnerText = IntArrayToString(p.k);
        child.AppendChild(knight);
        
        XmlElement day = xmlDoc.CreateElement("Day");
        day.InnerText = System.Convert.ToString(p.day);
        child.AppendChild(day);

        SaveOverlapXml("파티추가");
    }
    public void UpdateParty()
    {

    }

    public void AddRandomKnight(RandomKnight[] k)
    {
        XmlNode root = xmlDoc.SelectSingleNode("PlayerData/Info/RandomKnightInfo");
        
        XmlNodeList list =  root.SelectNodes("Knight");
        if(list != null)
        {
            foreach(XmlNode node in list)
            {
                root.RemoveChild(node);
            }
        }

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
        string filePath = "empty";
        if(platform == "pc")
            filePath = "Assets/StreamingAssets/PlayerData.xml";
        else if(platform == "android")
            filePath = Application.persistentDataPath + "/PlayerData.xml";

        System.IO.File.Delete(filePath);
        GameController.Instance.LoadScene(0);
        Destroy(this.gameObject);
    }

}