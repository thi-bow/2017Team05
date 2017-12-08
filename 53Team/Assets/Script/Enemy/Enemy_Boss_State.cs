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
        battle,
        dead
    }

    public class Enemy_Boss_State : EnemyBase<Enemy_Boss_State, boss_State>, IEnemy
    {

        public GameObject m_armorUnits;
        public Enemy_Boss_battle m_battle;

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

            m_battle.Init(this);
        }

        public override void ChangeState(boss_State state)
        {
            base.ChangeState(state);
        }

        public override void Damage(int attackPower)
        {
            base.Damage(attackPower);
        }

        public override void OnParge(Parts aParts)
        {
            base.OnParge(aParts);

            var list = GetPartsList(aParts);
            for (int i = 0; i < list.Count; i++)
            {
                var list2 = list[i].gameObject.GetComponentsInChildren<weaponFire>();
                for (int k = 0; k < list2.Length; k++)
                {
                    Destroy(list2[k]);
                }
            }

            var r_list = aParts != Parts.RightArm ? GetPartsList(Parts.RightArm) : new List<Armor>();
            var l_list = aParts != Parts.LeftArm ? GetPartsList(Parts.LeftArm) : new List<Armor>();
            if(r_list.Count == 0 && l_list.Count == 0)
            {
                Destroy(m_armorUnits);
                m_battle.ExMode();
            }
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
                _base.m_battle.Update();
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
