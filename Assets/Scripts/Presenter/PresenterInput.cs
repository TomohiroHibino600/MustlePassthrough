using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// Model�Ƀf�[�^����͂���Presenter
    /// </summary>
    public class PresenterInput : MonoBehaviour
    {
        [SerializeField] Model _model = null;

        [SerializeField] TrainView _trainView = null;

        void Start( ) {
            //�؃g���񐔂����Z
            _trainView.TrainSubject
                .TakeUntilDestroy(this)
                .Subscribe(x => CountTrainNumber(x));

            //�؃g���񐔂�GoalNumber�ɒB������,TrainIndex�����Z
            _model.TrainNumber
                .Where(x => x != 0 & x % _model.GoalNumber == 0)
                .Where( x => x <= _model.TrainNames.Length * _model.GoalNumber )
                .TakeUntilDestroy(this)
                .Subscribe(_ => { _model.TrainIndex.Value++; } );
        }

        void CountTrainNumber(int x) {
            //�Ō�̋؃g�����I����܂ŁA�؃g���񐔂��J�E���g����
            if ( _model.TrainNumber.Value < _model.TrainNames.Length * _model.GoalNumber ) {
                _model.TrainNumber.Value += x;
            }
        }
    }
}
