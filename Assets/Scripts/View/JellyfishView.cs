using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MustlePassthrough
{
    /// <summary>
    /// クラゲを出現させるViewの管理
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
            //ランダムな座標を決める
            _xPoint = Random.Range( _mainCamera.position.x - ( _spawnRangeSize / 2 ), _mainCamera.position.x + ( _spawnRangeSize / 2 ) );
            _yPoint = Random.Range( _mainCamera.position.y - ( _spawnRangeSize / 2 ), _mainCamera.position.y + ( _spawnRangeSize / 2 ) );
            _zPoint = Random.Range( _mainCamera.position.z , _mainCamera.position.z + _spawnRangeSize );
            _spawnPoint = new Vector3(_xPoint, _yPoint, _zPoint);

            //ランダムなクラゲを出現させる
            Instantiate( _jellyfishes[ Random.Range( 0, _jellyfishes.Length ) ], _spawnPoint, Quaternion.identity);

            //効果音を出す
            _spawnSE.PlayOneShot(_spawnSE.clip);
        }
    }
}
