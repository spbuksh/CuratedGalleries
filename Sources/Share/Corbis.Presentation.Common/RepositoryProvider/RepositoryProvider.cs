using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Common.Configuration;
using Microsoft.Practices.Unity;
using System.IO;
using System.Reflection;
using Corbis.Common;

namespace Corbis.Presentation.Common.RepositoryProvider
{
    public class RepositoryProvider
    {
        public static void Register(IUnityContainer container, string sectionName = null)
        {
            var section = RepositoryProviderConfigurationSection.GetSection(sectionName);

            string filepath = section.Settings.FilePath;

            if(!Path.IsPathRooted(filepath))
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath.TrimStart(Path.DirectorySeparatorChar));

            Assembly repository = Assembly.LoadFrom(filepath);

            Type itype = repository.GetTypes().Where(x => typeof(IRepositoryEntryPoint).IsAssignableFrom(x)).SingleOrDefault();

            if (itype == null)
                throw new TypeAccessException(string.Format("Assembly '{0}' does not have any type which implements '{1}'", filepath, typeof(IRepositoryEntryPoint).AssemblyQualifiedName));

            IRepositoryEntryPoint entryPoint = Activator.CreateInstance(itype) as IRepositoryEntryPoint;
            entryPoint.Register(container, section.Settings.CreationMode);
        }
    }
}
