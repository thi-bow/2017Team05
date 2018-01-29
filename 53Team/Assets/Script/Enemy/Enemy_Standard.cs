using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

namespace Enemy
{
    public enum standard_State
    {
        move,
        warning,
        chase,
        attack,
        dead
    }

    public class Enemy_Standard : EnemyBase<Enemy_Standard, standard_State>, IEnemy
    {

        [Header("現在のステート")]
        public standard_State m_state;

        [Header("分隊番号")]
        public int m_group;

        public float m_time = 2.0f;
        public Transform[] m_lootPosition;
        [HideInInspector]
        public Vector3 m_lastPosition;
        public ParticleSystem m_ExplosionParticles;
        public ParticleSystem m_SmokeEffect;

        [Space(10)]
        public bool m_isTutorial = false;

        private Enemy_Turret m_turret;

        private readonly Vector3 m_vectorZero = new Vector3(0, 0, 0);

        protected override void Start()
        {
            m_stateList.Add(new StateMove(this));
            m_stateList.Add(new StateWarning(this));
            m_stateList.Add(new StateChase(this));
            m_stateList.Add(new StateAttack(this));
            m_stateList.Add(new StateDead(this));

            m_stateMachine = new StateMachine<Enemy_Standard>();

            m_turret = GetComponent<Enemy_Turret>();
            m_turret.SetOffset(m_enemyStatus.viewOffset);
            m_enemyStatus.attackDis = m_turret.m_range;

            base.Start();
        }

        public override void Initialize()
        {
            ChangeState(standard_State.move);
        }

        public override void ChangeState(standard_State state)
        {
            base.ChangeState(state);
            this.m_state = state;
        }

        public override void Damage(int damage)
        {
            if (_charaPara.isDead) { return; }

            EnemyMgr.i.GetWarningEnemys(transform.position);
            base.Damage(damage);
        }

        public override void Dead()
        {
            // Debug.Log("死んだぁ！！");
            m_animator.Dead();
            m_ExplosionParticles.Play();
            m_SmokeEffect.gameObject.SetActive(true);
            m_SmokeEffect.Play();

            ChangeState(standard_State.dead);
            EnemyMgr.i.OnDeadEnemy(m_group);
            base.Dead();
            // Destroy(gameObject);
        }

        public Transform GetLootPos(int n)
        {
            Transform t = transform;
            if(m_lootPosition.Length > 0)
            {
                t = m_lootPosition[n];
            }
            return t;
        }

        public Transform[] LootPosition
        {
            get { return m_lootPosition; }
            set { m_lootPosition = value; }
        }


        public void RightShot(Vector3 vector)
        {
            EnemyRightArmtShot(new Ray(m_viewPoint.position, vector));
        }

        public void LeftShot(Vector3 vector)
        {
            EnemyLeftArmShot(new Ray(m_viewPoint.position, vector));
        }

        #region ---------------  State処理  ---------------

        // 移動ステート
        public class StateMove : State<Enemy_Standard>
        {
            public StateMove(Enemy_Standard dev) : base(dev) { }

            private float distance;
            private int currentRootNum = 0;

            public override void OnEnter()
            {

                // 最初の徘徊ポジションの決定
                // 現在のポジションから一番近いポジションをスタートにする
                distance = Vector3.SqrMagnitude(_base.GetLootPos(0).position - _base.transform.position);
                float adis;
                for (int i = 1; i < _base.m_lootPosition.Length; i++)
                {
                    adis = Vector3.SqrMagnitude(_base.m_lootPosition[i].position - _base.transform.position);
                    if (distance > adis)
                    {
                        distance = adis;
                        currentRootNum = i;
                    }
                }
            }

            public override void OnExecute()
            {
                // メイン視界に敵を発見
                if (_base.IsMainSearch())
                {
                    _base.ChangeState(standard_State.chase);
                    return;
                }
                // サブ視界に敵、又は何かを発見
                if (_base.IsSubSearch())
                {
                    _base.ChangeState(standard_State.warning);
                    return;
                }

                // ルート徘徊
                if (_base.IsArrival())
                    currentRootNum = (currentRootNum + 1) % _base.m_lootPosition.Length;
                _base.Move(_base.GetLootPos(currentRootNum).position, false);
                //if (_base.m_agent.remainingDistance < _base.m_agent.stoppingDistance && _base.m_agent.hasPath)
                //{
                //    currentRootNum = (currentRootNum + 1) % _base.m_lootPosition.Length;
                //    _base.m_rootNum = currentRootNum;
                //}
                //_base.m_agent.SetDestination(_base.m_lootPosition[currentRootNum].position);
            }

            public override void OnExit()
            {
            }
        }


        // 警戒ステート
        public class StateWarning : State<Enemy_Standard>
        {
            public StateWarning(Enemy_Standard dev) : base(dev) { }

            public override void OnExecute()
            {
                // メイン視界に敵を発見
                if (_base.IsMainSearch() || _base.IsSubSearch())
                {
                    _base.ChangeState(standard_State.chase);
                    return;
                }

                // ターゲットの最終発見ポイントがあればそこまで移動
                if (_base.m_lastPosition != _base.m_vectorZero)
                {
                    _base.m_turret.Aim(_base.m_lastPosition);
                    _base.Move(_base.m_lastPosition, false);
                    if (_base.IsArrival())
                    {
                        Observable.Timer(System.TimeSpan.FromSeconds(3f))
                            .Where(x => _base.IsCurrentState(standard_State.warning)).Subscribe(_ => 
                            {
                                _base.m_lastPosition = _base.m_vectorZero;
                                _base.ChangeState(standard_State.move);
                            });
                    };
                }
            }
        }


        // 追跡ステート
        public class StateChase : State<Enemy_Standard>
        {
            public StateChase(Enemy_Standard dev) : base(dev) { }

            private int m_timer = 0;

            public override void OnEnter()
            {
                EnemyMgr.i.GetWarningEnemys(_base.transform.position);
                m_timer = 0;
            }

            public override void OnExecute()
            {
                // ターゲットが視界外に消えてからn秒後にMoveStateに移行
                if (!_base.IsMainSearch() && !_base.IsSubSearch())
                {
                    m_timer++;
                    if(m_timer >= (_base.m_time / Time.deltaTime))
                    {
                        _base.m_lastPosition = _base.m_target.position;
                        _base.ChangeState(standard_State.warning);
                        return;
                    }
                }
                else if(!_base.IsMainSearch() || !_base.IsSubSearch())
                {
                    // 視界内、尚且つタイマーがカウントされていればリセット
                    if(m_timer > 0)
                    {
                        m_timer = 0;
                    }
                }


                if(_base.IsAttackSearch(-_base.m_enemyStatus.attackDis / 5f))
                {
                    _base.ChangeState(standard_State.attack);
                    return;
                }

                _base.m_turret.Aim(_base.m_target);
                _base.Move(_base.m_target.position, true);
                // _base.Rotate(_base.m_target.position);
            }

            public override void OnExit()
            {
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
                if (!_base.IsAttackSearch())
                {
                    _base.ChangeState(standard_State.chase);
                    return;
                }

                if (_base.m_isTutorial) return;

                _base.m_turret.Aim(_base.m_target);
                if (!_base.m_turret.IsOutRange)
                {
                    _base.m_turret.Fire();
                }
            }

            public override void OnExit()
            {
            }
        }


        public class StateDead : State<Enemy_Standard>
        {
            public StateDead(Enemy_Standard dev) : base(dev) { }

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
