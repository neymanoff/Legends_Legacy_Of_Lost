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
        Instance = this;
        GenerateHexArena(radius);
    }

    void GenerateHexArena(int r) {
        for (int q = -r; q <= r; q++) {
            int r1 = Mathf.Max(-r, -q - r);
            int r2 = Mathf.Min( r, -q + r);
            for (int s = r1; s <= r2; s++) {
                var coord = new HexCoord(q, s);
                var worldPos = HexToWorld(coord);
                var go = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                var cell = go.GetComponent<GridCell>();
                cell.Initialize(coord, worldPos, hexSprite);
                Cells[coord] = cell;
            }
        }
    }

    Vector3 HexToWorld(HexCoord h) {
        float x = h.Q * 1f + h.R * 0.5f;
        float y = h.R * Mathf.Sqrt(3)/2;
        return new Vector3(x, y, 0);
    }

    public bool SpawnUnit(UnitBase prefab, HexCoord coord) {
        if (!Cells.TryGetValue(coord, out var cell)) return false;
        var u = Instantiate(prefab, cell.transform.position, Quaternion.identity);
        if (!cell.TryOccupy(u)) { Destroy(u.gameObject); return false; }
        u.GridPosition = coord;
        return true;
    }
}