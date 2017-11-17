using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Enemy
{
    public class EnemyDataLink : MonoBehaviour
    {
        [Header("分隊長")]
        public Enemy_Standard _commander;
        [Header("分隊員")]
        public Enemy_Standard[] _squad;
        

    }
}
