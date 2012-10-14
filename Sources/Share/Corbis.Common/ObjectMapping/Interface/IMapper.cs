namespace Corbis.Common.ObjectMapping.Interface
{
    /// <summary>
    /// A base interface for any object mapper.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps the source object to the target object.
        /// </summary>
        /// <param name="from">The source object.</param>
        /// <param name="to">The target object.</param>
        /// <param name="mappingData">The mapping data.</param>
        void DoMapping(object from, object to, MappingData mappingData);
    }
}
