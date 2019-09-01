using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투시 몬스터의 상태를 저장하는 클래스
public class MonsterState
{
    public Monster m{get;set;}
    
    public int hp;
    public int power;
    public int speed;
    public AliveType type{get;set;}
    public List<SkillImpact> impact;

    public MonsterState(Monster _m)
    {
        m = _m;

        hp = m.hp;
        power = m.power;
        speed = m.speed;
        
        type = AliveType.생존;
    }
    
}
public class BattleController : MonoBehaviour
{   
    #region 싱글톤 (Awake여깃음)

    private static BattleController _instance;
    public static BattleController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(BattleController)) as BattleController;

                if(_instance == null)
                {
                    Debug.LogError("There's no active BattleController object");
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        _instance = this;
    }


    #endregion

    #region UI 관련
    public GameObject[] knight = new GameObject[4];


    #endregion

    #region 진행 관련 함수 및 변수들
    public enum BattleState
    {
        로드,// : 몬스터의 정보를 로드함. 
        전투,// : 전투 중
        처치,// : 몬스터 처치시, 마지막 몬스터일 경우 로드 or 승리됨. 아닐경우 다시 전투로
        죽음,// : 아군의 사망, 마지막 까지 죽은 경우, 전투는 패배.
        승리,
        패배,
        도망
    }

    private Battle Bdata;
    private BattleState state{
        get{
            return state;
        }
        set{
            state = value;
            switch(state){
            case BattleState.로드 :
                phase++;
                break;
            case BattleState.전투 :
                Battle();
                break;
            case BattleState.처치 :
                break;
            case BattleState.죽음 :
                break;
            case BattleState.승리 :
                Win();
                break;
            case BattleState.패배 :
                Lose();
                break;
            case BattleState.도망 :
                Escape();
                break;
            default :
                CodeBox.PrintError("Battle - BattleController - SwitchState() 에서 예외가 생겼음.");
                break;
        }
        }
    }
    /** 순서 변수 **
        phase   : 몬스터 몇 번째 페이즈?
        turn    : n페이즈의 n번째 '턴'
        who     : n번째 턴 중 '누구의 차례?'    */
    private int phase{get;set;}
    private int turn{get;set;}
    private int who;



    //몬스터 데이타
    private GameObject[,] monsterPl; //배치된 몬스터의 위치 및 데이타
    
    private Decoration deco;
    
    private GameObject [] player = new GameObject[4];

    //던전 전투 데이타
    Dungeon dungeon;
    Knight[] team;
    int gold;
    int honor;
    int exper;

    private void Start() {
        gold = 0;
        honor = 0;
        exper = 0;

        LoadBattleData();
    }

    
    //1. 테이타 로드.
    public List<Monster> monsterList = new List<Monster>();
    public ArrayList thing = new ArrayList();
    public void LoadBattleData()
    {
        //1-1 . 데이타 로드.
        //      -  몬스터 위치 지정은 그냥 순서대로 4명 씩 Load함. 
        //      -  모든 기사와 몬스터의 요소를 Thing(ArrayList)에 집어넣음
        Bdata = EventData.Instance.Bdata;
        
        
        foreach(KnightState k in Bdata.p.knights)
        {

            if(k.type == AliveType.생존)
                thing.Add(k);
        }
        
            
        
        //1-1-2 Bdata(Battle)에서 m은 int로 저장되있다. monsterArr를 생성해 직접 Monster를 삽입한다.
        Monster[] monsterArr = new Monster[Bdata.m.Length];
        for(int i = 0 ; i < Bdata.m.Length; i ++)
        {
            monsterArr[i] = DataController.Instance.monster.monsters[i];
        }
        foreach(Monster m in monsterArr)
            monsterList.Add(m);

        //1-2 . 던전에 따른 세팅 (데코part)
        
        //---------- 필드 세팅 완료 -----------
        //1-3. 전투 턴, 페이즈 초기화 및 전투개시.
        phase = 1; //해당 변수 프로퍼티에서2.[로드] 호출됨.
    }

    //2. 몬스터 정보 로드(Phase가 변경될때 프로퍼티에서 호출됨.)
    // [(몬스터)로드] 상태에 해당함. 표시되는 몬스터를 로드..
    //turn이 변경될 때 마다 SettingTurn() 호출.
    public List<MonsterState> showMonster = new List<MonsterState>();
    public void MonsterSetting()
    {
        //2-1 표시될 몬스터를 showMonster에서 호출함
        for(int i = 0; i < 4; i ++)
        {
            //2-1-E 더 이상 호출한 몬스터가 없다면 [승리] 상태가됨.
            if(monsterList.Count == 0)
            {
                state = BattleState.승리;
                return;
            }
            
            /*
            //2-1-1 show와 thing 에 추가
            showMonster.Add(monsterList[0]);
            thing.Add(monsterList[0]);
            //2-1-2 호출된 몬스터는 리스트에서 삭제
            monsterList.Remove(monsterList[0]);
             */    
        }
        

        //2-2 몬스터 세팅 완료했으니 전투 순서를 정해줌. (3-1 이동)
        turn = 0;
        SettingTurn();
    }

    //3. 순서 전투
    //3-1. Speed 순서에 따라 순서 지정
    List<int> sequence = new List<int>(); //순서를 저장하는 배열.
    bool isSpeed; //Speed변경이 감지 되면 true; 
    public void SettingTurn()
    {
        //3-1-1 생존한 존재 중에서 순서를 지정하여 배열에 넣어줌.
        //      speed에 따라 시퀀스 배열에 순서를 삽입정렬해줌. (작은 수를 앞으로)
        sequence[0] = 0;
        
        /* 수정파트
        for(int i = 1; i < thing.Count; i++)
        {//i는 thing의 피봇..
            for(int j = 0; j < sequence.Count; j++)
            {
                //3-1-1-1 만약 sequence리스트 순회 중 보다 작은 수면 앞에 삽입 후 순회탈출
                if(thing[i].speed < thing[sequence[j]].speed)
                {
                    sequence.Insert(j, i);//j번재에 i삽입.
                    break;
                }

                //3-1-1-2 최종 순회까지 작은수가 없으면 그냥 맨뒤
                if(j == sequence.Count-1)
                    sequence.Add(i);
            }
        }
*/
        //3-1-2 턴 카운트 시작(1부터)하며 전투 시작.
        turn++;
        who = 0; //pivot이라 0부터 시작.
        state = BattleState.전투; 
        isSpeed = false; // 감지 매턴 순서 지정 후 false해줌.
    }

    //3-2 [전투] 상태임.
    public void Battle()
    {
        /*
        //3-2-1 기사 혹은 몬스터의 공격
        if(thing[who].type)

        //3-2-2 마지막 사람 종료시
        //      who는 sequence의 pivot임.
        if(who == sequence.Length)
        {
            //스피드 변경이 감지되면 순서 다시 정해줌.
            //아닐경우 해당 순서로 다시 진행.
            if(isSpeed)
                SettingTurn();
            else
            {
                turn++;
                who = 0;
                state = BattleState.전투;
            }
            return;
        }
        //다음 턴으로 넘어가며 함수 재호출.

        state = BattleState.전투; //재호출하는 부분임.
         */
    }
    

    //4. [처치] 상태
    public void KillMonster(Monster m)
    {
        //4-1. 몬스터의 정보에서 보상을 가져와 파티 정보에 누적시킴.
        
        //4-2. 해당 몬스터 죽음 처리. thing들에서 제외시킴
        //showMonster.Remove(m);
        thing.Remove(m);
        //4-2. 몬스터가 남아있는 경우 [전투], 끝난경우 [로드]로감.
        if(showMonster.Count == 0)
            state = BattleState.로드;
        else
        {
            state = BattleState.전투;
        }
    }

    //5. [죽음] 상태, 해당 기사의 정보를 직접 로드함.
    public void DieKnight(KnightState ks)
    {

    }

    //6. [승리] 상태, [로드] 상태에서 판별 후 호출됨.
    public void Win()
    {

    }
    //7. [패배] 상태
    public void Lose()
    {

    }
    //8. [도망] 상태
    public void Escape()
    {

    }
    #endregion

    
}
