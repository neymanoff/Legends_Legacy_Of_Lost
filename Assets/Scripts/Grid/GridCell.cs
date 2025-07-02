using Core;
using UnityEngine;

namespace Grid
{
    public enum CellOwner {Neutral, Player, Enemy}
    public class GridCell : MonoBehaviour {
        public CellOwner Owner { get; private set; }
        public HexCoord Coord { get; private set; }
        private UnitBase Occupant { get; set; }
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

        public void SetOwner(CellOwner owner)
        {
            Owner = owner;
            var sr = GetComponent<SpriteRenderer>();
            switch (owner)
            {
                case CellOwner.Player:
                    sr.color = new Color(0.4f, 0.6f, 1f);
                    break;
                case CellOwner.Enemy:
                    sr.color = new Color(1f, 0.4f, 0.4f);
                    break;
                case CellOwner.Neutral:
                default:
                    var c = Color.white;
                    c.a = 0f;
                    sr.color = c;
                    break;
            }
            {
                
            }
        }
    }
}