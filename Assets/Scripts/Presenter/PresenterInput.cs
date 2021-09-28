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
            //TrainIndex‚É‰ÁZ
            //TrainScore‚É‰ÁZ
            //TrainScores‚ğ•ÏX
            //ÅIŒ‹‰Ê‚ğCSV‚É‘‚«‚Ş
        }
    }
}
