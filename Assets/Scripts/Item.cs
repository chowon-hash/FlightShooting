using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        var rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * 1.5f;
    }
}
