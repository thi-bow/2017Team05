using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

namespace Enemy
{

    [Serializable]
    public class EnemyStatus
    {
        public string name;
        public int hp;
        public int max_hp;
        public float seachMainDis;
        public float seachMainAng;
        public float seachSubDis;
        public float seachSubAng;
        public float rotateSpd;


        public EnemyStatus(string name, int hp, int max_hp, float seachMainDis, float seachMainAng, float seachSubDis, float seachSubAng, float rotateSpd)
        {
            this.name = name;
            this.hp = hp;
            this.max_hp = max_hp;
            this.seachMainDis = seachMainDis;
            this.seachMainAng = seachMainAng;
            this.seachSubDis = seachSubDis;
            this.seachSubAng = seachSubAng;
            this.rotateSpd = rotateSpd;
        }
    }

    // Enemyの基底クラス
    // [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class EnemyBase<T, TEnum> : MonoBehaviour
        where T : class where TEnum : IConvertible
    {
        // Enemyの初期ステータス
        public EnemyStatus m_enemyStatus;

        [Space(10)]
        public Sector m_sectorMain;
        public Sector m_sectorSub;
        public bool m_seachAreaDraw;

        // RaycastHit
        protected RaycastHit m_raycastHit;

        // 状態を格納する配列
        protected List<State<T>> m_stateList = new List<State<T>>();

        // ステートマシンクラス
        protected StateMachine<T> m_stateMachine;

        protected virtual void Start()
        {
            if (m_seachAreaDraw)
            {
                // メイン視界の描画
                var startdeg = 90 - m_enemyStatus.seachMainAng;
                var enddeg = 90 + m_enemyStatus.seachMainAng;
                m_sectorMain.Show(m_enemyStatus.seachMainDis, startdeg, enddeg);

                // サブ視界の描画
                startdeg = 90 - m_enemyStatus.seachSubAng;
                enddeg = 90 + m_enemyStatus.seachSubAng;
                m_sectorSub.Show(m_enemyStatus.seachSubDis, startdeg, enddeg);
            }
        }

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

        protected virtual void Update()
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
            bool run = false;

            // 対象までのベクトル
            var vec = target.position - my.position;
            Debug.DrawLine(my.position, target.position, Color.blue);

            // 対象までの距離(2乗)
            var dis = Vector3.SqrMagnitude(vec);
            // 対象との角度
            var ang = Vector3.Angle(my.forward, vec);

            // 指定した距離以内
            if(dis <= distance * distance)
            {
                // 指定した角度以内
                if(ang <= angle)
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
                            Debug.Log("HitObjct Name." + m_raycastHit.transform.name);
                        }
                    }
                }
            }

            return run;
        }

        public Subject<int> OnDead
        {
            get;
            set;
        }

        // 角度を求める(xy面上)
        public virtual float GetAngle(Vector2 p1, Vector2 p2)
        {
            float x = p2.x - p1.x;
            float y = p2.y - p1.y;
            float rad = Mathf.Atan2(y, x);
            return rad * Mathf.Rad2Deg;
        }
    }
}