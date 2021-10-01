using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// Modelにデータを入力するPresenter
    /// </summary>
    public class PresenterInput : MonoBehaviour
    {
        [SerializeField] Model _model = null;

        [SerializeField] TrainView _trainView = null;

        void Start( ) {
            //筋トレ回数を加算
            _trainView.TrainSubject
                .TakeUntilDestroy(this)
                .Subscribe(x => CountTrainNumber(x));

            //筋トレ回数がGoalNumberに達したら,TrainIndexを加算
            _model.TrainNumber
                .Where(x => x != 0 & x % _model.GoalNumber == 0)
                .Where( x => x <= _model.TrainNames.Length * _model.GoalNumber )
                .TakeUntilDestroy(this)
                .Subscribe(_ => { _model.TrainIndex.Value++; } );
        }

        void CountTrainNumber(int x) {
            //最後の筋トレを終えるまで、筋トレ回数をカウントする
            if ( _model.TrainNumber.Value < _model.TrainNames.Length * _model.GoalNumber ) {
                _model.TrainNumber.Value += x;
            }
        }
    }
}
