using System.Xml.Serialization;

namespace MetricsDelta.Helpers
{
    /// <summary>
    /// Xml helping functions
    /// </summary>
    public static class XmlHelper
    {
        #region Public Methods

        /// <summary>
        /// Restore object of type <typeparamref name="T"/> from file specified by path.
        /// </summary>
        /// <typeparam name="T">Type of object to restore.</typeparam>
        /// <param name="filePath">Path of file to restore object from.</param>
        /// <returns>Restored object of type <typeparamref name="T"/> or null</returns>
        public static T? RestoreFromXml<T>(string filePath) where T : class
        {
            ArgumentNullException.ThrowIfNullOrEmpty(filePath);

            var serializer = new XmlSerializer(typeof(T));

            using (var s = IOHelper.RepeatTillResultOrThrow(() => File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), 5, 1000, 0))
            {
                var obj = serializer.Deserialize(s);

                return obj is null ? default : (T)obj;
            }
        }

        /// <summary>
        /// Try to restore object of type <typeparamref name="T"/> from file specified by path.
        /// </summary>
        /// <typeparam name="T">Type of object to restore.</typeparam>
        /// <param name="filePath">Path of file to restore object from.</param>
        /// <param name="restoredObj">Restored object of type <typeparamref name="T"/> if successful, null otherwise.</param>
        /// <param name="errorMessage">Error message if unsuccessful.</param>
        /// <returns>True if success, false otherwise.</returns>
        public static bool TryRestoreFromXml<T>(string filePath, out T? restoredObj, out string? errorMessage) where T : class
        {
            try
            {
                restoredObj = RestoreFromXml<T>(filePath);
                errorMessage = default;
                return true;
            }
            catch (Exception ex)
            {
                restoredObj = default;
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion Public Methods
    }
}