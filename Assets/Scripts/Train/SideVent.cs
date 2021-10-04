using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// 
    /// </summary>
    public class SideVent : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        [SerializeField] GameObject _rightSphere;

        private Vector3 _maxPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //Sphere�𓮂����A�ڕW�n�_�����߁A���̖ڕW�n�_��Sphere���Œ�
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //�ڕW�n�_�����܂�����A�t���O��؂�ւ���
            //�܂��A�E�ɖڕW�n�_�ƂȂ鋅��ǉ�����
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { 
                    _setTarget = true;
                    _rightSphere.transform.position = new Vector3(-transform.position.x, transform.position.y, -transform.position.z );
                });

            //���ʂƌŒ肵�������ڂ�����TrainView�ɋ؃g���񐔂����
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //���ʂ��Œ肵�������痣�ꂽ�獶�E�̋��̐F�����ɖ߂�
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { 
                    _renderer.material = _materials[ 0 ];
                    _rightSphere.GetComponent<MeshRenderer>().material = _materials[ 0 ];
                } );
        }

        void MoveSphere( ) {
            //�����̒��Ԃ��Œ��_�Ƃ��Đݒ�
            _maxPoint = ( _leftFoot.position + _rightFoot.position ) / 2f;

            //�J�������Œ��_�ɋ߂Â��Ƃ��̂�
            if ( Vector3.SqrMagnitude( _maxPoint - _lastCameraPos ) > Vector3.SqrMagnitude( _maxPoint - _mainCamera.position ) ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
            }

            //�ŐV�̃J�����̈ʒu��ێ�
            _lastCameraPos = _mainCamera.position;
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //���E��Sphere�̃}�e���A�����������ATriggerExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
            _rightSphere.GetComponent<MeshRenderer>( ).material = _materials[ 1 ];
        }
    }
}
