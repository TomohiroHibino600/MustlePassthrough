using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// ���f���̃f�[�^��View�ɔ��f����Presenter
    /// </summary>
    public class PresenterOutput : IPresenterOutput
    {
        [Inject]
        PresenterOutput(IModel _model, JellyfishView _jellyfishView, GuidanceView _guidanceView, NumberView _numberView, TrainView _trainView, ResultView _resultView)
        {
            //�؃g���񐔂̕ω���View�ɔ��f
            _model.TrainNumber
                .Subscribe( x => NumberView( x, _numberView, _jellyfishView, _model ) );

            //TrainIndex�̕ω���View�ɔ��f
            _model.TrainIndex
                .Where( x => x < _model.TrainNames.Length )
                .Subscribe(x => IndexView(x, _guidanceView, _trainView, _model));

            //�؃g�����I�������猋�ʂ�View�ŕ\��
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .Subscribe(_ => ResultView( _guidanceView, _trainView, _numberView, _resultView));
        }

        public void NumberView( int number, NumberView _numberView, JellyfishView _jellyfishView, IModel _model) {
            //�؃g�����s�����񐔂�����
            _numberView.ShowNumber(number % _model.GoalNumber);

            //�N���Q�𐶐��B�����Ȃ肷���Ȃ��悤�Ɏ�ڐ��ɂ���Ē����B
            if (number % (int)(_model.TrainNames.Length / 2) == 1) {
                _jellyfishView.SpawnJellyfish();
            }
        }

        public void IndexView(int index, GuidanceView _guidanceView, TrainView _trainView, IModel _model) {
            //�؃g���̎�ږ���\��
            _guidanceView.ShowTrainName(_model.TrainNames[index]);

            //�؃g���񐔂𐔂���UI�̕\��
            _trainView.ShowTrainUI(index);
        }

        public void ResultView(GuidanceView _guidanceView, TrainView _trainView, NumberView _numberView, ResultView _resultView) {
            //�؃g���̎�ږ��Ɛ����邽�߂�GameObject, �؃g���񐔂��\��
            _guidanceView.DisactiveTrainName();
            _trainView.DisactiveAllTrainUI();
            _numberView.DisactiveNumber();

            //�؃g���̌��ʂ�\��
            _resultView.ShowResultUI();
        }
    }

    public interface IPresenterOutput
    {
        void NumberView( int number, NumberView _numberView, JellyfishView _jellyfishView, IModel _model );
        void IndexView( int index, GuidanceView _guidanceView, TrainView _trainView, IModel _model );
        void ResultView( GuidanceView _guidanceView, TrainView _trainView, NumberView _numberView, ResultView _resultView );
    }
}
