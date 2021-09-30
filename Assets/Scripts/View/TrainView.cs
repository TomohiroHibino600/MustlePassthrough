using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    public class TrainView : MonoBehaviour
    {
        public Subject<int> _trainSubject = new Subject<int>( );

        public void AddTrainNumber(int value) {
            _trainSubject.OnNext(value);
        }

        void OnDestroy( ) {
            _trainSubject.OnCompleted();
            _trainSubject.Dispose();
        }
    }
}
