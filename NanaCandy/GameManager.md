## GameManager
게임 전체를 관리하는 GameManager의 script다.  
게임을 실행하면 homeScreen이 가장 먼저 나타난다. homeScreen에는 '게임 시작', '크레딧', '게임 종료'의 3가지 버튼이 들어있다.  
'게임 시작' 버튼을 누르면 homeScreen이 비활성화되고 gameScreen이 활성화된다. '크레딧' 버튼을 누르면 homeScreen 위에 creditScreen이 활성화된다. '게임 종료' 버튼을 누르면 프로그램을 종료한다.

```C#
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int candyNum;
    public int stageIndex;
    public Star leftStar;
    public Star rightStar;
    public GameObject[] stages;

    public TextMeshProUGUI UIStage;
    public TextMeshProUGUI RecordText;
    public GameObject homeScreen;
    public GameObject gameScreen;
    public GameObject creditScreen;
    public GameObject ResetBtn;

    public bool clear = false;
    
    float clearTime;

    public void GameStart()
    {
        clear = false;
        homeScreen.SetActive(false);
        RecordText.text = "";
        gameScreen.SetActive(true);
        stages[stageIndex].SetActive(true);

        leftStar.isMoving = false;
        rightStar.isMoving = false;
        leftStar.gameObject.SetActive(true);
        rightStar.gameObject.SetActive(true);
        leftStar.gameObject.layer = 10;
        rightStar.gameObject.layer = 10;
        Reset();
        clearTime = 0;
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
```

설명을 위한 편의상 코드를 2부분으로 나누었다. 위에서 구현된 함수들은 모두 homeScreen과 creditScreen의 버튼을 눌렀을 때 호출되는 함수들이다.  
'게임 시작' 버튼을 누르면 GameStart() 함수가 호출되어 homeScreen을 비활성화하고 gameScreen을 활성화한다. leftStar와 rightStar는 게임 화면에 나타나는 2개의 캐릭터이다. 캐릭터의 활성화 외에 다른 속성들에 대한 할당은 게임을 클리어 한 후 재시작했을 때 상태를 초기화시켜주기 위함이다.  
'크레딧' 버튼을 누르면 CreditOpen() 함수가 호출되어 creditScreen을 활성화하고, creditScreen에 있는 '닫기' 버튼을 누르면 CreditClose()가 호출된다. 두 함수에서 Star를 관리하는 것은 엔딩을 보면 homeScreen에 캐릭터인 별이 나타나기 때문이다.  
'게임 종료' 버튼을 누르면 Quit() 함수가 호출되어 게임을 종료한다.

```C#
    void Update()
    {
        if (candyNum == 2)
        {
            NextStage();
            candyNum = 0;
        }

        clearTime += Time.deltaTime;
    }

    void NextStage()
    {
        stages[stageIndex].SetActive(false);
        // Change Stage
        if (stageIndex < stages.Length-1)
        {
            stageIndex++;
            stages[stageIndex].SetActive(true);
            leftStar.Spawn();
            rightStar.Spawn();

            UIStage.text = "STAGE " + (stageIndex+1);

        } else
        {// Game Clear
            clear = true;

            // Result UI
            UIStage.text = "Clear!";
            ResetBtn.gameObject.SetActive(false);
            RecordText.text = "기록 " + System.Math.Truncate(clearTime/60) + ":" + System.Math.Truncate(clearTime%60);
            Debug.Log("Game Clear!" + clearTime);
            stageIndex= 0;
        }
    }

    public void Reset()
    {
        if (clear == false)
        {
            leftStar.Spawn();
            rightStar.Spawn();
        } else
        {
            BackHome();
        }
    }
    public void BackHome()
    {
        gameScreen.SetActive(false);
        UIStage.text = "STAGE 1";
        ResetBtn.SetActive(true);
        homeScreen.SetActive(true);

        if (!clear)
        {
            stages[stageIndex].SetActive(false);
            stageIndex = 0;
            leftStar.gameObject.SetActive(false);
            rightStar.gameObject.SetActive(false);
        }
    }
```

나머지 부분들은 gameScreen에서 실제 게임을 작동시킨다. 캐릭터가 목표물인 사탕에 닿으면 candyNum이 1 증가하고, 두 캐릭터가 동시에 사탕에 닿아 이 값이 2가 되면 NextStage()를 호출한다.  
각 stage는 gameScreen 위에 해당 스테이지의 지형을 띄우는 식으로 구현했다. 그래서 NextStage()가 호출되면 현재 stage를 비활성화하고, stageIndex를 증가시켜 다음 stage를 활성화한다.  
만약 주어진 stage를 모두 클리어하면 게임 클리어에 걸린 시간과 함께 결과창을 띄운다.  
Reset()은 gameScreen에 있는 '리셋 버튼'을 누르면 호출되는 함수다. 두 캐릭터의 위치를 모두 초기 상태로 되돌린다. 개발 초기 구상에서는 '리셋 버튼'이 게임을 클리어하면 '홈 버튼'으로 바뀌도록 하려 했는데 나중에 gameScreen에 상시 존재하는 '홈 버튼'을 추가했다. 그러면서 게임을 클리어하면 '리셋 버튼'이 비활성화 되도록 변경했는데, 이때 Reset()에 있는 조건문을 빼는 걸 까먹어서 그대로 남아버렸다.  
BackHome()은 '홈 버튼'을 누르면 호출된다. gameScreen을 비활성화하고 초기화시킨 후 homeScreen을 활성화한다. 만약 게임을 클리어 하기 전에 '홈 버튼'을 누르면 진행 상태를 초기화한다. 게임을 클리어했을 때는 결과창을 띄울 때 stageIndex를 초기화하는 과정이 포함되는데 이 부분을 그냥 BackHome() 함수에서 항상 실행되는 부분에 넣는 게 더 깔끔했을 것 같다.  



