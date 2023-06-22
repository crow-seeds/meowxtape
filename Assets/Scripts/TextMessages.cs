using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Globalization;
using System.IO;
using System;

public class TextMessages : MonoBehaviour
{
    int currentIndex = 0;
    
    List<TextObject> textsList = new List<TextObject>();
    IFormatProvider format = new CultureInfo("en-US");

    List<TextMessage> textMessageObjectList = new List<TextMessage>();

    float timer = 0;
    bool canShowTexts = false;

    [SerializeField] AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex < textsList.Count && canShowTexts)
        {
            timer += Time.deltaTime;

            if (textsList[currentIndex].time < 0)
            {
                currentIndex++;
            }
            else if (timer > textsList[currentIndex].time)
            {
                float m = 90;
                if(textsList[currentIndex].sender == "them")
                {
                    TextMessage t = Instantiate<GameObject>(Resources.Load<GameObject>("Texts/Their Text"), new Vector3(13,-250, 0), Quaternion.identity, GetComponent<RectTransform>()).GetComponent<TextMessage>();
                    t.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(30, -250, 0);
                    t.textObject.text = textsList[currentIndex].text;
                    textMessageObjectList.Add(t);
                    m = 75;
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/message"), 0.8f);
                }
                else if(textsList[currentIndex].sender == "you")
                {
                    TextMessage t = Instantiate<GameObject>(Resources.Load<GameObject>("Texts/Your Text"), new Vector3(221, -230, 0), Quaternion.identity, GetComponent<RectTransform>()).GetComponent<TextMessage>();
                    t.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-17, -230, 0);
                    t.textObject.text = textsList[currentIndex].text;
                    textMessageObjectList.Add(t);
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/sent"), 0.8f);
                }
                moveTextsUp(m);
                currentIndex++;
            }
        }
    }

    [SerializeField] TextAsset subtitleData;

    void moveTextsUp(float y)
    {
        for(int i = 0; i < textMessageObjectList.Count; i++)
        {
            if(textMessageObjectList[i] == null)
            {
                textMessageObjectList.RemoveAt(i);
                i--;
            }
            else
            {
                textMessageObjectList[i].moveUp(y);
            }
        }
    }

    public void loadSubtitles(int level)
    {
        textsList.Clear();
        currentIndex = 0;
        subtitleData = Resources.Load<TextAsset>("Data/texts_" + level.ToString());

        string data = subtitleData.text;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(data));

        XmlNodeList dialogueNodeList = xmlDoc.SelectNodes("//data/text");

        foreach (XmlNode infonode in dialogueNodeList)
        {
            TextObject t = new TextObject((float)Convert.ToDouble(infonode.Attributes["time"].Value, format), infonode.Attributes["text"].Value.Replace("\\n", "\n"), infonode.Attributes["sender"].Value);
            textsList.Add(t);
        }

        textsList.Sort(delegate (TextObject x, TextObject y) {
            return x.time.CompareTo(y.time);
        });

        canShowTexts = true;
    }

    public void clearMessages()
    {
        for(int i = textMessageObjectList.Count - 1; i >= 0; i--)
        {
            if(textMessageObjectList[i] != null)
            {
                Destroy(textMessageObjectList[i].gameObject);
                textMessageObjectList.RemoveAt(i);
            }
        }
        canShowTexts = false;
        timer = 0;

    }
}
