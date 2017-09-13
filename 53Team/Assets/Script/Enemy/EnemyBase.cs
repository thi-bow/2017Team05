using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    [Serializable]
    public class EnemyStatus
    {
        public string name;
        public int hp;
        public int max_hp;

        public EnemyStatus(string name, int hp, int max_hp)
        {
            this.name = name;
            this.hp = hp;
            this.max_hp = max_hp;
        }
    }

    // Enemyの基底クラス
    public class EnemyBase<T, TEnum> : MonoBehaviour
        where T : class where TEnum : IConvertible
    {
        // Enemyの初期ステータス
        public EnemyStatus m_enemyStatus;

        // 状態を格納する配列
        protected List<State<T>> m_stateList = new List<State<T>>();

        // ステートマシンクラス
        protected StateMachine<T> m_stateMachine;

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
        /// <param name="ray">障害物を貫通するかどうか</param>
        /// <returns></returns>
        public virtual bool Search(Transform my, Transform target, float distance = Mathf.Infinity, float angle = 180.0f, bool ray = true)
        {
            bool run = false;

            // 対象までのベクトル
            var vec = target.position - my.position;
            Debug.DrawLine(my.position, target.position, Color.blue);

            // 対象までの距離(2乗)
            var dis = Vector3.SqrMagnitude(vec);
            // 対象との角度
            var ang = Vector3.Angle(my.forward, vec);

            if(dis <= distance * distance)
            {
                if(ang <= angle)
                {
                    run = true;
                }
            }

            return run;
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