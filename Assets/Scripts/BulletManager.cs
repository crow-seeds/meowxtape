using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    // Start is called before the first frame update

    float score = 0;
    int timesHit = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] AudioSource sfx;
    [SerializeField] RoadBackground roadBackground;
    [SerializeField] RectTransform catFaceProgress;

    int level = 0;
    bool hardMode = false;
    bool endlessMode = false;

    float ratePothole = 1;
    float rateCar = 1;
    float aimCarChance = 0;
    float spiralClusterChance = 0;
    float rateCluster = 1;
    float rateMoney = 1;

    float totalTime;
    float timer;

    [SerializeField] Image progressBar;
    [SerializeField] TextMessages chat;

    int health = 4;
    int healthCap = 4;

    List<string> dialogue;
    int dialoguePhase = 0;
    int dialogueNum = 0;

    [SerializeField] TextMeshProUGUI moneyTextUI;
    [SerializeField] RectTransform moneyIndicator;
    [SerializeField] RectTransform upgradeIndicator;

    [SerializeField] Movement player;

    [SerializeField] RectTransform cityBackground;

    [SerializeField] Image catWindow;
    [SerializeField] Image backgroundImage;

    int windowState = 0;

    float spaceBetweenSpaces = 0;
    float timeHeld = 0;

    [SerializeField] int debugLevel;

    [SerializeField] List<Color> UI_colors;
    [SerializeField] List<Color> road_colors;

    [SerializeField] SpriteRenderer road0;
    [SerializeField] SpriteRenderer road1;
    [SerializeField] Image UI_background;
    [SerializeField] Image cat_background;
    [SerializeField] Image progressBackground;

    achivementHandler aHandler;

    List<int> playlistSong;
    List<int> playlistTexts;
    [SerializeField] int amountOfEndlessTexts;

    void Start()
    {
        //StartCoroutine(clusterLoop());
        //StartCoroutine(carLoop());

        aHandler = FindObjectOfType<achivementHandler>();

        if(PlayerPrefs.GetInt("mode", 0) == 1)
        {
            hardMode = true;
            cityNameDialogue.text = "Evil Mew York";
            cityNameDriving.text = "Evil Mew York";
            cityNameDriving.fontSize = 64;
        }else if(PlayerPrefs.GetInt("mode", 0) == 2)
        {
            endlessMode = true;
            cityNameDialogue.text = "100 mi";
            cityNameDriving.text = "100 mi";

            backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + Random.Range(0, 4));
            backgroundImage.color = Color.HSVToRGB(Random.Range(0, 360) / 360f, .4f, 1);

            roadmap.setCatFace(Random.Range(0, 8));
            float randHue = Random.Range(0, 360)/360f;
            Color roadColor = Color.HSVToRGB(randHue, .30f, 1f);
            Color UIColor = Color.HSVToRGB(randHue, .45f, .83f);
            road0.color = roadColor;
            road1.color = roadColor;
            progressBackground.color = UIColor;
            UI_background.color = UIColor;
            cat_background.color = UIColor;
            playlistSong = new List<int>();
            playlistTexts = new List<int>();


            for(int i = 0; i < endlessModeSongs.Count; i++)
            {
                playlistSong.Add(i);
            }
            Shuffle(playlistSong);

            for (int i = 0; i < amountOfEndlessTexts; i++)
            {
                playlistTexts.Add(i);
            }

            Shuffle(playlistTexts);
        }

        sfx.volume = PlayerPrefs.GetFloat("volume_sfx", 0.5f);
        dialogueAudio.volume = PlayerPrefs.GetFloat("volume_voice", 0.7f);
        listOfUpgrades = new List<string>(upgrades.Keys);
        moneyTextUI.text = "$" + money.ToString();
        moneyText.text = "Money: " + "$" + money.ToString();
        dialogue = new List<string>()
        {
            "&Hey, I'm walkin here!",
            "Wait.. are you the guy?",
            "The disgraced rapper, what's it called... uh...",
            "..uh...Used Tissue?",
            "Yea I can see why you's escaping to the west coast.",
            "What's that?",
            "You got a new mixtape huh?",
            "You have my blessing kid, I believe in you...",
            "..bringing that hot garbage away from this east coast.",
            "Don't even try to make me listen to that garbage!",
            "Now scram!!",
            "(Boos from Mew Yorkers)",
            "",
            "I've been hearing rumors about Fat Cat. Saying he's gonna get my tracks.",
            "Imagine that. He's got no rhymes, just a fancy hat.",
            "Let me tell you something 'bout that dude.",
            "The only beat he hasn't copyrighted is a metronome. It's true.",
            "!(He busts a rhyme)",
            "|Buy my merch.",
            "",
            "Well, hey there! I reckon you're that uh... Used Tissue fella making that new-fangled rap music.",
            "Ah, it's a shame. Everyone person who's ever come up with a heat mixtape like yours ends up dead within a week, cause of that Fat Cat  fella.",
            "Nya I reckon, the reason why he's after you is cause your mixtape is a bit different from the rest.",
            "It might be... the best mixtape that I've ever heard. And that's saying a lot since I listen to nothing but country music.",
            "Well, while you're getting hunted, do you mind if I interest you in some of my wares? I got some yarn..",
            "You can diddle, you can fiddle it, and you can uh... tie it around!",
            "And uhh.. if you want.. I got some catnip.. got a little catnip here.. you know..",
            "|it's perfectly legal.. I promise..",
            "",
            "Hey! Hey you!! Come over here!!",
            "Could I interest you in any of my wares?",
            "I got the finest in the area, sourced ethically, high quality, bits and bobs of all sorts to help you in your journey.",
            "Loads of options, I mean I got thinga-majigs, whatcha-macallem's, just take a look and let me know if you're interested.",
            "Say you look like one of those rapper types, is your producer Fat Cat?",
            "Ohh you better watch out, let me tell you something, he's gonna try to kill you and steal your mixtape.",
            "Mm? It's happened before, I don't wanna talk about it, it's kind of a touchy subject.",
            "Please.. please.. would you wanna buy any of my wares. I'm a single cat!",
            "I've got six kittens at home, I'm just trying to support them, you know..",
            "I get a lotta you rappers coming in here and looking at the stuff, but you don't buy anything and I gotta go home empty handed... I...",
            "It's just been really hard for me... just..",
            "|*sniff* Let me know if you want anything..",
            "",
            "Oh my god... you killed him!",
            "Don't ask why I'm in Los Angelhiss now.",
            "",
            "Well I'll be a hairball covered in catnip.",
            "You done killed Fat Cat!",
            "Congratulations!",
            "You saved the economy, the city, my wife Alison,",
            "My sweet son Billy Bob, his cousin Drayvon,",
            "And of course, me.",
            "",

        };

        if (!endlessMode)
        {
            if (debugLevel == 0)
            {
                StartCoroutine(intro());
            }
            else
            {
                Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(0, 0, 0, 0), 0.1f);
                level = debugLevel;
                cityNameDialogue.text = cityNames[level];
                cityNameDriving.text = cityNames[level];
                road0.color = road_colors[level];
                road1.color = road_colors[level];
                progressBackground.color = UI_colors[level];
                UI_background.color = UI_colors[level];
                cat_background.color = UI_colors[level];
                dialoguePhase = debugLevel;

                dialoguePhase = level+1;

                int emptyCount = 0;
                int tempInd = 0;
                foreach(string s in dialogue)
                {
                    if(s == "")
                    {
                        emptyCount++;
                    }

                    tempInd++;
                    if(emptyCount == dialoguePhase)
                    {
                        break;
                    }
                }

                dialogue.RemoveRange(0, tempInd);
                dialogueNum = tempInd - emptyCount;

                StartCoroutine(nextLevel());
            }
        }
        else
        {
            if(debugLevel == 0)
            {
                StartCoroutine(intro());
            }
            else
            {
                Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(0, 0, 0, 0), 0.1f);
                level = debugLevel;
                cityNameDialogue.text = ((level + 1) * 100).ToString() + " mi";
                cityNameDriving.text = ((level + 1) * 100).ToString() + " mi";
                bulletSpeedModifier = .6f;
                StartCoroutine(nextLevel());
            }
        }

        


    }

    bool drivingMode = false;
    [SerializeField] TextMeshProUGUI upgradeToolTip;

    bool goingToRetry = true;

    [SerializeField] TextMeshProUGUI retryText;
    [SerializeField] TextMeshProUGUI quitText;

    // Update is called once per frame
    void Update()
    {
        if (onGameOverScreen)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Gamepad.current != null && Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.down.ReadValue() > 0.8f || Gamepad.current != null && Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.up.ReadValue() > 0.8f)
            {
                goingToRetry = !goingToRetry;
                if (goingToRetry)
                {
                    retryText.color = Color.red;
                    retryText.text = ">" + retryText.text;
                    quitText.color = Color.white;
                    quitText.text = quitText.text.Substring(1);
                }
                else
                {
                    quitText.color = Color.red;
                    quitText.text = ">" + quitText.text;
                    retryText.color = Color.white;
                    retryText.text = retryText.text.Substring(1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                if (goingToRetry)
                {
                    StartCoroutine(retry());
                }
                else
                {
                    StartCoroutine(quit());
                }
            }
        }



        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("get up");
            timeHeld = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (drivingMode && !onGameOverScreen)
        {
            score += (Time.deltaTime * 25);
            scoreText.text = "Score: " + ((int)score).ToString();

            timer += Time.deltaTime;

            progressBar.fillAmount = timer / totalTime;
            catFaceProgress.localPosition = new Vector3(747, -375 + (int)((timer / totalTime) * 775f), 0);
            cityBackground.localPosition = new Vector3(316f - progressBar.fillAmount * 614f, cityBackground.localPosition.y, cityBackground.localPosition.z);


            if (timer > totalTime)
            {
                StopAllCoroutines();

                StartCoroutine(nextLevel());
                
                drivingMode = false;
            }

            screamTimer += Time.deltaTime;
        }
        else
        {
            

            if (onShop)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Gamepad.current != null && Gamepad.current.dpad.left.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.left.ReadValue() > 0.8f || Gamepad.current != null && Gamepad.current.dpad.right.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.right.ReadValue() > 0.8f)
                {
                    choiceTexts[choiceNum].text = choiceTexts[choiceNum].text.Substring(1);

                    if (choiceTexts[choiceNum].color.r == 1)
                    {
                        choiceTexts[choiceNum].color = Color.white;
                    }
                    else
                    {
                        choiceTexts[choiceNum].color = new Color(.5f, .5f, .5f, 1);
                    }

                    if (choiceNum % 2 == 0)
                    {
                        choiceNum++;
                    }
                    else
                    {
                        choiceNum--;
                    }

                    choiceTexts[choiceNum].text = ">" + choiceTexts[choiceNum].text;
                    if (choiceNum < 2 && !buyDisabled[choiceNum])
                    {
                        upgradeToolTip.text = upgradeDescriptions[upgrades[upgradeString[choiceNum]]];
                    }else if(choiceNum == 2)
                    {
                        upgradeToolTip.text = "Get new items for sale";
                    }
                    else
                    {
                        upgradeToolTip.text = "";
                    }

                    if (choiceTexts[choiceNum].color.r == 1)
                    {
                        choiceTexts[choiceNum].color = Color.red;
                    }
                    else
                    {
                        choiceTexts[choiceNum].color = new Color(.5f, 0, 0, 1);
                    }
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Gamepad.current != null && Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.down.ReadValue() > 0.8f || Gamepad.current != null && Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.leftStick.up.ReadValue() > 0.8f)
                {
                    choiceTexts[choiceNum].text = choiceTexts[choiceNum].text.Substring(1);

                    if (choiceTexts[choiceNum].color.r == 1)
                    {
                        choiceTexts[choiceNum].color = Color.white;
                    }
                    else
                    {
                        choiceTexts[choiceNum].color = new Color(.5f, .5f, .5f, 1);
                    }

                    choiceNum = (choiceNum + 2) % 4;
                    choiceTexts[choiceNum].text = ">" + choiceTexts[choiceNum].text;

                    if (choiceNum < 2 && !buyDisabled[choiceNum])
                    {
                        upgradeToolTip.text = upgradeDescriptions[upgrades[upgradeString[choiceNum]]];
                    }
                    else if (choiceNum == 2)
                    {
                        upgradeToolTip.text = "Get new items for sale";
                    }
                    else
                    {
                        upgradeToolTip.text = "";
                    }

                    if (choiceTexts[choiceNum].color.r == 1)
                    {
                        choiceTexts[choiceNum].color = Color.red;
                    }
                    else
                    {
                        choiceTexts[choiceNum].color = new Color(.5f, 0, 0, 1);
                    }
                }
            }
            

            if (!isMoving && !onGameOverScreen)
            {
                spaceBetweenSpaces += Time.deltaTime;

                if (Input.GetKey(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonSouth.isPressed || Gamepad.current != null && Gamepad.current.buttonEast.isPressed)
                {
                    timeHeld += Time.deltaTime;
                }
                else
                {
                    timeHeld = 0;
                }

                if ((Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) || timeHeld > 1)
                {
                    
                    if (!onShop || choiceNum == 3)
                    {
                        if (endlessMode)
                        {
                            endDialoguePhase();
                            musicTown.mute = true;
                            shopContainer.SetActive(false);
                            onShop = false;
                            musicTown.mute = false;
                        }
                        else
                        {
                            nextDialogue();
                        }
                        
                    }
                    else
                    {
                        if(timeHeld <= 1)
                        {
                            buyItem();
                        }
                    }

                    spaceBetweenSpaces = 0;
                }

                
            }



        }

    }


    [SerializeField] AudioSource dialogueAudio;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueNameText;

    float bulletSpeedModifier = 1;

    int choiceNum = 0;

    public void beginDialoguePhase()
    {
        if (!endlessMode)
        {
            switch (dialoguePhase)
            {
                case 0:
                    StartCoroutine(showDialogueCo());
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat0");
                    dialogueNameText.text = "Bubba";
                    nextDialogue();
                    break;
                case 1:
                    //StartCoroutine(showDialogueCo());
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat1");
                    dialogueNameText.text = "City Cat";
                    nextDialogue();
                    break;
                case 2:
                    //StartCoroutine(showDialogueCo());
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat2");
                    dialogueNameText.text = "Jack";
                    nextDialogue();
                    break;
                case 3:
                    //StartCoroutine(showDialogueCo());
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat3");
                    dialogueNameText.text = "Catrina";
                    nextDialogue();
                    break;
                case 4:
                    StartCoroutine(showDialogueCo());
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat0");
                    dialogueNameText.text = "Bubba";
                    nextDialogue();
                    break;
                case 5:
                    dialogueNameText.text = "Jack";
                    nextDialogue();
                    break;
            }
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat0");
                    dialogueNameText.text = "Bubba";
                    playDialogueClip("shop0");
                    dialogueText.text = "Ayy, I'm sellin here!";
                    break;
                case 1:
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat1");
                    dialogueNameText.text = "City Cat";
                    playDialogueClip("shop1");
                    dialogueText.text = "Buy My Merch.";
                    break;
                case 2:
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat2");
                    dialogueNameText.text = "Jack";
                    playDialogueClip("shop2");
                    dialogueText.text = "It's perfectly legal.. I promise..";
                    break;
                case 3:
                    creature.sprite = Resources.Load<Sprite>("Sprites/cat3");
                    dialogueNameText.text = "Catrina";
                    playDialogueClip("shop3");
                    dialogueText.text = "*sniff* Let me know if you want anything..";
                    break;

            }

            
            shopContainer.SetActive(true);
            onShop = true;
            setUpShop();
        }
        

    }

    public void beginDialoguePhaseEndless()
    {
        switch (dialoguePhase)
        {
            case 0:
                StartCoroutine(showDialogueCo());
                dialogueNameText.text = "Bubba";
                dialogueText.text = "Hey I'm walkin in an endless time loop.";
                playDialogueClip("endless0");
                break;
            default:


                break;
        }

    }




    bool onShop = false;
    [SerializeField] GameObject shopContainer;

    public void nextDialogue()
    {
        if(dialogue.Count == 0)
        {
            return;
        }

        if (endlessMode)
        {
            endDialoguePhase();
            return;
        }

        if (dialogue[0] == "")
        {
            dialogue.RemoveAt(0);
            endDialoguePhase();
            return;
        }

        Debug.Log(dialogueNum.ToString());
        playDialogueClip(dialogueNum.ToString());

        if(dialogue[0][0] == '&')
        {
            if(PlayerPrefs.GetInt("seen_intro", 0) == 0)
            {
                dialogueText.text = dialogue[0].Substring(1) + "\n\n\n[Space] for next dialogue";
                PlayerPrefs.SetInt("seen_intro", 1);
            }
            else
            {
                dialogueText.text = dialogue[0].Substring(1) + "\n\n\nHold [Space] to skip all dialogue";
            }
            
        }
        else if(dialogue[0][0] == '|')
        {
            dialogueText.text = dialogue[0].Substring(1);
            shopContainer.SetActive(true);
            onShop = true;
            setUpShop();
            musicTown.mute = false;
        }
        else if(dialogue[0][0] == '!')
        {
            musicTown.mute = true;
            dialogueText.text = dialogue[0].Substring(1);
            shopContainer.SetActive(false);
            onShop = false;
        }
        else
        {
            musicTown.mute = false;
            shopContainer.SetActive(false);
            onShop = false;
            dialogueText.text = dialogue[0];
        }
        dialogueNum++;
        dialogue.RemoveAt(0);
    }

    Dictionary<string, string> upgrades = new Dictionary<string, string>{
        {"Stronger Doors", "health" },
        {"Thicker Fur", "health" },
        {"Scratch-Proof Paint", "health" },
        {"Yarn Cover", "health" },
        {"Cone Collar", "health" },
        {"Meowphine", "enemyspeed" },
        {"Catamine", "enemyspeed" },
        {"Anti-Hisstamines", "enemyspeed" },
        {"Catnip", "enemyspeed" },
        {"Crystal Meowth", "enemyspeed" },
        {"Furr Efficient Battery", "speed" },
        {"Overclawked Pistons", "speed" },
        {"1,000 Horsepawer Engine", "speed" },
        {"Hamster Wheel", "speed" },
        {"Laser Pointer", "speed" },
        {"Milk Coolant", "cooldown" },
        {"Radiator Bed", "cooldown" },
        {"Air Conditioner Repair", "cooldown" },
        {"Nap Pillow", "cooldown" },
        {"Sitting Box", "cooldown" },
        {"Tiny Box", "size" },
        {"Kitten Essence", "size" },
        {"Tight Collar", "size" }
    };

    Dictionary<string, string> upgradeDescriptions = new Dictionary<string, string>
    {
        {"health", "Car Health Up!" },
        {"cooldown", "Faster Dash Cooldown!" },
        {"speed", "Car Speed Up!" },
        {"enemyspeed", "Slower Enemies!" },
        {"size", "Smaller Car!" },
    };

    List<string> listOfUpgrades;
    string[] upgradeString = new string[2];
    int[] cost = new int[3];
    bool[] buyDisabled = new bool[3];


    void setUpShop()
    {
        upgradeString[0] = listOfUpgrades[Random.Range(0, listOfUpgrades.Count)];
        upgradeString[1] = listOfUpgrades[Random.Range(0, listOfUpgrades.Count)];
        moneyTextUI.text = "$" + money.ToString();

        if(upgradeString[0] == upgradeString[1])
        {
            upgradeString[1] = listOfUpgrades[Random.Range(0, listOfUpgrades.Count)];
        }

        cost[0] = Random.Range(200, 500);
        cost[1] = Random.Range(200, 500);
        cost[2] = Random.Range(50, 200);
        buyDisabled = new bool[3];

        foreach(TextMeshProUGUI t in choiceTexts)
        {
            t.color = new Color(1f, 1f, 1f, 1);
        }

        choiceNum = 0;
        choiceTexts[0].text = ">" + upgradeString[0] + " ($" + cost[0].ToString() + ")";
        upgradeToolTip.text = upgradeDescriptions[upgrades[upgradeString[choiceNum]]];
        choiceTexts[0].color = Color.red;
        choiceTexts[1].text = upgradeString[1] + " ($" + cost[1].ToString() + ")";
        choiceTexts[2].text = "Restock Items " + " ($" + cost[2].ToString() + ")";
        choiceTexts[3].text = "Finish Buying";
    }


    [SerializeField] TextMeshProUGUI upgradeDescText;
    void buyItem()
    {
        if(money >= cost[choiceNum] && !buyDisabled[choiceNum])
        {
            if(choiceNum != 2)
            {
                buyDisabled[choiceNum] = true;
                choiceTexts[choiceNum].color = new Color(.5f, .0f, .0f, 1);
                choiceTexts[choiceNum].text = ">Sold Out :(";
                sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cash"), 0.8f);
            }
            money -= cost[choiceNum];

            moneyText.text = "Money: $" + money.ToString();
            moneyTextUI.text = "$" + money.ToString();

            if (choiceNum == 2)
            {
                setUpShop();
            }
            else
            {
                switch (upgrades[upgradeString[choiceNum]])
                {
                    case "health":
                        healthCap++;
                        healthCap = Mathf.Min(12, healthCap);
                        break;
                    case "speed":
                        if (!endlessMode)
                        {
                            player.setSpeed(player.getSpeed() + 0.8f);
                        }
                        else
                        {
                            player.setSpeed(player.getSpeed() + 0.5f);
                        }
                        
                        break;
                    case "cooldown":
                        if (!endlessMode)
                        {
                            player.dashCooldownTime -= .25f;
                        }
                        else
                        {
                            player.dashCooldownTime -= .2f;
                        }
                        
                        player.dashCooldownTime = Mathf.Max(player.dashCooldownTime, 0.5f);
                        break;
                    case "enemyspeed":
                        if (!endlessMode)
                        {
                            bulletSpeedModifier *= 0.92f;
                        }
                        else
                        {
                            bulletSpeedModifier *= 0.95f;
                        }
                        
                        bulletSpeedModifier = Mathf.Max(bulletSpeedModifier, 0.6f);
                        break;
                    case "size":
                        if (!endlessMode)
                        {
                            player.transform.localScale = new Vector3(player.transform.localScale.x * 0.87f, player.transform.localScale.y * 0.87f, player.transform.localScale.z * 0.87f);
                        }
                        else
                        {
                            player.transform.localScale = new Vector3(player.transform.localScale.x * 0.94f, player.transform.localScale.y * 0.94f, player.transform.localScale.z * 0.94f);
                        }
                        
                        if(player.transform.localScale.x < 0.3f)
                        {
                            player.transform.localScale = new Vector3(.3f, .3f, .3f);
                        }
                        break;
                }
                upgradeToolTip.text = "";
                upgradeDescText.text = upgradeDescriptions[upgrades[upgradeString[choiceNum]]]; //this is so bad omg time limit
                StartCoroutine(showUpgrade());
            }
        }
        else
        {
            //can't buy
        }

       
    }

    public void endDialoguePhase()
    {
        Debug.Log("ended");
        dialogueAudio.Stop();

        if (endlessMode)
        {
            StartCoroutine(nextLevel());
            return;
        }

        dialoguePhase++;
        switch (dialoguePhase - 1) //cases are which dialogue phase just ended
        {
            case 0:
                StartCoroutine(nextLevel());
                break;
            case 1:
                StartCoroutine(nextLevel());
                break;
            case 2:
                StartCoroutine(nextLevel());
                break;
            case 3:
                StartCoroutine(nextLevel());
                break;
            case 4:
                StartCoroutine(changeCharacterPortrait("Sprites/cat2"));
                beginDialoguePhase();
                break;
            case 5:
                StartCoroutine(winToMenu());
                break;
        }
    }

    public void playDialogueClip(string s)
    {
        dialogueAudio.Stop();
        dialogueAudio.clip = Resources.Load<AudioClip>("Sounds/Dialogue/" + s);
        dialogueAudio.time = 0;
        dialogueAudio.Play();
    }

    IEnumerator clusterLoop()
    {
        yield return new WaitForSeconds(rateCluster);
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/gun"), 0.3f);


        if (Random.Range(0f, 1f) < spiralClusterChance)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), new Vector3(Random.Range(-1.5f, 7f), 12, 0), Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(2.5f * bulletSpeedModifier, 6 + Random.Range(0, 5), true, 45);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), new Vector3(Random.Range(-1.5f, 7f), 12, 0), Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(2f * bulletSpeedModifier, 6 + Random.Range(0, 5), true, 0);       
        }
        
        StartCoroutine(clusterLoop());
    }

    IEnumerator carLoop()
    {
        yield return new WaitForSeconds(rateCar);
        string carType = "regular";
        if (Random.Range(0,1f) < aimCarChance)
        {
            carType = "aim";

            if(Random.Range(0, 1f) < shotgunChance)
            {
                carType = "shotgun";
            }
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.8f);
        }
        else
        {
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.8f);
        }

        if(level != 0 || timer > 5f || hardMode || endlessMode)
        {
            if (carType == "aim" || carType == "shotgun")
            {
                sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.4f);
            }
            else
            {
                if(Random.Range(0,1f) < 0.5f)
                {
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.4f);
                }
                
            }
            Instantiate(Resources.Load<GameObject>("Enemies/Enemy Car"), new Vector3(Random.Range(-1.5f, 2.75f), -6, 0), Quaternion.Euler(0, 0, 0)).GetComponent<EnemyCar>().init(Random.Range(2f, 5f) * bulletSpeedModifier, Random.Range(2,5), carType, bulletSpeedModifier, this);
        }
        
        
        yield return new WaitForSeconds(rateCar);
        carType = "regular";
        if (Random.Range(0, 1f) < aimCarChance)
        {
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.8f);
            carType = "aim";

            if (Random.Range(0, 1f) < shotgunChance)
            {
                carType = "shotgun";
            }
        }
        else
        {
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.8f);
        }


        if (level != 0 || timer > 5f || endlessMode)
        {
            if(carType == "aim" || carType == "shotgun")
            {
                sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.4f);
            }
            else
            {
                if (Random.Range(0, 1f) < 0.5f)
                {
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.4f);
                }
            }
            
            Instantiate(Resources.Load<GameObject>("Enemies/Enemy Car"), new Vector3(Random.Range(2.75f, 7f), -6, 0), Quaternion.Euler(0, 0, 0)).GetComponent<EnemyCar>().init(Random.Range(2f, 5f) * bulletSpeedModifier, Random.Range(2, 5), carType, bulletSpeedModifier, this);
        }

        StartCoroutine(carLoop());
    }

    IEnumerator potLoop()
    {
        yield return new WaitForSeconds(ratePothole);

        if (level != 0 || timer > 7f)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Pothole"), new Vector3(Random.Range(-1.5f, 7f), 7, 1), Quaternion.Euler(0, 0, 0)).GetComponent<Pothole>().init(roadBackground.speed);
        }
        StartCoroutine(potLoop());
    }

    IEnumerator moneyLoop()
    {
        Debug.Log("money!");
        yield return new WaitForSeconds(rateMoney);
        Debug.Log("money!");
        Instantiate(Resources.Load<GameObject>("Enemies/Money"), new Vector3(Random.Range(-1.5f, 7f), 7, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Money>().init(roadBackground.speed);

        StartCoroutine(moneyLoop());
    }

    public void takeDamage()
    {
        score -= 500;
        money -= 100;

        score = Mathf.Max(0, score);
        money = Mathf.Max(0, money);

        moneyText.text = "Money: $" + money.ToString();
        moneyTextUI.text = "$" + money.ToString();

        scoreText.text = "Score: " + score.ToString();

        if(windowState == 0)
        {
            windowState = 1;
            StartCoroutine(windowHurt());
        }


        timesHit++;
        health--;
        setHealth(health);
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/hurt"), 0.7f);

        if(health <= 0)
        {
            //SceneManager.LoadScene("menu");
            StopAllCoroutines();
            StartCoroutine(gameOver());
        }
    }
    [SerializeField] int money = 12;
    int oldMoney = 12;
    float oldScore = 0;

    public void getMoney()
    {
        score += 500;
        money += Random.Range(50, 200);
        moneyText.text = "Money: $" + money.ToString();
        moneyTextUI.text = "$" + money.ToString();

        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cash"), 0.8f);
    }

    IEnumerator windowHurt()
    {
        catWindow.sprite = Resources.Load<Sprite>("Sprites/cathit");
        yield return new WaitForSeconds(0.5f);
        if(windowState == 1)
        {
            windowState = 0;
            catWindow.sprite = Resources.Load<Sprite>("Sprites/catidle");
        }
    }

    bool onGameOverScreen = false;
    int livesLeft = 9;
    [SerializeField] RectTransform gameOverText;

    IEnumerator gameOver()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.black, 2);
        player.canMove = false;
        player.transform.position = new Vector3(9999, 9999, 0);
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
        yield return new WaitForSeconds(2.1f);

        livesLeft--;
        if (livesLeft <= 0)
        {
            if (endlessMode)
            {
                if (aHandler != null) { aHandler.submitScore((int)score, 2); };
                if (aHandler != null) { aHandler.submitScore((int)level * 100, 3); };
            }else if (hardMode)
            {
                if (aHandler != null) { aHandler.submitScore((int)score, 1); };
            }
            else
            {
                if (aHandler != null) { aHandler.submitScore((int)score, 0); };
            }
            
            SceneManager.LoadScene("menu");
        }

        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(gameOverText, Vector3.zero, 1f);
        
        chat.clearMessages();
        

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("damage"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("tank"))
        {
            Destroy(g);
        }

        //drivingMode = false;
        if(livesLeft == 1)
        {
            retryText.text = ">Retry (" + livesLeft.ToString() + " life left)";
        }
        else
        {
            retryText.text = ">Retry (" + livesLeft.ToString() + " lives left)";
        }
        
        retryText.color = Color.red;
        quitText.text = "Quit";
        quitText.color = Color.white;

        yield return new WaitForSeconds(1.3f);
        onGameOverScreen = true;
        Debug.Log("weiner");

        //SceneManager.LoadScene("menu");
    }

    IEnumerator winGame()
    {
        if (aHandler != null) {
            if (hardMode)
            {
                aHandler.submitScore((int)score, 1);
            }
            else
            {
                aHandler.submitScore((int)score, 0);
            }
        }


        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.white, 2);
        player.canMove = false;
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
        yield return new WaitForSeconds(2.5f);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("damage"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("tank"))
        {
            Destroy(g);
        }

        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(1,1,1,0), 2);
        chat.clearMessages();
        cam.transform.position = new Vector3(0, 10, -10);
        beginDialoguePhase();
        
        //SceneManager.LoadScene("menu");
    }

    IEnumerator winToMenu()
    {
        StartCoroutine(leaveDialogueCo());
        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(1, 1, 1, 1), 2);
        yield return new WaitForSeconds(2.5f);
        PlayerPrefs.SetInt("intro_mode", 2);
        SceneManager.LoadScene("menu");
    }

    [SerializeField] Camera cam;
    float shotgunChance = 0;

    public void startLevel(int i)
    {
        level = i;
        switch (i)
        {
            case 0:               
                if (!hardMode)
                {
                    roadBackground.speed = 6 * bulletSpeedModifier;
                    rateCar = 3.3f;
                    ratePothole = 6.1f;
                    rateMoney = 5f; 
                }
                else
                {
                    roadBackground.speed = 8 * bulletSpeedModifier;
                    rateCar = 1.5f;
                    ratePothole = 1.9f;
                    rateMoney = 5f;
                }

                aimCarChance = 0;
                shotgunChance = 0;

                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                totalTime = musicRoad.clip.length - 2f;
                break;
            case 1:
                
                if (!hardMode)
                {
                    roadBackground.speed = 8 * bulletSpeedModifier;
                    rateCar = 3.3f;
                    ratePothole = 5.7f;
                    rateMoney = 5f;
                    aimCarChance = 0.4f;
                    shotgunChance = 0;
                }
                else
                {
                    roadBackground.speed = 9 * bulletSpeedModifier;
                    rateCar = 2f;
                    ratePothole = 3.4f;
                    rateMoney = 5f;
                    aimCarChance = 0.5f;
                    shotgunChance = 0.3f;
                }
                
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                totalTime = musicRoad.clip.length - 4f;
                break;
            case 2:
                if (!hardMode)
                {
                    roadBackground.speed = 9 * bulletSpeedModifier;
                    rateCar = 6.6f;
                    ratePothole = 6.1f;
                    rateMoney = 5f;
                    aimCarChance = 0.3f;
                    rateCluster = 5.3f;
                    shotgunChance = 0;
                }
                else
                {
                    roadBackground.speed = 9 * bulletSpeedModifier;
                    rateCar = 3.3f;
                    ratePothole = 3.4f;
                    rateMoney = 5f;
                    aimCarChance = 0.4f;
                    rateCluster = 2.9f;
                    spiralClusterChance = 0.7f;
                    shotgunChance = 0.4f;
                }
                
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                StartCoroutine(clusterLoop());
                totalTime = musicRoad.clip.length - 2f;
                break;
            case 3:
                if (!hardMode)
                {
                    roadBackground.speed = 9 * bulletSpeedModifier;
                    rateCar = 7.8f;
                    ratePothole = 6f;
                    rateMoney = 5f;
                    aimCarChance = 0.3f;
                    rateCluster = 6.7f;
                }
                else
                {
                    roadBackground.speed = 9 * bulletSpeedModifier;
                    rateCar = 3.3f;
                    ratePothole = 4.5f;
                    rateMoney = 5f;
                    aimCarChance = 0.4f;
                    spiralClusterChance = 0.5f;
                    shotgunChance = 0.4f;
                    rateCluster = 6.7f;
                }
                
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                StartCoroutine(clusterLoop());

                Instantiate(Resources.Load<GameObject>("Enemies/Tank"), new Vector3(3, 9, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Tank>().init(bulletSpeedModifier, hardMode);
                totalTime = musicRoad.clip.length - 9f;
                break;
        }

        
        if(!hardMode)
        {
            chat.loadSubtitles("texts_" + level.ToString());
        }
        else
        {
            chat.loadSubtitles("texts_" + level.ToString() + "_expert");
        }
        
        timer = 0;
        progressBar.fillAmount = 0;
        catFaceProgress.localPosition = new Vector3(747, -375, 0);
        drivingMode = true;
        StartCoroutine(songCredit(level));
    }

    public void startLevelEndless()
    {
        rateCar = 999f;
        ratePothole = 999f;
        rateMoney = 5f;
        aimCarChance = 0f;
        rateCluster = 0f;
        shotgunChance = 0f;
        spiralClusterChance = 0f;
        roadBackground.speed = (9 + (int)(level/3)) * bulletSpeedModifier;
        hardMode = false;

        chat.loadSubtitles("endless_texts_" + playlistTexts[0].ToString());
        playlistTexts.Add(playlistTexts[0]);
        playlistTexts.RemoveAt(0);

        switch (level)
        {
            case 0:
                rateCar = 2f;
                ratePothole = 1.6f;
                roadBackground.speed = 8 * bulletSpeedModifier;
                break;
            case 1:
                rateCar = 1.5f;
                ratePothole = 2.4f;
                aimCarChance = .3f;
                break;
            case 2:
                rateCar = 1.8f;
                ratePothole = 2.4f;
                aimCarChance = .3f;
                rateCluster = 4.2f;
                StartCoroutine(clusterLoop());
                break;
            case 3:
                rateCar = 1.5f;
                ratePothole = 1.5f;
                aimCarChance = .5f;
                shotgunChance = .5f;
                break;
            case 4:
                rateCar = 999f;
                //ratePothole = 6.5f;
                rateCluster = 1.6f;
                spiralClusterChance = 0.75f;
                StartCoroutine(clusterLoop());
                break;
            case 5:
                rateCar = 1.8f;
                ratePothole = 1.5f;
                aimCarChance = .5f;
                shotgunChance = .5f;
                rateCluster = 2.7f;
                StartCoroutine(clusterLoop());
                break;
            case 6:
                rateCar = 1.2f;
                ratePothole = 0.9f;
                aimCarChance = .7f;
                shotgunChance = .8f;
                break;
            case 7:
                rateCar = 1.8f;
                ratePothole = 0.9f;
                aimCarChance = .5f;
                shotgunChance = .5f;
                rateCluster = 2.4f;
                spiralClusterChance = 0.6f;
                StartCoroutine(clusterLoop());
                break;
            case 8:
                rateCar = 999f;
                ratePothole = 3.2f;
                rateCluster = 0.9f;
                spiralClusterChance = 0.75f;
                StartCoroutine(clusterLoop());
                break;
            case 9:
                rateCar = 0.9f;
                ratePothole = 999f;
                aimCarChance = 1f;
                shotgunChance = .7f;
                break;
            case 10:
                rateCar = 0.9f;
                ratePothole = 3.2f;
                aimCarChance = 1f;
                shotgunChance = .5f;
                rateCluster = 2.7f;
                StartCoroutine(clusterLoop());
                break;
            case 11:
                rateCar = 0.9f;
                aimCarChance = 0.5f;
                rateCluster = 2.7f;
                StartCoroutine(clusterLoop());
                break;
            case 12:
                rateCluster = 0.6f;
                spiralClusterChance = 0.75f;
                StartCoroutine(clusterLoop());
                break;
            case 13:
                rateCluster = 1.2f;
                spiralClusterChance = 0.75f;
                rateCar = 0.9f;
                aimCarChance = 0.5f;
                shotgunChance = .5f;
                StartCoroutine(clusterLoop());
                break;
            case 14:
                rateCar = 0.7f;
                aimCarChance = 1f;
                shotgunChance = 1f;
                break;
            case 15:
                ratePothole = 0.2f;
                break;
        }


        if(level > 15)
        {
            hardMode = true;
            float modifier = Mathf.Pow(0.94f, (int)((level - 16)/3));


            switch (Random.Range(0, 4))
            {
                case 0:
                    rateCar = 1.3f;
                    ratePothole = 3.2f;
                    aimCarChance = 0.5f;
                    shotgunChance = .5f;
                    rateCluster = 5.7f;
                    spiralClusterChance = 0.5f;
                    StartCoroutine(clusterLoop());
                    break;
                case 1:
                    rateCar = 1.5f;
                    ratePothole = 4.5f;
                    aimCarChance = 1f;
                    shotgunChance = .7f;
                    rateCluster = 6.7f;
                    spiralClusterChance = 0.5f;
                    StartCoroutine(clusterLoop());
                    break;
                case 2:
                    rateCar = 3.2f;
                    ratePothole = 6.5f;
                    aimCarChance = 0.5f;
                    shotgunChance = .5f;
                    rateCluster = 1.3f;
                    spiralClusterChance = 0.6f;
                    StartCoroutine(clusterLoop());
                    break;
                case 3:
                    rateCar = 3.2f;
                    ratePothole = 0.7f;
                    aimCarChance = 0.5f;
                    shotgunChance = .5f;
                    rateCluster = 6.7f;
                    spiralClusterChance = 0.6f;
                    StartCoroutine(clusterLoop());
                    break;
            }

            rateCar *= modifier;
            ratePothole *= modifier;
            rateCluster *= modifier;

        }
        
        if(level > 10)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Tank"), new Vector3(3, 9, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Tank>().init(bulletSpeedModifier, hardMode);
        }

        StartCoroutine(carLoop());
        StartCoroutine(potLoop());
        StartCoroutine(moneyLoop());
        //chat.loadSubtitles(i);
        timer = 0;
        if(lastEndlessModeSong == 5 || lastEndlessModeSong == 6)
        {
            totalTime = 90;
        }
        else
        {
            totalTime = musicRoad.clip.length - 2f;
        }

        StartCoroutine(songCredit(lastEndlessModeSong));
        
        progressBar.fillAmount = 0;
        catFaceProgress.localPosition = new Vector3(747, -375, 0);
        drivingMode = true;
    }

    [SerializeField] List<GameObject> healthObjects;
    public void setHealth(int i)
    {
        foreach(GameObject h in healthObjects)
        {
            h.SetActive(false);
        }

        for(int j = 0; j <= i-1; j++)
        {
            healthObjects[j].SetActive(true);
        }
    }

    [SerializeField] List<string> cityNames;
    int lastEndlessModeSong = 0;

    [SerializeField] Roadmap roadmap;
    public IEnumerator nextLevel()
    {
        if (drivingMode)
        {
            oldMoney = money;
            oldScore = score;
            isMoving = true;
            level++;
            drivingMode = false;

            if (!endlessMode)
            {
                switch (level)
                {
                    case 1:
                        if (aHandler != null) { aHandler.unlockAchievement(0); }
                        PlayerPrefs.SetInt("song_1", 1);
                        break;
                    case 2:
                        if (aHandler != null) { aHandler.unlockAchievement(1); }
                        PlayerPrefs.SetInt("song_2", 1);
                        break;
                    case 3:
                        if (aHandler != null) { aHandler.unlockAchievement(2); }
                        PlayerPrefs.SetInt("song_3", 1);
                        break;
                    case 4:
                        if (aHandler != null) { aHandler.unlockAchievement(3); }
                        PlayerPrefs.SetInt("song_4", 1);
                        break;
                }

                if (level < 4)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0, 2);
            }

            

            player.canMove = false;
            player.toggleHitbox(false);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(player.transform, new Vector3(player.transform.localPosition.x, 7, 2.17f), 3);
            yield return new WaitForSeconds(1f);

            if(level >= 4 && !endlessMode)
            {
                StartCoroutine(winGame());
                yield break;
            }
            else
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.black, 2);
            }
            
            yield return new WaitForSeconds(2f);

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("damage"))
            {
                Destroy(g);
            }

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("tank"))
            {
                Destroy(g);
            }

            if (!endlessMode)
            {
                backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + level.ToString());
            }
            else
            {
                backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + Random.Range(0,4));
                backgroundImage.color = Color.HSVToRGB(Random.Range(0, 360) / 360f, .4f, 1f);
            }
            
            StartCoroutine(townSong());
            cam.transform.position = new Vector3(0, 10, -10);
            map.localPosition = new Vector2(1600, 900);
            mapCityName.localPosition = new Vector2(-1200, 1230);

            if (!endlessMode)
            {
                roadmap.setCatFace(level);
            }
            else
            {
                roadmap.setCatFace(Random.Range(0,8));
            }



            float randHue = Random.Range(0, 360) / 360f;
            Color roadColor = Color.HSVToRGB(randHue, .30f, 1);
            Color UIColor = Color.HSVToRGB(randHue, .45f, .83f);

            if (!endlessMode)
            {
                cityNameDialogue.text = hardMode ? "Evil " + cityNames[level] : cityNames[level];
                cityNameDriving.text = hardMode ? "Evil " + cityNames[level] : cityNames[level];

                roadColor = road_colors[level];
                UIColor = UI_colors[level];
            }
            else
            {
                cityNameDialogue.text = ((level + 1) * 100).ToString() + " mi";
                cityNameDriving.text = ((level + 1) * 100).ToString() + " mi";
            }
            

            road0.color = roadColor;
            road1.color = roadColor;
            progressBackground.color = UIColor;
            UI_background.color = UIColor;
            cat_background.color = UIColor;

            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(map, new Vector2(0, 900), 1f, false);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(mapCityName, new Vector2(0, 1230), 1f, false);
            yield return new WaitForSeconds(3f);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(map, new Vector2(-1600, 900), 1f, true);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(mapCityName, new Vector2(1200, 1230), 1f, true);
            yield return new WaitForSeconds(0.5f);
            Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(0,0,0,0), 3);
            chat.clearMessages();
            
            beginDialoguePhase();
            //Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(cam.transform, new Vector3(0, 10, -10f), 2);
            StartCoroutine(showDialogueCo());
        }
        else
        {
            catWindow.sprite = Resources.Load<Sprite>("Sprites/catidle");

            if (!endlessMode)
            {
                musicRoad.clip = Resources.Load<AudioClip>("Sounds/Music/song" + level.ToString());
            }
            else
            {
                lastEndlessModeSong = playlistSong[0];
                playlistSong.Add(playlistSong[0]);
                playlistSong.RemoveAt(0);
                musicRoad.clip = endlessModeSongs[lastEndlessModeSong];

                PlayerPrefs.SetInt("song_" + (lastEndlessModeSong+1).ToString(), 1);

                if (lastEndlessModeSong == 5 && Random.Range(0f, 1f) < .5f)
                {
                    musicRoad.time = 90;
                }
                else
                {
                    musicRoad.time = 0;
                }
            }
            
            musicRoad.Play();
            Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicTown, 0, 2);
            Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, PlayerPrefs.GetFloat("volume_music", 0.7f), 2);
            player.transform.localPosition = new Vector3(3.5f, -1.7f, 2.17f);
            isMoving = true;
            cityBackground.localPosition = new Vector3(316f, cityBackground.localPosition.y, cityBackground.localPosition.z);
            health = healthCap;
            setHealth(health);
            chat.clearMessages();
            progressBar.fillAmount = 0;
            catFaceProgress.localPosition = new Vector3(747, -375, 0);
            StartCoroutine(leaveDialogueCo());
            yield return new WaitForSeconds(1f);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(cam.transform, new Vector3(0, 0, -10f), 2);
            yield return new WaitForSeconds(2f);
            player.toggleHitbox(true);
            if (!endlessMode)
            {
                startLevel(level);
            }
            else
            {
                startLevelEndless();
            }
            
            player.canMove = true;         
        }
        
    }

    [SerializeField] List<AudioClip> endlessModeSongs;


    void retryHelper()
    {
        onGameOverScreen = false;
        drivingMode = true;
        catWindow.sprite = Resources.Load<Sprite>("Sprites/catidle");
        if (!endlessMode)
        {
            musicRoad.clip = Resources.Load<AudioClip>("Sounds/Music/song" + level.ToString());
        }
        else
        {
            musicRoad.clip = endlessModeSongs[lastEndlessModeSong];
        }
        
        musicRoad.Play();
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicTown, 0, 2);
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, PlayerPrefs.GetFloat("volume_music", 0.7f), 2);
        player.transform.localPosition = new Vector3(3.5f, -1.7f, 2.17f);
        isMoving = true;
        cityBackground.localPosition = new Vector3(316f, cityBackground.localPosition.y, cityBackground.localPosition.z);
        health = healthCap;
        setHealth(health);
        chat.clearMessages();
        progressBar.fillAmount = 0;
        catFaceProgress.localPosition = new Vector3(747, -375, 0);
        player.toggleHitbox(true);
        if (!endlessMode)
        {
            startLevel(level);
        }
        else
        {
            startLevelEndless();
        }

        if (endlessMode && lastEndlessModeSong == 5 && Random.Range(0f, 1f) < .5f)
        {
            musicRoad.time = 90;
        }

        player.canMove = true;
    }

    IEnumerator retry()
    {
        money = oldMoney;
        score = oldScore - 1000;
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(gameOverText, new Vector3(0,-1000), 1f);
        yield return new WaitForSeconds(1f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(0,0,0,0), 2);
        retryHelper();

        
    }

    IEnumerator quit()
    {
        PlayerPrefs.SetInt("intro_mode", 1);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(gameOverText, new Vector3(0, -1000), 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("menu");
    }

    //visual novel portion

    bool isMoving = false;
    //[SerializeField] RawImage dimmer;
    [SerializeField] RectTransform dialogueBoxes;
    [SerializeField] Image creature;

    [SerializeField] List<TextMeshProUGUI> choiceTexts;

    public void showDialogueScreen()
    {
        isMoving = true;
        StartCoroutine(showDialogueCo());
    }

    public void leaveDialogueScreen()
    {
        isMoving = true;
        StartCoroutine(leaveDialogueCo());
    }

    IEnumerator showDialogueCo()
    {
        //dimmer.gameObject.SetActive(true);
        //ColorFader cf = Instantiate(Resources.Load<GameObject>("Prefabs/ColorFader")).GetComponent<ColorFader>();
        //cf.set(dimmer, new Color(0, 0, 0, .5f), .4f);
        Mover m = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m.set(dialogueBoxes, new Vector2(0, -2), .6f, false);
        Mover m2 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m2.set(creature.rectTransform, new Vector2(0, -60f), .6f, false);

        Mover m3 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m3.set(moneyIndicator, new Vector2(-600, 340), .6f, false);

        yield return new WaitForSeconds(.6f);
        isMoving = false;
    }

    IEnumerator showUpgrade()
    {
        isMoving = true;
        Mover m = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m.set(upgradeIndicator, new Vector2(-600, 205), .6f, false);
        yield return new WaitForSeconds(1.2f);
        Mover m2 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m2.set(upgradeIndicator, new Vector2(-1250, 205), .6f, true);
        isMoving = false;
    }

    IEnumerator leaveDialogueCo()
    {
        //ColorFader cf = Instantiate(Resources.Load<GameObject>("Prefabs/ColorFader")).GetComponent<ColorFader>();
        //cf.set(dimmer, new Color(0, 0, 0, 0f), .4f);

        Mover m = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m.set(dialogueBoxes, new Vector2(-1422f, -26), 0.8f, true);
        Mover m2 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m2.set(creature.rectTransform, new Vector2(1200f, -60f), 0.8f, true);
        Mover m3 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m3.set(moneyIndicator, new Vector2(-1060f, 340), 0.8f, true);
        yield return new WaitForSeconds(1.2f);
        //dimmer.gameObject.SetActive(false);
        isMoving = false;
    }

    IEnumerator changeCharacterPortrait(string filePath)
    {
        isMoving = true;
        Mover m2 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m2.set(creature.rectTransform, new Vector2(1200f, -60f), .4f, true);
        yield return new WaitForSeconds(.4f);
        creature.sprite = Resources.Load<Sprite>(filePath);
        Mover m3 = Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>();
        m3.set(creature.rectTransform, new Vector2(0, -60f), .6f, false);
        yield return new WaitForSeconds(.4f);
        isMoving = false;
    }

    [SerializeField] RawImage blackOverlay;
    [SerializeField] RawImage whiteOverlay;
    [SerializeField] RectTransform map;
    [SerializeField] RectTransform mapCityName;
    [SerializeField] AudioSource musicTown;
    [SerializeField] AudioSource musicRoad;

    [SerializeField] TextMeshProUGUI cityNameDialogue;
    [SerializeField] TextMeshProUGUI cityNameDriving;

    IEnumerator intro()
    {
        isMoving = true;
        StartCoroutine(townSong());
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(map,new Vector2(0,900),1f,false);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(mapCityName,new Vector2(0,1230),1f,false);
        yield return new WaitForSeconds(3f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(map, new Vector2(-1600, 900), 1f,true);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(mapCityName, new Vector2(1200, 1230), 1f,true);
        yield return new WaitForSeconds(0.5f);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, new Color(0,0,0,0), 2f);
        yield return new WaitForSeconds(0.5f);
        if (!endlessMode)
        {
            beginDialoguePhase();
        }
        else
        {
            beginDialoguePhaseEndless();
        }
        
        isMoving = false;
    }

    [SerializeField] AudioClip town_intro;

    IEnumerator townSong()
    {
        musicTown.loop = false;
        musicTown.volume = 0.35f * PlayerPrefs.GetFloat("volume_music", 0.7f);
        musicTown.clip = town_intro;
        musicTown.Play();
        yield return new WaitWhile(() => musicTown.isPlaying);
        musicTown.loop = true;
        if (!endlessMode)
        {
            musicTown.clip = Resources.Load<AudioClip>("Sounds/Music/town_" + level.ToString());
        }
        else
        {
            musicTown.clip = Resources.Load<AudioClip>("Sounds/Music/town_" + Random.Range(0,4));

        }
        
        musicTown.time = 0;
        musicTown.Play();
    }
    float screamTimer = 0;

    public void playScream()
    {
        if(screamTimer > 2)
        {
            sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/scream" + Random.Range(0,3)), 1f);
        }
    }

    [SerializeField] TextMeshProUGUI songCreditText;
    [SerializeField] List<string> songCredits;

    IEnumerator songCredit(int i)
    {
        songCreditText.text = songCredits[i].Replace("\\n", "\n"); ;
        Debug.Log(songCredits[i]);
        songCreditText.color = new Color(1, 1, 1, 0);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(songCreditText, new Color(1, 1, 1, 1), 0.5f);
        yield return new WaitForSeconds(3);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(songCreditText, new Color(1, 1, 1, 0), 0.5f);
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
