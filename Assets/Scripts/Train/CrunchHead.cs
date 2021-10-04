using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �N�����`�̐ݒ�ƃX�R�A�J�E���g
    /// </summary>
    public class CrunchHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private Vector3 _legPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            if (_trainView == null) {
                _trainView = GameObject.FindGameObjectWithTag("TrainView").GetComponent<TrainView>();
            }

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
            //�����̒��Ԃ���_�Ƃ��Đݒ�
            _legPoint = ( _leftFoot.position + _rightFoot.position ) / 2f;

            //�J��������_�ɋ߂Â��Ƃ��̂�
            if ( Vector3.SqrMagnitude( _mainCamera.position - _legPoint ) < Vector3.SqrMagnitude( _lastCameraPos - _legPoint ) ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
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