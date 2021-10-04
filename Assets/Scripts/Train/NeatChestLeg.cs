using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// ニートチェストレッグの設定とスコアカウント
    /// </summary>
    public class NeatChestLeg : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private Vector3 _legPos = Vector3.zero;
        private Vector3 _lastLegPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //Sphereを動かし、目標地点を決め、その目標地点にSphereを固定
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //目標地点が決まったら、フラグを切り替える
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { _setTarget = true; } );

            //部位と固定した球が接したらTrainViewに筋トレ回数を入力
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject.tag == "Leg" )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //部位が固定した球から離れたら球の色を元に戻す
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject.tag == "Leg" )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //足の位置を取得
            _legPos = ( _leftFoot.position + _rightFoot.position ) / 2f;

            //足がカメラに近づくときのみ
            if ( Vector3.SqrMagnitude( _legPos - _mainCamera.position ) < Vector3.SqrMagnitude( _lastLegPos - _mainCamera.position ) ) {
                //Sphereを足のポジションに追随させる
                transform.position = ( _leftFoot.position + _rightFoot.position ) / 2f;
            }

            //最新の足の位置を保持
            _lastLegPos = ( _leftFoot.position + _rightFoot.position ) / 2f;
        }

        void AddTrainNum( Collider collider ) {
            //筋トレ回数を加算する
            _trainView.AddTrainNumber( 1 );

            //Sphereのマテリアルを交換し、TriggerExitでまた元に戻す
            _renderer.material = _materials[ 1 ];
        }
    }
}
