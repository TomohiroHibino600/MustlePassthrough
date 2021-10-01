using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

namespace MustlePassthrough
{
    /// <summary>
    /// ‹ØƒgƒŒ‰ñ”‚ğ‰ÁZ‚·‚é‚½‚ß‚ÌGameObject‚ÌŠÇ—
    /// </summary>
    public class TrainView : MonoBehaviour
    {
        [SerializeField] GameObject[] TrainUI = null;

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
            TrainUI[trainIndex].SetActive(true);
        }

        public void DisactiveAllTrainUI( ) {
            for ( int i = 0; i < TrainUI.Length; i++ ) {
                TrainUI[ i ].SetActive( false );
            }
        }

        void OnDestroy( ) {
            _trainSubject.OnCompleted();
            _trainSubject.Dispose();
        }
    }
}
