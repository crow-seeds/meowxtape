using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roadmap : MonoBehaviour
{
    [SerializeField] List<GameObject> catFaces;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCatFace(int c)
    {
        for(int i = 0; i < catFaces.Count; i++)
        {
            catFaces[i].SetActive(false);
            if(i == c)
            {
                catFaces[i].SetActive(true);
            }
        }
    }
}
