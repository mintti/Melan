using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleController : MonoBehaviour
{   
    #region 데코 부분

    public BattleKnightPrefab[] kps = new BattleKnightPrefab[4];//0~3;
    public MonsterStateViewer[] msv = new MonsterStateViewer[5];
    public Transform[] monsterPos = new Transform[4];
    public BattleMonsterPrefab[] mps = new BattleMonsterPrefab[4];//4~7;
    
    public Text phaseText;
    public Text turnText;
    
    #endregion
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

    private DungeonProgress dp;
    private BattleState state;
    public BattleState State{
        get{return state;}
        set{ state = value;
            Debug.Log("'" + State + "'상태 실행");
            switch(state){
            case BattleState.로드 :
                Phase++;
                MonsterSetting();
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
    private int phase;
    public int Phase
    {   get{return phase;}  set{phase = value; phaseText.text = string.Format("PHASE " + (phase < 10 ? "0" : "") + phase); turn = 0;}}
    private int turn;
    public int Turn
    {   get{return turn;}  set{turn =value;turnText.text = string.Format("TURN " + (turn < 10 ? "0" : "") + turn);}}
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

        Phase = 0;
        LoadBattleData();
    }

    
    //1. 테이타 로드.
    public List<Monster> monsterList = new List<Monster>();
    public List<int> monsterTarget = new List<int>();
    public List<int> knightTarget = new List<int>();
    public int phaseInMonsterCnt;

    public List<State> thing = new List<State>();
    public List<int> thingTarget = new List<int>();
    public int knightCount;

    //보스일 경우
    BattleController_Boss boss;
    private void LoadBattleData()
    {
        //1-1 . 데이타 로드.
        //      -  몬스터 위치 지정은 그냥 순서대로 4명 씩 Load함. 
        //      -  모든 기사와 몬스터의 요소를 Thing(ArrayList)에 집어넣음
        dp = EventData.Instance.battle_dp;
        knightCount = dp.p.k.Length;
        
        int cnt = dp.p.k.Length;
        thing.Clear();
        for(int i = 0; i < cnt; i++)
        {
            KnightState ks = dp.p.knightStates[i]; 
            
            if(ks.s.Hp > 0)
            {
                thing.Add(ks.s);
                kps[i].SetData(ks, i);
                ks.s.SetBKP(kps[i]);
                thingTarget.Add(i);
            }
        }
        for(int i = cnt; i < 4; i++)
        {
            kps[i].gameObject.SetActive(false);
        }
        //1-1-2 스킬 폼을 세팅해줌.
        ColController.Instance.SetForm();
        
        //1-1-3 dp(Battle)에서 m은 int로 저장되있다. monsterArr를 생성해 직접 Monster를 삽입한다.
        int size = dp.m.Length;
        Monster[] monsterArr = new Monster[size];
        for(int i = 0 ; i < size; i ++)
        {
            monsterArr[i] = MonsterData.Instance.monsters[dp.m[i]];
        }
        foreach(Monster m in monsterArr)
            monsterList.Add(m);

        //1-2 . 던전에 따른 세팅 (데코part)
        
        //---------- 필드 세팅 완료 -----------
        //1-3. 전투 턴, 페이즈 초기화 및 전투개시.
        State = BattleState.로드;
    }

    //2. 몬스터 정보 로드(Phase가 변경될때 프로퍼티에서 호출됨.)
    // [(몬스터)로드] 상태에 해당함. 표시되는 몬스터를 로드..
    //turn이 변경될 때 마다 SettingTurn() 호출.
    private GameObject monsterObj;
    private void MonsterSetting()
    {
        Debug.Log(" MonsterSetting() 실행");
    
        //2-0 초기화
        phaseInMonsterCnt = 0;
        while(thing.Count > knightCount)
        {
            thing.RemoveAt(knightCount);
            thingTarget.RemoveAt(knightCount);
        }
        
        //2-1 표시될 몬스터를 showMonster에서 호출함
        for(int i = 0; i < 4; i ++)
        {
            //2-1-E 더 이상 호출한 몬스터가 없음.
            if(monsterList.Count == 0)
            {
                //표시될 몬스터가 있으면, 로드 종료
                //               없으면, [승리] 상태가됨.
                if(mps[0].s.alive == AliveType.생존)
                {
                    for(; i<4 ;i++)
                        msv[i].gameObject.SetActive(false);//스테이트,COL임
                    
                    break;
                }
                State = BattleState.승리;
                return;
            }

            monsterObj = MonsterData.Instance.monsterPrefabs[monsterList[0].num];
            
            mps[i] = CodeBox.AddChildInParent(monsterPos[i], monsterObj).GetComponentInChildren<BattleMonsterPrefab>();
            //2-1-1 show와 thing 에 추가
            mps[i].SetData(monsterList[0], i, msv[i]);
            thing.Add(mps[i].s);
            thingTarget.Add(4+i);
            phaseInMonsterCnt++;
            //2-1-1-2 스테이트 상태COL 활성화
            msv[i].gameObject.SetActive(true);
            msv[i].SetData(mps[i]);
            //2-1-2 호출된 몬스터는 리스트에서 삭제
            monsterList.Remove(monsterList[0]);
        }
        msv[4].gameObject.SetActive(false);//※이거 보스...
        

        //2-2 몬스터 세팅 완료했으니 전투 순서를 정해줌. (3-1 이동)
        Turn = 0;
        SettingTurn();
    }
    //3. 순서 전투
    //3-1. Speed 순서에 따라 순서 지정
    List<int> sequence = new List<int>(); //순서를 저장하는 배열.
    bool isSpeed; //Speed변경이 감지 되면 true; 
    private void SettingTurn()
    {
        sequence.Clear();
        Debug.Log(" SettingTurn() 실행");
        //3-1-1 생존한 존재 중에서 순서를 지정하여 배열에 넣어줌.
        //      speed에 따라 시퀀스 배열에 순서를 삽입정렬해줌. (작은 수를 앞으로)
        sequence.Add(0);
        
        
        for(int i = 1; i < thing.Count; i++)
        {//i는 thing의 피봇..
            int end = sequence.Count; //카운트가 아래 for문 조건문에 들어가면 수가 계속 늘어나면서 무한반복함.
            for(int j = 0; j < end; j++)
            {
                //3-1-1-1 만약 sequence리스트 순회 중 보다 작은 수면 앞에 삽입 후 순회탈출
                if(thing[i].Speed > thing[sequence[j]].Speed)
                {
                    //아래에 Speed가 동일한 경우 몬스터 턴보다 Knight의 턴이 먼저 오도록 하는 코드를 작성
                    /* 
                    if(thing[i].Speed == )
                    */
                    sequence.Insert(j, i);//j번재에 i삽입.
                    break;
                }
                //3-1-1-2 최종 순회까지 작은수가 없으면 그냥 맨뒤
                if(j == sequence.Count-1)
                    sequence.Add(i);
            }
        }
        
        //Target지정자
        knightTarget.Clear();
        monsterTarget.Clear();
        for(int i = 0; i < thing.Count; i++)
        {
            if(thing[i].Hp > 0)
            {
                if(thing[i].LifeType == LifeType.K)
                    knightTarget.Add(i);
                else
                    monsterTarget.Add(i);
            }
        }

        //3-1-2 턴 카운트 시작(1부터)하며 전투 시작.
        Turn++;
        who = 0; //pivot이라 0부터 시작.
        State = BattleState.전투; 
        isSpeed = false; // 감지 매턴 순서 지정 후 false해줌.
    }

    //3-2 [전투] 상태임.
    private void Battle()
    {
        //3-2-0 마지막 사람 종료시
        //      who는 sequence의 pivot임.
        if(who == sequence.Count)
        {
            //스피드 변경이 감지되면 순서 다시 정해줌.
            //아닐경우 해당 순서로 다시 진행.
            if(isSpeed)
                SettingTurn();
            else
            {
                Turn++;
                who = 0;
                State = BattleState.전투;
            }
            return;
        }

        //전투 실행.(Player 행동결정)

        Debug.Log("  Battle() 실행");
        
        //알맞는 thing을 위해 몬스터의 경우, 부족한 기사 수만큼 피봇 수를 빼줌. 
        int n = sequence[who];
        if(n >= 4) n  -= (4 - knightCount);
        //3-2-1 기사 혹은 몬스터의 공격
        //      해당 턴이 기사일 경우, (사용자에게 입력받는) 해당 기사의 Skill셋으로 변경해줌.

        try
        {
        if(thing[n].LifeType == LifeType.K)
        {
            int num = thingTarget[sequence[who]];
            KnightState ks = kps[num].ks;
            kps[num].TurnStart();
            //스킬을 세팅해줌
            ColController.Instance.TurnStart(num);
        }
        else//일단 임시(테스트용)
        {
            if(mps[n-knightCount].s.alive == AliveType.죽음)
            {
                StartCoroutine("NextTurn");
                return;
            }
            int num = thingTarget[sequence[who]];
            mps[n-knightCount].MyTurn();
        }
        }
        catch(ArgumentOutOfRangeException ex)
        {
            Debug.Log("오류가 발생햇지만 넘어간당");
        }

    }
    public void LogThing()
    {
        string text = string.Format("thing size({0}) >> ", thing.Count);

        for(int i = 0 ;i <thing.Count;i ++)
        {
            text += thing[i].LifeType;
        }
        Debug.Log(text);
    }
    
    //턴을 넘긴 상태.
    public GameObject waitObj;
    IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(0.2f);
        waitObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        
        waitObj.SetActive(false);
        who++;
        State = BattleState.전투;
    }

    private void FastNextTurn()
    {
        who++;
        State = BattleState.전투;
    }
    

    //4. [처치] 상태
    public void KillMonster(int num)
    {
        //4-1. 몬스터의 정보에서 보상을 가져와 파티 정보에 누적시킴.
        
        //4-2. 해당 몬스터 죽음 처리. thing들에서 제외시킴

        //int index = num + knightCount;
        //monsterTarget.Remove(index);
        //thingTarget.Remove(index);
        //thing.RemoveAt(index);

        //4-2. 몬스터가 끝난경우 [로드]로감.
        if(!IsAliveMonster())
            State = BattleState.로드;
        
    }

    private bool IsAliveMonster() //살아있는게 있으면 true 
    {
        for(int i = 0 ; i < phaseInMonsterCnt; i++)
        {
            if(mps[i].s.alive == AliveType.생존)
                return true;
        }
        return false;
    }

    //5. [죽음] 상태, 해당 기사의 정보를 직접 로드함.
    public void DieKnight(int n)
    {
        kps[n].Die();
        Debug.Log("기사 주금");
    }

    public GameObject winObj;
    public Text winRewardText;
    //6. [승리] 상태, [로드] 상태에서 판별 후 호출됨.
    private void Win()
    {
        ColController.Instance.BattleEnd();

        winObj.SetActive(true);
        int reward =  GetReward();
        winRewardText.text = string.Format("WE GET {0}G", reward);
        dp.Reward += reward;
    }
    //7. [패배] 상태
    private void Lose()
    {
        
    }
    //8. [도망] 상태
    private void Escape()
    {

    }
    #endregion

    private void GetPrefabState(int _n)
    {
        int num = thingTarget[_n];
        Knight k;
        string name;
        if(num < 4)
        {
            name = kps[num].ks.k.name;
        }
        else
        {
            name = mps[num-4].ms.m.name;
        }

        Debug.Log(name);
    }

    //배틀 시 State 로드
    public State GetSingleTarget()
    {
        int targetNum = UnityEngine.Random.Range(0, knightTarget.Count);
        State target = thing[targetNum];
        
        ColController.Instance.SetSkillTarget(targetNum);

        return target;
    }

    public State GetSingleTarget_Lowest_HP()
    {
        int targetNum = 0;
        for(int i = 1 ; i < knightTarget.Count; i++)
        {
           targetNum = thing[targetNum].Hp < thing[i].Hp ? targetNum : i;
        }
        State target = thing[targetNum];
        
        ColController.Instance.SetSkillTarget(9);
        return target;
    }

    //대기 코루틴
    IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private int GetReward()
    {
        int gold = 0;
        foreach(int index in dp.m)
            gold += MonsterData.Instance.monsters[index].GetReward();

        return gold;
    }

    public SceneController sceneController; 
    public void EndBattle()
    {
        dp.SetData(8);
        
        //HP정보  Knight 에 저장
        foreach(KnightState ks in dp.p.knightStates)
            ks.UpdateState();
            
        sceneController.MoveToMain();
    }
    
}
