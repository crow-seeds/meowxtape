using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BulletManager : MonoBehaviour
{
    // Start is called before the first frame update

    float score = 0;
    int timesHit = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] AudioSource sfx;
    [SerializeField] RoadBackground roadBackground;
    int level = 0;

    float ratePothole = 1;
    float rateCar = 1;
    float aimCarChance = 0;
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

    void Start()
    {
        //StartCoroutine(clusterLoop());
        //StartCoroutine(carLoop());

        listOfUpgrades = new List<string>(upgrades.Keys);
        moneyTextUI.text = "$" + money.ToString();
        moneyText.text = "Money: " + "$" + money.ToString();
        dialogue = new List<string>()
        {
            "Hey, I'm walkin here!",
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
        };

        StartCoroutine(intro());
        //beginDialoguePhase();

        //startLevel(0);
    }

    bool drivingMode = false;
    [SerializeField] TextMeshProUGUI upgradeToolTip;

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("get up");
            timeHeld = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (drivingMode)
        {
            score += (Time.deltaTime * 50);
            scoreText.text = "Score: " + ((int)score).ToString();

            timer += Time.deltaTime;

            progressBar.fillAmount = timer / totalTime;
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
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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

                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
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
            

            if (!isMoving)
            {
                spaceBetweenSpaces += Time.deltaTime;

                if (Input.GetKey(KeyCode.Space))
                {
                    timeHeld += Time.deltaTime;
                }
                else
                {
                    timeHeld = 0;
                }

                if ((Input.GetKeyDown(KeyCode.Space)) || timeHeld > 1)
                {
                    
                    if (!onShop || choiceNum == 3)
                    {
                        nextDialogue();
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
        switch (dialoguePhase)
        {
            case 0:
                StartCoroutine(showDialogueCo());
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
        }

    }

    bool onShop = false;
    [SerializeField] GameObject shopContainer;

    public void nextDialogue()
    {
        if (dialogue[0] == "")
        {
            dialogue.RemoveAt(0);
            endDialoguePhase();
            return;
        }

        Debug.Log(dialogueNum.ToString());
        playDialogueClip(dialogueNum.ToString());

        if(dialogue[0][0] == '|')
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
        {"Furr Efficient Battery", "speed" },
        {"Overclawked Pistons", "speed" },
        {"1,000 Horsepawer Engine", "speed" },
        {"Milk Coolant", "cooldown" },
        {"Radiator Bed", "cooldown" },
        {"Air Conditioner Repair", "cooldown" },
    };

    Dictionary<string, string> upgradeDescriptions = new Dictionary<string, string>
    {
        {"health", "Car Health Up!" },
        {"cooldown", "Faster Dash Cooldown!" },
        {"speed", "Car Speed Up!" },
        {"enemyspeed", "Slower Enemies!" },
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
                        player.setSpeed(player.getSpeed() + 1);
                        break;
                    case "cooldown":
                        player.dashCooldownTime -= .25f;
                        player.dashCooldownTime = Mathf.Max(player.dashCooldownTime, 0.5f);
                        break;
                    case "enemyspeed":
                        bulletSpeedModifier *= 0.9f;
                        bulletSpeedModifier = Mathf.Max(bulletSpeedModifier, 0.4f);
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
        Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), new Vector3(Random.Range(-1.5f, 7f), 12, 0), Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(2f * bulletSpeedModifier, 6 + Random.Range(0, 5), true);
        StartCoroutine(clusterLoop());
    }

    IEnumerator carLoop()
    {
        yield return new WaitForSeconds(rateCar);
        string carType = "regular";
        if (Random.Range(0,1f) < aimCarChance)
        {
            carType = "aim";
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.8f);
        }
        else
        {
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.8f);
        }

        if(level != 0 || timer > 7f)
        {
            if (carType == "aim")
            {
                sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.1f);
            }
            else
            {
                if(Random.Range(0,1f) < 0.5f)
                {
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.1f);
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
        }
        else
        {
            //sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.8f);
        }


        if (level != 0 || timer > 7f)
        {
            if(carType == "aim")
            {
                sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/cop"), 0.1f);
            }
            else
            {
                if (Random.Range(0, 1f) < 0.5f)
                {
                    sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/honk"), 0.1f);
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
        score -= 1000;
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
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/hurt"), 0.8f);

        if(health <= 0)
        {
            //SceneManager.LoadScene("menu");
            StartCoroutine(gameOver());
        }
    }
    [SerializeField] int money = 12;

    public void getMoney()
    {
        score += 1000;
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

    IEnumerator gameOver()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.black, 2);
        player.canMove = false;
        player.transform.position = new Vector3(9999, 9999, 0);
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("menu");
    }

    IEnumerator winGame()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.white, 2);
        player.canMove = false;
        Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("menu");
    }

    [SerializeField] Camera cam;

    public void startLevel(int i)
    {
        Debug.Log("this level is: " + i.ToString());
        level = i;
        switch (i)
        {
            case 0:
                roadBackground.speed = 6 * bulletSpeedModifier;
                rateCar = 4f;
                ratePothole = 6f;
                rateMoney = 5f;
                aimCarChance = 0;
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                totalTime = musicRoad.clip.length - 2f;
                break;
            case 1:
                roadBackground.speed = 8 * bulletSpeedModifier;
                rateCar = 4f;
                ratePothole = 6f;
                rateMoney = 5f;
                aimCarChance = 0.4f;
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                totalTime = musicRoad.clip.length - 4f;
                break;
            case 2:
                roadBackground.speed = 9 * bulletSpeedModifier;
                rateCar = 6.6f;
                ratePothole = 6f;
                rateMoney = 5f;
                aimCarChance = 0.3f;
                rateCluster = 5.3f;
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                StartCoroutine(clusterLoop());
                totalTime = musicRoad.clip.length - 2f;
                break;
            case 3:
                roadBackground.speed = 9 * bulletSpeedModifier;
                rateCar = 7.8f;
                ratePothole = 6f;
                rateMoney = 5f;
                aimCarChance = 0.3f;
                rateCluster = 6.7f;
                StartCoroutine(carLoop());
                StartCoroutine(potLoop());
                StartCoroutine(moneyLoop());
                StartCoroutine(clusterLoop());

                Instantiate(Resources.Load<GameObject>("Enemies/Tank"), new Vector3(3, 9, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Tank>().init(bulletSpeedModifier);
                totalTime = musicRoad.clip.length - 9f;
                break;
        }
        chat.loadSubtitles(i);
        timer = 0;
        progressBar.fillAmount = 0;
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


    [SerializeField] Roadmap roadmap;
    public IEnumerator nextLevel()
    {
        

        if (drivingMode)
        {
            
            isMoving = true;
            level++;
            drivingMode = false;

            if (level < 4)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 0f, 2);
                yield return new WaitForSeconds(1f);
            }
            player.canMove = false;
            player.toggleHitbox(false);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(player.transform, new Vector3(player.transform.localPosition.x, 7, 2.17f), 3);
            yield return new WaitForSeconds(1f);

            if(level >= 4)
            {
                StartCoroutine(winGame());
                yield break;
            }
            else
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(blackOverlay, Color.black, 2);
            }
            
            yield return new WaitForSeconds(2f);
            backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + level.ToString());
            StartCoroutine(townSong());
            cam.transform.position = new Vector3(0, 10, -10);
            map.localPosition = new Vector2(1600, 900);
            mapCityName.localPosition = new Vector2(-1200, 1230);
            roadmap.setCatFace(level);
            cityNameDialogue.text = cityNames[level];
            cityNameDriving.text = cityNames[level];
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
            musicRoad.clip = Resources.Load<AudioClip>("Sounds/Music/song" + level.ToString());
            musicRoad.Play();
            Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicTown, 0, 2);
            Instantiate(Resources.Load<GameObject>("Prefabs/Volume Fader")).GetComponent<VolumeFader>().set(musicRoad, 1f, 2);
            player.transform.localPosition = new Vector3(3.5f, -1.7f, 2.17f);
            isMoving = true;
            cityBackground.localPosition = new Vector3(316f, cityBackground.localPosition.y, cityBackground.localPosition.z);
            health = healthCap;
            setHealth(health);
            chat.clearMessages();
            progressBar.fillAmount = 0;
            StartCoroutine(leaveDialogueCo());
            yield return new WaitForSeconds(1f);
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(cam.transform, new Vector3(0, 0, -10f), 2);
            yield return new WaitForSeconds(2f);
            player.toggleHitbox(true);
            startLevel(level);
            player.canMove = true;         
        }
        
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
        m3.set(creature.rectTransform, new Vector2(400f, -60f), .4f, false);
        yield return new WaitForSeconds(.4f);
        isMoving = false;
    }

    [SerializeField] RawImage blackOverlay;
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
        beginDialoguePhase();
        isMoving = false;
    }

    [SerializeField] AudioClip town_intro;

    IEnumerator townSong()
    {
        musicTown.loop = false;
        musicTown.volume = 0.35f;
        musicTown.clip = town_intro;
        musicTown.Play();
        yield return new WaitWhile(() => musicTown.isPlaying);
        musicTown.loop = true;
        musicTown.clip = Resources.Load<AudioClip>("Sounds/Music/town_" + level.ToString());
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
}
