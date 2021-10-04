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

        private Vector3 _leftLegPos = Vector3.zero;
        private Vector3 _lastLeftLegPos = Vector3.zero;
        private Vector3 _rightLegPos = Vector3.zero;
        private Vector3 _lastRightLegPos = Vector3.zero;
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

            //�ڕW�n�_�����܂�����A�t���O��؂�ւ���
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { _setTarget = true; } );

            //���ʂƌŒ肵�������ڂ�����TrainView�ɋ؃g���񐔂����
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject.tag == "Leg" )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //���ʂ��Œ肵�������痣�ꂽ�狅�̐F�����ɖ߂�
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject.tag == "Leg" )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            if ( _isLeft ) {
                //���̈ʒu���擾
                _leftLegPos = _leftFoot.position;

                //�����J�����ɋ߂Â��Ƃ��̂�
                if ( _leftLegPos.y - _mainCamera.position.y < _lastLeftLegPos.y - _mainCamera.position.y ) {
                    //Sphere�𑫂̃|�W�V�����ɒǐ�������
                    transform.position = _leftFoot.position;
                }

                //�ŐV�̑��̈ʒu��ێ�
                _lastLeftLegPos = _leftFoot.position;
            } else {
                //���̈ʒu���擾
                _rightLegPos = _rightFoot.position;

                //�����J�����ɋ߂Â��Ƃ��̂�
                if ( _rightLegPos.y - _mainCamera.position.y < _lastRightLegPos.y - _mainCamera.position.y ) {
                    //Sphere�𑫂̃|�W�V�����ɒǐ�������
                    transform.position = _rightFoot.position;
                }

                //�ŐV�̑��̈ʒu��ێ�
                _lastRightLegPos = _rightFoot.position;
            }
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //Sphere�̃}�e���A�����������ATriggerExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
        }
    }
}