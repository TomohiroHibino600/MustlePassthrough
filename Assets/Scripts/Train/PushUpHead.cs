using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// 腕立て伏せしている時の設定とスコアカウントを行う
    /// </summary>
    public class PushUpHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;
        [SerializeField] float _maxHeight = 1.4f;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[] _materials = new Material[2];

        private float _averageRot = 0f;
        private float _nowHeight = 0f;
        private float _lastHeight = 0f;
        private Vector3 _lastCameraPos = Vector3.zero;
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

            //目標地点が決まったらフラグを切り替える
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => {_setTarget = true; } );

            //部位と固定した球が接したらTrainViewに筋トレ回数を入力
            this.OnTriggerEnterAsObservable()
                .Where( _ => _setTarget )
                .Where(collider => collider.gameObject == _mainCamera.gameObject)
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum(collider) );

            //部位が固定した球から離れたら球の色を元に戻す
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //頭と足につけたコントローラーを結んだ辺を斜辺とし、
            //床を底辺として三角関数で腕立て伏せ中の頭の高さを求める
            _averageRot = ( Mathf.Abs( _leftFoot.localEulerAngles.x ) + Mathf.Abs( _rightFoot.localEulerAngles.x ) ) / 2;
            _nowHeight = Vector3.Distance( _mainCamera.position, _leftFoot.position ) * Mathf.Sin( _averageRot * Mathf.PI / 180f );
            _lastHeight = Vector3.Distance( _lastCameraPos, _leftFoot.position ) * Mathf.Sin( _averageRot * Mathf.PI / 180f );

            //頭と床が離れすぎているときは無視
            if ( _nowHeight > _maxHeight ) {
                return;
            }

            //頭が床に近づくときのみ
            if ( _nowHeight < _lastHeight ) {
                //HeadSphereをカメラのポジションに追随させる
                transform.position = _mainCamera.position;
            }

            //最新のカメラの位置を保持
            _lastCameraPos = _mainCamera.position;
        }

        void AddTrainNum(Collider collider)
        {
            //筋トレ回数を加算する
            _trainView.AddTrainNumber(1);

            //Sphereのマテリアルを交換し、TriggerExitでまた元に戻す
            _renderer.material = _materials[1];
        }
    }
}
