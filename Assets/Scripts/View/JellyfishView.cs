using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// �N���Q���o��������View�̊Ǘ�
    /// </summary>
    public class JellyfishView : MonoBehaviour
    {
        private float _xPoint = 0f;
        private float _yPoint = 0f;
        private float _zPoint = 0f;
        private Vector3 _spawnPoint = Vector3.zero;

        [SerializeField] Transform _mainCamera = null;
        [SerializeField] GameObject[] _jellyfishes = null;
        [SerializeField] AudioSource _spawnSE = null;
        [SerializeField] float _spawnRangeSize = 0f;

        public void SpawnJellyfish( ) {
            //�����_���ȍ��W�����߂�
            _xPoint = Random.Range( _mainCamera.position.x - ( _spawnRangeSize / 2 ), _mainCamera.position.x + ( _spawnRangeSize / 2 ) );
            _yPoint = Random.Range( _mainCamera.position.y - ( _spawnRangeSize / 2 ), _mainCamera.position.y + ( _spawnRangeSize / 2 ) );
            _zPoint = Random.Range( _mainCamera.position.z , _mainCamera.position.z + _spawnRangeSize );
            _spawnPoint = new Vector3(_xPoint, _yPoint, _zPoint);

            //�����_���ȃN���Q���o��������
            Instantiate( _jellyfishes[ Random.Range( 0, _jellyfishes.Length ) ], _spawnPoint, Quaternion.identity);

            //���ʉ����o��
            _spawnSE.PlayOneShot(_spawnSE.clip);
        }
    }
}
