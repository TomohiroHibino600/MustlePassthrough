using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// モデルのデータをViewに反映するPresenter
    /// </summary>
    public class PresenterOutput : IPresenterOutput
    {
        [Inject]
        PresenterOutput(IModel _model, JellyfishView _jellyfishView, GuidanceView _guidanceView, NumberView _numberView, TrainView _trainView, ResultView _resultView)
        {
            //筋トレ回数の変化をViewに反映
            _model.TrainNumber
                .Subscribe( x => NumberView( x, _numberView, _jellyfishView, _model ) );

            //TrainIndexの変化をViewに反映
            _model.TrainIndex
                .Where( x => x < _model.TrainNames.Length )
                .Subscribe(x => IndexView(x, _guidanceView, _trainView, _model));

            //筋トレが終了したら結果をViewで表示
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .Subscribe(_ => ResultView( _guidanceView, _trainView, _numberView, _resultView));
        }

        public void NumberView( int number, NumberView _numberView, JellyfishView _jellyfishView, IModel _model) {
            //筋トレを行った回数を示す
            _numberView.ShowNumber(number % _model.GoalNumber);

            //クラゲを生成。多くなりすぎないように種目数によって調整。
            if (number % (int)(_model.TrainNames.Length / 2) == 1) {
                _jellyfishView.SpawnJellyfish();
            }
        }

        public void IndexView(int index, GuidanceView _guidanceView, TrainView _trainView, IModel _model) {
            //筋トレの種目名を表示
            _guidanceView.ShowTrainName(_model.TrainNames[index]);

            //筋トレ回数を数えるUIの表示
            _trainView.ShowTrainUI(index);
        }

        public void ResultView(GuidanceView _guidanceView, TrainView _trainView, NumberView _numberView, ResultView _resultView) {
            //筋トレの種目名と数えるためのGameObject, 筋トレ回数を非表示
            _guidanceView.DisactiveTrainName();
            _trainView.DisactiveAllTrainUI();
            _numberView.DisactiveNumber();

            //筋トレの結果を表示
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
