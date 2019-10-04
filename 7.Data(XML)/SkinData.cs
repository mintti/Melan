using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class Skin
{
    public Sprite frontHair;
    public Sprite backHair;
    public Sprite topBrow;
    public Sprite underBrow;
    public Sprite back;
    public Sprite pupil;

    
    public Skin()
    {

    }
    public Skin(int[] skinNum)
    {
        frontHair = SkinData.Instance.frontHair[skinNum[0]];
        backHair = SkinData.Instance.backHair[skinNum[1]];
        topBrow = SkinData.Instance.topBrow[skinNum[2]];
        underBrow = SkinData.Instance.underBrow[skinNum[3]];
        back = SkinData.Instance.back[skinNum[4]];
        pupil = SkinData.Instance.pupil[skinNum[5]];
    }
}
public class SkinData : MonoBehaviour
{
    private static SkinData _instance = null;

    public static SkinData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(SkinData)) as SkinData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active SkinData object");
                }
            }
            return _instance;
        }
    }

    public Sprite[] closet;

    public Sprite[] frontHair;
    public Sprite[] backHair;
    public Sprite[] topBrow;
    public Sprite[] underBrow;
    public Sprite[] back;
    public Sprite[] pupil;


    public void LoadResource()
    {
        string path = "1.Skin/0.FrontHair/";
        frontHair = Resources.LoadAll<Sprite>(path);
        path = "1.Skin/1.BackHair/";
        backHair = Resources.LoadAll<Sprite>(path);

        path = "1.Skin/2.Eye/";
        int size = Resources.LoadAll<Sprite>(path).Length / 3;
        topBrow = new Sprite[size];
        underBrow = new Sprite[size];
        back = new Sprite[size];
        for(int i = 0 ; i < size; i++)
        {
            topBrow[i] = Resources.Load<Sprite>(path + i);
            underBrow[i] = Resources.Load<Sprite>(path + i + "_1");
            back[i] = Resources.Load<Sprite>(path + i + "_2");
        }


        path = "1.Skin/3.Pupil/";
        pupil = Resources.LoadAll<Sprite>(path);

        Debug.Log("로드완료");

    }

    private void LoadResource2(string path, Sprite[] array, int type)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        int size = sprites.Length;
        //Eye 용( 3분할되잇음 )
        string tail = "";
        if(type > 0)
        {
            size/=3;
            tail = type == 1 ? "" : type == 2 ? "_1" : "_2";
        }
        System.Array.Resize(ref array, size);
        array = new Sprite[size];
        Debug.Log(array.Length);

        for(int i = 0; i< size; i++)
        {
            array[i] = Resources.Load<Sprite>(path + i + tail);
        }
    }


    public int[] RandomSkin()
    {
        int[] skin = new int[6]{
            Random.Range(0, frontHair.Length),
            Random.Range(0, backHair.Length),
            Random.Range(0, topBrow.Length),
            Random.Range(0, underBrow.Length),
            Random.Range(0, back.Length),
            Random.Range(0, pupil.Length)
        };
        

        return skin;
    }
}
