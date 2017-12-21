using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Enemy
{
    public interface IEnemy
    {
        // ダメージ関数
        void Damage(int damage);

        // 移動ルート
        Transform[] LootPosition { get; set; }
    }



    [Serializable]
    public class EnemyStatus
    {
        public string name;
        public float attackDis;         // 攻撃開始距離
        public float attackAng;         // 攻撃範囲(角度)
        public float seachMainDis;      // メイン索敵距離
        public float seachMainAng;      // メイン索敵範囲(角度)
        public float seachSubDis;       // サブ索敵距離
        public float seachSubAng;       // サブ索敵範囲(角度)

        public readonly Vector3 viewOffset = new Vector3(0, 1, 0);
    }

    // Enemyの基底クラス
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterMovement))]
    public class EnemyBase<T, TEnum> : CharaBase
        where T : class where TEnum : IConvertible
    {
        // 開始のタイミングを制御するかどうか
        public bool m_PlayAwake = true;

        [Header("ターゲット")]
        public Transform m_target;

        [Header("各部位の当たり判定")]
        public BoneCollide[] m_boneCollides;

        // Enemyの初期ステータス
        [Space(10)]
        public EnemyStatus m_enemyStatus;

        [Header("目となるポイント")]
        public Transform m_viewPoint;

        [Space(10)]
        public Sector m_sectorAtk;
        public Sector m_sectorMain;
        public Sector m_sectorSub;
        public bool m_seachAreaDraw;

        [Header("ドロップ率")]
        public float m_probability = 50.0f;

        // RaycastHit
        protected RaycastHit m_raycastHit;

        // NavMeshAgent
        protected NavMeshAgent m_agent;
        protected CharacterMovement m_character;

        // 状態を格納する配列
        protected List<State<T>> m_stateList = new List<State<T>>();

        // ステートマシンクラス
        protected StateMachine<T> m_stateMachine;
        
        protected override void Start()
        {
            base.Start();
            m_agent = GetComponent<NavMeshAgent>();
            m_character = GetComponent<CharacterMovement>();

            m_agent.updateRotation = false;
            m_agent.updatePosition = false;


            for (int i = 0; i < m_boneCollides.Length; i++)
            {
                int n = i;
                m_boneCollides[n].OnDamage.Subscribe(dmg =>
                {
                    Parts parts = m_boneCollides[n].m_parts;
                    _deadAction = () => ResultScore.AddKiilCount(dmg.type);
                    if(parts == Parts.WeakPoint || dmg.type == Weapon.Attack_State.approach)
                    {
                        Damage(dmg.value);
                    }
                    else
                    {
                        PartsDamage(dmg.value, parts, () => {
                            OnParge(parts);
                        });

                    }
                });
            }

            if (m_seachAreaDraw)
            {
                float startdeg = 0.0f;
                float enddeg = 0.0f;

                // 武器射程の描画
                if(m_sectorAtk != null)
                {
                    startdeg = 90 - m_enemyStatus.attackAng;
                    enddeg = 90 + m_enemyStatus.attackAng;
                    m_sectorAtk.Show(m_enemyStatus.attackDis, startdeg, enddeg);
                }

                // メイン視界の描画
                if (m_sectorMain != null)
                {
                    startdeg = 90 - m_enemyStatus.seachMainAng;
                    enddeg = 90 + m_enemyStatus.seachMainAng;
                    m_sectorMain.Show(m_enemyStatus.seachMainDis, startdeg, enddeg);
                }

                // サブ視界の描画
                if (m_sectorSub != null)
                {
                    startdeg = 90 - m_enemyStatus.seachSubAng;
                    enddeg = 90 + m_enemyStatus.seachSubAng;
                    m_sectorSub.Show(m_enemyStatus.seachSubDis, startdeg, enddeg);
                }

            }

            if(m_viewPoint == null)
            {
                m_viewPoint = transform.Find("view_point").GetComponent<Transform>();
            }

            if (!m_target)
            {
                m_target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            if (m_PlayAwake)
            {
                Initialize();
            }
        }

        public virtual void Initialize() { }

        // ステートを変更
        public virtual void ChangeState(TEnum state)
        {
            if (m_stateMachine == null)
            {
                Debug.Log("StateMachineがnull");
                return;
            }

            m_stateMachine.ChengeState(m_stateList[state.ToInt32(null)]);
            OnChangeState(state);
        }

        public virtual void OnChangeState(TEnum state){ }

        public virtual void OnParge(Parts parts)
        {
            Debug.Log(parts + "パージ!!");

            var list = GetPartsList(parts);
            DropWeapon(list);
        }


        // 現在のステートと指定したステートが等しいかを返す
        public virtual bool IsCurrentState(TEnum state)
        {
            return m_stateMachine.GetCurrentState() == m_stateList[state.ToInt32(null)];
        }

        public override void Dead()
        {
            var list = GetLotteryWeapon();
            DropWeapon(list);

            // Animetorに死亡アニメーションを流す処理

            Destroy(this);
            Destroy(gameObject, 3f);
            //var transforms = GetComponentsInChildren<Transform>();
            //Vector3 pos = transform.position + transform.forward * 2;

            //for (int i = 0; i < transforms.Length; i++)
            //{
            //    int n = i;
            //    if (transform == transforms[i]) { continue; }

            //    transforms[n].transform.SetParent(null);
            //    var rd = transforms[n].gameObject.GetComponent<Rigidbody>();
            //    rd = rd != null ? rd : transforms[n].gameObject.AddComponent<Rigidbody>();
            //    if (rd != null)
            //    {
            //        rd.AddExplosionForce(10.0f, pos, 30.0f, 10.0f, ForceMode.Impulse);
            //        Observable.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(5.0f, 6.0f))).Subscribe(_ =>
            //        {
            //            if (rd != null)
            //            {
            //                Destroy(rd.gameObject);
            //            }
            //        });
            //    }
            //}
        }

        protected override void Update()
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.Update();
            }

            if (m_agent)
            {
                m_agent.nextPosition = transform.position;
            }
        }

        public virtual void Move(Vector3 position, bool run = false)
        {
            if (m_agent.isStopped == true)
                m_agent.isStopped = false;

            m_agent.SetDestination(position);

            if (m_agent.remainingDistance > m_agent.stoppingDistance)
            {
                var velocity = m_agent.desiredVelocity;
                if (!run)
                    velocity *= 0.5f;
                m_character.Move(velocity, false, false);
            }
            else
            {
                m_character.Move(new Vector3(0, 0, 0), false, false);                
            }
        }

        public virtual void LookRotate(Vector3 position)
        {
            position.y = transform.position.y;
            transform.LookAt(position);
        }

        public virtual void Rotate(Vector3 position)
        {     
            Vector3 vec = (position - transform.position).normalized;
            vec.y = 0;
            Vector3 fowerd = transform.forward;
            fowerd.y = 0;
            float angle = Vector3.Angle(vec, fowerd);

            if (angle > 10)
            {
                float direction = Mathf.Atan2(fowerd.z, fowerd.x) * Mathf.Rad2Deg;
                float targetDirection = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
                float deltaAngle = Mathf.DeltaAngle(direction, targetDirection);
                if (deltaAngle > 0)
                {
                    m_character.Move(transform.right * -1, false, false);
                }
                else if (deltaAngle < 0)
                {
                    m_character.Move(transform.right, false, false);
                }
            }
            else
            {
                m_character.Move(new Vector3(0, 0, 0), false, false);
            }
        }

        public virtual void Stop()
        {
            m_agent.isStopped = true;

            var t = Observable.Timer(TimeSpan.FromSeconds(1));
            Observable.EveryUpdate().Where(x => m_agent.isStopped).TakeUntil(t).Subscribe(_ => 
            {
                m_agent.SetDestination(transform.position);
                m_agent.nextPosition = transform.position;

                m_character.Move(new Vector3(0, 0, 0), false, false);
            }).AddTo(this);
        }

        public virtual bool IsArrival()
        {
            return m_agent.remainingDistance <= m_agent.stoppingDistance;
        }

        // メイン視界にターゲットがいるかどうか
        public virtual bool IsMainSearch(Transform Target, bool Penetration = false)
        {
            return Search(m_viewPoint, Target, m_enemyStatus.seachMainDis, m_enemyStatus.seachMainAng, Penetration);
        }

        public virtual bool IsMainSearch(bool Penetration = false)
        {
            return Search(m_viewPoint, m_target, m_enemyStatus.seachMainDis, m_enemyStatus.seachMainAng, Penetration);
        }

        // サブ視界にターゲットがいるかどうか
        public virtual bool IsSubSearch(Transform Target, bool Penetration = false)
        {
            return Search(m_viewPoint, Target, m_enemyStatus.seachSubDis, m_enemyStatus.seachSubAng, Penetration);
        }

        public virtual bool IsSubSearch(bool Penetration = false)
        {
            return Search(m_viewPoint, m_target, m_enemyStatus.seachSubDis, m_enemyStatus.seachSubAng, Penetration);
        }

        // 攻撃射程内にターゲットがいるかどうか
        public virtual bool IsAttackSearch(Transform Target, bool Penetration = false)
        {
            return Search(m_viewPoint, Target, m_enemyStatus.attackDis, m_enemyStatus.attackAng, Penetration);
        }

        public virtual bool IsAttackSearch(float distance)
        {
            return Search(m_viewPoint, m_target, m_enemyStatus.attackDis + distance, m_enemyStatus.attackAng, false);
        }

        public virtual bool IsAttackSearch(bool Penetration = false)
        {
            return Search(m_viewPoint, m_target, m_enemyStatus.attackDis, m_enemyStatus.attackAng, Penetration);
        }

        /// <summary>
        /// サーチ関数
        /// </summary>
        /// <param name="my">サーチをする自分自身</param>
        /// <param name="target">サーチ対象</param>
        /// <param name="distance">サーチ距離</param>
        /// <param name="angle">サーチ角度</param>
        /// <param name="Penetration">障害物を無視するかどうか</param>
        /// <returns></returns>
        public virtual bool Search(Transform my, Transform target, float distance = Mathf.Infinity, float angle = 180.0f, bool Penetration = false)
        {
            if(my == null) { return false; }

            Vector3 targetPos = target.position + m_enemyStatus.viewOffset;

            // 対象までのベクトル
            var vec = targetPos - my.position;
            // 対象までの距離(2乗)
            var dis = Vector3.SqrMagnitude(vec);
            // 対象との角度
            var vec2 = vec;
            vec2.y = my.forward.y;
            var ang = Vector3.Angle(my.forward, vec2);

            // 指定した距離以内
            if (dis <= distance * distance)
            {
                // 指定した角度以内
                if (ang <= angle)
                {
                    if (Penetration)
                        return true;

                    // Rayがtrueの場合対象方向にRayを飛ばす
                    if(Physics.Raycast(my.position, vec.normalized, out m_raycastHit, distance) && !Penetration)
                    {
                        Debug.DrawLine(my.position, m_raycastHit.point, Color.blue);
                        // Rayが当たった対象がtargetならtrue
                        if (m_raycastHit.transform.gameObject.tag == target.gameObject.tag)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public float GetSqrDistance()
        {
            var vec = m_target.position - transform.position;
            return Vector3.SqrMagnitude(vec);
        }

        public float GetSqrDistance(Vector3 my, Vector3 target)
        {
            var vec = target - my;
            return Vector3.SqrMagnitude(vec);
        }

        public bool IsInDistance(Vector3 vector, float distance)
        {
            return Vector3.SqrMagnitude(vector) <= distance * distance;
        }

        public bool IsInDistance(Vector3 my, Vector3 target, float distance)
        {
            return GetSqrDistance(my, target) <= distance * distance;
        }

        public virtual void DropWeapon(List<Armor> list)
        {
            for (int j = 0; j < list.Count; j++)
            {
                list[j].transform.SetParent(null);
                list[j].gameObject.GetComponent<Collider>().enabled = true;
                var rd = list[j].gameObject.GetComponent<Rigidbody>();
                rd = rd ? rd : list[j].gameObject.AddComponent<Rigidbody>();

                rd.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rd.ObserveEveryValueChanged(x => x.IsSleeping()).Where(x => x).Take(1).Subscribe(_ =>
                {
                    var cols = rd.transform.GetComponentsInChildren<Collider>();
                    for (int i = 0; i < cols.Length; i++)
                    {
                        cols[i].isTrigger = true;
                    }
                    rd.isKinematic = true;
                });

            }
        }

        private List<Armor> GetLotteryWeapon()
        {
            List<Armor> list = new List<Armor>();

            var count = Enum.GetValues(typeof(Parts)).Length;
            for (int i = 0; i < count; i++)
            {
                var rand = UnityEngine.Random.Range(0.0f, 100.0f);
                if(m_probability >= rand)
                {
                    list.AddRange(GetPartsList((Parts)i));
                }

            }

            return list;
        }

        // 周りを見渡す動作をする
        public virtual void SearchAction(Transform aTransform = null, Action aEndAction = null)
        {
            if(aTransform == null)
            {
                aTransform = transform;
            }

            // 目標角度
            // Vector3 targetVec1, targetVec2;
            // targetVec1 = transform.forward

            // Sequence sequence = DOTween.Sequence();

            //sequence.Append(aTransform.DORotate())
        }
    }
}