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
        none,
        battle,
        dead
    }

    public class Enemy_Boss_State : EnemyBase<Enemy_Boss_State, boss_State>, IEnemy
    {
        public Enemy_Boss_Behavior m_bhavior;

        protected override void Start()
        {
            m_stateList.Add(new StateNone(this));
            m_stateList.Add(new StateBattle(this));
            m_stateList.Add(new StateDead(this));

            m_stateMachine = new StateMachine<Enemy_Boss_State>();

            base.Start();
        }

        public override void Initialize()
        {
            if (!m_target)
            {
                m_target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            ChangeState(boss_State.none);
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

        public class StateNone : State<Enemy_Boss_State>
        {
            public StateNone(Enemy_Boss_State dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                _base.ChangeState(boss_State.battle);
            }
        }


        public class StateBattle : State<Enemy_Boss_State>
        {
            public StateBattle(Enemy_Boss_State dev) : base(dev) { }

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


        public class StateDead : State<Enemy_Boss_State>
        {
            public StateDead(Enemy_Boss_State dev) : base(dev) { }

            public override void OnEnter()
            {
                EnemyMgr.i.BossDead();
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
