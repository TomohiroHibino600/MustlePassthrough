using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// �؃g���̎�ڂ₻�̉��������View�̊Ǘ�
    /// </summary>
    public class GuidanceView : MonoBehaviour
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
        [SerializeField] GameObject _trainNameObj = null;
        [SerializeField] TextMeshProUGUI _trainName = null;

        private void Start( ) {
            //�ŏ��̊�_���擾
            _firstPosition = _trainNameObj.transform.localPosition;
            _firstRot = _trainNameObj.transform.localRotation;

            //�J�������O�X���Ă��Ȃ��ꍇ�͐��ʂ�UI��u��
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x <= _posChangeRot & !_putFront )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutFront( ) );

            //�J�������X�����珰�̉���UI���ړ�������
            this.UpdateAsObservable( )
                .Where( _ => _mainCamera.localEulerAngles.x > _posChangeRot & !_putUnder )
                .TakeUntilDestroy( this )
                .Subscribe( _ => PutUnder( ) );
        }

        public void ShowTrainName(string trainName) {
            _trainName.text = trainName;
        }
        
        public void DisactiveTrainName( ) {
            _trainNameObj.SetActive(false);
        }

        void PutUnder( ) {
            //����̈ʒu��UI��u���A�����������
            _trainNameObj.transform.SetPositionAndRotation( _underPoint.position, Quaternion.Euler( 90f, 0f, 0f ) );

            //Update���ɉ��x�����s���Ȃ��悤�t���O�ύX
            _putUnder = true;
            _putFront = false;
        }

        void PutFront( ) {
            //�ŏ��̈ʒu��UI��u��
            _trainNameObj.transform.SetPositionAndRotation( _firstPosition, _firstRot );

            //Update���ɉ��x�����s���Ȃ��悤�t���O�ύX
            _putUnder = false;
            _putFront = true;
        }
    }
}
