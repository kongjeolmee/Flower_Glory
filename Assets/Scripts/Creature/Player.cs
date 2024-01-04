using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 0.005f;
    [SerializeField] FloatingJoystick _joystick;
    Vector2 moveVector;
    public GameObject dirCol;
    public List<Tile> reachTileList = new List<Tile>();
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    bool isWater = false;
    bool isDam = false;
    [SerializeField] float waterAmount;
    float maxWater = 15;
    private Rigidbody2D rb;
    [SerializeField] GameObject sword;

    private int hp;
    float invisibleTime = 1.5f;

    float waterFullTime_p;

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public bool IsWater
    {
        get { return isWater; }
        set { isWater = value; }
    }
    public bool IsAttack { get; set; }

    public float WaterAmount
    {
        get { return waterAmount; }
        set { waterAmount = value; }
    }

    public float MaxWater
    {
        get { return maxWater; }
        set { maxWater = value; }
    }

    enum Dirs
    {
        Up,
        Left,
        Down,
        Right
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        waterAmount = maxWater;
        waterFullTime_p = GameManager.Instance.WaterFullTime;
        hp = GameManager.Instance.MaxHeart;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {

        rb.velocity = Vector2.zero;
        if (!isWater && !IsAttack)
        {
            Move();

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.velocity = new Vector2(0, speed);

            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.velocity = new Vector2(0, -speed);

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = false;
                sword.transform.localPosition = new Vector3(-0.134f, 0, 0);
                sword.transform.localScale = new Vector3(0.6f, 0.6f, 1);

                rb.velocity = new Vector2(-speed, 0);

            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = true;
                sword.transform.localPosition = new Vector3(0.134f, 0, 0);
                sword.transform.localScale = new Vector3(-0.6f, 0.6f, 1);

                rb.velocity = new Vector2(speed, 0);


            }
#endif
        }

    }

    void Move()
    {
        moveVector = Vector2.zero;
        moveVector.x = _joystick.Horizontal * speed * Time.deltaTime;
        moveVector.y = _joystick.Vertical * speed * Time.deltaTime;


        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            anim.SetBool("Move", true);
            float angle = Mathf.Atan2(_joystick.Vertical, _joystick.Horizontal) * Mathf.Rad2Deg;
            dirCol.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (_joystick.Horizontal > 0)
            {
                spriteRenderer.flipX = true;
                sword.transform.localPosition = new Vector3(0.134f, 0, 0);
                sword.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            }
            else
            {
                spriteRenderer.flipX = false;
                sword.transform.localPosition = new Vector3(-0.134f, 0, 0);
                sword.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }
        }
        else if(_joystick.Horizontal == 0 && _joystick.Vertical == 0)
        {
            anim.SetBool("Move", false);
        }

        rb.velocity = moveVector;

    }




    
    IEnumerator GiveWater()
    {
        if (reachTileList.Count > 0)
        {
            if (reachTileList.Last().tileData.tileType == TileData.TileType.Water)
            {
                waterAmount = maxWater;
                SPXManager.instance.PlayOneShot((int)EffectClips.WaterWall);
                isWater = false;
            }
            else
            {
                if (reachTileList.Last().tileData.tileHp < 3)
                {
                    SPXManager.instance.PlaySound((int)EffectClips.Watering);

                    while (isWater)
                    {

                        if (waterAmount > 0)
                        {

                            if (reachTileList.Last().tileData.tileWater < GameManager.Instance.WaterFullTime)
                            {
                                yield return new WaitForSeconds(Time.deltaTime);
                                if (reachTileList.Last().waterProcess != null)
                                { reachTileList.Last().waterProcess.SetActive(true); }
                                reachTileList.Last().tileData.tileWater += Time.deltaTime;
                                waterAmount -= Time.deltaTime;


                            }
                            else
                            {
                                if (reachTileList.Last().tileData.tileHp == 0) GameManager.waveManager.flowerTileList.Add(reachTileList.Last().transform);
                                reachTileList.Last().tileData.tileHp = 3;
                                reachTileList.Last().tileData.tileWater = 0;
                                reachTileList.Last().waterProcess.SetActive(false);

                                reachTileList.Last().TileTypeFlowerCheck();
                                //GameManager.waveManager.UpdateFlowerTile();
                                SPXManager.instance.Stop();
                                isWater = false;
                                GameManager.Instance.flowerCount++;
                                yield break;
                            }
                        }
                        else
                        {
                            SPXManager.instance.PlayOneShot((int)EffectClips.No);
                            isWater = false;
                            yield break;
                        }
                        
                    }
                    SPXManager.instance.Stop();

                }
                else
                {
                    isWater = false;
                    yield break;
                }

            }

        }
        else
        {
            isWater = false;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 7) return;

        StartCoroutine(PlayerHit(collision.gameObject));
        
    }

    public IEnumerator PlayerHit(GameObject attacker)
    {
        if (!isDam)
        {
            SPXManager.instance.PlayOneShot((int)EffectClips.PlayerDamage);

            gameObject.layer = 12;
            hp -= attacker.GetComponent<MonsterAi>().monster.playerAtk;
            isDam = true;
            rb.velocity = Vector3.zero;
            spriteRenderer.color = new Color(255, 255, 255, 0.5f);
            yield return new WaitForSeconds(invisibleTime);
            rb.velocity = Vector2.zero;
            spriteRenderer.color = new Color(255, 255, 255, 1);
            isDam = false;
            gameObject.layer = 6;
        }

    }

}
