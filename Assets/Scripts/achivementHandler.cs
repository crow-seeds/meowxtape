using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class achivementHandler : MonoBehaviour
{
    List<int> NGAchievementNums = new List<int> { 75167, 75168, 75169, 75170};
    List<int> NGScoreBoardNums = new List<int> { 13105, 13106, 13107, 13116 };
    NGHelper ng;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ng = gameObject.GetComponent<NGHelper>();
        runThroughAchievements();
        Debug.Log(NGAchievementNums.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void unlockAchievement(int i)
    {
        //SaveData("achievement" + i.ToString(), 1);
        PlayerPrefs.SetInt("achievement" + i.ToString(), 1);

        if (ng.hasNewgrounds)
        {
            ng.unlockMedal(NGAchievementNums[i]);
        }
    }

    public void runThroughAchievements()
    {
        if (ng.hasNewgrounds)
        {
            for(int i = 0; i < NGAchievementNums.Count; i++)
            {
                if(PlayerPrefs.GetInt("achievement" + i.ToString(), 0) == 1)
                {
                    ng.unlockMedal(NGAchievementNums[i]);
                }
            }
        }
    }

    public void submitScore(int score, int board)
    {
        if (ng.hasNewgrounds)
        {
            ng.submitScore(NGScoreBoardNums[board], score);
        }
    }

    public void achInit()
    {
        runThroughAchievements();

        for(int i = 0; i < NGScoreBoardNums.Count; i++)
        {
            if(PlayerPrefs.GetInt("endless_record_" + i.ToString(), 0) > 0)
            {
                submitScore(i, PlayerPrefs.GetInt("endless_record_" + i.ToString(), 0));
            }
        }

        
    }
}
