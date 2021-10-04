using System.Collections;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// Model�Ƀf�[�^����͂���Presenter
    /// </summary>
    public class PresenterInput : IPresenterInput
    {
        [Inject]
        PresenterInput(IModel _model, TrainView _trainView) {
            //�؃g���񐔂����Z
            _trainView.TrainSubject
                .Subscribe(x => CountTrainNumber(x, _model));

            //�؃g���񐔂�GoalNumber�ɒB������,TrainIndex�����Z
            _model.TrainNumber
                .Where(x => x != 0 & x % _model.GoalNumber == 0)
                .Where( x => x <= _model.TrainNames.Length * _model.GoalNumber )
                .Subscribe(_ => { _model.TrainIndex.Value++; } );
        }

        public void CountTrainNumber(int x, IModel _model) {
            //�Ō�̋؃g�����I����܂ŁA�؃g���񐔂��J�E���g����
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
