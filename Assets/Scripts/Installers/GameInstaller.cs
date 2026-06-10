using Repositories;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameInstaller : LifetimeScope
    { 
        [SerializeField] private MapConfigSo _mapConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameService>(Lifetime.Singleton)
                .WithParameter(Instantiate(_mapConfig));
        }
    }
}