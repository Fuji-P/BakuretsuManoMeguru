using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float moveableRange = 6.2f;
    float shot_speed = 600;
    [SerializeField] GameObject BlueStar;
    private Animator animator;
    private bool Damage = false;
    private bool Resporn = false;
    private int RespornCount = 0;
    private static int _RespornCountMax = 5;
    public AudioClip ShotSE;
    public AudioClip DamageSE;
    public AudioClip CountSE;
    public AudioClip HeartSE;
    AudioSource audioSource;
    public GameObject CountTextObj;
    public Text CountText;
    private float countTime = 0f;
    private float nextCountTime = 0f;
    private int Count = 0;
    float shot_ForceX = 0.0f;
    float shot_ForceY = 0.0f;
    [SerializeField] GameObject YellowStar;
    public GameManager gameManager;
    private SpriteRenderer renderer;

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
    private void PlayerDamageCount()
    {
        if (Damage == false)
        {
            return;
        }
        if(countTime >= nextCountTime)
        {
            audioSource.PlayOneShot(CountSE);
            Count++;
            CountText.text = Count.ToString();
            CountTextObj.SetActive(true);
            if(countTime >= 3)
            {
                //ゲームオーバー
                PlayerShotGameOver();
                Destroy(gameObject);
                gameManager.GameOverFlag = true;
                return;
            }
            nextCountTime = countTime + 1;
        }
        countTime += Time.deltaTime;
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
            CountReset();
        }
    }

    private void CountReset()
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
            audioSource.PlayOneShot(DamageSE);
            animator.SetBool("Damage", true);
            Damage = true;
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
