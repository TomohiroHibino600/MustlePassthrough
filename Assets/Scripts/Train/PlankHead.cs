using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �v�����N�̐ݒ�ƃX�R�A�J�E���g
    /// </summary>
    public class PlankHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[ ] _materials = new Material[ 2 ];

        private bool _setTarget = false;
        private Vector3 _perpendicular = Vector3.zero;
        private Vector3 _averageController = Vector3.zero;
        private float _firstMolculeY = 0f;
        private float _secondMolculeY = 0f;
        private float _denominatorY = 0f;
        private float _intersectionY = 0f;
        private float _lastIntersectionY = 0f;

        void OnEnable( ) {
            //�w�b�h�Z�b�g�𓮂����ڕW�n�_��HeadSphere���߂Â���
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveHeadSphere( ) );

            //�ڕW�n�_�����܂�����t���O��؂�ւ��A�����u���ׂ�����������
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

        void MoveHeadSphere( ) {
            //�ڐ��Ƌt�����̃x�N�g���Ɨ����̃R���g���[���[�̏�����̌����̕��ς̃x�N�g���̌�_��y���W�����߂�
            _intersectionY = GetIntersectionY(_leftFoot, _rightFoot, _mainCamera);

            //��_��y���W�������Ȃ�Ƃ��̂�
            if ( _intersectionY > _lastIntersectionY ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
            }

            //�ŐV�̃J�����̈ʒu��ێ�
            _lastIntersectionY = GetIntersectionY( _leftFoot, _rightFoot, _mainCamera ); ;
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //HeadSphere�̃}�e���A�����������AExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
        }

        /// <summary>
        /// �ڐ��Ƌt�����̃x�N�g���Ɨ����̃R���g���[���[�̏�����̌����̕��ς̃x�N�g���̌�_��y���W�����߂�
        /// </summary>
        /// <param name="leftFoot"></param>
        /// <param name="rightFoot"></param>
        /// <param name="mainCamera"></param>
        /// <returns></returns>
        float GetIntersectionY(Transform leftFoot, Transform rightFoot, Transform mainCamera) {
            //�J�����Ɨ��������񂾎O�p�`�ɂ��āA�J���������ӂɌ��񂾐��̌�_�̍��W�����߂�
            _perpendicular = leftFoot.position + Vector3.Project( mainCamera.position - leftFoot.position, rightFoot.position - leftFoot.position );
            if ( _perpendicular == Vector3.zero ) {
                //��_�̍��W��Vector3.zero�ł���ƃG���[�ɂȂ�̂�return;
                return 0f;
            }

            //�R���g���[���[�̌����ɂ��āAyz�����̕��ς����߂�
            _averageController = ( leftFoot.forward + leftFoot.up + rightFoot.forward + rightFoot.up ).normalized;
            
            //�ڐ��Ƌt������yz�����x�N�g���ƁA�R���g���[���[�̌�����yz�����x�N�g���̌�_���v�Z�ɂ���ċ��߂�
            _firstMolculeY = mainCamera.position.z - _perpendicular.z;
            _secondMolculeY = _perpendicular.y * _averageController.z / _averageController.y - mainCamera.position.y * mainCamera.forward.z / mainCamera.forward.y;
            _denominatorY = _averageController.z / _averageController.y - mainCamera.forward.z / mainCamera.forward.y;
            return _intersectionY = ( _firstMolculeY + _secondMolculeY ) / _denominatorY;
        }
    }
}
