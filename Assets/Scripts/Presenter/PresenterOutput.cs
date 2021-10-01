using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// モデルのデータをViewに反映するPresenter
    /// </summary>
    public class PresenterOutput : MonoBehaviour
    {
        [SerializeField] Model _model = null;

        [SerializeField] JellyfishView _jellyfishView = null;
        [SerializeField] GuidanceView _guidanceView = null;
        [SerializeField] NumberView _numberView = null;
        [SerializeField] TrainView _trainView = null;
        [SerializeField] ResultView _resultView = null;

        // Start is called before the first frame update
        void Start( )
        {
            //筋トレ回数の変化をViewに反映
            _model.TrainNumber
                .TakeUntilDestroy( this )
                .Subscribe( x => NumberView( x ) );

            //TrainIndexの変化をViewに反映
            _model.TrainIndex
                .Where( x => x < _model.TrainNames.Length )
                .TakeUntilDestroy(this)
                .Subscribe(x => IndexView(x));

            //筋トレが終了したら結果をViewで表示
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .TakeUntilDestroy(this)
                .Subscribe(_ => ResultView());
        }

        void NumberView( int number ) {
            //筋トレを行った回数を示す
            _numberView.ShowNumber(number % _model.GoalNumber);

            //クラゲを生成。多くなりすぎないように種目数によって調整。
            if (number % (int)(_model.TrainNames.Length / 2) == 1) {
                _jellyfishView.SpawnJellyfish();
            }
        }

        void IndexView(int index) {
            //筋トレの種目名を表示
            _guidanceView.ShowTrainName(_model.TrainNames[index]);

            //筋トレ回数を数えるUIの表示
            _trainView.ShowTrainUI(index);
        }

        void ResultView( ) {
            //筋トレの種目名と数えるためのGameObject, 筋トレ回数を非表示
            _guidanceView.DisactiveTrainName();
            _trainView.DisactiveAllTrainUI();
            _numberView.DisactiveNumber();

            //筋トレの結果を表示
            _resultView.ShowResultUI();
        }
    }
}
