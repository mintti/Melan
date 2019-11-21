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
    public int[] skills;
    public Sprite img;

    public int hp;
    public int power;
    public int speed;
    public int stress;

    public int[] uni;

    public Monster(int _num, string _name, int[] _skills,
                   int _hp, int _power, int _speed, int[] _uni)
    {
        num = _num;
        name = _name;
        skills = _skills;

        stress = 0;

        hp = _hp;
        power = _power;
        speed = _speed;
        uni = _uni;
    }
}

//전투시 몬스터의 상태를 저장하는 클래스
public class MonsterState
{
    public Monster m{get;set;}
    public State s;
    
    public MonsterState(Monster _m)
    {
        m = _m;
        s = new State(m.hp, m.power, m.speed, m.stress, m.uni, LifeType.M);
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
        Debug.Log(monsterPrefabs.Length);
    }
    public void InsertData()
    {
         monsters = new Monster[size];

        //데이터 삽입
        #region 몬스터 리스트
        monsters[0] = new Monster(0, "슬라임", new int[]{0,1}, 10, 6, 5, null);
        monsters[1] = new Monster(1, "토끼슬라임", new int[]{0,1}, 10, 6, 5, null);
        monsters[2] = new Monster(2, "킹슬라임", new int[]{0,1}, 10, 6, 5, null);

        #endregion
        //이미지로드
        for(int i = 0 ; i < size; i ++)
        {
            string path =  string.Format("2.Monster/" + i);
            Sprite sprite =  Resources.Load <Sprite>(path);
            monsters[i].img = sprite;
        }
    }


    public GameObject[] monsterPrefabs;

}
