using UnityEngine;
using Zenject;

namespace MustlePassthrough
{
    /// <summary>
    /// 依存性注入を行っているクラス
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