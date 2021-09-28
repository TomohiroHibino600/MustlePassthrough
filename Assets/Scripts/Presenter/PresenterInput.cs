using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    public class PresenterInput : MonoBehaviour
    {
        [SerializeField] Model _model;
        [SerializeField] CumulativeScore _cumulativeScore;

        [SerializeField] JellyfishView _jellyfish;
        [SerializeField] GuidanceView _guidance;
        [SerializeField] ScoreView _score;
        [SerializeField] TrainView _train;
        [SerializeField] ResultView _result;

        // Start is called before the first frame update
        void Start( ) {
            //TrainIndexに加算
            //TrainScoreに加算
            //TrainScoresを変更
            //最終結果をCSVに書き込む
        }
    }
}
