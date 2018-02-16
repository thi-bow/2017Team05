using System;
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
        battle
    }

    public class Enemy_Boss_State : EnemyBase<Enemy_Boss_State, boss_State>, IEnemy
    {

        public GameObject m_armorUnits;
        public Enemy_Boss_battle m_battle;

        public ExplostionParticle[] m_explostions;

        protected override void Start()
        {
            m_stateList.Add(new StateNone(this));
            m_stateList.Add(new StateBattle(this));

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

            m_battle.Init(this);
        }

        public override void ChangeState(boss_State state)
        {
            base.ChangeState(state);
        }

        public override void Damage(int attackPower)
        {
            base.Damage(attackPower);
            if(HP <= _charaPara._maxHP / 2)
            {
                m_battle.ExMode();
            }
        }

        public override void Dead()
        {
            m_battle.ChangeType(Enemy_Boss_battle.BattleType.none);
            // base.Dead();
            // ボス死亡アニメーションを流す

            EnemyMgr.i.BossDead();
            Destroy(this);
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
                _base.m_battle.Update();
            }

            public override void OnExit()
            {
            }
        }
        #endregion
    }
}
