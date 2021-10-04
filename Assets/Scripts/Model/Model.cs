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
    public class Model : IModel
    {
        //全筋トレの順番を管理するindex
        private IntReactiveProperty _trainIndex = new IntReactiveProperty(0);
        public IReactiveProperty<int> TrainIndex { get => _trainIndex; }

        //筋トレ総回数
        private IntReactiveProperty _trainNumber = new IntReactiveProperty(0);
        public IReactiveProperty<int> TrainNumber { get => _trainNumber; }
        
        //各筋トレの目標数
        private int _goalNumber = 10;
        public int GoalNumber { get => _goalNumber; }
        
        //筋トレの全種目名
        private string[] _trainNames = {
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
        public string[] TrainNames { get => _trainNames; }
    }

    public interface IModel
    {
        IReactiveProperty<int> TrainIndex { get; }
        IReactiveProperty<int> TrainNumber { get; }
        int GoalNumber { get; }
        string[] TrainNames { get; }
    }
}
