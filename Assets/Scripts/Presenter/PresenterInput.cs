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
            //�؃g���񐔂����Z
            _train._trainSubject
                .TakeUntilDestroy(this)
                .Subscribe(x => CountTrainNumber(x));

            //�؃g��10��ڂ�TrainIndex�����Z
            _model.TrainNumber
                .Where(x => x != 0 & x % _model.GoalNumber == 0)
                .Where( x => x <= _model.TrainNames.Length * _model.GoalNumber )
                .TakeUntilDestroy(this)
                .Subscribe(_ => { _model.TrainIndex.Value++; } );

            //�ŏI���ʂ�CSV�ɏ�������
        }

        void CountTrainNumber(int x) {
            if ( _model.TrainNumber.Value < _model.TrainNames.Length * _model.GoalNumber ) {
                _model.TrainNumber.Value += x;
            }
        }
    }
}
