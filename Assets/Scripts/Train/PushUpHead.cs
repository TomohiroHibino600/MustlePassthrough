using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �r���ĕ������Ă��鎞�̐ݒ�ƃX�R�A�J�E���g���s��
    /// </summary>
    public class PushUpHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;
        [SerializeField] float _maxHeight = 1.4f;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[] _materials = new Material[2];

        private float _averageRot = 0f;
        private float _nowHeight = 0f;
        private float _lastHeight = 0f;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            if ( _trainView == null ) {
                _trainView = GameObject.FindGameObjectWithTag( "TrainView" ).GetComponent<TrainView>( );
            }

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
            //���Ƒ��ɂ����R���g���[���[�����񂾕ӂ��ΕӂƂ��A
            //�����ӂƂ��ĎO�p�֐��Řr���ĕ������̓��̍��������߂�
            _averageRot = ( Mathf.Abs( _leftFoot.localEulerAngles.x ) + Mathf.Abs( _rightFoot.localEulerAngles.x ) ) / 2;
            _nowHeight = Vector3.Distance( _mainCamera.position, _leftFoot.position ) * Mathf.Sin( _averageRot * Mathf.PI / 180f );
            _lastHeight = Vector3.Distance( _lastCameraPos, _leftFoot.position ) * Mathf.Sin( _averageRot * Mathf.PI / 180f );

            //���Ə������ꂷ���Ă���Ƃ��͖���
            if ( _nowHeight > _maxHeight ) {
                return;
            }

            //�������ɋ߂Â��Ƃ��̂�
            if ( _nowHeight < _lastHeight ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
            }

            //�ŐV�̃J�����̈ʒu��ێ�
            _lastCameraPos = _mainCamera.position;
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
