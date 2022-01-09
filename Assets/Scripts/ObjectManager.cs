using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject enemyBPrefab;

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    public GameObject BulletPlayerAPrefab;
    public GameObject BulletPlayerBPrefab;
    public GameObject BulletEnemyAPrefab;
    public GameObject BulletEnemyBPrefab;

    public GameObject BulletFollowerPrefab;

    public GameObject BulletBossAPrefab;
    public GameObject BulletBossBPrefab;

    public GameObject ExplosionPrefab;


    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] enemyB;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;

    GameObject[] bulletBossA;
    GameObject[] bulletBossB;

    GameObject[] bulletFollower;

    GameObject[] explosion;

    GameObject[] targetPool;

    private void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];
        enemyB = new GameObject[10];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];

        bulletFollower = new GameObject[100];

        bulletBossA = new GameObject[100];
        bulletBossB = new GameObject[1000];

        explosion = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < enemyL.Length; ++i) {
            enemyL[i] = Instantiate(enemyLPrefab); 
            enemyL[i].SetActive(false); 
        }
        for (int i = 0; i < enemyM.Length; ++i) { 
            enemyM[i] = Instantiate(enemyMPrefab); 
            enemyM[i].SetActive(false); 
        }
        for (int i = 0; i < enemyS.Length; ++i) { 
            enemyS[i] = Instantiate(enemySPrefab); 
            enemyS[i].SetActive(false); 
        }
        for (int i = 0; i < enemyB.Length; ++i)
        {
            enemyB[i] = Instantiate(enemyBPrefab);
            enemyB[i].SetActive(false);
        }

        for (int i = 0; i < itemCoin.Length; ++i) { 
            itemCoin[i] = Instantiate(itemCoinPrefab); 
            itemCoin[i].SetActive(false); 
        }
        for (int i = 0; i < itemPower.Length; ++i) { 
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; ++i) { 
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        for (int i = 0; i < bulletPlayerA.Length; ++i) { 
            bulletPlayerA[i] = Instantiate(BulletPlayerAPrefab); 
            bulletPlayerA[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayerB.Length; ++i) {
            bulletPlayerB[i] = Instantiate(BulletPlayerBPrefab); 
            bulletPlayerB[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyA.Length; ++i) {
            bulletEnemyA[i] = Instantiate(BulletEnemyAPrefab); 
            bulletEnemyA[i].SetActive(false); 
        }
        for (int i = 0; i < bulletEnemyB.Length; ++i) { 
            bulletEnemyB[i] = Instantiate(BulletEnemyBPrefab);
            bulletEnemyB[i].SetActive(false);
        }
        for (int i = 0; i < bulletFollower.Length; ++i){
            bulletFollower[i] = Instantiate(BulletFollowerPrefab);
            bulletFollower[i].SetActive(false);
        }
        for (int i = 0; i < bulletBossA.Length; ++i)
        {
            bulletBossA[i] = Instantiate(BulletBossAPrefab);
            bulletBossA[i].SetActive(false);
        }
        for (int i = 0; i < bulletBossB.Length; ++i)
        {
            bulletBossB[i] = Instantiate(BulletBossBPrefab);
            bulletBossB[i].SetActive(false);
        }
        for (int i = 0; i < explosion.Length; ++i)
        {
            explosion[i] = Instantiate(ExplosionPrefab);
            explosion[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "EnemyL": targetPool = enemyL; break;
            case "EnemyM": targetPool = enemyM; break;
            case "EnemyS": targetPool = enemyS; break;
            case "EnemyB": targetPool = enemyB; break;

            case "ItemCoin": targetPool = itemCoin; break;
            case "ItemPower": targetPool = itemPower; break;
            case "ItemBoom": targetPool = itemBoom; break;

            case "BulletPlayerA": targetPool = bulletPlayerA; break;
            case "BulletPlayerB": targetPool = bulletPlayerB; break;
            case "BulletEnemyA": targetPool = bulletEnemyA; break;
            case "BulletEnemyB": targetPool = bulletEnemyB; break;
            case "BulletFollower": targetPool = bulletFollower;break;

            case "BulletBossA": targetPool = bulletBossA; break;
            case "BulletBossB": targetPool = bulletBossB; break;

            case "Explosion": targetPool = explosion; break;
        }

        for (int i = 0; i < targetPool.Length; ++i)
        {
            if (targetPool[i].activeSelf == false)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL": targetPool = enemyL; break;
            case "EnemyM": targetPool = enemyM; break;
            case "EnemyS": targetPool = enemyS; break;
            case "EnemyB": targetPool = enemyS; break;

            case "ItemCoin": targetPool = itemCoin; break;
            case "ItemPower": targetPool = itemPower; break;
            case "ItemBoom": targetPool = itemBoom; break;

            case "BulletPlayerA": targetPool = bulletPlayerA; break;
            case "BulletPlayerB": targetPool = bulletPlayerB; break;
            case "BulletEnemyA": targetPool = bulletEnemyA; break;
            case "BulletEnemyB": targetPool = bulletEnemyB; break;
            case "BulletFollwer": targetPool = bulletFollower; break;

            case "BulletBossA": targetPool = bulletBossA; break;
            case "BulletBossB": targetPool = bulletBossB; break;

            case "Explosion": targetPool = explosion; break;
        }

        return targetPool;
    }

    public void DeleteAllObject(string type)
    {
        if (type == "Boss")
        {
            for (int i = 0; i < bulletBossA.Length; ++i)
                bulletBossA[i].SetActive(false);

            for (int i = 0; i < bulletBossB.Length; ++i)
                bulletBossB[i].SetActive(false);
        }
    }
}
