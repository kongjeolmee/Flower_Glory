using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    TextMeshProUGUI currentWaveText;
    [SerializeField]
    GameObject waveCheckObject;
    [SerializeField]
    Animator waveCheckAnimator;

    [SerializeField]
    GameObject resultGo;
    [SerializeField]
    TextMeshProUGUI finalWaveText;
    [SerializeField]
    TextMeshProUGUI finalMonsterText;
    [SerializeField]
    TextMeshProUGUI finalFlowerText;
    [SerializeField]
    TextMeshProUGUI finalCurrentFlowerText;

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

        currentWaveText.text = GameManager.waveManager.CurrentWave.ToString();
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
        

        if(hpCount != GameManager.player.Hp)
        {
            if(hpCount < GameManager.player.Hp)
            {
                hpCount++;
                hpObject[hpCount].GetComponent<Heart>().HeartSprite(true);
            }
            else
            {
                hpCount--;
                hpObject[hpCount].GetComponent<Heart>().HeartSprite(false);
                if(hpCount == 0)
                {
                    StartCoroutine(WaveCheckLoading(false));
                }
            }
        }
    }


    public void OnClickButton(int type)
    {
        if (type == 0)
        {
            SceneManager.LoadScene("InGame");
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

    public IEnumerator WaveCheckLoading(bool clear)
    {
        Time.timeScale = 0;
        if (clear)
        {
            //¼º°ø
            SPXManager.instance.PlayOneShot((int)EffectClips.Clear);
            waveCheckObject.SetActive(true);
            waveCheckAnimator.SetInteger("GameState", 1);
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(GameManager.waveManager.NextWave());
        }
        else
        {
            SPXManager.instance.PlayOneShot((int)EffectClips.Failed);
            waveCheckObject.SetActive(true);
            waveCheckAnimator.SetInteger("GameState", 2);
            yield return new WaitForSecondsRealtime(2f);
            GameManager.Instance.GameOver = true;
            GameOverResult();

        }

        currentWaveText.text = GameManager.waveManager.CurrentWave.ToString();
        waveCheckAnimator.SetInteger("GameState", 0);
        waveCheckObject.SetActive(false);

        yield return null;
    }

    public void GameOverResult()
    {
        resultGo.SetActive(true);
        finalWaveText.text = $"ÃÖ´ë ¿þÀÌºê : {GameManager.waveManager.CurrentWave}";
        finalMonsterText.text = $"¸ó½ºÅÍ Å³ : {GameManager.Instance.monsterKillCount}";
        finalFlowerText.text = $"¸¸µç ²É¹ç °¹¼ö : {GameManager.Instance.flowerCount}";
        finalCurrentFlowerText.text = $"¸¶Áö¸· ²É¹ç °¹¼ö : {GameManager.waveManager.flowerTileList.Count}";

    }
}
