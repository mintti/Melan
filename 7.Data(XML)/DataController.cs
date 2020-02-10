using System.Collections;
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
    public OfficeData office;
    public SkinData skin;

    public TextData text;

    
    public string platform;
    private void Awake()
    {
       LoadResource();
    }

    private void LoadResource()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            ConnectData();
            return;
        }
                
        skin.LoadResource();
        MonsterData.Instance.InsertData();
        dungeon.SetData();
        SkillData.Instance.SetData();
        TextData.Instance.SetData();

        LoadXmlDocument();
    }

    public XmlDocument xmlDoc; //첫 데이터 로드 시 지정됨.
    private void LoadXmlDocument()
    {
        xmlDoc = new XmlDocument();

        //Platform Check
        string filePath;
        if(Application.platform == RuntimePlatform.Android)
        {
            platform = "android";
            filePath = Application.persistentDataPath + "/PlayerData.xml";
        }
        else
        {
            platform = "pc";
            filePath = "Assets/StreamingAssets/PlayerData.xml";
        }

        //PlayerData 존재시 로드, 없을 경우 신규데이타 작성.
        if(!File.Exists(filePath))
        {       //신규데이타 로드
            if(platform =="pc")
            {
                filePath = "Assets/StreamingAssets/FirstData.xml";
                xmlDoc.Load(filePath);
            }
            else
            {   //Android
                filePath = "jar:file://" + Application.dataPath + "!/assets/FirstData.xml";
                WWW www = new WWW(filePath);

                while(!www.isDone) {};

                StringReader stringReader = new StringReader(www.text);
                xmlDoc.LoadXml(stringReader.ReadToEnd());
            }
            FirstData();
        }
        else
        {       //기존데이타 로드
            Debug.Log( "PlayerData 로딩성공.");
            xmlDoc.Load(filePath);

            LoadXml();
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void ConnectData()
    {
        GameController.Instance.ConnectData();
    }

    #region  새로운 데이타(던전, 용병 등)를 생성하는 부분
    private void FirstData()
    {
        player.Day = 1;
        player.Gold = 100;
        player.Stress = 0;

        unit.CreateRandomKnight(3);
        dungeon.DungeonMake();

        //생성후 한번 저장
        while(!SaveXml());

        ConnectData();
    }

    #endregion

    #region 데이타 로드
    public void LoadXml()
    {
        //0. Check
        string text = xmlDoc.SelectSingleNode("PlayerData/DataSave").InnerText;
        if(! (text == "Done"))
        {
            Debug.Log("이 전 Save에서 비정상적인 종료 확인됨.");
        }
        //1. PlayerData
        XmlNode node = xmlDoc.SelectSingleNode("PlayerData/Player");
        
        LoadNode(player.Day, node, "Day");
        LoadNode(player.Gold, node, "Gold");
        LoadNode(player.Stress, node, "Stress");

        //2. DungeonProgress;
        XmlNodeList nodes = xmlDoc.SelectNodes("PlayerData/DungeonInfo/Dungeon");
        int index = 0;
        
        foreach (XmlNode _node in nodes)
        {
            DungeonProgress dp = new DungeonProgress();
            
            dp.d = dungeon.dungeons[LoadNode<int>(_node, "Num")];
            LoadNode(dp.isClear, _node, "IsClear");
            LoadNode(dp.Saturation, _node, "Saturation");
            LoadNode(dp.SearchP, _node, "SearchP");
            LoadNode(dp.eventType, _node, "Type");
            switch (dp.eventType)
            {
                case 1 :
                    LoadNodeArray(dp.m, _node, "Info");
                    break;
                case 3 :
                    LoadNode(dp.choice_Event_Type, _node, "Info");
                    break;
                default:    break;
            }
            LoadNode(dp.Reward, _node, "Reward");
            LoadNode(dp.experPoint, _node, "ExperPoint");

            dungeon.dungeon_Progress[index++] = dp;
        }

        //3-1. Unit - Knight
        nodes = xmlDoc.SelectNodes("PlayerData/KnightInfo/Knight");
        foreach (XmlNode _node in nodes)
        {
            Knight k = new Knight();

            LoadNode(k.num, _node, "Num");
            LoadNode(k.name, _node, "Name");
            LoadNode(k.job, _node, "Job");
            LoadNode(k.level, _node, "Level");
            LoadNode(k.exper, _node, "Exper");
            LoadNode(k.favor, _node, "Favor");
            LoadNode(k.day, _node, "Day");
            LoadNodeArray(k.skinArr, _node, "Skin");

            LoadNode(k.isAlive, _node, "IsAlive");
            LoadNode(k.hp, _node, "Hp");
            LoadNode(k.stress, _node, "Stress");
            LoadNodeArray(k.uni, _node, "Uni");

            /*
            int[] array = new int[3];
            LoadNodeArray(array, _node, "Personality");
            k.personality.type = array[0];
            k.personality.despair = array[1];
            k.personality.positive = array[2];
            LoadNode(k.mentalLevel, _node, "MentalLevel");
            */
            k.Load();
            unit.knights.Add(k);
        }

        //3-2. Unit - Party
        nodes = xmlDoc.SelectNodes("PlayerData/PartyInfo/Party");
        foreach (XmlNode _node in nodes)
        {
            Party p = new Party();
            LoadNode(p.dungeonNum, _node, "Dungeon");
            LoadNodeArray(p.k, _node, "Knight");
            LoadNode(p.day, _node, "Day");
            LoadNode(p.dayIndex, _node, "DayIndex");
            UnitData.Instance.partys.Add(p);
        }
        //4 EventInfo(Admin, Office)

        //5 Info
        //  5-1 RandomKnightInfo - Knight
        nodes = xmlDoc.SelectNodes("PlayerData/Info/RandomKnightInfo/Knight");
        index = 0;
        foreach (XmlNode _node in nodes)
        {
            RandomKnight rk = new RandomKnight();
            
            LoadNode(rk.name, _node, "Name");
            LoadNode(rk.job, _node, "Job");
            LoadNode(rk.level, _node, "Level");
            LoadNodeArray(rk.skinNum, _node, "Skin");

            unit.randomKnightList[index++] = rk;
        }

        //6 DungeonInfo
        nodes = xmlDoc.SelectNodes("PlayerData/DungeonInfo/Dungeon");
        index = 0;
        foreach(XmlNode _node in nodes)
        {
            DungeonProgress dp = DungeonData.Instance.dungeon_Progress[index];
            dp.d = DungeonData.Instance.dungeons[LoadNode<int>(_node, "Num")];
            LoadNode(dp.isClear, _node, "IsClear");
            LoadNode(dp.Saturation, _node, "Saturation");
            LoadNode(dp.SearchP, _node, "SearchP");
            LoadNode(dp.eventType, _node, "Type");

            switch (dp.eventType)
            {
                case 0: // 로드할 이벤트가 없음. 로드안해도 되는 값들임.
                    dp.isParty = false;
                    index++;
                    continue;
                case 1: 
                    LoadNodeArray(dp.m, _node, "Info");
                    break;    
                case 3:
                    LoadNode(dp.choice_Event_Type, _node, "Info");
                    break;            
                default:break;
            }
            dp.isParty = true;

            LoadNode(dp.Reward, _node, "Reward");
            LoadNode(dp.experPoint, _node, "ExperPoint");
            index++;
        }
        //7 Office
        node = xmlDoc.SelectSingleNode("PlayerData/Office");
        LoadNode(office.officePoint, node, "OfficePoint");
        LoadNode(office.OfficeGage, node, "OfficeGage");
        
        //N. End
        ConnectData();
    }
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

    #region SaveData
    
    public bool SaveXml()
    {
        XmlNode playerDataNode = xmlDoc.SelectSingleNode("PlayerData");
        XmlNode node;
        XmlNodeList nodes;

        //0 dataSave
        UpdateNode("DataSave", playerDataNode, "Saving");
        
        node = playerDataNode.SelectSingleNode("Player");
        //1 Player day,gold, stress
        UpdateNode("Day", node, ToString(player.Day));
        UpdateNode("Gold", node, ToString(player.Gold));
        UpdateNode("Stress", node, ToString(player.Stress));

        //2 KnightInfo - Knight -상세
        //  2-1 기존Knight 업데이트
        int index = 0;
        foreach(XmlNode node_ in playerDataNode.SelectNodes("KnightInfo/Knight"))
        {
            if( System.Convert.ToInt32(node_.SelectSingleNode("Num").InnerText) != index) 
            {
                Debug.Log("Knight 번호 매칭 오류");
                return true;
            }
            Knight k = UnitData.Instance.knights[index];
            UpdateNode("Job", node_, ToString<int>(k.job));
            UpdateNode("Level", node_, ToString<int>(k.level));
            UpdateNode("Exper", node_, ToString<int>(k.exper));
            UpdateNode("Favor", node_, ToString<int>(k.favor));
            UpdateNode("Day", node_, ToString<int>(k.day));
            UpdateNode("Skin", node_, ToStringArray<int>(k.skinArr));

            UpdateNode("IsAlive", node_, ToString<bool>(k.isAlive));
            UpdateNode("Hp", node_, ToString<int>(k.Hp));
            UpdateNode("Stress", node_, ToString<int>(k.Stress));
            UpdateNode("Uni", node_, ToStringArray<int>(k.uni));
            
            /* 수정필요
            UpdateNode("Personality", node_, string.Format( "{0},{1},{2}", k.personality.type, k.personality.despair, k.personality.positive));
            UpdateNode("MentalLevel", node_, ToString<int>(k.mentalLevel));
            */
            index ++;
        }
        //  2-2 신규Knight 등록
        node = playerDataNode.SelectSingleNode("KnightInfo");
        for(int i = index ; i < UnitData.Instance.knights.Count; i++ )
        {
            Knight k = UnitData.Instance.knights[i];
            XmlNode child = CreateNode("Knight", node);

            // * 기본적인 인포
            CreateNode("Num", child, ToString<int>(k.num));
            CreateNode("Name", child, k.name);
            CreateNode("Job", child, ToString<int>(k.job));
            CreateNode("Level", child, ToString<int>(k.level));
            CreateNode("Exper", child, ToString<int>(k.exper));
            CreateNode("Favor", child, ToString<int>(k.favor));
            CreateNode("Day", child, ToString<int>(k.day));
            CreateNode("Skin", child, ToStringArray<int>(k.skinArr));
            //  ---- 전투관련정보
            CreateNode("IsAlive", child, ToString<bool>(k.isAlive));
            CreateNode("Hp", child, ToString<int>(k.hp));
            CreateNode("Stress", child, ToString<int>(k.stress));
            CreateNode("Uni", child, ToStringArray<int>(k.uni));
            //  ---- 성격관련정보
            /* 수정 필요
            CreateNode("Personality", child, string.Format("{0},{1},{2}", k.personality.type, k.personality.despair, k.personality.positive));
            CreateNode("MentalLevel", child, ToString<int>(k.mentalLevel));
            */
        }
        
        nodes = playerDataNode.SelectNodes("PartyInfo/Party");
        //3 Party -- Dungeon정보를 기반으로 기존정보 생성 및 업데이트 후 이외는 삭제
        for(int i = 0; i < 8; i++)
        {
            DungeonProgress dp = dungeon.dungeon_Progress[i];
            int value = Check_InValue(nodes, "Dungeon", System.Convert.ToString(i));
            if(dp.isParty)
            {
                // 3-1 ---- Node생성 _ Xml에 데이타 없어서 신규 Node 생성 필요
                if(value == -1) 
                {
                    XmlNode child = CreateNode("Party", xmlDoc.SelectSingleNode("PlayerData/PartyInfo"));

                    CreateNode("Dungeon", child, ToString<int>(dp.p.dungeonNum));
                    CreateNode("Knight", child, ToStringArray<int>(dp.p.k));
                    CreateNode("Day", child, ToString<int>(dp.p.day));
                    CreateNode("DayIndex", child, ToString<int>(dp.p.dayIndex));

                }
                // 3-2 ---- Node업데이트
                else
                {
                    UpdateNode("Day", nodes[value], ToString<int>(dp.p.day));
                    UpdateNode("DayIndex", nodes[value], ToString<int>(dp.p.dayIndex));
                    UpdateNode("Knight", nodes[value], ToStringArray<int>(dp.p.k));
                }
            }
            else
            {
                // 3-3 ---- Node삭제
                if(value != -1)
                {
                    RemoveNode(nodes[value], playerDataNode.SelectSingleNode("PartyInfo"));
                }
            }
        }           
        //4 EventInfo (Admin, Office)
        //5 Info
        //  5-1 RandomKnightInfo - Knight
        RandomKnight[] rk = unit.randomKnightList;
        node = playerDataNode.SelectSingleNode("Info/RandomKnightInfo");

        nodes =  node.SelectNodes("Knight");
        if(nodes != null)
        {
            foreach(XmlNode node_ in nodes)
            {
                node.RemoveChild(node_);
            }
        }
        for(int i = 0 ; i < rk.Length; i++)
        {
            XmlNode child = CreateNode("Knight", node);
            CreateNode("Name", child, rk[i].name);
            CreateNode("Job", child, ToString<int>(rk[i].job));
            CreateNode("Level", child, ToString<int>(rk[i].level));
            CreateNode("Skin", child, ToStringArray<int>(rk[i].skinNum));
        }

        //6 DungeonInfo Dungeon - 여러개, Type(유동)
        index = 0;
        nodes = playerDataNode.SelectNodes("DungeonInfo/Dungeon");
        if(nodes.Count == 0)
        {   //6-1 Create Dungeon
            for(int i = 0 ; i < 8 ; i ++)
            {
                XmlNode child = CreateNode("Dungeon", playerDataNode.SelectSingleNode("DungeonInfo"));
                CreateNode("Num", child, ToString(DungeonData.Instance.dungeon_Progress[i].d.num));
            }
            nodes = playerDataNode.SelectNodes("DungeonInfo/Dungeon");
        }
        foreach(DungeonProgress dp in DungeonData.Instance.dungeon_Progress)
        {
            UpdateNode("IsClear",    nodes[index], ToString<bool>(dp.isClear));
            UpdateNode("Saturation", nodes[index], ToString<double>(dp.Saturation));
            UpdateNode("SearchP", nodes[index], ToString<double>(dp.SearchP));
            UpdateNode("Type", nodes[index], ToString<int>(dp.eventType));
            switch (dp.eventType)
            {
                case 1 ://배틀
                    UpdateNode("Info", nodes[index], ToStringArray(dp.m));
                    break;
                case 3 ://선택이벤트
                    UpdateNode("Info", nodes[index], ToString<int>(dp.choice_Event_Type));
                    break;
                default:
                    break;
            }
            UpdateNode("Reward", nodes[index], ToString<int>(dp.Reward));
            UpdateNode("ExperPoint", nodes[index], ToString<int>(dp.experPoint));

            index ++;
        }
        //7 Office - 정책, OfficeGage, Point
        node = playerDataNode.SelectSingleNode("Office");
        UpdateNode("OfficePoint", node, ToString(OfficeData.Instance.officePoint));
        UpdateNode("OfficePoint", node, ToString(OfficeData.Instance.OfficeGage));

        //n-1 Game - Retry, Dungeon, Challenge(도전과제)

        //n dataSave-Done
        UpdateNode("DataSave", playerDataNode, "Done");

        StartCoroutine("SaveXml_");

        return true;
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
        yield return true;
    }
    #endregion

    #region DataController 보조 함수
    //기존에 있는지 Xml 데이터 체크
    private int Check_InValue(XmlNodeList nodes, string root, string value)
    {
        int index = 0;
        foreach(XmlNode node in nodes)
        {
            if(node.SelectSingleNode(root).InnerText == value) return index;
            index++;    
        }
        return -1;
    }

    //새로Element생성
    private void CreateNode(string name, XmlNode parent, string value)
    {
        XmlElement child = xmlDoc.CreateElement(name);
        child.InnerText = value;
        parent.AppendChild(child);
    }
    
    //Node생성 
    private XmlNode CreateNode(string name, XmlNode parent)
    {
        XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, string.Empty);
        parent.AppendChild(node);

        return node;
    }

    //Node값 업데이트
    private void UpdateNode(string name, XmlNode node, string value)
    {
        try
        {
            node.SelectSingleNode(name).InnerText = value;
        }
        catch(NullReferenceException  e)
        {
            Debug.Log("UpdateNode() - NullNode -> CreateNode!! <" + name + ">");
            CreateNode(name, node, value);
        }

    }

    //Node삭제
    private void RemoveNode(XmlNode node, XmlNode parent)
    {
        parent.RemoveChild(node);
    }

    //Node값 Load
    private void LoadNode<T>(T value, XmlNode node, string name)
    {
        value = (T)Convert.ChangeType(node.SelectSingleNode(name).InnerText, typeof(T));
    }
    private T LoadNode<T>(XmlNode node, string name)
    {
        return (T)Convert.ChangeType(node.SelectSingleNode(name).InnerText, typeof(T));
    }
    private void LoadNodeArray<T>(T[] array, XmlNode node, string name)
    {
        int[] split = CodeBox.StringSplit(node.SelectSingleNode(name).InnerText);
        array = new T[split.Length];

        for(int index = 0; index < array.Length; index++)
        {
            array[index] = (T)Convert.ChangeType(split[index], typeof(T));
        }
    }
    
    //ToString
    private string ToString<T>(T value)
    {
        return System.Convert.ToString(value);
    }

    //Int배열을 String으로
    private string ToStringArray<T>(T[] array)
    {
        if(array == null)
            return "";

        string text = "";
        for(int i = 0 ; i < array.Length; i++)
            text+= array[i] + ",";
        text = text.Substring(0, text.Length -1);
        
        return text;
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
    #endregion

    #region 기타 함수
    public XmlDocument LoadStory(string fileName)
    {
        string filePath_Story;

        XmlDocument xmlDoc_Story = new XmlDocument();
        if(platform == "pc")
        {
            filePath_Story = "Assets/StreamingAssets/ExternalData/Story/" + fileName + ".xml";
            xmlDoc_Story.Load(filePath_Story);
        }
        else
        {
            filePath_Story =  "jar:file://" + Application.dataPath + "!/assets/ExternalData/Story/" + fileName + ".xml";

            WWW www = new WWW(filePath_Story);

            while(!www.isDone){};

            StringReader stringReader = new StringReader(www.text);
            xmlDoc_Story.LoadXml(stringReader.ReadToEnd());
        }
        
        return xmlDoc_Story;
    }

    public GameObject dataUpdateText;
    public Transform TextPos;
    public void TextViewer(string msg)
    {
        GameObject obj =  CodeBox.AddChildInParent(TextPos, dataUpdateText);
        DataUpdateText dut = obj.GetComponent<DataUpdateText>();

        dut.TextUpdate(msg);
    }
    #endregion
}
