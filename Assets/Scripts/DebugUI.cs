using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MustlePassthrough {
    /// <summary>
    /// Uキーを押すと、筋トレしたと判断されたときの挙動を行う
    /// </summary>
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        void Start( ) {
            if (_trainView == null) {
                _trainView = GameObject.FindGameObjectWithTag("TrainView").GetComponent<TrainView>();
            }
        }

        private void Update( ) {
            if ( Input.GetKeyDown( KeyCode.U ) ) {
                _trainView.AddTrainNumber( 1 );
            }
        }
    }
}
