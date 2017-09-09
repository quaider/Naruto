using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.Reflection;

namespace Naruto.Reflection
{
    public class BinFolderFinder : AppDomainTypeFinder
    {
        private bool _ensureBinFolderAssembliesLoaded = true;
        private bool _binFolderAssembliesLoaded;

        public bool EnsureBinFolderAssembliesLoaded
        {
            get { return _ensureBinFolderAssembliesLoaded; }
            set { _ensureBinFolderAssembliesLoaded = value; }
        }

        public virtual string GetBinDirectory()
        {
            return PlatformServices.Default.Application.ApplicationBasePath;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (this.EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded)
            {
                _binFolderAssembliesLoaded = true;
                string binPath = GetBinDirectory();
                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }
    }
}
