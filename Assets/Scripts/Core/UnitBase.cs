using System;
using System.Collections;
using Game.Skills;
using Managers;
using Unity.Collections;
using UnityEngine;

namespace Core
{
    public enum SkillAnimationType
    {
        Attack,
        Cast,
        Block
    }

    public abstract class UnitBase : MonoBehaviour
    {
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Cast = Animator.StringToHash("Cast");
        private static readonly int Attack = Animator.StringToHash("Attack");
        [Header("Stats")] public UnitStats stats;
        public string unitName;
        [SerializeField] private int maxHp = 100;
        [ReadOnly] private int currentHp;
        public int speed;
        public bool isAlive = true;
        public bool hasActedThisTurn = false;
        public Animator animator;
        protected Skill SelectedSkill;
        [SerializeField] protected Skill[] skills;
        [SerializeField] private SkillAnimationType type;
        [HideInInspector] public float turnMeter = 0f;


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
        }

        public virtual void TakeTurn(UnitBase target)
        {
            if (!isAlive) return;

            StartCoroutine(PerformTurn(target));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual IEnumerator PerformTurn(UnitBase target)
        {
            hasActedThisTurn = true;

            UseSkill(target);

            // Wait until animation or attack logic finishes, if needed
            yield return new WaitForSeconds(1f);

            TurnManager.Instance.FinishTurn();
        }

        public abstract void UseSkill(UnitBase target);

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

        public void PlaySkillAnimation(SkillAnimationType type)
        {
            if (animator == null) return;

            switch (type)
            {
                case SkillAnimationType.Attack:
                    animator.SetTrigger(Attack);
                    break;
                case SkillAnimationType.Cast:
                    animator.SetTrigger(Cast);
                    break;
                case SkillAnimationType.Block:
                    animator.SetTrigger(Block);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public virtual Skill[] GetSkills() => skills;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            var skills = GetSkills();
            if (skills == null || skills.Length == 0)
                Debug.LogError(
                    $"[{name}] {GetType().Name} должен иметь хотя бы один навык!",
                    this
                );
        }
#endif
    }
}