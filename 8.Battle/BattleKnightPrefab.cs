using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleKnightPrefab : MonoBehaviour
{
    public KnightState ks;

    public Image img;

    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isKnight;
    
    private void Start() {
        isKnight = false;
    }
    //BattleCon - MonsterSetting()에서 호출됨.
    public void SetData(KnightState _ks)
    {
        ks = _ks;
        img.sprite = ks.k.skin;

        isKnight = true;
    }
}
