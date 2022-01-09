using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    int maxStage = 3;

    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjects;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList = new List<Spawn>();
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        enemyObjects = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };

        ReadSpawnFile();

        StageStart();
    }

    public void StageStart()
    {
        stageAnim.SetTrigger("On");

        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart";
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear";

        ReadSpawnFile();

        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        clearAnim.SetTrigger("On");

        fadeAnim.SetTrigger("Out");

        player.transform.position = playerPos.position;

        stage++;

        if (stage > maxStage)
            Invoke("GameOver", 6);
        else
            Invoke("StageStart", 5);
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        var textFile = Resources.Load<TextAsset>("Table/Stage_" + stage);
        if (textFile == null)
            return;

        var strReader = new StringReader(textFile.text);

        while (strReader != null)
        {
            var line = strReader.ReadLine();

            Debug.Log(line);

            if (line == null)
                break;

            var spawnData = new Spawn();
            var datas = line.Split(',');
            spawnData.delay = float.Parse(datas[0]);
            spawnData.type = datas[1];
            spawnData.point = int.Parse(datas[2]);
            spawnList.Add(spawnData);
        }

        strReader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && spawnEnd == false)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;

        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;

            case "M":
                enemyIndex = 1;
                break;

            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        var enemy = objectManager.MakeObj(enemyObjects[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        enemy.transform.rotation = spawnPoints[enemyPoint].rotation;

        var enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;
        enemyLogic.health = 1;

        var rigid = enemy.GetComponent<Rigidbody2D>();

        // left spawn
        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        // right spwan
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        spawnIndex++;

        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; ++index)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; ++index)
        {
            lifeImage[index].color = new Color(1, 1, 1,1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; ++index)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; ++index)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        var explosion = objectManager.MakeObj("Explosion");
        var explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
