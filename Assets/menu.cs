using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        startingVolume = music.volume;
    }

    float startingVolume;
    [SerializeField] AudioSource music;
    [SerializeField] RawImage black;
    bool startingScene = false;
    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !startingScene)
        {
            startingScene = true;
            Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, Color.black, 1f);
        }

        if (startingScene)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(startingVolume, 0, timer/2);

            if(timer > 2)
            {
                SceneManager.LoadScene("road");
            }
        }
    }
}
