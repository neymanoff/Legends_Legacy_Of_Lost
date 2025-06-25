using System.Collections.Generic;
using Core;
using Grid;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public static GridManager Instance { get; private set; }
    public Dictionary<HexCoord, GridCell> Cells = new();
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Sprite hexSprite;
    [SerializeField] private int radius; // «радиус» арены в клетках

    private void Awake() {
        if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GenerateHexArena(radius);
    }

    private void OnDestroy() {
        if (Instance == this) Instance = null;
    }


    void GenerateHexArena(int r) {
        for (int q = -r; q <= r; q++) {
            int r1 = Mathf.Max(-r, -q - r);
            int r2 = Mathf.Min( r, -q + r);
            for (int s = r1; s <= r2; s++) {
                var coord = new HexCoord(q, s);
                var worldPos = HexToWorld(coord);
                var go = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                go.name = $"Cell ({q}, {s})";
                var cell = go.GetComponent<GridCell>();
                cell.Initialize(coord, worldPos, hexSprite);
                Cells[coord] = cell;
            }
        }
    }

    Vector3 HexToWorld(HexCoord h) {
        float w = hexSprite.bounds.size.x;
        float hgt = hexSprite.bounds.size.y;
        float x = (h.Q + h.R * 0.5f) * w;
        float y = h.R * (hgt * 0.75f);
        return new Vector3(x, y, 0);
    }

    public bool SpawnUnit(UnitBase prefab, HexCoord coord) {
        if (!Cells.TryGetValue(coord, out var cell)) return false;
        var u = Instantiate(prefab, cell.transform.position, Quaternion.identity);
        if (!cell.TryOccupy(u)) { Destroy(u.gameObject); return false; }
        u.GridPosition = coord;
        u.transform.position = cell.transform.position;
        return true;
    }
}