using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;


    [SerializeField] private List<string> waterWells = new List<string>();
    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
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
                    
                     _tiles[new Vector2(x, y)] = spawnedTile;
                }
                spawnedTile.Init();
            }
        }

    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}
