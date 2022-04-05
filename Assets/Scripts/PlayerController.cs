using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float moveableRange = 6.2f;
    private float shot_speed = 600;
    //「YellowStar」の発射方向
    private float shot_ForceX = 0.0f;
    private float shot_ForceY = 0.0f;
    //ダメージ中判定
    private bool Damage = false;
    //リスポーン後判定
    private bool Resporn = false;
    //リスポーン用ボタン押下回数
    private int RespornCount = 0;
    //リスポーン用最大ボタン押下回数
    private int _RespornCountMax = 5;

    //オーディオソース群
    public AudioClip ShotSE;
    public AudioClip DamageSE;
    public AudioClip CountSE;
    public AudioClip HeartSE;
    AudioSource audioSource;

    //ダメージカウント用テキスト
    public Text CountText;
    public GameObject CountTextObj;

    //ダメージカウント用時間
    private float countTime = 0f;
    private float nextCountTime = 0f;
    private int Count = 0;

    private Animator animator;
    public GameManager gameManager;
    private SpriteRenderer renderer;
    [SerializeField] GameObject YellowStar;
    [SerializeField] GameObject BlueStar;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        CountTextObj.SetActive(false);
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //リスポーン後の点滅処理
        if (Resporn == true)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 20));
            renderer.color = new Color(1f, 1f, 1f, level);
        }
        if (Damage == true)
        {
            PlayerResporn();
            PlayerDamageCount();
            return;
        }

        if (Damage == false)
        {
            PlayerShot();
            playerMove();
            return;
        }
    }

    private void PlayerResporn()
    {
        if (Damage == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.X))
        {
            RespornCount += 1;
        }
        if (RespornCountMax <= RespornCount)
        {
            PlayerCountReset();
        }
    }

    private void PlayerCountReset()
    {
        animator.SetBool("Damage", false);
        CountTextObj.SetActive(false);
        Damage = false;
        Resporn = true;
        StartCoroutine(OnDamage());
        Count = 0;
        nextCountTime = 0;
        countTime = 0;
        RespornCount = 0;
        RespornCountMax += 1;
    }

    private void PlayerShot()
    {
        if (Damage == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Z) ||
            Input.GetKeyDown(KeyCode.X))
        {
            audioSource.PlayOneShot(ShotSE);
            GameObject BlueStarShot = Instantiate(BlueStar, transform.position, transform.rotation);
            BlueStarShot.GetComponent<Rigidbody2D>().AddForce(BlueStarShot.transform.up * shot_speed);
            Destroy(BlueStarShot, 0.6f);
        }
    }

    private void playerMove()
    {
        if (Damage == true)
        {
            return;
        }
        // 通常スピード
        speed = 6.0f;

        //Shift押下で低速化
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift))
        {
            speed = 3.0f;
        }

        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -moveableRange, moveableRange), transform.position.y);
    }

    private void PlayerDamageCount()
    {
        if (Damage == false)
        {
            return;
        }
        if(countTime >= nextCountTime)
        {
            PlayerCountTime();
        }
        countTime += Time.deltaTime;
    }

    private void PlayerCountTime()
    {
        audioSource.PlayOneShot(CountSE);
        Count++;
        CountText.text = Count.ToString();
        CountTextObj.SetActive(true);
        if(countTime >= 3)
        {
            PlayerGameOver();
            return;
        }
        nextCountTime = countTime + 1;
    }

    private void PlayerGameOver()
    {
        PlayerShotGameOver();
        Destroy(gameObject);
        gameManager.GameOverFlag = true;
    }

    private void PlayerShotGameOver()
    {
        //ゲームオーバー時星を4つ生成
        for (int i = 0; i < 4; i++)
        {
            GameObject YellowStarShot = Instantiate(YellowStar, transform.position, transform.rotation);
            YellowStarShot.GetComponent<YellowStar>().Score = 2;
            shot_ForceX = Random.Range(-1.0f, 1.0f);
            shot_ForceY = Random.Range(-1.0f, 1.0f);
            YellowStarShot.GetComponent<Rigidbody2D>().AddForce(new Vector2(shot_ForceX, shot_ForceY) * shot_speed);
            Destroy(YellowStarShot, 0.3f);
        }
    }

    private void PlayerDamage()
    {
        audioSource.PlayOneShot(DamageSE);
        animator.SetBool("Damage", true);
        Damage = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Heart")
        {
            //ハート取得
            if (RespornCountMax > 5)
            {
                audioSource.PlayOneShot(HeartSE);
                RespornCountMax -= 1;
            }
            Destroy(collider.gameObject);
        }
        if (Damage == true || Resporn == true)
        {
            return;
        }
        if (collider.gameObject.tag == "Pumpkin" ||
            collider.gameObject.tag == "YellowStar")
        {
            //ダメージ判定
            PlayerDamage();
        }
    }

    //点滅処理
    public IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(3.0f);
        Resporn = false;
        renderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public int RespornCountMax
    {
        set
        {
            _RespornCountMax = value;
        }
        get
        {
            return _RespornCountMax;
        }
    }
}
