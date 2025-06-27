using System;
using System.Collections.Generic;
using Core;
using Grid;
using UnityEngine;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        [Header("Units Prefabs")]
        [SerializeField] private List<UnitBase> playerPrefabs;
        [SerializeField] private List<UnitBase> enemyPrefabs;
 
        [Header("Formation")]
        [SerializeField] private FormationType formation = FormationType.ThreeFrontTwoBack;

        private void Start()
        {
            StartBattle();
        }

        void StartBattle()
        {
            var players = GridManager.Instance.SpawnFormation(playerPrefabs, CellOwner.Player, formation);
            var enemies = GridManager.Instance.SpawnFormation(enemyPrefabs,  CellOwner.Enemy,  formation);
            TurnManager.Instance.InitializeUnits(players, enemies);
        }
    }
}