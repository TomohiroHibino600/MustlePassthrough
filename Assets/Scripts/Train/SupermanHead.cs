using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �w�؂��Ă��鎞�̐ݒ�ƃX�R�A�J�E���g
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
            //Sphere�𓮂����A�ڕW�n�_�����߁A���̖ڕW�n�_��Sphere���Œ�
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //�ڕW�n�_�����܂�����t���O��؂�ւ���
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => {_setTarget = true; } );

            //���ʂƌŒ肵�������ڂ�����TrainView�ɋ؃g���񐔂����
            this.OnTriggerEnterAsObservable()
                .Where( _ => _setTarget )
                .Where(collider => collider.gameObject == _mainCamera.gameObject)
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum(collider) );

            //���ʂ��Œ肵�������痣�ꂽ�狅�̐F�����ɖ߂�
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //��_�����߂�
            _referencePoint = (_leftFoot.position + _rightFoot.position + _mainCamera.position) / 3f + Vector3.up * _referenceParam;

            //���Ƒ��̍��፷���傫������ꍇ�͒��~
            if (_mainCamera.position.y - _leftFoot.position.y > _maxHeight) {
                return;
            }

            //�J�����̈ʒu����_�ɋ߂��Ȃ�Ƃ��̂�
            if ( Vector3.Distance(_mainCamera.position, _referencePoint) < Vector3.Distance( _lastCameraPos, _referencePoint ) ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
            }

            //�ŐV�̃J�����̈ʒu��ێ�
            _lastCameraPos = _mainCamera.transform.position;
        }

        void AddTrainNum(Collider collider)
        {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber(1);

            //Sphere�̃}�e���A�����������ATriggerExit�ł܂����ɖ߂�
            _renderer.material = _materials[1];
        }
    }
}
