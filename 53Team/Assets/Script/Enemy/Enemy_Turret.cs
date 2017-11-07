using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Enemy {

    public enum turret_State
    {
        search,
        attack
    }

    public class Enemy_Turret : EnemyBase<Enemy_Turret, turret_State>, IEnemy
    {
        [Header("現在のステート")]
        public turret_State state;

        private AimTurret m_turret;

        protected override void Start()
        {
            m_turret = GetComponent<AimTurret>();

            m_stateList.Add(new StateSearch(this));
            m_stateList.Add(new StateAttack(this));

            m_stateMachine = new StateMachine<Enemy_Turret>();

            base.Start();
        }

        public override void Initialize()
        {
            ChangeState(turret_State.search);
        }

        public void ChengeState(turret_State state)
        {
            base.ChangeState(state);
            this.state = state;
        }

        public override void Damage(int damage)
        {
            Debug.LogFormat("{0}に{1}ダメージ!!", m_enemyStatus.name, damage);
        }

        public Transform[] LootPosition
        {
            get; set;
        }

        #region ---------------  State処理  ---------------

        public class StateSearch : State<Enemy_Turret>
        {
            public StateSearch(Enemy_Turret dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                if (_base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_enemyStatus.seachMainDis, _base.m_enemyStatus.seachMainAng, true)
                    || _base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_enemyStatus.seachSubDis, _base.m_enemyStatus.seachSubAng, true))
                {
                    Debug.Log("発見");
                    _base.ChangeState(turret_State.attack);
                    return;
                }
            }

            public override void OnExit()
            {
            }
        }

        public class StateAttack : State<Enemy_Turret>
        {
            public StateAttack(Enemy_Turret dev) : base(dev) { }

            public override void OnEnter()
            {

            }

            public override void OnExecute()
            {
                if (!_base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_enemyStatus.seachMainDis, _base.m_enemyStatus.seachMainAng, true)
                    && !_base.Search(_base.m_turret.GetCannon(), _base.m_target, _base.m_enemyStatus.seachSubDis, _base.m_enemyStatus.seachSubAng, true))
                {
                    Debug.Log("ロスト");
                    _base.ChangeState(turret_State.search);
                    return;
                }

                _base.m_turret.Aim(_base.m_target.localPosition, _base.m_enemyStatus.rotateSpd);
            }

            public override void OnExit()
            {
            }
        }

        #endregion

    }
}
