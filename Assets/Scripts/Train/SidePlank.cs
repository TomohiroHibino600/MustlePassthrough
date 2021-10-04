using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    public class SidePlank : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private bool _setTarget = false;
        private Vector3 _perpendicular = Vector3.zero;
        private Vector3 _averageController = Vector3.zero;
        private float _firstMolculeY = 0f;
        private float _secondMolculeY = 0f;
        private float _denominatorY = 0f;
        private float _intersectionY = 0f;
        private float _lastIntersectionY = 0f;

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
                .Subscribe( _ => { _setTarget = true; } );

            //部位と固定した球が接したらTrainViewに筋トレ回数を入力
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //部位が固定した球から離れたら球の色を元に戻す
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //目線に対して下方向のベクトルと両足のコントローラーの上方向の向きの平均のベクトルの交点のy座標を求める
            _intersectionY = GetIntersectionY( _leftFoot, _rightFoot, _mainCamera );

            //交点のy座標が高くなるときのみ
            if ( _intersectionY > _lastIntersectionY ) {
                //HeadSphereをカメラのポジションに追随させる
                transform.position = _mainCamera.position;
            }

            //最新の交点の位置を保持
            _lastIntersectionY = GetIntersectionY( _leftFoot, _rightFoot, _mainCamera ); ;
        }

        void AddTrainNum( Collider collider ) {
            //筋トレ回数を加算する
            _trainView.AddTrainNumber( 1 );

            //Sphereのマテリアルを交換し、TriggerExitでまた元に戻す
            _renderer.material = _materials[ 1 ];
        }

        /// <summary>
        /// 目線に対して下方向のベクトルと両足のコントローラーの上方向の向きの平均のベクトルの交点のy座標を求める
        /// </summary>
        /// <param name="leftFoot"></param>
        /// <param name="rightFoot"></param>
        /// <param name="mainCamera"></param>
        /// <returns></returns>
        float GetIntersectionY( Transform leftFoot, Transform rightFoot, Transform mainCamera ) {
            //カメラと両足を結んだ三角形について、カメラから底辺に結んだ線の交点の座標を求める
            _perpendicular = leftFoot.position + Vector3.Project( mainCamera.position - leftFoot.position, rightFoot.position - leftFoot.position );
            if ( _perpendicular == Vector3.zero ) {
                //交点の座標がVector3.zeroであるとエラーになるのでreturn;
                return 0f;
            }

            //コントローラーの向きについて、xy次元の平均を求める
            _averageController = ( leftFoot.right + leftFoot.up + rightFoot.right + rightFoot.up ).normalized;

            //目線と逆方向のyz次元ベクトルと、コントローラーの向きのyz次元ベクトルの交点を計算によって求める
            _firstMolculeY = mainCamera.position.x - _perpendicular.x;
            _secondMolculeY = _perpendicular.y * _averageController.x / _averageController.y + mainCamera.position.y * mainCamera.up.x / mainCamera.up.y;
            _denominatorY = _averageController.x / _averageController.y + mainCamera.up.x / mainCamera.up.y;
            return _intersectionY = ( _firstMolculeY + _secondMolculeY ) / _denominatorY;
        }
    }
}
