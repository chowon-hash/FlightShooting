using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int power = 1;
    public int maxPower;
    public int boom;
    public int maxBoom;
    public float maxShotDelay;
    public float curShotDelay;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    Animator anim;

    public GameObject bulletA;
    public GameObject bulletB;
    public GameObject effectBoom;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public int life;
    public int score;

    public bool isHit;
    public bool isBoomTime;

    public GameObject[] followers;

    public bool isRespawnTime;

    public SpriteRenderer spriteRenderer;
    public bool[] joyControl;
    public bool isControl;

    public bool isButtonA;
    public bool isButtonB;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for (int i = 0; i < followers.Length; ++i)
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for (int i = 0; i < followers.Length; ++i)
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        Move2();
        //Move();
        Fire();
        Boom();
        Reload();
    }

    void Move2()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                      Input.mousePosition.y, distanceToScreen));

            float nextX = gameObject.transform.position.x + posMove.x;
            float nextY = gameObject.transform.position.y + posMove.y;

            //Debug.Log(nextX);

            //Debug.Log("player: " + gameObject.transform.position.x);

            if ((nextX <= 5.0 && nextX >= -5.0) ||
                (nextY <= 5.0 && nextY >= -5.0))
            {
                gameObject.transform.position = new Vector3(posMove.x, posMove.y, 0);

            } // Object Move Limited Android Screen
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount != 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = Camera.main.ScreenToWorldPoint(
                                     new Vector3(t.position.x, t.position.y, distanceToScreen));

                float nextX = gameObject.transform.position.x + posMove.x;
                float nextY = gameObject.transform.position.y + posMove.y;

                if ((nextX <= 5.0 && nextX >= -5.0) ||
                    (nextY <= 5.0 && nextY >= -5.0))
                {
                    gameObject.transform.position = new Vector3(posMove.x, posMove.y, 0);
                }
            }
        }
#endif
    }

    void Move()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || isControl == false)
            h = 0;
        
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || isControl == false)
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.localPosition = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }

    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;
    }

    void Fire()
    {
        //if (Input.GetButton("Fire1") == false)
        //    return;

        if (isButtonA == false)
            return;



        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.left * 0.1f;
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                bulletCC.transform.position = transform.position;
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }



        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        //if (Input.GetButton("Fire2") == false)
        //    return;

        if (isButtonB == false)
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;

        isBoomTime = true;

        gameManager.UpdateBoomIcon(boom);

        effectBoom.SetActive(true);
        Invoke("OffEffectBoom", 4f);

        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int i = 0; i < enemiesL.Length; ++i)
        {
            if (enemiesL[i].activeSelf)
            {
                var enemyLogic = enemiesL[i].GetComponent<Enemy>();
                enemyLogic.OnHit(100000);
            }
        }

        for (int i = 0; i < enemiesM.Length; ++i)
        {
            if (enemiesM[i].activeSelf)
            {
                var enemyLogic = enemiesM[i].GetComponent<Enemy>();
                enemyLogic.OnHit(100000);
            }
        }

        for (int i = 0; i < enemiesS.Length; ++i)
        {
            if (enemiesS[i].activeSelf)
            {
                var enemyLogic = enemiesS[i].GetComponent<Enemy>();
                enemyLogic.OnHit(100000);
            }
        }

        var enemyBulletA = objectManager.GetPool("BulletEnemyA");
        var enemyBulletB = objectManager.GetPool("BulletEnemyB");

        for (int i = 0; i < enemyBulletA.Length; ++i)
        {
            if (enemyBulletA[i].activeSelf)
            {
                enemyBulletA[i].SetActive(false);
            }
        }

        for (int i = 0; i < enemyBulletB.Length; ++i)
        {
            if (enemyBulletB[i].activeSelf)
            {
                enemyBulletB[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top": isTouchTop = true; break;
                case "Bottom": isTouchBottom = true; break;
                case "Right": isTouchRight = true; break;
                case "Left": isTouchLeft = true; break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Enemy Bullet")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;

            life--;
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "P");

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "ItemCoin":
                    score += 1000;
                    break;

                case "ItemPower":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                        
                    break;

                case "ItemBoom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }

    void OffEffectBoom()
    {
        effectBoom.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top": isTouchTop = false; break;
                case "Bottom": isTouchBottom = false; break;
                case "Right": isTouchRight = false; break;
                case "Left": isTouchLeft = false; break;
            }
        }
    }

    public void JoyPanel(int type)
    {
        for (int i = 0; i < 9; ++i)
        {
            joyControl[i] = i == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl = false;
    }
}
