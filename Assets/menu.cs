using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("intro_mode", 0) == 1)
        {
            black.color = Color.black;
        }else if(PlayerPrefs.GetInt("intro_mode", 0) == 2)
        {
            black.color = Color.white;
        }

        music.volume = PlayerPrefs.GetFloat("volume_music", 0.7f);
        sfx.volume = PlayerPrefs.GetFloat("volume_sfx", 0.5f);
        voice.volume = PlayerPrefs.GetFloat("volume_voice", 0.7f);

        startingVolume = PlayerPrefs.GetFloat("volume_music", 0.7f);

        settingsTexts[0].text = ">Music Volume  < " + (Mathf.RoundToInt(startingVolume * 10)).ToString() + "/ 10 >";
        settingsTexts[1].text = "SFX Volume  < " + (Mathf.RoundToInt(sfx.volume * 10)).ToString() + "/ 10 >";
        settingsTexts[2].text = "Dialogue Volume  < " + (Mathf.RoundToInt(voice.volume * 10)).ToString() + "/ 10 >";

        if(PlayerPrefs.GetInt("achievement3", 0) == 0)
        {
            optionTexts[0].text = ">Normal Mode";
            optionTexts[1].rectTransform.localPosition = new Vector3(optionTexts[1].rectTransform.localPosition.x, optionTexts[1].rectTransform.localPosition.y - 200, optionTexts[1].rectTransform.localPosition.z);
            optionTexts[2].rectTransform.localPosition = new Vector3(optionTexts[2].rectTransform.localPosition.x, optionTexts[2].rectTransform.localPosition.y - 200, optionTexts[2].rectTransform.localPosition.z);
            optionTexts[3].rectTransform.localPosition = new Vector3(optionTexts[3].rectTransform.localPosition.x, optionTexts[3].rectTransform.localPosition.y - 200, optionTexts[3].rectTransform.localPosition.z);
            expertMode.gameObject.SetActive(true);
            endlessMode.gameObject.SetActive(true);
            optionTexts.Insert(1, expertMode);
            optionTexts.Insert(2, endlessMode);
            indexOfCredits += 2;
            indexOfTracks += 2;
            indexOfSettings += 2;
            indexOfExpert = 1;
            indexOfEndless = 2;
        }

        StartCoroutine(intro());
    }

    float startingVolume;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource voice;
    [SerializeField] RawImage black;
    bool startingScene = false;
    float timer = 0;

    [SerializeField] List<TextMeshProUGUI> optionTexts;
    [SerializeField] TextMeshProUGUI expertMode;
    [SerializeField] TextMeshProUGUI endlessMode;
    [SerializeField] List<TextMeshProUGUI> settingsTexts;
    [SerializeField] List<TextMeshProUGUI> trackTexts;
    
    int optionIndex = 0;
    int indexOfSettings = 1;
    int indexOfTracks = 2;
    int indexOfCredits = 3;
    int indexOfExpert = -1;
    int indexOfEndless = -1;
    int pageOfMenu = 0;
    int trackIndex = 0;

    [SerializeField] List<AudioClip> tracks; 
    [SerializeField] List<string> trackNames; 

    [SerializeField] RectTransform menuPage1; 
    [SerializeField] RectTransform settingsPage1; 
    [SerializeField] RectTransform tracksPage1; 
    [SerializeField] RectTransform creditsPage1; 



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pageOfMenu == 0)
            {
                if (!startingScene && optionIndex == 0)
                {
                    if(FindObjectOfType<VolumeFader>())
                    {
                        Destroy(FindObjectOfType<VolumeFader>().gameObject);
                    }
                    


                    PlayerPrefs.SetInt("mode", 0);
                    startingScene = true;
                    startingVolume = music.volume;
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, Color.black, 1f);
                }
                else if (optionIndex == indexOfSettings)
                {
                    pageOfMenu = 1;
                    optionIndex = 0;

                    if(settingsTexts[4].color.r == 1)
                    {
                        settingsTexts[4].color = Color.black;
                        settingsTexts[4].text = "Back to Menu";
                        settingsTexts[0].color = Color.red;
                        settingsTexts[0].text = ">Music Volume  < " + (Mathf.RoundToInt(music.volume * 10)).ToString() + "/ 10 >";
                    }
                    
                    StartCoroutine(showSettings());
                }else if(optionIndex == indexOfTracks)
                {
                    pageOfMenu = 2;
                    optionIndex = 0;

                    if (trackTexts[1].color.r == 1)
                    {
                        trackTexts[1].color = Color.black;
                        trackTexts[1].text = "Back to Menu";
                        trackTexts[0].color = Color.red;
                        if (PlayerPrefs.GetInt("song_" + trackIndex.ToString(), 0) == 1 || trackIndex == 0)
                        {
                            trackTexts[0].text = ">Playing [" + trackIndex.ToString() + ". " + trackNames[trackIndex] + "]";
                        }
                        else
                        {
                            trackTexts[0].text = ">Playing [" + trackIndex.ToString() + ". Locked Song]";
                        }
                    }

                    StartCoroutine(showTracks());
                }
                else if (optionIndex == indexOfCredits)
                {
                    pageOfMenu = 3;
                    optionIndex = 0;

                    StartCoroutine(showCredits());
                }else if(optionIndex == indexOfExpert)
                {
                    if (FindObjectOfType<VolumeFader>())
                    {
                        Destroy(FindObjectOfType<VolumeFader>().gameObject);
                    }
                    PlayerPrefs.SetInt("mode", 1);
                    startingScene = true;
                    startingVolume = music.volume;
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, Color.black, 1f);
                }else if(optionIndex == indexOfEndless)
                {
                    if (FindObjectOfType<VolumeFader>())
                    {
                        Destroy(FindObjectOfType<VolumeFader>().gameObject);
                    }
                    PlayerPrefs.SetInt("mode", 2);
                    startingScene = true;
                    startingVolume = music.volume;
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, Color.black, 1f);
                }
            }
            else if(pageOfMenu == 1)
            {
                if(optionIndex == 3)
                {
                    Screen.fullScreen = !Screen.fullScreen;
                }else if(optionIndex == 4)
                {
                    pageOfMenu = 0;
                    optionIndex = indexOfSettings;

                    StartCoroutine(leaveSettings());
                }
            }else if(pageOfMenu == 2)
            {
                if (optionIndex == 1)
                {
                    pageOfMenu = 0;
                    optionIndex = indexOfTracks;

                    StartCoroutine(leaveTracks());
                }
            }else if(pageOfMenu == 3)
            {
                pageOfMenu = 0;
                optionIndex = indexOfCredits;
                StartCoroutine(leaveCredits());
            }
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

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            changeSelection(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            changeSelection(false);
        }else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            changeSelection2(false);
        }else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            changeSelection2(true);
        }

        if (Screen.fullScreen)
        {
            settingsTexts[3].text = "Fullscreen: On";
            if(optionIndex == 3)
            {
                settingsTexts[3].text = ">Fullscreen: On";
            }
        }
        else
        {
            settingsTexts[3].text = "Fullscreen: Off";
            if (optionIndex == 3)
            {
                settingsTexts[3].text = ">Fullscreen: Off";
            }
        }

    }

    void changeSelection2(bool forward)
    {
        switch (pageOfMenu)
        {
            case 1:
                switch (optionIndex)
                {
                    case 0:
                        if (forward)
                        {
                            music.volume += .1f;
                        }
                        else
                        {
                            music.volume -= .1f;
                        }
                        PlayerPrefs.SetFloat("volume_music", music.volume);
                        settingsTexts[0].text = ">Music Volume  < " + (Mathf.RoundToInt(music.volume*10)).ToString() + "/ 10 >";
                        break;
                    case 1:
                        if (forward)
                        {
                            sfx.volume += .1f;
                        }
                        else
                        {
                            sfx.volume -= .1f;
                        }
                        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/hurt"), 0.7f);
                        PlayerPrefs.SetFloat("volume_sfx", sfx.volume);
                        settingsTexts[1].text = ">SFX Volume  < " + (Mathf.RoundToInt(sfx.volume * 10)).ToString() + "/ 10 >";
                        break;
                    case 2:
                        if (forward)
                        {
                            voice.volume += .1f;
                        }
                        else
                        {
                            voice.volume -= .1f;
                        }
                        voice.PlayOneShot(Resources.Load<AudioClip>("Sounds/Dialogue/0"), 1f);
                        PlayerPrefs.SetFloat("volume_voice", voice.volume);
                        settingsTexts[2].text = ">Dialogue Volume  < " + (Mathf.RoundToInt(voice.volume * 10)).ToString() + "/ 10 >";
                        break;
                    case 3:
                        Screen.fullScreen = !Screen.fullScreen;
                        break;
                }
                break;
            case 2:
                if(optionIndex == 0)
                {
                    if (forward)
                    {
                        trackIndex++;
                    }
                    else
                    {
                        trackIndex--;
                    }

                    if(trackIndex < 0)
                    {
                        trackIndex = tracks.Count - 1;
                    }
                    if(trackIndex >= tracks.Count)
                    {
                        trackIndex = 0;
                    }

                    if(PlayerPrefs.GetInt("song_" + trackIndex.ToString(), 0) == 1 || trackIndex == 0)
                    {
                        if(music.clip.name != tracks[trackIndex].name)
                        {
                            music.clip = tracks[trackIndex];
                            music.time = 0;
                            music.Play();
                        }
                        
                        trackTexts[0].text = ">Playing [" + trackIndex.ToString() + ". " + trackNames[trackIndex] + "]";
                    }
                    else
                    {
                        trackTexts[0].text = ">Playing [" + trackIndex.ToString() + ". Locked Song]";
                    }
                    
                }
                break;
        }
    }

    void changeSelection(bool forward)
    {
        if (pageOfMenu == 3)
        {
            return;
        }


        List<TextMeshProUGUI> list = optionTexts;
        switch (pageOfMenu)
        {
            case 0:
                list = optionTexts;
                break;
            case 1:
                list = settingsTexts;
                break;
            case 2:
                list = trackTexts;
                break;
        }

        list[optionIndex].color = Color.black;
        list[optionIndex].text = list[optionIndex].text.Substring(1);

        if (forward)
        {
            optionIndex++;
        }
        else
        {
            optionIndex--;
        }
        
        if (optionIndex >= list.Count)
        {
            optionIndex = 0;
        }

        if (optionIndex < 0)
        {
            optionIndex = list.Count - 1;
        }

        list[optionIndex].color = Color.red;
        list[optionIndex].text = ">" + list[optionIndex].text;
    }

    IEnumerator showSettings()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(settingsPage1, new Vector2(0, 0), 0.5f, true);
    }

    IEnumerator showTracks()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(tracksPage1, new Vector2(0, 0), 0.5f, true);
    }

    IEnumerator showCredits()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(creditsPage1, new Vector2(0, 0), 0.5f, true);
    }

    IEnumerator leaveCredits()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(creditsPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
    }

    IEnumerator leaveSettings()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(settingsPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
    }

    IEnumerator leaveTracks()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(tracksPage1, new Vector2(0, -700), 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
    }


    IEnumerator intro()
    {
        yield return new WaitForSeconds(0.25f);

        switch (PlayerPrefs.GetInt("intro_mode", 0))
        {
            case 0:
                Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
                //Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(music, startingVolume, 5f);
                break;
            case 1:
                Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, new Color(0,0,0,0), 2f);
                yield return new WaitForSeconds(0.5f);
                Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
                break;
            case 2:
                Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(black, new Color(1, 1, 1, 0), 2f);
                yield return new WaitForSeconds(0.5f);
                Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(menuPage1, new Vector2(0, 0), 0.5f, true);
                break;
        }

        yield return new WaitForSeconds(0);
    }
}
