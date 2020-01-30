using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event_Story : MonoBehaviour
{
    public Image image;
    private TypingText typingText;
    
    private int storyNum;
    public int StoryNum{get{return storyNum;}
        set{
            storyNum = value;
            if(story.texts.Length <= storyNum)
            {
                End();
                return;
            }
            Story();
        }}
    private Story story;
    private int sprNum;
    public void SetData(string name)
    {
        story = StoryData.Instance.GetStory(name);
        typingText = transform.GetComponentInChildren<TypingText>();
        
        storyNum = 0;
        sprNum = -1;
        Story();
    }
    

    private void Story()
    {
        int num = story.texts_To_Sprite[storyNum];
        if(sprNum != num)
            image.sprite = story.sprites[num];

        typingText.SetData(transform, story.texts[storyNum]);
    }

    public void Next()
    {
        StoryNum++;
    }

    public void End()
    {
        Destroy(gameObject);
    }
}