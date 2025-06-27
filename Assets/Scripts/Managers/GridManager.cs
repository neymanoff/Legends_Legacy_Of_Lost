using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Grid;
using UnityEngine;

namespace Managers
{
    public enum FormationType
    {
        ThreeFrontTwoBack,
        TwoFrontThreeBack
    }
    public class GridManager : MonoBehaviour {
        public static GridManager Instance { get; set; }
        private readonly Dictionary<HexCoord, GridCell> Cells = new();
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Sprite hexSprite;
        [SerializeField] private int radius; 
        
        private readonly Dictionary<FormationType, int[]> formationPatterns = new()
        {
            { FormationType.ThreeFrontTwoBack, new[] { 3, 2 } },
            { FormationType.TwoFrontThreeBack, new[] { 2, 3 } }
        };
        private void Awake() {
            if (Instance != null && Instance != this) {
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateHexArena(radius);
            AssignZones();
        }

        private void AssignZones()
        {
            foreach (var kv in Cells)
            {
                var coord = kv.Key;
                var cell = kv.Value;
                if (coord.Q < 0) cell.SetOwner(CellOwner.Player);
                else if (coord.Q > 0) cell.SetOwner(CellOwner.Enemy);
                else cell.SetOwner(CellOwner.Neutral);
            }
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
                    var go = Instantiate(cellPrefab, HexToWorld(coord), Quaternion.Euler(90f, 0f, 0f), transform);
                    go.name = $"Cell ({q}, {s})";
                    var cell = go.GetComponent<GridCell>();
                    cell.Initialize(coord, worldPos, hexSprite);
                    Cells[coord] = cell;
                }
            }
        }

        private Vector3 HexToWorld(HexCoord h) {
            var w = hexSprite.bounds.size.x;
            var hgt = hexSprite.bounds.size.y;
            var x = (h.Q + h.R * 0.5f) * w;
            var z = h.R * (hgt * 0.75f);
            return new Vector3(x, 0, z);
        }

        public UnitBase SpawnUnit(UnitBase prefab, HexCoord coord, CellOwner owner) {
            if (!Cells.TryGetValue(coord, out var cell)) return null;
            float yRot = owner == CellOwner.Player ? 90f : -90f;
            var u = Instantiate(prefab, cell.transform.position, Quaternion.Euler(0, yRot, 0));
            if (!cell.TryOccupy(u)) { Destroy(u.gameObject); return null; }
            u.GridPosition = coord;
            u.transform.position = cell.transform.position;
            return u;
        }

        // GridManager.cs
        public List<UnitBase> SpawnFormation(List<UnitBase> prefabs, CellOwner owner, FormationType formation) {
            var spawned = new List<UnitBase>();
            int prefabIndex = 0;
            var pattern = formationPatterns[formation];
            foreach (var rowCount in pattern.Select((cnt, row) => (cnt, row))) {
                int q = owner==CellOwner.Player ? -1 - rowCount.row : 1 + rowCount.row;
                var cells = Cells
                    .Where(kv => kv.Key.Q == q && kv.Value.Owner == owner)
                    .OrderBy(kv => Mathf.Abs(kv.Key.R))
                    .Select(kv => kv.Value)
                    .Take(rowCount.cnt);

                foreach (var cell in cells) {
                    if (prefabIndex >= prefabs.Count) break;
                    var inst = SpawnUnit(prefabs[prefabIndex], cell.Coord, owner);
                    if (inst != null) spawned.Add(inst);
                    prefabIndex++;
                }
            }
            return spawned;
        }
        
    }
}