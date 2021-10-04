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
        [SerializeField] GameObject[] _trainUI = null;

        private Subject<int> _trainSubject = new Subject<int>( );
        public IObservable<int> TrainSubject => _trainSubject.TakeUntilDestroy(this);

        void Start( ) {
            if ( _trainUI.Length > 0 ) {
                DisactiveAllTrainUI( );
                _trainUI[ 0 ].SetActive( true );
            }
        }

        void Update( ) {
            if ( _trainUI.Length <= 0 ) {
                _trainUI = GameObject.FindGameObjectsWithTag( "TrainUI" );
                DisactiveAllTrainUI( );
                _trainUI[ 0 ].SetActive( true );
            }
        }

        public void AddTrainNumber(int value) {
            _trainSubject.OnNext(value);
        }

        public void ShowTrainUI(int trainIndex) {
            DisactiveAllTrainUI();
            _trainUI[ trainIndex ].SetActive( true );
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
