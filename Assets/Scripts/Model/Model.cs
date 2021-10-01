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
    public class Model : MonoBehaviour
    {
        //�S�؃g���̏��Ԃ��Ǘ�����index
        public readonly IntReactiveProperty TrainIndex = new IntReactiveProperty(0);
        
        //�؃g������
        public readonly IntReactiveProperty TrainNumber = new IntReactiveProperty(0);
        
        //�e�؃g���̖ڕW��
        public readonly int GoalNumber = 10;
        
        //�؃g���̑S��ږ�
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
