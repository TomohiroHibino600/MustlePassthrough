using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// �؃g���ɂ����������ԂȂǂ̌��ʂ�����View
    /// </summary>
    public class ResultView : MonoBehaviour
    {
        private float _trainTime = 0f;
        private bool _endTraining = false;

        [SerializeField] GameObject _resultUI = null;
        [SerializeField] TextMeshProUGUI _resultText = null;

        void Start( ) {
            //�ŏ��͌��ʂ��\��
            _resultUI.SetActive(false);

            //�؃g�����Ԃ̌v��
            this.UpdateAsObservable( )
                .Where( _ => !_endTraining )
                .TakeUntilDestroy( this )
                .Subscribe( _ => { _trainTime += Time.deltaTime; } );
        }

        public void ShowResultUI( ) {
            //�؃g�����Ԃ̌v�����~�߂�
            _endTraining = true;
            
            //�؃g�����ʂ�����UI��\��
            _resultUI.SetActive(true);

            //�؃g�����ʂ������e�L�X�g��\��
            _resultText.text = $"Training Time<br>{(_trainTime/60f).ToString("F0")}: {( _trainTime % 60f ).ToString( "F0" )}";
        }
    }
}
