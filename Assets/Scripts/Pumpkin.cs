using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pumpkin : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    float Force_X = 0.0f;
    public AudioClip DestroySE;
    public AudioClip ItemSE;
    GameManager gameManager;
    float shot_speed = 600f;
    float shot_ForceX = 0.0f;
    float shot_ForceY = 0.0f;
    [SerializeField] GameObject YellowStar;
    public int Score = 1;
    [SerializeField] GameObject PointTextObj;
    private bool CollisionFlag;
    [SerializeField] GameObject Heart;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        CollisionFlag = false;
        PumpkinMove();
    }

    private void PumpkinMove()
    {
        Force_X = Random.Range(-2.0f, 2.0f);
        while(Force_X == 0)
        {
            Force_X = Random.Range(-2.0f, 2.0f);
        }
        if (Force_X > 0 &&
            Force_X < 2.0)
        {
            Force_X = 2.0f;
        }
        else if (Force_X > -2.0 &&
            Force_X < 0)
        {
            Force_X = -2.0f;
        }
        if (transform.position.x >= 4.0f)
        {
            Force_X = -2.0f;
        }
        else if (transform.position.x <= -4.0f) {
            Force_X = 2.0f;
        }
        Vector2 force = new Vector2(Force_X * 200, 0);
        rigidbody2D.AddForce(force);
    }

    private void PumpkinDestroy()
    {
        //ハート生成
        if (Score == 9999)
        {
            PumpkinHeart();
        }
        else
        {
            AudioSource.PlayClipAtPoint(DestroySE, new Vector3(0, 3, -10));
        }
        Destroy(gameObject);
        gameManager.AddScore(Score);
    }

    private void PumpkinHeart()
    {
        AudioSource.PlayClipAtPoint(ItemSE, new Vector3(0, 3, -10));
        if (gameManager.GameOverFlag == true)
        {
            return;
        }
        int count = GameObject.FindGameObjectsWithTag("Heart").Length;
        Player = GameObject.Find("Hiori3");
        if (Player.GetComponent<PlayerController>().RespornCountMax == 5 ||
            Player.GetComponent<PlayerController>().RespornCountMax - count <= 5)
        {
            return;
        }
        float HeartPosX = Random.Range(-6.0f, 6.0f);
        Instantiate(Heart, new Vector2(HeartPosX, 6), Quaternion.identity);
    }

    private void PumpkinShot()
    {
        //星4つ生成
        for (int i = 0; i < 4; i++)
        {
            GameObject YellowStarShot = Instantiate(YellowStar, transform.position, transform.rotation);
            shot_ForceX = Random.Range(-1.0f, 1.0f);
            shot_ForceY = Random.Range(-1.0f, 1.0f);
            YellowStarShot.GetComponent<Rigidbody2D>().AddForce(new Vector2(shot_ForceX, shot_ForceY) * shot_speed);
            //スコア乗算処理
            YellowStarShot.GetComponent<YellowStar>().Score = Score * 2;
            if (YellowStarShot.GetComponent<YellowStar>().Score >= 9999)
            {
                YellowStarShot.GetComponent<YellowStar>().Score = 9999;
            }
            Destroy(YellowStarShot, 0.3f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollisionFlag == true)
        {
            return;
        }
        if (collision.gameObject.tag != "BlueStar" &&
            collision.gameObject.tag != "YellowStar")
        {
            return;
        }
        if (collision.gameObject.tag == "BlueStar")
        {
            Score = 1;
        }
        if (collision.gameObject.tag == "YellowStar")
        {
            //スコア乗算処理
            Score = collision.gameObject.GetComponent<YellowStar>().Score;
            GameObject pointTextGameObject = Instantiate(PointTextObj, transform.position, transform.rotation);
            pointTextGameObject.GetComponent<Point>().Score = Score;
            Destroy(pointTextGameObject, 0.6f);
        }
        PumpkinDestroy();
        PumpkinShot();
        CollisionFlag = true;
    }
}
