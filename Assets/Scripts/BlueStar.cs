using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStar : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pumpkin" ||
            collision.gameObject.tag == "YellowStar")
        {
            Destroy(gameObject);
        }
    }
}
