using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DungeonTheme
{
    public string name;
    public Sprite bg;
    public Sprite ground;

}

public class Decoration : MonoBehaviour
{
    private static Decoration _instance = null;

    public static Decoration Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(Decoration)) as Decoration;

                if(_instance == null)
                {
                    Debug.LogError("There's no active Decoration object");
                }
            }
            return _instance;
        }
    }


    public DungeonTheme[] theme;

    //현제 오브젝트
    public SpriteRenderer bg;
    public SpriteRenderer ground;

    
}
