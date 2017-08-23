using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Naruto.Runtime.Caching.Configuration
{
    internal class CachingConfiguration : ICachingConfiguration
    {
        private readonly List<CacheConfigurator> _configurators;

        public CachingConfiguration()
        {
            _configurators = new List<CacheConfigurator>();
        }

        public IReadOnlyList<CacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }

        public void ConfigureAll(Action<ICache> configure)
        {
            _configurators.Add(new CacheConfigurator(configure));
        }

        public void Configure(string cacheName, Action<ICache> configure)
        {
            _configurators.Add(new CacheConfigurator(cacheName, configure));
        }
    }
}
