using Core;
using UnityEngine;

namespace Grid
{
    public class GridCell : MonoBehaviour {
        public HexCoord Coord { get; private set; }
        public UnitBase Occupant { get; private set; }
        public void Initialize(HexCoord coord, Vector3 worldPos, Sprite sprite) {
            Coord = coord;
            transform.position = worldPos;
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        public bool TryOccupy(UnitBase u) {
            if (Occupant != null) return false;
            Occupant = u;
            return true;
        }
        public void Vacate() => Occupant = null;
    }
}