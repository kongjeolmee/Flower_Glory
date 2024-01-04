using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[System.Serializable]
public class TileData
{
    public enum TileType
    {
        None = 0,
        Flower,
        Seal,
        Slow,
        Tree,
        AllTarget,
        Water
    }

    public int tileHp = 0;
    public TileType tileType = TileType.None;
    public float tileWater = 0;
    public int tileSpc = 0;

}
public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    public TileData tileData = new TileData();
    
    public GameObject waterProcess;
    private Slider waterProcessSlider;
    private SpriteAtlas tileAtlas;
    private static int wellNum = 1;

    public void Init()
    {
        if(gameObject.layer != 10)
        {
            tileData.tileSpc = Random.Range(0, 2);
            if(tileData.tileSpc == 0)
            {
                _renderer.sprite = TileSpriteReturn(tileData.tileSpc, tileData.tileType);
            }else if(tileData.tileSpc == 1)
            {
                _renderer.sprite = TileSpriteReturn(tileData.tileSpc, tileData.tileType);

            }
        }
        else
        {
            _renderer.sprite = TileSpriteReturn(wellNum,TileData.TileType.Water);
            wellNum++;
        }
        
    }


    Sprite TileSpriteReturn(int num, TileData.TileType type)
    {
        string alphabet = "";
        if(type != TileData.TileType.Water)
        {
            if (num == 0) alphabet = "A";
            else if (num == 1) alphabet = "B";
        }
        switch(type)
        {
            case TileData.TileType.None:
                return tileAtlas.GetSprite($"Tile{alphabet}_Dirt");
            case TileData.TileType.Flower:
                return tileAtlas.GetSprite($"Tile{alphabet}_Flower{tileData.tileHp}");
            case TileData.TileType.Water:
                return tileAtlas.GetSprite($"Tile_WaterWell{num}");
                return null;
            case TileData.TileType.Seal:
                return null;
            case TileData.TileType.Slow:
                return null;
            case TileData.TileType.Tree:
                return null;
            case TileData.TileType.AllTarget:
                return null;
            default:
                return null;
        }
    }
    private void Awake()
    {
        tileAtlas = Resources.Load<SpriteAtlas>("Atlas/Tile");
        
        waterProcess = Instantiate(waterProcess, transform);
        waterProcess.transform.SetParent(GameObject.Find("TileCanvas").transform);
        waterProcessSlider = waterProcess.GetComponent<Slider>();
        waterProcess.SetActive(false);
        
    }
    public void ChangewaterWell()
    {

        tileData.tileType = TileData.TileType.Water;
        Destroy(waterProcess);
    }

    
    private void Update()
    {
        if(waterProcess != null)
        {
            if (waterProcess.gameObject.activeSelf)
            {
                if (waterProcessSlider.value != tileData.tileWater)
                {
                    waterProcessSlider.value = tileData.tileWater / GameManager.Instance.WaterFullTime;
                }

            }
        }

    }

    public void TileTypeFlowerCheck()
    {
        if (tileData.tileHp > 0) tileData.tileType = TileData.TileType.Flower;
        else if (tileData.tileHp == 0) tileData.tileType = TileData.TileType.None;

        _renderer.sprite = TileSpriteReturn(tileData.tileSpc, tileData.tileType);


    }

    public void TileTypeChanger(TileData.TileType type)
    {
        tileData.tileType = type;
        _renderer.sprite = TileSpriteReturn(tileData.tileSpc, tileData.tileType);

    }

    public void TileDamage(int damage)
    {
        tileData.tileHp -= damage;
        if(tileData.tileHp < 0) tileData.tileHp = 0;
        TileTypeFlowerCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameManager.player.reachTileList.Add(this);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.player.reachTileList != null)
        {
            foreach (var i in GameManager.player.reachTileList)
            {
                if (i == this)
                {
                    GameManager.player.reachTileList.Remove(i);
                    break;
                }
            }

        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster") && collision.GetComponent<MonsterAi>().monsterOriginData.name == "SadLarvava")
        {
            TileTypeChanger(TileData.TileType.Seal);
        }

    }
}
