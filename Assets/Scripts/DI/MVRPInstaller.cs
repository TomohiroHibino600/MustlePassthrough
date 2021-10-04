using UnityEngine;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// �ˑ����������s���Ă���N���X
    /// </summary>
    public class MVRPInstaller : MonoInstaller
    {
        public override void InstallBindings( ) {
            Container.Bind<IModel>( ).To<Model>( ).AsCached( ).NonLazy( );
            Container.Bind<IPresenterInput>( ).To<PresenterInput>( ).AsCached( ).NonLazy( );
            Container.Bind<IPresenterOutput>( ).To<PresenterOutput>( ).AsCached( ).NonLazy( );
        }
    }
}