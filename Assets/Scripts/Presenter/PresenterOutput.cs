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
            //����܂ł̗݌v�X�R�A��View�ɔ��f

            //TrainIndex�̕ω���View�ɔ��f
            _model.TrainIndex
                .TakeUntilDestroy(this)
                .Subscribe(x => IndexView(x));

            //�؃g���񐔂̕ω���View�ɔ��f
            _model.TrainNumber
                .TakeUntilDestroy(this)
                .Subscribe(x => NumberView(x));

            //�؃g�����I�������猋�ʂ�View�ŕ\��
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .TakeUntilDestroy(this)
                .Subscribe(_ => ResultView());
        }

        void IndexView(int index) {
            //�؃g���̃K�C�hUI���X�V
            //�؃g���̃X�R�AUI�����Z�b�g
            //�؃g���̉񐔁E�b�����v������d�g�݂��X�V
        }

        void NumberView(int number) {
            //�؃g���̃X�R�AUI���X�V
            //���������A�X�R�AUI��_model.GoalNumber�̏�]�ŕ\������
            //�N���Q�𐶐�
        }

        void ResultView( ) {

        }
    }
}
