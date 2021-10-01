using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] GameObject _numberUI = null;
        [SerializeField] TextMeshProUGUI _numberText = null;
        [SerializeField] AudioSource _numberSE = null;
        [SerializeField] float _smallScaleParam = 0f;
        [SerializeField] float _scaleTime = 0f;

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
    }
}
