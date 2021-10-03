using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// サイクリングの設定とスコアカウント
    /// </summary>
    public class CyclingLeg : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;
        [SerializeField] bool _isLeft = false;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private Vector3 _maxPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //ヘッドセットを動かす目標地点にHeadSphereを近づける
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //目標地点が決まったら、フラグを切り替える
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { _setTarget = true; } );

            //ヘッドセットとHeadSphereが接したらTrainViewに筋トレ回数を入力
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //ヘッドセットがHeadSphereから離れたらHeadSphereの色を元に戻す
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //足がカメラに近づくときのみ
            if ( Vector3.SqrMagnitude( _maxPoint - _mainCamera.position ) < Vector3.SqrMagnitude( _maxPoint - _lastCameraPos ) ) {
                //Sphereを足のポジションに追随させる
                if (_isLeft) {
                    transform.position = _leftFoot.position;
                } else {
                    transform.position = _rightFoot.position;
                }
            }

            //最新のカメラの位置を保持
            _lastCameraPos = _mainCamera.position;
        }

        void AddTrainNum( Collider collider ) {
            //筋トレ回数を加算する
            _trainView.AddTrainNumber( 1 );

            //HeadSphereのマテリアルを交換し、Exitでまた元に戻す
            _renderer.material = _materials[ 1 ];
        }
    }
}