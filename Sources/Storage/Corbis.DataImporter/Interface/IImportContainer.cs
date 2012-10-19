namespace Corbis.DataImporter.Interface
{
    public interface IImportContainer
    {
        /// <summary>
        /// Adds Importer to collection.
        /// </summary>
        /// <param name="importer"></param>
        void AddImporter(IImporter importer);

        /// <summary>
        /// Calls DoImport method for all Importres.
        /// </summary>
        void DoImports();
    }
}