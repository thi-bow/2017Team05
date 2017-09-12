using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {

    public enum dev_State
    {
        search,
        attack
    }

    public class Enemy_dev : EnemyBase<Enemy_dev, dev_State>
    {
        public Transform m_target;

        public dev_State state;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            m_enemyStatus.hp = m_enemyStatus.max_hp;

            m_stateList.Add(new StateSearch(this));
            m_stateList.Add(new StateAttack(this));

            m_stateMachine = new StateMachine<Enemy_dev>();

            ChangeState(dev_State.search);
        }

        public void ChengeState(dev_State state)
        {
            base.ChangeState(state);
            this.state = state;
        }

        #region ---------------  State処理  ---------------

        public class StateSearch : State<Enemy_dev>
        {
            public StateSearch(Enemy_dev dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                if (_base.Search(_base.transform, _base.m_target, 5, 60))
                {
                    Debug.Log("発見");
                    _base.ChangeState(dev_State.attack);
                }
            }

            public override void OnExit()
            {
            }
        }

        public class StateAttack : State<Enemy_dev>
        {
            public StateAttack(Enemy_dev dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                if (!_base.Search(_base.transform, _base.m_target, 5, 60))
                {
                    Debug.Log("ロスト");
                    _base.ChangeState(dev_State.search);
                }


            }

            public override void OnExit()
            {
            }
        }

        #endregion

    }
}
