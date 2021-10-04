using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �j�[�g�`�F�X�g���b�O�̐ݒ�ƃX�R�A�J�E���g
    /// </summary>
    public class NeatChestLeg : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private Vector3 _legPos = Vector3.zero;
        private Vector3 _lastLegPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
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
            //���̈ʒu���擾
            _legPos = ( _leftFoot.position + _rightFoot.position ) / 2f;

            //�����J�����ɋ߂Â��Ƃ��̂�
            if ( Vector3.SqrMagnitude( _legPos - _mainCamera.position ) < Vector3.SqrMagnitude( _lastLegPos - _mainCamera.position ) ) {
                //Sphere�𑫂̃|�W�V�����ɒǐ�������
                transform.position = ( _leftFoot.position + _rightFoot.position ) / 2f;
            }

            //�ŐV�̑��̈ʒu��ێ�
            _lastLegPos = ( _leftFoot.position + _rightFoot.position ) / 2f;
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //Sphere�̃}�e���A�����������ATriggerExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
        }
    }
}
