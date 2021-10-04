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

        private Vector3 _leftLegPos = Vector3.zero;
        private Vector3 _lastLeftLegPos = Vector3.zero;
        private Vector3 _rightLegPos = Vector3.zero;
        private Vector3 _lastRightLegPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            if ( _trainView == null ) {
                _trainView = GameObject.FindGameObjectWithTag( "TrainView" ).GetComponent<TrainView>( );
            }

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
            if ( _isLeft ) {
                //足の位置を取得
                _leftLegPos = _leftFoot.position;

                //足がカメラに近づくときのみ
                if ( _leftLegPos.y - _mainCamera.position.y < _lastLeftLegPos.y - _mainCamera.position.y ) {
                    //Sphereを足のポジションに追随させる
                    transform.position = _leftFoot.position;
                }

                //最新の足の位置を保持
                _lastLeftLegPos = _leftFoot.position;
            } else {
                //足の位置を取得
                _rightLegPos = _rightFoot.position;

                //足がカメラに近づくときのみ
                if ( _rightLegPos.y - _mainCamera.position.y < _lastRightLegPos.y - _mainCamera.position.y ) {
                    //Sphereを足のポジションに追随させる
                    transform.position = _rightFoot.position;
                }

                //最新の足の位置を保持
                _lastRightLegPos = _rightFoot.position;
            }
        }

        void AddTrainNum( Collider collider ) {
            //筋トレ回数を加算する
            _trainView.AddTrainNumber( 1 );

            //Sphereのマテリアルを交換し、TriggerExitでまた元に戻す
            _renderer.material = _materials[ 1 ];
        }
    }
}