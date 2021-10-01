using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// 筋トレにかかった時間などの結果を示すView
    /// </summary>
    public class ResultView : MonoBehaviour
    {
        private float _trainTime = 0f;
        private bool _endTraining = false;

        [SerializeField] GameObject _resultUI = null;
        [SerializeField] TextMeshProUGUI _resultText = null;

        void Start( ) {
            //最初は結果を非表示
            _resultUI.SetActive(false);

            //筋トレ時間の計測
            this.UpdateAsObservable( )
                .Where( _ => !_endTraining )
                .TakeUntilDestroy( this )
                .Subscribe( _ => { _trainTime += Time.deltaTime; } );
        }

        public void ShowResultUI( ) {
            //筋トレ時間の計測を止める
            _endTraining = true;
            
            //筋トレ結果を示すUIを表示
            _resultUI.SetActive(true);

            //筋トレ結果を示すテキストを表示
            _resultText.text = $"Training Time<br>{(_trainTime/60f).ToString("F0")}: {( _trainTime % 60f ).ToString( "F0" )}";
        }
    }
}
