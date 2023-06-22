using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour
{
    public TextMeshProUGUI textObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveUp(float y)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(GetComponent<RectTransform>(), new Vector3(GetComponent<RectTransform>().localPosition.x, GetComponent<RectTransform>().localPosition.y + y, 0), 1, false);
    }
}
