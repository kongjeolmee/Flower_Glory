using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public SwordData swordData { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Collider2D swordCol;
    private Animator animator;

    private void Awake()
    {

        swordData = Resources.Load<SwordData>($"ScriptableScripts/Sword/Sword{GameManager.Instance.SwordType}");
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = swordData.swordSprite;
        swordCol = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        
    }

    

    public int DamageInt()
    {
        int finalDam = 0;
        if(Random.Range(0,10) < 2)
        {
            Debug.Log("Å©¸®Æ¼ÄÃ!");
            finalDam = swordData.damage + (swordData.damage / 2);
        }
        else
        {
            int randomValue = Random.Range(-swordData.damage/3, swordData.damage / 3 + 1);
            Debug.Log("·£´ý ¹ë·ù: " + randomValue);
            finalDam = swordData.damage + randomValue;
        }
        Debug.Log(finalDam);
        return finalDam;
    }
    public IEnumerator Attack()
    {
        spriteRenderer.enabled = true;
        swordCol.enabled = true;
        GameManager.player.IsAttack = true;
        animator.SetTrigger("Atk");
        /*
        if (spriteRenderer.flipX)
        {
            transform.localPosition = new Vector3(0.134f, 0, 0);
            for (int i = 90; i >= -90; i -= 5)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(0.00001f);
            }
        }
        else
        {
            transform.localPosition = new Vector3(-0.134f, 0, 0);

            for (int i = -90; i <= 90; i += 5)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(0.00001f);
            }
        }*/
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {

            SPXManager.instance.PlayOneShot((int)EffectClips.Sword1);
        }
        else
        {

            SPXManager.instance.PlayOneShot((int)EffectClips.Sword2);
        }
        yield return new WaitForSeconds(0.2f);

        spriteRenderer.enabled = false;
        swordCol.enabled = false;
        GameManager.player.IsAttack = false;



    }




    public Vector2 ParentPosition()
    {
        return transform.parent.position;
    }
}
