using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public enum TargetType
    {
        all = 0,
        tileOnly,
        playerOnly,
        none
    }
    public enum MoveType
    {
        land = 0,
        sky
    }
    public int hp;
    public int tileAtk;
    public int playerAtk;
    //public float atkSpd;
    public float atkDelBefore;
    public float atkDelAfter;
    public float speed;
    public float attackRange;
    public float knockbackRange;
    public TargetType targetType;
    public MoveType moveType;
}

public class MonsterAi : MonoBehaviour
{
    public MonsterData monsterOriginData;
    public Monster monster = new Monster();

    Coroutine atkCoroutine;
    Coroutine hitCoroutine;

    public Transform _target;
    int flowerBeforCount = 0;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriteRenderer;
    Animator anim;
    void Awake()
    {
        MonsterSetting();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void MonsterSetting()
    {
        monster.hp = monsterOriginData.hp;
        monster.tileAtk = monsterOriginData.tileAtk;
        monster.playerAtk = monsterOriginData.playerAtk;
        monster.atkDelAfter = monsterOriginData.atkDelAfter;
        monster.atkDelBefore = monsterOriginData.atkDelBefore;
        monster.speed = monsterOriginData.speed;
        monster.speed += UnityEngine.Random.Range(-0.3f,0.3f);
        monster.attackRange = monsterOriginData.attackRange;
        monster.knockbackRange = monsterOriginData.knockbackRange;
        monster.targetType = monsterOriginData.targetType;
        monster.moveType = monsterOriginData.moveType;

    }
    private void Start()
    {
        StartCoroutine("TargetSearch");
    }


    void FixedUpdate()
    {
        if(_target != null)
        {
            if(hitCoroutine == null)
            {

                rigid.velocity = Vector2.zero;
                if (Vector2.Distance(_target.position, rigid.position) < monster.attackRange)
                {
                    if (atkCoroutine == null)
                    {

                        StartAttack();
                    }
                }
                else
                {
                    if (atkCoroutine != null)
                    {
                        StopAttack();
                    }
                    Vector2 direction = _target.position - transform.position;
                    direction.Normalize();
                    
                    spriteRenderer.flipX = _target.transform.position.x > transform.position.x?true : false;
                    Vector2 nextVec = direction * monster.speed * Time.fixedDeltaTime;
                    rigid.MovePosition(rigid.position + nextVec);
                    anim.SetBool("move", true);
                }
            }
            else
            {
            }
            
        }
    }

    IEnumerator TargetSearch()
    {
        while(true)
        {
            if(atkCoroutine == null)
            {
                if (_target == null)
                {
                    GetTarget();
                }
                else if (!GameManager.waveManager.flowerTileList.Contains(_target))
                {

                    if (monster.targetType == Monster.TargetType.all && _target == GameManager.player)
                    {
                        GetTarget();
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        _target = null;
                        GetTarget();

                    }
                }else if(GameManager.waveManager.flowerTileList.Count != flowerBeforCount)
                {
                    GetTarget();
                    yield return new WaitForSeconds(0.1f);
                }

            }

            yield return null;
        }
    }

    void GetTarget()
    {
        if(monster.targetType == Monster.TargetType.playerOnly)
        {
            _target = GameManager.player.transform;
        }
        else
        {
            if(GameManager.waveManager.flowerTileList.Count > 0)
            {
                int target = 0;
                int i;
                flowerBeforCount = GameManager.waveManager.flowerTileList.Count;
                float shortest = Vector2.Distance(GameManager.waveManager.flowerTileList[0].position, transform.position);
                for (i = 1; i < GameManager.waveManager.flowerTileList.Count; i++)
                {
                    float scanningDist = Vector2.Distance(GameManager.waveManager.flowerTileList[i].position, transform.position);
                    if (shortest > scanningDist)
                    {
                        target = i;
                        shortest = scanningDist;
                    }
                }
                _target = GameManager.waveManager.flowerTileList[target];
            }else if(monster.targetType == Monster.TargetType.all)
            {
                    _target = GameManager.player.transform;
                    //플레이어 타겟 지정
            }
        }
        
        
    }

    void StartAttack()
    {
        atkCoroutine = StartCoroutine(Attack());
    }
    
    void StopAttack()
    {
        if(atkCoroutine != null)
        {
            StopCoroutine(atkCoroutine);
            atkCoroutine = null;
        }
    }
    
    IEnumerator Attack()
    {
        anim.SetBool("move", false);
        yield return new WaitForSeconds(monster.atkDelBefore);
        if (_target != null && (GameManager.waveManager.flowerTileList.Contains(_target) || _target == GameManager.player.transform))
        {
            anim.SetTrigger("Attack");
            if(_target.gameObject.layer == 9) //Tile
            {
                _target.GetComponent<Tile>().TileDamage(monster.tileAtk);
                    
                if(_target.GetComponent<Tile>().tileData.tileHp < 1)
                {
                    GameManager.waveManager.flowerTileList.Remove(_target);
                    _target = null;
                }

            }
            else if(_target.gameObject.layer == 6) //Player
            {
                _target.GetComponent<Player>().StartCoroutine(_target.GetComponent<Player>().PlayerHit(this.gameObject));
            }
        }
        else
        {
            StopAttack();
            
        }
        
        yield return new WaitForSeconds(monster.atkDelAfter);
        anim.SetBool("move", true);

        atkCoroutine = null;
    }



    void StartHit(Collider2D collision)
    {
        hitCoroutine = StartCoroutine(Hit(collision));
    }

    void StopHit()
    {
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
            hitCoroutine = null;
        }
    }

    IEnumerator Hit(Collider2D collision)
    {
        anim.SetBool("move", false);
        rigid.velocity = Vector3.zero;
        monster.hp -= collision.GetComponent<Sword>().DamageInt();
        Vector2 AttackerPos = collision.GetComponent<Sword>().ParentPosition();
        Vector2 thisPos = transform.position;
        Vector2 direction = (AttackerPos - thisPos).normalized ;
        rigid.AddForce(-direction * monster.knockbackRange, ForceMode2D.Impulse);
        anim.SetTrigger("Hit");
        if (monster.hp > 0)
        {
            //live
        }
        else
        {

            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rigid.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
            //death
        }
        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;
        hitCoroutine = null;
        anim.SetBool("move", true);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Sword"))
        {
            return;
        }
        if(hitCoroutine == null)
        {
            StartHit(collision);
        }
    }
    
}