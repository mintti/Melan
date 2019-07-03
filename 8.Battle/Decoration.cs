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
    public DungeonTheme[] theme;

    //현제 오브젝트
    public SpriteRenderer bg;
    public SpriteRenderer ground;

    public void SetTheme(int num)
    {
        bg.sprite = theme[num].bg;
        ground.sprite = theme[num].ground;
    }
}
