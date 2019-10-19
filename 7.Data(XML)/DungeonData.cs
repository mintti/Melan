using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon
{
    public int num;
    public string name{get;set;}
    public int level{get;set;}
    public int[] monsters{get;set;}
    public int boss;
    public int day{get;set;}
    public Dungeon(string _name, int _level, int[] _monsters, int _boss)
    {
        name = _name;
        level = _level;
        monsters = _monsters;
        boss = _boss;

        day = 3 * level; 
    }
    
}

public class DungeonProgress
{
    public Dungeon d;
    
    public bool isClear; //보스를 잡았는가~
    private double saturation;//포화도
    public double Saturation
    {
        get{return saturation;}
        set{saturation = value; if(saturation > 100) saturation = 100f;}
    }
    private double searchP;//진행도 (탐색 90/boss 10) : 50 부터 포화도 카운트
    public double SearchP
    {
        get{return searchP;}
        set{searchP = value; if(searchP > 100) searchP = 100f;}
    }

    public DungeonProgress(Dungeon _d)
    {
        d = _d;
        Reset();
    }

    public void SetData(Dungeon _d, bool _isClear, double _saturation, double _searchP){
        d = _d;
        isClear = _isClear;
        Saturation = _saturation;
        SearchP = _searchP;
    }
    public void Reset()
    {
        isClear = false;
        Saturation = 50;
        searchP = 0;
    }

}

public class DungeonData : MonoBehaviour
{
    private static DungeonData _instance = null;

    public static DungeonData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(DungeonData)) as DungeonData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active DungeonData object");
                }
            }
            return _instance;
        }
    }




    public Dungeon[] dungeons = new Dungeon[16];
    public DungeonProgress[] dungeon_Progress = new DungeonProgress[8];

    public void SetData()
    {
        /*   level 1  */
        dungeons[0] = new Dungeon("슬라임 군락", 1 , new int[]{0,1}, 0);
        dungeons[1] = new Dungeon("음험한 산지", 1, new int[]{2,3}, 1);
        dungeons[2] = new Dungeon("맑은 오아시스", 1, new int[]{4,5}, 2);
        dungeons[3] = new Dungeon("의문의 땅굴", 1, new int[]{6,7}, 3);

        /*   level 2  */
        dungeons[4] = new Dungeon("비명의 산", 2, new int[]{8,9}, 4);
        dungeons[5] = new Dungeon("깊고 어두운 호수", 2, new int[]{10,11},5);
        dungeons[6] = new Dungeon("꽃피는 화원", 2, new int[]{12, 13}, 6);
        dungeons[7] = new Dungeon("환각의 숲", 2, new int[]{14, 15}, 7);
        dungeons[8] = new Dungeon("키큰 오두막", 2, new int[]{16,17}, 8);
        dungeons[9] = new Dungeon("의문의 탑", 2, new int[]{18, 19}, 9);
        
        /*   level 3   */
        dungeons[10] = new Dungeon("황폐한 평원", 3, new int[]{20,21}, 10);
        dungeons[11] = new Dungeon("크로드네 령", 3, new int[]{22,23}, 11);
        dungeons[12] = new Dungeon("테레제바 국", 3, new int[]{24,25}, 12);
        dungeons[13] = new Dungeon("X의 골짜기", 3, new int[]{}, 13);

        /*   level 4   */
        dungeons[14] = new Dungeon("마왕성", 4, new int[]{26,27}, 0);
        dungeons[15] = new Dungeon("이형의 땅", 4, new int[]{28,29}, 0);
    }

    
    //DataController에서 호출됨.
    public void DungeonMake()
    {
        int[] array = new int[8];
        /*          lv       dungeonArr[]
        0~3   : 1          2  0  -
        4~9   : 2          5  3  1
        10~13 : 3          7  6  4
        14~15 : 4
        */
        List<int> list = new List<int>(){ 0,1, 2,3, 4, 5,6, 7, 8,9,10, 11, 12, 13, 14,15};
        int[,] rangeList = new int[4, 2]{{0, 4}, {2, 8}, {5, 9}, {7, 9}};
        int[] con = new int[4]{2, 3, 2, 1};

        int index = 0;
        for(int i = 0 ; i < 4; i++)
        {
            for(int j = 0 ; j < con[i]; j ++)
            {
                array[index] = list[Random.Range(rangeList[i, 0], rangeList[i, 1]- j)];
                list.Remove(array[index++]);
            }
        }

        ConnectDungoen(array);
        DataController.Instance.SaveDungeon(array);
    }

    private void ConnectDungoen(int[] array)
    {
        for(int i = 0 ; i < 8; i++)
        {
            Dungeon d = dungeons[array[i]];
            dungeon_Progress[i] = new DungeonProgress(d);
        }
    }
    
}
