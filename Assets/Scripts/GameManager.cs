using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text hiScoreText;
    static int score = 0;
    private bool Retry = false;
    private static bool _GameOver = false;
    public GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
//        Time.timeScale = 0.3f;
        score = 0;
        scoreText.text = score.ToString();
        hiScoreText.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        _GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Retry == true)
        {
            PlayerRetry();
            return;
        }
        if(_GameOver == true)
        {
            PlayerGameOver();
            return;
        }
    }
    public void AddScore(int s)
    {
        score += s;
        scoreText.text = score.ToString();
        if (score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", score);
            hiScoreText.text = score.ToString();
        }
        return;
    }

    public void PlayerGameOver()
    {
        int count = GameObject.FindGameObjectsWithTag("YellowStar").Length;
        if (count <= 0)
        {
            GameOver.SetActive(true);
            Retry = true;
        }
    }

    public void PlayerRetry()
    {
        if (Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public bool GameOverFlag
    {
        set
        {
            _GameOver = value;
        }
        get
        {
            return _GameOver;
        }
    }
}
