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
    public int[] skill;
    public Sprite img;

    public Monster(int _num, string _name, int[] _skill)
    {
        num = _num;
        name = _name;
        skill = _skill;
    }
}

public class MonsterData : MonoBehaviour
{
    public int size;
    public Monster[] monsters;

    public void Test_InsertData()
    {   
        //몬스터 배열 정의
        monsters = new Monster[size];

        //데이터 삽입
        monsters[0] = new Monster(0, "슬라임", new int[]{0,1});

        
        //이미지로드
        for(int i = 0 ; i < size; i ++)
        {
            string path =  string.Format("2.Monster/" + i);
            Sprite sprite =  Resources.Load <Sprite>(path);
            monsters[i].img = sprite;
        }

    }

}
