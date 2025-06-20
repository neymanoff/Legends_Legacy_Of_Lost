using System.Collections.Generic;
using Core;
using Game.UI;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour {
        public static TurnManager Instance { get; private set; }

        [SerializeField] private List<UnitBase> playerUnits = new();
        [SerializeField] private List<UnitBase> enemyUnits = new();

        private readonly List<UnitBase> allUnits = new();
        private int currentIndex = -1;

        private void Awake() {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            allUnits.AddRange(playerUnits);
            allUnits.AddRange(enemyUnits);
            FinishTurn();
        }

        public void FinishTurn()
        {
            bool anyPlayerAlive = playerUnits.Exists(u => u.isAlive);
            bool anyEnemyAlive = enemyUnits.Exists(u => u.isAlive);


            if (!anyPlayerAlive)
            {
                Debug.Log("Defeat! All player units have fallen");
                return;
            }

            if (!anyEnemyAlive)
            {
                Debug.Log("Victory! All enemies have fallen");
                return;
            }

            int count = allUnits.Count;
            for (int step = 1; step <= count; step++)
            {
                int next = (currentIndex + step) % count;
                if (allUnits[next].isAlive)
                {
                    currentIndex = next;
                    BeginTurn(allUnits[next]);
                    return;
                }
            }
            BeginNextTurn();
        }

        private void BeginTurn(UnitBase actor)
        {
            //Choose a target from the opposite team
            UnitBase target = (actor is PlayerUnit)?enemyUnits.Find(e => e.isAlive) : playerUnits.Find(e => e.isAlive);
            Debug.Log($"[TurnManager] Actor {actor.unitName}");

            if (actor is PlayerUnit player)
            {
                SkillButtonsUI.Instance.ShowButtons(player, target);
            }
            else
            {
                actor.TakeTurn(target);
            }
            
        }

        private void BeginNextTurn() {
            UnitBase currentUnit = allUnits[currentIndex];
            UnitBase target = FindTargetFor(currentUnit);

            if (!currentUnit.isAlive || target == null) {
                FinishTurn();
                return;
            }

            if (currentUnit is PlayerUnit player) {
                // Показываем UI и ждём действия игрока
                SkillButtonsUI.Instance.ShowButtons(player, target);
            } else {
                // AI автоматически выполняет действие
                currentUnit.TakeTurn(target);
            }
        }

        private UnitBase FindTargetFor(UnitBase current) {
            foreach (var unit in allUnits) {
                if (unit != current && unit.isAlive)
                    return unit;
            }

            return null;
        }
    }
}
