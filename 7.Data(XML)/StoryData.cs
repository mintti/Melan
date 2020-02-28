using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Story
{
    public Sprite[] sprites;
    public string[] texts;
    public int[] texts_To_Sprite;

    public void SetData(Sprite[] s, string[] t, int[] tts)
    {
        sprites = s; texts = t; texts_To_Sprite = tts;
    }
}
public class StoryData : MonoBehaviour
{
    private static StoryData _instance = null;

    public static StoryData Instance
    {
        
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(StoryData)) as StoryData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active StoryData object");
                }
            }
            return _instance;
        }
    }

    public string[] list = new string[5];

    private void Start() {
        SetData();
    }

    private void SetData()
    {
        list[0] = "Prolog";
        list[1] = "MainStory";
        list[2] = "Slime";
        list[3] = "Boss";
        list[4] = "Boss";
    }

    public GameObject event_Story_Obj;
    public GameObject event_CnC_Obj;

    public void TestStory()
    {
        CodeBox.AddChildInParent(GameController.Instance.story_Tr, event_Story_Obj).GetComponent<Event_Story>().SetData("prolog");
        
    }
    public Story GetStory(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("4.Story/" + name);

        //텍스트 로드
        XmlDocument xmlDoc = DataController.Instance.LoadXml("ExternalData/Story/" + name + ".xml");
        XmlNodeList textNodes = xmlDoc.DocumentElement.SelectNodes("text");
        string[] texts = new string[textNodes.Count];
        int[] tts = new int[textNodes.Count];

        foreach(XmlNode node in textNodes)
        {
            int index = System.Convert.ToInt32(node.Attributes["id"].Value.Split('_')[1]);

            texts[index] = node.InnerText;
            tts[index] = System.Convert.ToInt32(node.Attributes["spr"].Value);
        }
        
        Story story = new Story();
        story.SetData(sprites, texts, tts);

        return  story;
    }
    #region 시나리오 세팅
    

    #endregion
}
