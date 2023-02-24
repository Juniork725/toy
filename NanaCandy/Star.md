## Star

유저가 움직이는 캐릭터인 두 별을 제어하는 script다. 

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Star : MonoBehaviour
{
    public GameManager gameManager;
    SpriteRenderer spriteRenderer;

    public bool isMoving = false;
    Vector2 target;
    Vector2 prepos;
    int dir;

    Vector2 initpos;
    Vector2 stageStartpos;

    public GameObject portalA;
    public GameObject portalB;
    bool portalFlag = true;
    GameObject lastPortal;

    private void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        initpos = transform.position;
        stageStartpos = initpos;

        // Which star is this
        if (this.gameObject.name == "Left Star")
            dir = -1;
        else
            dir = 1;

        // Destination position
        target = transform.position;
    }
    void Update()
    {
        // Move command
        if (isMoving == false)
        {
            prepos = transform.position;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isMoving= true;
                target = new Vector2(transform.position.x, transform.position.y + 1*dir);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isMoving= true;
                target = new Vector2(transform.position.x, transform.position.y - 1*dir);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isMoving= true;
                target = new Vector2(transform.position.x - 1*dir, transform.position.y);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isMoving= true;
                target = new Vector2(transform.position.x + 1*dir, transform.position.y);
            }
        }
    }
    private void FixedUpdate()
    {
        // Move action
        if (isMoving == true)
        {
            if (gameManager.clear == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, 0.1f);
                if (new Vector2(transform.position.x, transform.position.y) == target)
                    isMoving = false;
            } else
            {
                transform.position = Vector2.Lerp(transform.position, target, 0.05f);
            }
            
        }

        // Ending Scene
        if (gameManager.clear == true)
        {
            this.gameObject.layer = 11;
            target = new Vector2(0.25f * dir, 0);
            isMoving = true;
        }
    }
```

설명의 편의성을 위해 코드를 두 부분으로 나누었다. 위는 캐릭터의 이동에 관련된 부분이다.  
처음에 캐릭터 object가 생성되면 자신이 왼쪽 별인지, 오른쪽 별인지 파악한다. 두 별이 서로 반대 방향으로 움직이기 때문에, dir 변수의 부호로 두 별의 이동 방향을 결정할 것이다.  
Update()에서는 키 입력을 받는다. 지금 별이 움직이고 있지 않다면 키 입력을 받아 상,하,좌,우로 1칸 움직인다. 이때 dir에 의해 오른쪽 별은 방향키 입력과 같게 움직이고, 왼쪽 별은 이와 반대로 움직인다.  
움직이고 있지 않을 때만 키 입력을 받는 것은 퍼즐 게임의 특성상 1번의 키 입력으로 정확하게 1칸씩만 움직이는 것이 좋다고 판단했기 때문이다.  
FixedUpdate()에서는 Update()에서 키 입력을 받아 결정된 target의 위치로 별을 이동시킨다. 만약 게임을 클리어했다면 별이 화면 중앙으로 이동해 합쳐지며 엔딩씬을 보여준다. 이 과정에서 유저의 키 입력에 의해 별이 이동하지 않도록 isMoving에 true를 할당해준다.  

```C#
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Back to previous position
        if (collision.gameObject.tag == "Wall")
            target = prepos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On candy
        if (collision.gameObject.tag == "Candy")
            gameManager.candyNum++;

        // On trap
        if (collision.gameObject.tag == "Trap")
        {
            // Back to start position
            transform.position = stageStartpos;
            target = transform.position;

            // View alpha
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            Invoke("OffAlpha", 3);
        }

        // On Portal
        if (collision.gameObject == portalA && portalFlag == true)
        {
            transform.position = portalB.transform.position;
            target = transform.position;
            portalFlag= false;
            lastPortal = collision.gameObject;
        }
        if (collision.gameObject == portalB && portalFlag == true)
        {
            transform.position = portalA.transform.position;
            target = transform.position;
            portalFlag= false;
            lastPortal = collision.gameObject;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Away from candy
        if (collision.gameObject.tag == "Candy")
            gameManager.candyNum--;

        // Away from portal
        if (collision.gameObject.tag == "Portal" && collision.gameObject != lastPortal)
        {
            portalFlag= true;
        }
    }

    void OffAlpha()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Spawn()
    {
        if (gameManager.stageIndex == 0)
        {
            transform.position = initpos;
            target = initpos;
        }
        else if (gameManager.stageIndex == 1)
        {
            if (this.gameObject.name == "Left Star")
            {
                transform.position = new Vector2(-4.5f, -0.5f);
            } else
            {
                transform.position = new Vector2(5.5f, 0.5f);
            }
        }
        else if (gameManager.stageIndex == 2)
        {
            if (this.gameObject.name == "Left Star")
            {
                transform.position = new Vector2(-6.5f, 0.5f);
            } else
            {
                transform.position = new Vector2(2.5f, 1.5f);
            }
        } else if (gameManager.stageIndex == 3)
        {
            if (this.gameObject.name == "Left Star")
            {
                transform.position = new Vector2(-3.5f, 2.5f);
            } else
            {
                transform.position = new Vector2(8.5f, 1.5f);
            }
        }
        stageStartpos = transform.position;
        target = stageStartpos;
    }
}
```

나머지 부분들은 캐릭터가 지형과 상호작용하고, stage가 바뀜에 따라 새로운 좌표에 위치시키는 과정이다.  
벽이나 바위 등의 object에는 Rigidbody 2D 속성을 부여해 충돌이 일어나도록 했다. 캐릭터가 이들과 충돌하면 이동을 시도했던 위치로 튕겨나간다.  
  
사탕, 함정, 포탈에는 Rigidbody를 부여하지 않고 Collider 2D만 부여해 Trigger로 상호작용하도록 했다.  
사탕과 캐릭터가 닿으면 gameManager의 candyNum을 1 증가시키고, 캐릭터가 다시 이동하여 사탕과 떨어지면 candyNum을 1 감소시킨다. 두 캐릭터가 동시에 사탕에 닿아 candyNum이 2가 되면 gameManager를 통해 다음 stage로 이동한다.  
캐릭터가 함정을 밟으면 stage의 초기 위치로 되돌아가고 3초 동안 반투명해진다.  
캐릭터가 포탈에 들어가면 반대쪽 포탈의 위치로 이동한다. 이때 portalFlag를 통해 반대쪽 포탈로 이동하자마자 되돌아오는 현상을 막아준다. 캐릭터가 출구 포탈을 떠나면 다시 portalFlag를 초기화하여 포탈을 탈 수 있도록 한다.  
  
Spawn()은 stage가 바뀌거나 '리셋 버튼'을 눌렀을 때 등 두 캐릭터를 해당 stage의 초기 위치로 옮길 때 호출된다. stageIndex와 자신이 어느쪽 별인지에 따라 정해진 좌표에 위치된다.  
게임을 실행하면 맨 처음에는 캐릭터가 미리 설정된 초기 위치에서 생성되기 때문에 이를 initpos에 저장하고 첫 스테이지에서 사용했는데, 그냥 다른 경우들처럼 좌표를 직접 부여하는 쪽이 일관성 있고 깔끔했을 듯하다.  

