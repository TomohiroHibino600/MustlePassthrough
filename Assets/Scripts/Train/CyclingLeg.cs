using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �T�C�N�����O�̐ݒ�ƃX�R�A�J�E���g
    /// </summary>
    public class CyclingLeg : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;
        [SerializeField] bool _isLeft = false;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private Vector3 _maxPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //�w�b�h�Z�b�g�𓮂����ڕW�n�_��HeadSphere���߂Â���
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //�ڕW�n�_�����܂�����A�t���O��؂�ւ���
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { _setTarget = true; } );

            //�w�b�h�Z�b�g��HeadSphere���ڂ�����TrainView�ɋ؃g���񐔂����
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //�w�b�h�Z�b�g��HeadSphere���痣�ꂽ��HeadSphere�̐F�����ɖ߂�
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //�����J�����ɋ߂Â��Ƃ��̂�
            if ( Vector3.SqrMagnitude( _maxPoint - _mainCamera.position ) < Vector3.SqrMagnitude( _maxPoint - _lastCameraPos ) ) {
                //Sphere�𑫂̃|�W�V�����ɒǐ�������
                if (_isLeft) {
                    transform.position = _leftFoot.position;
                } else {
                    transform.position = _rightFoot.position;
                }
            }

            //�ŐV�̃J�����̈ʒu��ێ�
            _lastCameraPos = _mainCamera.position;
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //HeadSphere�̃}�e���A�����������AExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
        }
    }
}