using UnityEngine;
using UnityEngine.Serialization;

public class UnitStats : MonoBehaviour {

    [FormerlySerializedAs("maxHP")] public int maxHp = 100;
    [FormerlySerializedAs("currentHP")] [HideInInspector] public int currentHp;
    public int attackPower = 10;
    public int defense = 10;
    public int dexterity = 5;
    public int speed = 10;
    public int intelligence = 10;
    public int spirit = 10;
    public float critChance = 0.15f;

    protected void Awake() {
        currentHp = maxHp;   
    }

    public bool IsAlive => currentHp > 0;
}
