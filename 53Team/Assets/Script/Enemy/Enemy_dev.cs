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
        [Header("ターゲット")]
        public Transform m_target;

        [Header("現在のステート")]
        public dev_State state;

        [Space(10)]
        public Sector m_sector;
        public float m_seachDis;
        public float m_seachAng;
        public bool m_seachAreaDraw;

        private AimTurret m_turret;

        private void Start()
        {
            Initialize();

            if (m_seachAreaDraw)
            {
                var startdeg = 90 - m_seachAng;
                var enddeg = 90 + m_seachAng;
                m_sector.Show(m_seachDis, startdeg, enddeg);
            }
        }

        public void Initialize()
        {
            m_turret = GetComponent<AimTurret>();

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
                if (_base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_seachDis, _base.m_seachAng))
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
                if (!_base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_seachDis, _base.m_seachAng))
                {
                    Debug.Log("ロスト");
                    _base.ChangeState(dev_State.search);
                }

                _base.m_turret.Aim(_base.m_target.localPosition);
            }

            public override void OnExit()
            {
            }
        }

        #endregion

    }
}
