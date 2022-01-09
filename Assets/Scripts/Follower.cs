using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objectManager;

    public Vector3 followPosition;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> queParentPosision;

    private void Awake()
    {
        queParentPosision = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        if (queParentPosision.Contains(parent.position) == false)
            queParentPosision.Enqueue(parent.position);

        if (queParentPosision.Count > followDelay)
            followPosition = queParentPosision.Dequeue();
        else if (queParentPosision.Count < followDelay)
            followPosition = parent.position;
    }

    void Follow()
    {
        transform.position = followPosition;
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") == false)
            return;

        if (curShotDelay < maxShotDelay)
            return;

        var bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;
        var rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
