using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// ���f���B
    /// PresenterInput�����f���ւ̃f�[�^���́A
    /// PresenterOutput�����f������GameObject�ւ̃f�[�^�̏o�͂��s���Ă���
    /// </summary>
    public class Model : IModel
    {
        //�S�؃g���̏��Ԃ��Ǘ�����index
        private IntReactiveProperty _trainIndex = new IntReactiveProperty(0);
        public IReactiveProperty<int> TrainIndex { get => _trainIndex; }

        //�؃g������
        private IntReactiveProperty _trainNumber = new IntReactiveProperty(0);
        public IReactiveProperty<int> TrainNumber { get => _trainNumber; }
        
        //�e�؃g���̖ڕW��
        private int _goalNumber = 10;
        public int GoalNumber { get => _goalNumber; }
        
        //�؃g���̑S��ږ�
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
