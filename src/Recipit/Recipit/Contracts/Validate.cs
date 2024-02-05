namespace Recipit.Contracts
{
    using Recipit.Contracts.Exceptions;

    public static class Validate
    {
        private const string ModelNullOrEmptyErrorMessage = "{ModelType} is null or empty.";

        public static void Model<T>(T model, ILogger logger)
        {
            if (model == null)
            {
                var typeName = typeof(T).Name;
                logger.LogError(ModelNullOrEmptyErrorMessage, typeName);

                throw new ModelNullOrEmptyException(ModelNullOrEmptyErrorMessage.Replace("{ModelType}", typeName));
            }
        }
    }
}
