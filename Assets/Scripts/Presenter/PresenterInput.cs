using System.Collections;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// Modelにデータを入力するPresenter
    /// </summary>
    public class PresenterInput : IPresenterInput
    {
        [Inject]
        PresenterInput(IModel _model, TrainView _trainView) {
            //筋トレ回数を加算
            _trainView.TrainSubject
                .Subscribe(x => CountTrainNumber(x, _model));

            //筋トレ回数がGoalNumberに達したら,TrainIndexを加算
            _model.TrainNumber
                .Where(x => x != 0 & x % _model.GoalNumber == 0)
                .Where( x => x <= _model.TrainNames.Length * _model.GoalNumber )
                .Subscribe(_ => { _model.TrainIndex.Value++; } );
        }

        public void CountTrainNumber(int x, IModel _model) {
            //最後の筋トレを終えるまで、筋トレ回数をカウントする
            if ( _model.TrainNumber.Value < _model.TrainNames.Length * _model.GoalNumber ) {
                _model.TrainNumber.Value += x;
            }
        }
    }

    public interface IPresenterInput
    {
        void CountTrainNumber( int x, IModel _model);
    }
}
