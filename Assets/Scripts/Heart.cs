using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    Rigidbody2D itemRd2d;
    //滞在時間
    public float deleteTime = 1.3f;

    // Start is called before the first frame update
    void Start()
    {
        itemRd2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        itemRd2d.velocity = transform.up * -1.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Range")
        {
            Destroy(gameObject, deleteTime);
        }
    }
}
