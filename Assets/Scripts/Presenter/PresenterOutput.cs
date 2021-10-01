using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// ���f���̃f�[�^��View�ɔ��f����Presenter
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
            //�؃g���񐔂̕ω���View�ɔ��f
            _model.TrainNumber
                .TakeUntilDestroy( this )
                .Subscribe( x => NumberView( x ) );

            //TrainIndex�̕ω���View�ɔ��f
            _model.TrainIndex
                .Where( x => x < _model.TrainNames.Length )
                .TakeUntilDestroy(this)
                .Subscribe(x => IndexView(x));

            //�؃g�����I�������猋�ʂ�View�ŕ\��
            _model.TrainIndex
                .Where(x => x == _model.TrainNames.Length)
                .TakeUntilDestroy(this)
                .Subscribe(_ => ResultView());
        }

        void NumberView( int number ) {
            //�؃g�����s�����񐔂�����
            _numberView.ShowNumber(number % _model.GoalNumber);

            //�N���Q�𐶐��B�����Ȃ肷���Ȃ��悤�Ɏ�ڐ��ɂ���Ē����B
            if (number % (int)(_model.TrainNames.Length / 2) == 1) {
                _jellyfishView.SpawnJellyfish();
            }
        }

        void IndexView(int index) {
            //�؃g���̎�ږ���\��
            _guidanceView.ShowTrainName(_model.TrainNames[index]);

            //�؃g���񐔂𐔂���UI�̕\��
            _trainView.ShowTrainUI(index);
        }

        void ResultView( ) {
            //�؃g���̎�ږ��Ɛ����邽�߂�GameObject, �؃g���񐔂��\��
            _guidanceView.DisactiveTrainName();
            _trainView.DisactiveAllTrainUI();
            _numberView.DisactiveNumber();

            //�؃g���̌��ʂ�\��
            _resultView.ShowResultUI();
        }
    }
}
