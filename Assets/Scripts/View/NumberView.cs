using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// 筋トレ回数を示すViewの管理
    /// </summary>
    public class NumberView : MonoBehaviour
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
        [SerializeField] GameObject _numberUI = null;
        [SerializeField] TextMeshProUGUI _numberText = null;
        [SerializeField] AudioSource _numberSE = null;
        [SerializeField] float _smallScaleParam = 0f;
        [SerializeField] float _scaleTime = 0f;

        private void Start( ) {
            //最初の基準点を取得
            _firstPosition = _numberUI.transform.localPosition;
            _firstRot = _numberUI.transform.localRotation;

            //カメラが前傾していない場合は正面にUIを置く
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x <= _posChangeRot & !_putFront )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutFront( ) );

            //カメラが傾いたら床の下にUIを移動させる
            this.UpdateAsObservable( )
                .Where(_ => _mainCamera.localEulerAngles.x > _posChangeRot & !_putUnder)
                .TakeUntilDestroy( this )
                .Subscribe(_ => PutUnder());
        }

        public void ShowNumber(int number) {
            //数字を変更するときに大小させるアニメーションを実行する
            _numberUI.transform.localScale = Vector3.one * _smallScaleParam;
            _numberUI.transform
                .DOScale(Vector3.one, _scaleTime)
                .SetEase(Ease.OutElastic);

            //加算時にSEを鳴らす
            _numberSE.PlayOneShot(_numberSE.clip);

            _numberText.text = number.ToString();
        }

        public void DisactiveNumber( ) {
            _numberUI.gameObject.SetActive(false);
        }

        void PutUnder( ) {
            //特定の位置にUIを置き、上を向かせる
            _numberUI.transform.SetPositionAndRotation( _underPoint.position , Quaternion.Euler( 90f, 0f, 0f ) );

            //Update中に何度も実行しないようフラグ変更
            _putUnder = true;
            _putFront = false;
        }

        void PutFront( ) {
            //最初の位置にUIを置く
            _numberUI.transform.SetPositionAndRotation(_firstPosition, _firstRot);

            //Update中に何度も実行しないようフラグ変更
            _putUnder = false;
            _putFront = true;
        }
    }
}
