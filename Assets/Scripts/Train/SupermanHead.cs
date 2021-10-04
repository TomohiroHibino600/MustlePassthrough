using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// 背筋している時の設定とスコアカウント
    /// </summary>
    public class SupermanHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;
        [SerializeField] float _maxHeight = 0.8f;
        [SerializeField] float _referenceParam = 1000f;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[] _materials = new Material[2];

        private bool _setTarget = false;
        private Vector3 _referencePoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;

        void OnEnable( ) {
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
            //基準点を決める
            _referencePoint = (_leftFoot.position + _rightFoot.position + _mainCamera.position) / 3f + Vector3.up * _referenceParam;

            //頭と足の高低差が大きすぎる場合は中止
            if (_mainCamera.position.y - _leftFoot.position.y > _maxHeight) {
                return;
            }

            //カメラの位置が基準点に近くなるときのみ
            if ( Vector3.Distance(_mainCamera.position, _referencePoint) < Vector3.Distance( _lastCameraPos, _referencePoint ) ) {
                //HeadSphereをカメラのポジションに追随させる
                transform.position = _mainCamera.position;
            }

            //最新のカメラの位置を保持
            _lastCameraPos = _mainCamera.transform.position;
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
