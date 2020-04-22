using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using System.IO;

[System.Serializable]
public class Monster
{
    public int num;
    public string name;

    public int hp;
    public int power;
    public int speed;
    public int stress;

    public int[] uni;

    public Monster(int _num, string _name,
                   int _hp, int _power, int _speed, int[] _uni, int _gold)
    {
        num = _num;
        name = _name;

        stress = 0;

        hp = _hp;
        power = _power;
        speed = _speed;
        uni = _uni;

        gold = _gold;
    }
    //REWARD
    int gold;
    public int GetReward()
    {
        return gold;
    }
}

//전투시 몬스터의 상태를 저장하는 클래스
public class MonsterState
{
    public Monster m{get;set;}
    public State s= new State();
    
    public MonsterState(Monster _m)
    {
        m = _m;
        s.SetData(this);
    }
}


public class MonsterData : MonoBehaviour
{
    #region 싱글톤
    private static MonsterData _instance = null;

    public static MonsterData Instance
    {
        
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(MonsterData)) as MonsterData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active MonsterData object");
                }
            }
            return _instance;
        }
    }
    #endregion

    public int size;
    public Monster[] monsters;

    private void Start() {
        
    }
    public void InsertData()
    {
        size = 80;
        monsters = new Monster[size];

        //데이터 삽입
        #region 몬스터 리스트
        //00 슬라임 숲
        monsters[0] = new Monster(0, "슬라임", 20, 3, 6, null, 5);
        monsters[1] = new Monster(1, "동물슬라임",  20, 2, 12, null, 5);
        monsters[2] = new Monster(2, "포이즌슬라임",  30, 5, 8, null, 5);
        monsters[3] = new Monster(3, "기사슬라임", 40, 10, 15, null, 10);
        monsters[4] = new Monster(4, "킹슬라임", 200, 20, 10, null, 10);



        //04 땅굴
        monsters[15] = new Monster(15, "개미", 20, 3, 6, null, 5);
        monsters[16] = new Monster(16, "유충",  20, 2, 12, null, 5);
        monsters[17] = new Monster(17, "병정개미",  30, 5, 8, null, 5);
        monsters[18] = new Monster(18, "진딧물", 40, 10, 15, null, 10);
        monsters[19] = new Monster(19, "여왕개미", 200, 20, 10, null, 10);


        //07 화원
        monsters[30] = new Monster(30, "마론", 20, 3, 6, null, 5);
        monsters[31] = new Monster(31, "레비",  20, 2, 12, null, 5);
        monsters[32] = new Monster(32, "유니",  30, 5, 8, null, 5);
        monsters[33] = new Monster(33, "케로", 40, 10, 15, null, 10);
        monsters[34] = new Monster(34, "산양메리", 200, 20, 10, null, 10);


        #endregion
        
    }


    public GameObject[] monsterPrefabs;

}
