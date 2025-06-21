using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Unity.Collections;
using UnityEngine;

namespace Core
{
    public abstract class UnitBase : MonoBehaviour
    {
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Cast = Animator.StringToHash("Cast");
        [Header("Stats")] public UnitStats stats;
        public string unitName;
        [SerializeField] private int maxHp = 100;
        private int currentHp;
        public int speed;
        public bool isAlive = true;
        public Animator animator;
        protected Skill SelectedSkill;
        [SerializeField] protected List<Skill> skills;
        private List<SkillInstance> skillBook;
        [HideInInspector] public float turnMeter = 0f;
        
        public IReadOnlyList<SkillInstance> SkillBook => skillBook;


        public void TickSkillCooldowns()
        {
            foreach (var inst in skillBook)
            {
                inst.TickCooldown();
            }
        }

        public bool HasActedThisTurn { get; private set; }

        protected virtual void Awake()
        {
            currentHp = maxHp;
            animator = GetComponent<Animator>();
            stats = GetComponent<UnitStats>();
            if (stats == null)
            {
                Debug.LogWarning($"{name} is missing UnitStats, adding one.");
                stats = gameObject.AddComponent<UnitStats>();
            }

            skillBook = skills.Select(s => new SkillInstance(s)).ToList();
        }

        public virtual void TakeTurn(UnitBase target)
        {
            if (!isAlive) return;

            StartCoroutine(PerformTurn(target));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual IEnumerator PerformTurn(UnitBase target)
        {
            HasActedThisTurn = true;

            UseSkill(target);

            // Wait until animation or attack logic finishes, if needed
            yield return new WaitForSeconds(1f);
            
            TickSkillCooldowns();
            TurnManager.Instance.FinishTurn();
        }

        public void UseSkill(int index, UnitBase target)
        {
            if (index < 0 || index >= skillBook.Count || target == null)
                return;

            skillBook[index].TryApply(this, target);
        }

        /// <summary>
        /// Переопределяется в EnemyUnit/PlayerUnit для ИИ или ввода игрока.
        /// </summary>
        public virtual void UseSkill(UnitBase target)
        {
            UseSkill(0, target);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void TakeDamage(int amount)
        {
            currentHp -= Mathf.Max(0, amount);
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            Debug.Log($"{unitName} takes {amount} dmg (HP: {currentHp}/{maxHp})");

            if (currentHp <= 0)
                Die();
        }

        protected virtual void Die()
        {
            isAlive = false;
            Debug.Log(unitName + " has died.");
            gameObject.SetActive(false);
            // Destroy or deactivate unit here if needed
        }

        public void PlayAnimation(string trigger)
        {
            if (animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }

        public void PlaySkillAnimation(SkillAnimationType animationType)
        {
            if (animator == null) return;

            switch (animationType)
            {
                case SkillAnimationType.Magic:
                    animator.SetTrigger(Cast);
                    break;
                case SkillAnimationType.Melee:
                    animator.SetTrigger(Attack);
                    break;
                case SkillAnimationType.Block:
                    animator.SetTrigger(Block);
                    break;
                case SkillAnimationType.Ranged:
                    animator.SetTrigger(Attack);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }

        protected virtual Skill[] GetSkills() => skills.ToArray();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            var skills = GetSkills();
            if (skills == null || skills.Length == 0)
                Debug.LogError(
                    $"[{name}] {GetType().Name} Must have at least one skill assigned to it.",
                    this
                );
        }
#endif
    }
}