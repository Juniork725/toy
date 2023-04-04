using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool onLeftCandy;
    public bool onRightCandy;
    public int stageIndex;
    public Star leftStar;
    public Star rightStar;
    public GameObject[] stages;

    public GameObject backBtn;
    public GameObject homeBtn;

    public TextMeshProUGUI UIStage;
    public TextMeshProUGUI RecordText;
    public TextMeshProUGUI StoryText;
    public GameObject homeScreen;
    public GameObject gameScreen;
    public GameObject creditScreen;
    public GameObject ResetBtn;
    public GameObject StageSelectionScreen;
    public GameObject StoryScreen;

    List<string> storyTxt = new List<string> { };
    List<int> moveLimit = new List<int> { 3, 5, 7, 8, 12,   6, 10, 5, 14, 11,   6, 9, 10, 16, 11};
    public int moveCount = 0;

    public bool clear = false;

    private void Awake()
    {
        // Read Story Text File
        TextAsset storyFile = Resources.Load("Story") as TextAsset;
        StringReader stringReader = new StringReader(storyFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            if (line == null)
            {
                break;
            }
            storyTxt.Add(line.ToString().Replace("\\n","\n"));
        }
        stringReader.Close();
    }
    void Update()
    {
        if (onLeftCandy && onRightCandy )
        {
            NextStage();
            onLeftCandy= false;
            onRightCandy= false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void Counting()
    {
        // Count Moving of Star
        moveCount--;
        RecordText.text = "남은 이동 횟수: " + moveCount.ToString();
    }
    void NextStage()
    {
        stages[stageIndex].SetActive(false);
        stageIndex++;

        // Save Data
        if (DataController.Instance.gameData.StageNum < stageIndex+1)
        {
            DataController.Instance.gameData.StageNum = stageIndex + 1;
            DataController.Instance.SaveGameData();
        }
        // Change Stage
        if (stageIndex < stages.Length)
        {
            leftStar.gameObject.SetActive(false);
            rightStar.gameObject.SetActive(false);
            StageStart();
        } else
        {// Game Clear
            clear = true;

            // Result UI
            UIStage.text = "Clear!";
            ResetBtn.gameObject.SetActive(false);
            Debug.Log("Game Clear!");
        }
    }

    public void GameStart()
    {
        // Off Home Screen
        homeScreen.SetActive(false);
        leftStar.gameObject.SetActive(false);
        rightStar.gameObject.SetActive(false);

        // On Stage Selection Screen
        StageSelectionScreen.SetActive(true);
        backBtn.gameObject.SetActive(true);

        // Show Only Cleared Stages
        StageSelectionScreen.transform.GetChild(1).gameObject.SetActive(true);  //Bug?
        for (int i = 0; i < StageSelectionScreen.transform.childCount; i++){
            StageSelectionScreen.transform.GetChild(i).gameObject.SetActive(true);
            if (i >= DataController.Instance.gameData.StageNum)
            {
                break;
            }
        }
    }

    public void StageSelect()
    {
        // Off Stage Selection Screen
        StageSelectionScreen.SetActive(false);
        stageIndex = Convert.ToInt16(EventSystem.current.currentSelectedGameObject.name.Split(' ')[1]) - 1;
        Debug.Log("stageIndex = " + stageIndex);

        clear = false;
        StageStart();
    }
    public void StageStart()
    {
        // On Game Screen
        gameScreen.SetActive(true);
        homeBtn.gameObject.SetActive(true);
        UIStage.text = "STAGE " + (stageIndex + 1);
        moveCount = moveLimit[stageIndex];
        RecordText.text = "남은 이동 횟수: " + moveCount.ToString();

        // On Story Screen
        StoryScreen.SetActive(true);
        StoryText.text = storyTxt[stageIndex];
    }

    public void StoryClose()
    {
        // Off Story Screen
        StoryScreen.SetActive(false);

        // On Stage and Game Object
        stages[stageIndex].SetActive(true);
        leftStar.isMoving = false;
        rightStar.isMoving = false;
        leftStar.gameObject.SetActive(true);
        rightStar.gameObject.SetActive(true);
        leftStar.gameObject.layer = 10;
        rightStar.gameObject.layer = 10;
        Reset();
    }
    public void Reset()
    {
        moveCount = moveLimit[stageIndex];
        RecordText.text = "남은 이동 횟수: " + moveCount.ToString();
        leftStar.Spawn();
        rightStar.Spawn();
    }
    public void BackHome()
    {
        gameScreen.SetActive(false);
        StageSelectionScreen.SetActive(false);
        homeBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
        ResetBtn.SetActive(true);
        homeScreen.SetActive(true);

        if (!clear)
        {
            stages[stageIndex].SetActive(false);
            leftStar.gameObject.SetActive(false);
            rightStar.gameObject.SetActive(false);
        }
    }

    public void Back()
    {
        if (gameScreen.gameObject.activeSelf == true)
        {
            BackHome();
            GameStart();
        } else if (StageSelectionScreen.activeSelf == true)
        {
            BackHome();
            if (clear)
            {
                leftStar.gameObject.SetActive(true);
                rightStar.gameObject.SetActive(true);
            }
        } else if (homeScreen.activeSelf == true)
        {
            Quit();
        }
    }

    public void CreditOpen()
    {
        leftStar.gameObject.SetActive(false);
        rightStar.gameObject.SetActive(false);
        creditScreen.SetActive(true);
    }
    public void CreditClose()
    {
        if (clear)
        {
            leftStar.gameObject.SetActive(true);
            rightStar.gameObject.SetActive(true);
        }
        creditScreen.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
