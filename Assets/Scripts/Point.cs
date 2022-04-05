using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Point : MonoBehaviour
{
    [SerializeField] GameObject YellowStar;
    public Text PointText;
    private int _Score = 0;

    // Start is called before the first frame update
    void Start()
    {
        PointText.text = Score.ToString();
        if (Score == 64)
        {
            PointText.color = Color.red;
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
