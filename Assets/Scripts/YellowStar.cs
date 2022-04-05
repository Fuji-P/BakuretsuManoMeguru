using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowStar : MonoBehaviour
{
    private int _Score = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pumpkin" ||
            collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "BlueStar")
        {
            Destroy(gameObject);
        }
    }
    public int Score
    {
        set
        {
            _Score = value;
        }
        get
        {
            return _Score;
        }
    }
}
