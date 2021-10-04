using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MustlePassthrough
{
    /// <summary>
    /// Lobbyシーンでシングルプレイかマルチプレイか選択し、ロードするためのシーン
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