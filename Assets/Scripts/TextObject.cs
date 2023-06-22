using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject
{
    public float time;
    public string text;
    public string sender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TextObject(float t, string tx, string s)
    {
        time = t;
        text = tx;
        sender = s;
    }
}
