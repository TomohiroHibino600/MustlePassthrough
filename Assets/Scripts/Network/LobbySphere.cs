using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MustlePassthrough
{
    /// <summary>
    /// Lobby�V�[���ŃV���O���v���C���}���`�v���C���I�����A���[�h���邽�߂̃V�[��
    /// </summary>
    public class LobbySphere : MonoBehaviour
    {
        [SerializeField] string _nexSceneName;
        [SerializeField] Transform _mainCamera;

        void Start( ) {
            this.OnTriggerEnterAsObservable( )
                .Where( collider => collider.gameObject == _mainCamera.gameObject )
                .TakeUntilDestroy( this )
                .Subscribe( collider => { SceneManager.LoadScene( _nexSceneName ); });
        }
    }
}