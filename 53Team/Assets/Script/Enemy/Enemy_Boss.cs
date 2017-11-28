using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

namespace Enemy
{
    public enum boss_State
    {
        move,
        attack,
        dead
    }

    public class Enemy_Boss : EnemyBase<Enemy_Boss, boss_State>, IEnemy
    {


        protected override void Start()
        {
            // m_stateList.Add();

            m_stateMachine = new StateMachine<Enemy_Boss>();

            base.Start();
        }

        public override void Initialize()
        {
            if (!m_target)
            {
                m_target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            ChangeState(boss_State.move);
        }

        public override void ChangeState(boss_State state)
        {
            base.ChangeState(state);
        }

        public override void Damage(int attackPower)
        {
            base.Damage(attackPower);
        }

        public override void Dead()
        {
            base.Dead();
        }

        public Transform[] LootPosition
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        #region ---------------  State処理  ---------------

        public class StateMove : State<Enemy_Boss>
        {
            public StateMove(Enemy_Boss dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
            }

            public override void OnExit()
            {
            }
        }

        #endregion
    }
}
