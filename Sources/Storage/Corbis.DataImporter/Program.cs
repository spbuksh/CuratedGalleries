using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface;
using Corbis.DataImporter.Importers;
using Microsoft.Practices.Unity;
using Corbis.Presentation.Common.RepositoryProvider;

namespace Corbis.DataImporter
{
    class Program
    {

        static void Main(string[] args)
        {
            var importContainer = new ImportContainer();
            //importContainer.AddImporter(new UserImporter());
            //importContainer.AddImporter(new TemplateImporter());
            importContainer.DoImports();
        }
      
    }
}
