using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface;
using Corbis.DataImporter.Interface;
using Corbis.Presentation.Common.RepositoryProvider;
using Microsoft.Practices.Unity;

namespace Corbis.DataImporter.Importers
{
    /// <summary>
    /// Class to import Users to The System
    /// </summary>
    public class UserImporter : IImporter
    {
        /// <summary>
        /// Imports Data to the System.
        /// </summary>
        public void DoImport()
        {
            var container = new UnityContainer();
            RepositoryProvider.Register(container, "RepositoryProvider");
            var repository = container.Resolve<IAdminUserRepository>();
            var users = Path.Combine(ImportHelper.GetPath(ConfigurationManager.AppSettings["AdminUsersStorage"]), "AdminUsers.xml");
            var serializer = new XmlSerializer(typeof(AdminRegistrationForm));
            var document = XDocument.Load(users);
            var usersSource = document.XPathSelectElements("//*/AdminRegistrationForm").ToList();
            foreach (var user in usersSource)
            {
                repository.Register((AdminRegistrationForm)serializer.Deserialize(user.CreateReader()));
            }
        }

       
    }
}
