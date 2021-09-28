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
            //�݌v�X�R�A��View�ɔ��f

            //TrainIndex�̕ω���View�ɔ��f
            _model.TrainIndex
                .Subscribe(x => IndexView(x))
                .AddTo(this);

            //TrainScore�̕ω���View�ɔ��f
            _model.TrainScore
                .Subscribe(x => ScoreView(x))
                .AddTo(this);

            //�؃g�����I�������猋�ʂ�View�ŕ\��
        }

        void IndexView(int index) {
            //�؃g���̃K�C�hUI���X�V
            //�؃g���̃X�R�AUI�����Z�b�g
            //�؃g���̉񐔁E�b�����v������d�g�݂��X�V
        }

        void ScoreView(float score) {
            //�؃g���̃X�R�AUI���X�V
            //�N���Q�𐶐�
        }
    }
}
