using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MustlePassthrough {
    /// <summary>
    /// U�L�[�������ƁA�؃g�������Ɣ��f���ꂽ�Ƃ��̋������s��
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
