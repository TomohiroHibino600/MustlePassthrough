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
            //累計スコアをViewに反映

            //TrainIndexの変化をViewに反映
            _model.TrainIndex
                .Subscribe(x => IndexView(x))
                .AddTo(this);

            //TrainScoreの変化をViewに反映
            _model.TrainScore
                .Subscribe(x => ScoreView(x))
                .AddTo(this);

            //筋トレが終了したら結果をViewで表示
        }

        void IndexView(int index) {
            //筋トレのガイドUIを更新
            //筋トレのスコアUIをリセット
            //筋トレの回数・秒数を計測する仕組みを更新
        }

        void ScoreView(float score) {
            //筋トレのスコアUIを更新
            //クラゲを生成
        }
    }
}
