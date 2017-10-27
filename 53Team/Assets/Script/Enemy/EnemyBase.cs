using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace Enemy
{
    public interface IEnemy
    {

        // ダメージ関数
        void Damage(int damage);
    }



    [Serializable]
    public class EnemyStatus
    {
        public string name;
        public int hp;
        public int max_hp;
        public float attackDis;
        public float attackAng;
        public float seachMainDis;
        public float seachMainAng;
        public float seachSubDis;
        public float seachSubAng;
        public float rotateSpd;


        public EnemyStatus(string name, int hp, int max_hp, float attackDis, float attackAng, float seachMainDis, float seachMainAng, float seachSubDis, float seachSubAng, float rotateSpd)
        {
            this.name = name;
            this.hp = hp;
            this.max_hp = max_hp;
            this.attackDis = attackDis;
            this.attackAng = attackAng;
            this.seachMainDis = seachMainDis;
            this.seachMainAng = seachMainAng;
            this.seachSubDis = seachSubDis;
            this.seachSubAng = seachSubAng;
            this.rotateSpd = rotateSpd;
        }
    }

    // Enemyの基底クラス
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBase<T, TEnum> : CharaBase
        where T : class where TEnum : IConvertible
    {
        // 開始のタイミングを制御するかどうか
        public bool m_PlayAwake = true;

        [Header("ターゲット")]
        public Transform m_target;

        [Header("各部位の当たり判定")]
        public BoneCollide[] m_boneCollides;

        [Header("デバッグ用パーツ")]
        public Armor m_armor;

        // Enemyの初期ステータス
        [Space(10)]
        public EnemyStatus m_enemyStatus;

        [Space(10)]
        public Sector m_sectorAtk;
        public Sector m_sectorMain;
        public Sector m_sectorSub;
        public bool m_seachAreaDraw;

        // RaycastHit
        protected RaycastHit m_raycastHit;

        // NavMeshAgent
        protected NavMeshAgent m_agent;

        // 状態を格納する配列
        protected List<State<T>> m_stateList = new List<State<T>>();

        // ステートマシンクラス
        protected StateMachine<T> m_stateMachine;
        
        protected override void Start()
        {
            m_agent = GetComponent<NavMeshAgent>();

            m_enemyStatus.hp = m_enemyStatus.max_hp;
            m_agent.angularSpeed = 30 * m_enemyStatus.rotateSpd;

            if (m_armor != null) { PartsAdd(Parts.RightArm, m_armor); }


            for (int i = 0; i < m_boneCollides.Length; i++)
            {
                int n = i;
                m_boneCollides[n].OnDamage.Subscribe(dmg =>
                {
                    Parts parts = m_boneCollides[n].m_parts;
                    Debug.LogFormat("Hit!!!!!!  Parts.{0} {1}damage", parts.ToString(), dmg);
                    PartsDamage(dmg, parts, () => {
                        Debug.Log(parts + "パージ!!");
                        m_armor.gameObject.GetComponent<Collider>().enabled = true;
                        m_armor.gameObject.AddComponent<Rigidbody>();
                    });
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
        }

        // 現在のステートと指定したステートが等しいかを返す
        public virtual bool IsCurrentState(TEnum state)
        {
            return m_stateMachine.GetCurrentState() == m_stateList[state.ToInt32(null)];
        }

        protected override void Update()
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.Update();
            }
        }

        /// <summary>
        /// サーチ関数
        /// </summary>
        /// <param name="my">サーチをする自分自身</param>
        /// <param name="target">サーチ対象</param>
        /// <param name="distance">サーチ距離</param>
        /// <param name="angle">サーチ角度</param>
        /// <param name="ray">Rayを飛ばすかどうか(障害物を無視するかどうか)</param>
        /// <returns></returns>
        public virtual bool Search(Transform my, Transform target, float distance = Mathf.Infinity, float angle = 180.0f, bool ray = false)
        {
            if(my == null) { return false; }

            bool run = false;

            Vector3 targetPos = target.position;
            targetPos.y += 0.5f;

            // 対象までのベクトル
            var vec = targetPos - my.position;
            // 対象までの距離(2乗)
            var dis = Vector3.SqrMagnitude(vec);
            // 対象との角度
            var vec2 = vec;
            vec2.y = my.forward.y;
            var ang = Vector3.Angle(my.forward, vec2);

            // 指定した距離以内
            if(dis <= distance * distance)
            {
                Debug.DrawLine(my.position, targetPos, Color.blue);

                // 指定した角度以内
                if (ang <= angle)
                {
                    if(!ray)
                        run = true;

                    // Rayがtrueの場合対象方向にRayを飛ばす
                    if(Physics.Raycast(my.position, vec.normalized, out m_raycastHit, distance) && ray)
                    {
                        // Rayが当たった対象がtargetならtrue
                        if(m_raycastHit.transform.gameObject == target.gameObject)
                        {
                            run = true;
                            //Debug.Log("HitObjct Name." + m_raycastHit.transform.name);
                        }
                    }
                }
            }

            return run;
        }

        public Subject<int> OnDead
        {
            get; set;
        }

        // 角度を求める(xy面上)
        public virtual float GetAngle(Vector2 p1, Vector2 p2)
        {
            float x = p2.x - p1.x;
            float y = p2.y - p1.y;
            float rad = Mathf.Atan2(y, x);
            return rad * Mathf.Rad2Deg;
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

            Sequence sequence = DOTween.Sequence();

            //sequence.Append(aTransform.DORotate())
        }
    }
}