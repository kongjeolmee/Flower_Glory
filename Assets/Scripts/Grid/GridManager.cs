using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;


    [SerializeField] private List<string> waterWells = new List<string>();
    public List<Transform> _tiles = new List<Transform>();


    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width * 3; x += 3)
        {
            for (int y = 0; y < _height * 3; y += 3)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                foreach (var item in waterWells)
                {
                    if(spawnedTile.name == item)
                    {
                        spawnedTile.gameObject.layer = 10;
                        spawnedTile.ChangewaterWell();
                        break;
                    }
                }
                if(spawnedTile.gameObject.layer != 10)
                {

                    _tiles.Add(spawnedTile.GetComponent<Transform>());
                }
                spawnedTile.Init();
            }
        }

    }

}
