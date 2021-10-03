using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;
using DG.Tweening;

namespace MustlePassthrough
{
    /// <summary>
    /// �؃g���񐔂����Z���邽�߂�GameObject�̊Ǘ�
    /// </summary>
    public class TrainView : MonoBehaviour
    {
        [SerializeField] GameObject[] _trainUI = null;
        [SerializeField] float _waitTime = 3f;

        private Subject<int> _trainSubject = new Subject<int>( );
        public IObservable<int> TrainSubject => _trainSubject.TakeUntilDestroy(this);

        void Awake( ) {
            DisactiveAllTrainUI();
        }

        public void AddTrainNumber(int value) {
            _trainSubject.OnNext(value);
        }

        public void ShowTrainUI(int trainIndex) {
            DisactiveAllTrainUI();
            DOVirtual.DelayedCall(_waitTime, () => {
                _trainUI[ trainIndex ].SetActive( true );
            });
        }

        public void DisactiveAllTrainUI( ) {
            for ( int i = 0; i < _trainUI.Length; i++ ) {
                _trainUI[ i ].SetActive( false );
            }
        }

        void OnDestroy( ) {
            _trainSubject.OnCompleted();
            _trainSubject.Dispose();
        }
    }
}
