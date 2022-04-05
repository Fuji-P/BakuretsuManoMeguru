using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinGenerator : MonoBehaviour
{
    public GameObject gameObject;
    private float interval = 0.3f;
    private float StartPos_X = 0.0f;
    private int CountMax = 64;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObj", 0.1f, interval);
    }

    void SpawnObj()
    {
        //カボチャを最大個数まで生成
        int count = GameObject.FindGameObjectsWithTag("Pumpkin").Length;
        if (count <= CountMax)
        {
            StartPos_X = Random.Range(-6.0f, 6.0f);
            Instantiate(gameObject, new Vector2(StartPos_X, transform.position.y), transform.rotation);
        }
    }
}
