using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// 筋トレの種目やその解説を示すViewの管理
    /// </summary>
    public class GuidanceView : MonoBehaviour
    {
        //位置調整用の変数
        private bool _putUnder = false;
        private bool _putFront = false;
        private Vector3 _firstPosition = Vector3.zero;
        private Quaternion _firstRot = Quaternion.identity;
        [SerializeField] Transform _underPoint = null;
        [SerializeField] Transform _mainCamera = null;
        [SerializeField] float _posChangeRot = 60f;

        //UI用の変数
        [SerializeField] GameObject _trainNameObj = null;
        [SerializeField] TextMeshProUGUI _trainName = null;

        private void Start( ) {
            //最初の基準点を取得
            _firstPosition = _trainNameObj.transform.localPosition;
            _firstRot = _trainNameObj.transform.localRotation;

            //カメラが前傾していない場合は正面にUIを置く
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x <= _posChangeRot & !_putFront )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutFront( ) );

            //カメラが傾いたら床の下にUIを移動させる
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x > _posChangeRot & !_putUnder )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutUnder( ) );
        }

        public void ShowTrainName(string trainName) {
            _trainName.text = trainName;
        }
        
        public void DisactiveTrainName( ) {
            _trainNameObj.SetActive(false);
        }

        void PutUnder( ) {
            //特定の位置にUIを置き、上を向かせる
            _trainNameObj.transform.SetPositionAndRotation( _underPoint.position, Quaternion.Euler( 90f, 0f, 0f ) );

            //Update中に何度も実行しないようフラグ変更
            _putUnder = true;
            _putFront = false;
        }

        void PutFront( ) {
            //最初の位置にUIを置く
            _trainNameObj.transform.SetPositionAndRotation( _firstPosition, _firstRot );

            //Update中に何度も実行しないようフラグ変更
            _putUnder = false;
            _putFront = true;
        }
    }
}
