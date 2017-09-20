using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

namespace Enemy
{
    public enum standard_State
    {
        move,
        warning,
        chase,
        attack
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy_Standard : EnemyBase<Enemy_Standard, standard_State>, IEnemy
    {
        [Header("現在のステート")]
        public standard_State state;

        protected override void Start()
        {
            m_stateList.Add(new StateMove(this));
            m_stateList.Add(new StateWarning(this));
            m_stateList.Add(new StateChase(this));
            m_stateList.Add(new StateAttack(this));

            m_stateMachine = new StateMachine<Enemy_Standard>();

            base.Start();
        }

        public override void Initialize()
        {
            ChangeState(standard_State.move);
        }

        public override void ChangeState(standard_State state)
        {
            base.ChangeState(state);
            this.state = state;
        }

        public void Damage(int damage)
        {
            Debug.LogFormat("{0}に{1}ダメージ!!", m_enemyStatus.name, damage);

            m_enemyStatus.hp = Mathf.Max(m_enemyStatus.hp - damage, 0);

            if (m_enemyStatus.hp == 0)
            {
                // ここに死亡時処理


                if (OnDead != null)
                {
                    OnDead.OnNext(1);
                }
            }
        }


        #region ---------------  State処理  ---------------

        // 移動ステート
        public class StateMove : State<Enemy_Standard>
        {
            public StateMove(Enemy_Standard dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                // メイン視界に敵を発見
                if (_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachMainDis, _base.m_enemyStatus.seachMainAng, true))
                {
                    Debug.Log("<< 発見 >>");
                    _base.ChangeState(standard_State.chase);
                    return;
                }
                // サブ視界に敵、又は何かを発見
                if (_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachSubDis, _base.m_enemyStatus.seachSubAng, true))
                {
                    Debug.Log("<< 警戒 >>");
                    _base.ChangeState(standard_State.warning);
                    return;
                }

            }

            public override void OnExit()
            {
            }
        }


        // 警戒ステート
        public class StateWarning : State<Enemy_Standard>
        {
            public StateWarning(Enemy_Standard dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                // メイン視界に敵を発見
                if (_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachMainDis, _base.m_enemyStatus.seachMainAng, true))
                {
                    Debug.Log("<< 発見 >>");
                    _base.ChangeState(standard_State.chase);
                    return;
                }
                // サブ視界に敵、又は何かを発見
                if (_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachSubDis, _base.m_enemyStatus.seachSubAng, true))
                {
                    Debug.Log("<< 警戒 >>");
                    _base.ChangeState(standard_State.chase);
                    return;
                }
            }

            public override void OnExit()
            {
            }
        }


        // 追跡ステート
        public class StateChase : State<Enemy_Standard>
        {
            public StateChase(Enemy_Standard dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                if (!_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachMainDis, _base.m_enemyStatus.seachMainAng, true)
                    && !_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.seachSubDis, _base.m_enemyStatus.seachSubAng, true))
                {
                    Debug.Log("<< ロスト >>");                   
                    _base.ChangeState(standard_State.move);
                    return;
                }

                if(_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.attackDis, _base.m_enemyStatus.attackAng, true))
                {
                    Debug.Log("<< 攻撃 >>");
                    _base.ChangeState(standard_State.attack);
                    return;
                }

                _base.m_agent.SetDestination(_base.m_target.localPosition);
            }

            public override void OnExit()
            {
                _base.m_agent.ResetPath();
            }
        }


        // 攻撃ステート
        public class StateAttack : State<Enemy_Standard>
        {
            public StateAttack(Enemy_Standard dev) : base(dev) { }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                if (!_base.Search(_base.transform, _base.m_target, _base.m_enemyStatus.attackDis, _base.m_enemyStatus.attackAng, true))
                {
                    Debug.Log("<< 射程外 >>");
                    _base.ChangeState(standard_State.chase);
                    return;
                }
            }

            public override void OnExit()
            {
            }
        }


        #endregion
    }
}
