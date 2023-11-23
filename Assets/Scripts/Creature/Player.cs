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
    public GameObject[] dirCols;
    public List<Tile> reachTileList = new List<Tile>();
    private SpriteRenderer spriteRenderer;
    bool isWater = false;
    bool isDam = false;
    [SerializeField] float waterAmount;
    float maxWater = 15;
    private Rigidbody2D rb;
    [SerializeField] GameObject sword;

    public int hp;
    float invisibleTime = 1.5f;

    float waterFullTime_p;

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
                DirsController(false, Dirs.Right);
                DirsController(false, Dirs.Down);
                DirsController(false, Dirs.Left);
                rb.velocity = new Vector2(0, speed);

                DirsController(true, Dirs.Up);

            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                DirsController(false, Dirs.Right);
                DirsController(false, Dirs.Up);
                DirsController(false, Dirs.Left);
                rb.velocity = new Vector2(0, -speed);

                DirsController(true, Dirs.Down);

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = false;
                sword.transform.localPosition = new Vector3(-0.134f, 0, 0);
                sword.transform.localScale = new Vector3(0.6f, 0.6f, 1);

                DirsController(false, Dirs.Right);
                DirsController(false, Dirs.Up);
                DirsController(false, Dirs.Down);
                rb.velocity = new Vector2(-speed, 0);
                DirsController(true, Dirs.Left);

            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = true;
                sword.transform.localPosition = new Vector3(0.134f, 0, 0);
                sword.transform.localScale = new Vector3(-0.6f, 0.6f, 1);

                DirsController(false, Dirs.Left);
                DirsController(false, Dirs.Up);
                DirsController(false, Dirs.Down);
                rb.velocity = new Vector2(speed, 0);
                DirsController(true, Dirs.Right);


            }
#endif
        }

    }

    void Move()
    {
        moveVector = Vector2.zero;
        moveVector.x = _joystick.Horizontal * speed * Time.deltaTime;
        moveVector.y = _joystick.Vertical * speed * Time.deltaTime;

        if(_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            DirsController();
            //애니메이터 walk 키기
        }else if(_joystick.Horizontal == 0 && _joystick.Vertical == 0)
        {
            //애니메이터 idle 키기
        }

        rb.velocity = moveVector;

    }

    void DirsController(bool setActive, Dirs dirs)
    {
        //dirCols[(int)dirs].GetComponent<SpriteRenderer>().enabled = setActive;
        dirCols[(int)dirs].GetComponent<CircleCollider2D>().enabled = setActive;
    }

    void DirsController()
    {
        if (!isWater && !IsAttack)
        {
            if (_joystick.Horizontal > 0)
            {
                spriteRenderer.flipX = true;
                sword.transform.localPosition = new Vector3(0.134f, 0, 0);
                sword.transform.localScale = new Vector3(-0.6f, 0.6f, 1);

                if (_joystick.Vertical >= 0)
                {
                    if (_joystick.Horizontal > _joystick.Vertical)
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                    else
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                }
                else
                {
                    if (_joystick.Horizontal > _joystick.Vertical * -1)
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                    else
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                }


            }
            else if (_joystick.Horizontal < 0)
            {
                spriteRenderer.flipX = false;
                sword.transform.localPosition = new Vector3(-0.134f, 0, 0);
                sword.transform.localScale = new Vector3(0.6f, 0.6f, 1);

                if (_joystick.Vertical >= 0)
                {
                    if (_joystick.Horizontal * -1 > _joystick.Vertical)
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                }
                else
                {
                    if (_joystick.Horizontal * -1 > _joystick.Vertical * -1)
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else
                    {
                        dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = true;
                        dirCols[(int)Dirs.Right].GetComponent<CircleCollider2D>().enabled = false;
                        dirCols[(int)Dirs.Left].GetComponent<CircleCollider2D>().enabled = false;
                    }
                }

            }
            else if (_joystick.Horizontal == 0 && _joystick.Vertical != 0)
            {
                if (_joystick.Vertical > 0)
                {

                    dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = true;
                    dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = false;
                }
                else if (_joystick.Vertical < 0)
                {
                    dirCols[(int)Dirs.Down].GetComponent<CircleCollider2D>().enabled = true;
                    dirCols[(int)Dirs.Up].GetComponent<CircleCollider2D>().enabled = false;
                }
            }
        }
        
        
    }

    
    IEnumerator GiveWater()
    {
        if (reachTileList.Count > 0)
        {
            if (reachTileList.Last().tileData.tileType == TileData.TileType.Water)
            {
                waterAmount = maxWater;
                Debug.Log("물을 채웠다!");
                isWater = false;
            }
            else
            {
                if (reachTileList.Last().tileData.tileHp < 3)
                {
                    while (isWater)
                    {

                        if (waterAmount > 0)
                        {
                            if (reachTileList.Last().tileData.tileWater < GameManager.Instance.WaterFullTime)
                            {
                                yield return new WaitForSeconds(Time.deltaTime);
                                reachTileList.Last().waterProcess.SetActive(true);
                                reachTileList.Last().tileData.tileWater += Time.deltaTime;
                                waterAmount -= Time.deltaTime;


                            }
                            else
                            {
                                if (reachTileList.Last().tileData.tileHp == 0) GameManager.waveManager.flowerTileList.Add(reachTileList.Last().transform);
                                reachTileList.Last().tileData.tileHp = 3;
                                reachTileList.Last().tileData.tileWater = 0;
                                reachTileList.Last().waterProcess.SetActive(false);

                                Debug.Log(reachTileList.Last() + "에게 물을 주었다!");
                                reachTileList.Last().TileTypeFlowerCheck();

                                isWater = false;
                                yield break;
                            }
                        }
                        else
                        {
                            Debug.Log("물이 부족하다!");
                            isWater = false;
                            yield break;
                        }
                        
                    }
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
            Debug.Log("땅이 없다!");
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
            Debug.Log("맞음");
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
