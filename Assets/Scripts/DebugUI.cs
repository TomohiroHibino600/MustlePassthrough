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

        private void Update( ) {
            if ( Input.GetKeyDown( KeyCode.U ) ) {
                _trainView.AddTrainNumber( 1 );
            }
        }
    }
}
