using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    public class PresenterOutput : MonoBehaviour
    {
        [SerializeField] Model _model;
        [SerializeField] CumulativeScore _cumulativeScore;

        [SerializeField] JellyfishView _jellyfish;
        [SerializeField] GuidanceView _guidance;
        [SerializeField] ScoreView _score;
        [SerializeField] TrainView _train;
        [SerializeField] ResultView _result;

        // Start is called before the first frame update
        void Start( )
        {
            //これまでの累計スコアをViewに反映

            //TrainIndexの変化をViewに反映
            _model.TrainIndex
                .TakeUntilDestroy(this)
                .Subscribe(x => IndexView(x));

            //筋トレ回数の変化をViewに反映
            _model.TrainNumber
                .TakeUntilDestroy(this)
                .Subscribe(x => NumberView(x));

            //筋トレが終了したら結果をViewで表示
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .TakeUntilDestroy(this)
                .Subscribe(_ => ResultView());
        }

        void IndexView(int index) {
            //筋トレのガイドUIを更新
            //筋トレのスコアUIをリセット
            //筋トレの回数・秒数を計測する仕組みを更新
        }

        void NumberView(int number) {
            //筋トレのスコアUIを更新
            //※ただし、スコアUIは_model.GoalNumberの剰余で表示する
            //クラゲを生成
        }

        void ResultView( ) {

        }
    }
}
