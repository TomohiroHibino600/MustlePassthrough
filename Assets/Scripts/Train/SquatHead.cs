using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �X�N���b�g���Ă��鎞�̐ݒ�ƃX�R�A�J�E���g���s��
    /// </summary>
    public class SquatHead : MonoBehaviour
    {
        [SerializeField] TrainView _trainView = null;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] Transform _leftFoot = null;
        [SerializeField] Transform _rightFoot = null;

        [SerializeField] MeshRenderer _renderer = null;
        [SerializeField] Material[] _materials = new Material[2];

        private Vector3 _maxPoint = Vector3.zero;
        private Vector3 _lastCameraPos = Vector3.zero;
        private bool _setTarget = false;

        void OnEnable( ) {
            //Sphere�𓮂����A�ڕW�n�_�����߁A���̖ڕW�n�_��Sphere���Œ�
            this.UpdateAsObservable()
                .Where(_ => !_setTarget)
                .TakeUntilDisable(this)
                .Subscribe(_ => MoveSphere());

            //�ڕW�n�_�����܂�����t���O��؂�ւ���
            this.OnTriggerExitAsObservable()
                .Where(_ => !_setTarget)
                .TakeUntilDisable(this)
                .Subscribe(_ => { _setTarget = true; } );

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

        void MoveSphere()
        {
            //�����̒��Ԃ��Œ��_�Ƃ��Đݒ�
            _maxPoint = (_leftFoot.position + _rightFoot.position) / 2f;

            //�J�������Œ��_�ɋ߂Â��Ƃ��̂�
            if (Vector3.SqrMagnitude(_maxPoint - _lastCameraPos) > Vector3.SqrMagnitude( _maxPoint - _mainCamera.position ))
            {
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
