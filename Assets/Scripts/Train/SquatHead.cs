using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// スクワットしている時の設定とスコアカウントを行う
    /// </summary>
    public class SquatHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[] _materials = new Material[2];

        private Vector3 _maxPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //ヘッドセットを動かす目標地点にHeadSphereを近づける
            this.UpdateAsObservable()
                .Where(_ => !_setTarget)
                .TakeUntilDisable(this)
                .Subscribe(_ => MoveHeadSphere());

            //目標地点が決まったら、フラグを切り替える
            this.OnTriggerExitAsObservable()
                .Where(_ => !_setTarget)
                .TakeUntilDisable(this)
                .Subscribe(_ => { _setTarget = true; } );

            //ヘッドセットとHeadSphereが接したらTrainViewにスコアを入力
            this.OnTriggerEnterAsObservable()
                .Where( _ => _setTarget )
                .Where(collider => collider.gameObject == _mainCamera.gameObject)
                .TakeUntilDisable( this )
                .Subscribe( collider => AddScore(collider) );

            //ヘッドセットがHeadSphereから離れたらHeadSphereの色を元に戻す
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveHeadSphere()
        {
            //両足の中間を最大基準点として設定
            _maxPoint = (_leftFoot.position + _rightFoot.position) / 2f;

            //カメラが最大基準点に近づくときのみ
            if (Vector3.SqrMagnitude(_maxPoint - _lastCameraPos) > Vector3.SqrMagnitude( _maxPoint - _mainCamera.position ))
            {
                //HeadSphereをカメラのポジションに追随させる
                transform.position = _mainCamera.position;
            }

            //最新のカメラの位置を保持
            _lastCameraPos = _mainCamera.position;
        }

        void AddScore(Collider collider)
        {
            Debug.LogWarning("SquareHead");
            //スコアを加算する
            //タイムを記録する(若干ゆっくりの方がいいので、そこは考える)

            //HeadSphereのマテリアルを交換し、Exitでまた元に戻す
            _renderer.material = _materials[1];
        }
    }
}
