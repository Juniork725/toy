using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class Star : MonoBehaviour
{
    public GameManager gameManager;
    SpriteRenderer spriteRenderer;

    string myName;
    public bool isMoving = false;
    Vector2 target;
    Vector2 prepos;
    int dir;

    List<Vector2> leftStarPos = new List<Vector2>() {
        new Vector2(-4.5f, 0.5f), new Vector2(-4.5f, -0.5f), new Vector2(-5.5f, -2.5f), new Vector2(-4.5f, 1), new Vector2(-5.5f, 0),
        new Vector2(-6.5f, -1), new Vector2(-3.5f, 0), new Vector2(-5.5f, 0), new Vector2(-7.5f, 1.5f), new Vector2(-6.5f, -1.5f),
        new Vector2(-8.5f, 1.5f), new Vector2(-5.5f, -0.5f), new Vector2(-9.5f, 3), new Vector2(-3.5f, 1), new Vector2(-5, 2)};
    List<Vector2> rightStarPos = new List<Vector2>() {
        new Vector2(4.5f, 0.5f), new Vector2(3.5f, 0.5f), new Vector2(4.5f, 1.5f), new Vector2(6.5f, 1), new Vector2(2.5f, 1),
        new Vector2(6.5f, 3),new Vector2(3.5f, -1),  new Vector2(6.5f, 1), new Vector2(4.5f, 1.5f), new Vector2(5.5f, 0.5f),
        new Vector2(3.5f, -2.5f), new Vector2(7.5f, 2.5f), new Vector2(7.5f, -2), new Vector2(7.5f, 3), new Vector2(6, 2)};

    bool onCandy = false;

    public GameObject portalA;
    public GameObject portalB;
    bool portalFlag = true;
    GameObject lastPortal;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myName = this.gameObject.name;

        // Which star is this
        if (myName == "Left Star")
            dir = -1;
        else
            dir = 1;

        // Destination position
        target = transform.position;
    }
    void Update()
    {
        // Move command
        // Moving lock
        if (isMoving == false)
        {
            if (gameManager.moveCount > 0)
            {
                prepos = transform.position;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isMoving = true;
                    target = new Vector2(transform.position.x, transform.position.y + 1 * dir);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    isMoving = true;
                    target = new Vector2(transform.position.x, transform.position.y - 1 * dir);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isMoving = true;
                    target = new Vector2(transform.position.x - 1 * dir, transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isMoving = true;
                    target = new Vector2(transform.position.x + 1 * dir, transform.position.y);
                }
            }

            if (myName == "Left Star")
            {
                gameManager.onLeftCandy = onCandy;
            }
            else
            {
                gameManager.onRightCandy = onCandy;
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
                {
                    isMoving = false;
                    if (myName == "Left Star")
                        gameManager.Counting();
                }

            }
            else
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Back to previous position
        if (collision.gameObject.tag == "Wall")
            target = prepos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On Candy
        if (collision.gameObject.tag == "Candy")
        {
            onCandy = true;
        }
        // On trap
        if (collision.gameObject.tag == "Trap")
        {
            if (myName == "Left Star")
                gameManager.Counting();

            // Back to start position
            Spawn();

            // View alpha
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            Invoke("OffAlpha", 3);
        }

        // On Portal
        if (collision.gameObject == portalA && portalFlag == true)
        {
            transform.position = portalB.transform.position;
            target = transform.position;
            portalFlag = false;
            lastPortal = collision.gameObject;
        }
        if (collision.gameObject == portalB && portalFlag == true)
        {
            transform.position = portalA.transform.position;
            target = transform.position;
            portalFlag = false;
            lastPortal = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Away from candy
        if (collision.gameObject.tag == "Candy")
        {
            onCandy = false;
        }
        // Away from portal
        if (collision.gameObject.tag == "Portal" && collision.gameObject != lastPortal)
        {
            portalFlag = true;
        }
    }

    void OffAlpha()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Spawn()
    {
        //Spawn a star at setted location
        if (myName == "Left Star")
        {
            transform.position = leftStarPos[gameManager.stageIndex];
        }
        else
        {
            transform.position = rightStarPos[gameManager.stageIndex];
        }
        target = transform.position;
        isMoving = false;
    }

    // Moving by Button Click
    public void Move()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        if (isMoving == false)
        {
            if (gameManager.moveCount > 0)
            {
                prepos = transform.position;
                isMoving = true;

                if (buttonName.StartsWith("Down"))
                {
                    if (myName.StartsWith(buttonName[buttonName.Length - 1]))
                    {
                        target = new Vector2(transform.position.x, transform.position.y - 1);
                    }
                    else
                    {
                        target = new Vector2(transform.position.x, transform.position.y + 1);
                    }
                }
                else if (buttonName.StartsWith("Left"))
                {
                    if (myName.StartsWith(buttonName[buttonName.Length - 1]))
                    {
                        target = new Vector2(transform.position.x - 1, transform.position.y);
                    }
                    else
                    {
                        target = new Vector2(transform.position.x + 1, transform.position.y);
                    }
                }
                else if (buttonName.StartsWith("Up"))
                {
                    if (myName.StartsWith(buttonName[buttonName.Length - 1]))
                    {
                        target = new Vector2(transform.position.x, transform.position.y + 1);
                    }
                    else
                    {
                        target = new Vector2(transform.position.x, transform.position.y - 1);
                    }
                }
                else if (buttonName.StartsWith("Right"))
                {
                    if (myName.StartsWith(buttonName[buttonName.Length - 1]))
                    {
                        target = new Vector2(transform.position.x + 1, transform.position.y);
                    }
                    else
                    {
                        target = new Vector2(transform.position.x - 1, transform.position.y);
                    }
                }
            }
        }
    }
}
