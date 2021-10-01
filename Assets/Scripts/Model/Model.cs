using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// モデル。
    /// PresenterInputがモデルへのデータ入力、
    /// PresenterOutputがモデルからGameObjectへのデータの出力を行っている
    /// </summary>
    public class Model : MonoBehaviour
    {
        //全筋トレの順番を管理するindex
        public readonly IntReactiveProperty TrainIndex = new IntReactiveProperty(0);
        
        //筋トレ総回数
        public readonly IntReactiveProperty TrainNumber = new IntReactiveProperty(0);
        
        //各筋トレの目標数
        public readonly int GoalNumber = 10;
        
        //筋トレの全種目名
        public readonly string[] TrainNames = {
            "Squat",
            "PushUp",
            "Superman",
            "Plank",
            "Cycling",
            "Crunch",
            "NeatChest",
            "SideVent",
            "SidePlank"
        };
    }
}
