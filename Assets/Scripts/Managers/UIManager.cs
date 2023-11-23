using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image waterGauge;

    float playerMaxWater;
    float currentSwordCool = 0f;
    float swordCoolMax;
    [SerializeField]
    GameObject sword;
    [SerializeField]
    Image swordCoolImg;

    [SerializeField]
    GameObject heartPref;
    List<GameObject> hpObject = new List<GameObject>();

    int hpCount;

    [SerializeField]
    Image timeBar;

    [SerializeField]
    TextMeshProUGUI waveText;


    private void Awake()
    {
        for (int i = 0; i < GameManager.Instance.MaxHeart; i++)
        {
            hpObject.Add(Instantiate(heartPref,GameObject.Find("UICanvas").transform.GetChild(5)));
        }

    }
    private void Start()
    {
        hpCount = GameManager.Instance.MaxHeart;
        playerMaxWater = GameManager.player.MaxWater;
        swordCoolMax = GameManager.Instance.SwordCool;

    }

    private void Update()
    {
        waterGauge.fillAmount = GameManager.player.WaterAmount / playerMaxWater;
        timeBar.fillAmount = GameManager.waveManager.currentTime / GameManager.waveManager.limitTime;
        if(currentSwordCool > 0)
        {
            currentSwordCool -= Time.deltaTime;
            swordCoolImg.fillAmount = currentSwordCool / swordCoolMax;

        }
        

        if(hpCount != GameManager.player.hp)
        {
            if(hpCount < GameManager.player.hp)
            {
                hpCount++;
                hpObject[hpCount].GetComponent<Heart>().HeartSprite(true);
            }
            else
            {
                hpCount--;
                hpObject[hpCount].GetComponent<Heart>().HeartSprite(false);
            }
        }
    }

    public void OnWaterDown()
    {
        GameManager.player.IsWater = true;
        GameManager.player.StartCoroutine("GiveWater");
    }

    public void OnWaterUp()
    {
        GameManager.player.IsWater = false;
    }

    public void OnClickAttack()
    {
        if(currentSwordCool <= 0)
        {
            sword.GetComponent<Sword>().StartCoroutine("Attack");
            currentSwordCool = swordCoolMax;
        }
    }

}
