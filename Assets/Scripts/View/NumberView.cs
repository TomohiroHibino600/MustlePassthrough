using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// �؃g���񐔂�����View�̊Ǘ�
    /// </summary>
    public class NumberView : MonoBehaviour
    {
        [SerializeField] GameObject _numberUI = null;
        [SerializeField] TextMeshProUGUI _numberText = null;
        [SerializeField] AudioSource _numberSE = null;
        [SerializeField] float _smallScaleParam = 0f;
        [SerializeField] float _scaleTime = 0f;

        public void ShowNumber(int number) {
            //������ύX����Ƃ��ɑ召������A�j���[�V���������s����
            _numberUI.transform.localScale = Vector3.one * _smallScaleParam;
            _numberUI.transform
                .DOScale(Vector3.one, _scaleTime)
                .SetEase(Ease.OutElastic);

            //���Z����SE��炷
            _numberSE.PlayOneShot(_numberSE.clip);

            _numberText.text = number.ToString();
        }

        public void DisactiveNumber( ) {
            _numberUI.gameObject.SetActive(false);
        }
    }
}
