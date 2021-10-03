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
    /// �؃g���񐔂�����View�̊Ǘ�
    /// </summary>
    public class NumberView : MonoBehaviour
    {
        //�ʒu�����p�̕ϐ�
        private bool _putUnder = false;
        private bool _putFront = false;
        private Vector3 _firstPosition = Vector3.zero;
        private Quaternion _firstRot = Quaternion.identity;
        [SerializeField] Transform _underPoint = null;
        [SerializeField] Transform _mainCamera = null;
        [SerializeField] float _posChangeRot = 60f;

        //UI�p�̕ϐ�
        [SerializeField] GameObject _numberUI = null;
        [SerializeField] TextMeshProUGUI _numberText = null;
        [SerializeField] AudioSource _numberSE = null;
        [SerializeField] float _smallScaleParam = 0f;
        [SerializeField] float _scaleTime = 0f;

        private void Start( ) {
            //�ŏ��̊�_���擾
            _firstPosition = _numberUI.transform.localPosition;
            _firstRot = _numberUI.transform.localRotation;

            //�J�������O�X���Ă��Ȃ��ꍇ�͐��ʂ�UI��u��
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x <= _posChangeRot & !_putFront )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutFront( ) );

            //�J�������X�����珰�̉���UI���ړ�������
            this.UpdateAsObservable( )
                .Where(_ => _mainCamera.localEulerAngles.x > _posChangeRot & !_putUnder)
                .TakeUntilDestroy( this )
                .Subscribe(_ => PutUnder());
        }

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

        void PutUnder( ) {
            //����̈ʒu��UI��u���A�����������
            _numberUI.transform.SetPositionAndRotation( _underPoint.position , Quaternion.Euler( 90f, 0f, 0f ) );

            //Update���ɉ��x�����s���Ȃ��悤�t���O�ύX
            _putUnder = true;
            _putFront = false;
        }

        void PutFront( ) {
            //�ŏ��̈ʒu��UI��u��
            _numberUI.transform.SetPositionAndRotation(_firstPosition, _firstRot);

            //Update���ɉ��x�����s���Ȃ��悤�t���O�ύX
            _putUnder = false;
            _putFront = true;
        }
    }
}
