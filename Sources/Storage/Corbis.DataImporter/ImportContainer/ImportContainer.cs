using System.Collections.Generic;
using Corbis.DataImporter.Interface;

namespace Corbis.DataImporter
{
    /// <summary>
    /// Class to work with Importers.
    /// </summary>
    public class ImportContainer : IImportContainer
    {
        #region Constructors

        public ImportContainer()
        {
            _importers = new List<IImporter>();
        }

        #endregion
        

        #region Fields

        private readonly IList<IImporter> _importers; 

        #endregion


        /// <summary>
        /// Adds Importer to collection.
        /// </summary>
        /// <param name="importer"></param>
        public void AddImporter(IImporter importer)
        {
            _importers.Add(importer);
        }

        /// <summary>
        /// Calls DoImport method for all Importres.
        /// </summary>
        public void DoImports()
        {
            foreach (var importer in _importers)
            {
                importer.DoImport();
            }
        }
    }
}
