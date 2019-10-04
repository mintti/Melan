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
        LoadResource2(path, frontHair, 0);
        path = "1.Skin/1.BackHair/";
        LoadResource2(path, backHair, 0);
        path = "1.Skin/2.Eye/";
        LoadResource2(path, topBrow, 1);
        path = "1.Skin/2.Eye/";
        LoadResource2(path, underBrow, 2);
        path = "1.Skin/2.Eye/";
        LoadResource2(path, back, 3);
        path = "1.Skin/3.Pupil/";
        LoadResource2(path, pupil, 0);

    }

    private void LoadResource2(string path, Sprite[] array, int type)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fis = di.GetFiles();

        int size = fis.Length;
        
        //Eye 용( 3분할되잇음 )
        string tail = "";
        if(type > 0)
        {
            size/=3;
            tail = type == 1 ? "" : type == 2 ? "_1" : "_2";
        }

        array = new Sprite[size];

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
