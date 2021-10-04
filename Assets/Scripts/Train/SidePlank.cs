using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    public class SidePlank : MonoBehaviour
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
            //Sphere�𓮂����A�ڕW�n�_�����߁A���̖ڕW�n�_��Sphere���Œ�
            this.UpdateAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => MoveSphere( ) );

            //�ڕW�n�_�����܂�����t���O��؂�ւ���
            this.OnTriggerExitAsObservable( )
                .Where( _ => !_setTarget )
                .TakeUntilDisable( this )
                .Subscribe( _ => { _setTarget = true; } );

            //���ʂƌŒ肵�������ڂ�����TrainView�ɋ؃g���񐔂����
            this.OnTriggerEnterAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => AddTrainNum( collider ) );

            //���ʂ��Œ肵�������痣�ꂽ�狅�̐F�����ɖ߂�
            this.OnTriggerExitAsObservable( )
                .Where( _ => _setTarget )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDisable( this )
                .Subscribe( collider => { _renderer.material = _materials[ 0 ]; } );
        }

        void MoveSphere( ) {
            //�ڐ��ɑ΂��ĉ������̃x�N�g���Ɨ����̃R���g���[���[�̏�����̌����̕��ς̃x�N�g���̌�_��y���W�����߂�
            _intersectionY = GetIntersectionY( _leftFoot, _rightFoot, _mainCamera );

            //��_��y���W�������Ȃ�Ƃ��̂�
            if ( _intersectionY > _lastIntersectionY ) {
                //HeadSphere���J�����̃|�W�V�����ɒǐ�������
                transform.position = _mainCamera.position;
            }

            //�ŐV�̌�_�̈ʒu��ێ�
            _lastIntersectionY = GetIntersectionY( _leftFoot, _rightFoot, _mainCamera ); ;
        }

        void AddTrainNum( Collider collider ) {
            //�؃g���񐔂����Z����
            _trainView.AddTrainNumber( 1 );

            //Sphere�̃}�e���A�����������ATriggerExit�ł܂����ɖ߂�
            _renderer.material = _materials[ 1 ];
        }

        /// <summary>
        /// �ڐ��ɑ΂��ĉ������̃x�N�g���Ɨ����̃R���g���[���[�̏�����̌����̕��ς̃x�N�g���̌�_��y���W�����߂�
        /// </summary>
        /// <param name="leftFoot"></param>
        /// <param name="rightFoot"></param>
        /// <param name="mainCamera"></param>
        /// <returns></returns>
        float GetIntersectionY( Transform leftFoot, Transform rightFoot, Transform mainCamera ) {
            //�J�����Ɨ��������񂾎O�p�`�ɂ��āA�J���������ӂɌ��񂾐��̌�_�̍��W�����߂�
            _perpendicular = leftFoot.position + Vector3.Project( mainCamera.position - leftFoot.position, rightFoot.position - leftFoot.position );
            if ( _perpendicular == Vector3.zero ) {
                //��_�̍��W��Vector3.zero�ł���ƃG���[�ɂȂ�̂�return;
                return 0f;
            }

            //�R���g���[���[�̌����ɂ��āAxy�����̕��ς����߂�
            _averageController = ( leftFoot.right + leftFoot.up + rightFoot.right + rightFoot.up ).normalized;

            //�ڐ��Ƌt������yz�����x�N�g���ƁA�R���g���[���[�̌�����yz�����x�N�g���̌�_���v�Z�ɂ���ċ��߂�
            _firstMolculeY = mainCamera.position.x - _perpendicular.x;
            _secondMolculeY = _perpendicular.y * _averageController.x / _averageController.y + mainCamera.position.y * mainCamera.up.x / mainCamera.up.y;
            _denominatorY = _averageController.x / _averageController.y + mainCamera.up.x / mainCamera.up.y;
            return _intersectionY = ( _firstMolculeY + _secondMolculeY ) / _denominatorY;
        }
    }
}
