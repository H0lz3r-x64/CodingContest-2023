namespace com.knapp.CodingContest.core
{
    /// <summary>
    /// class containing settings for the program
    /// 
    /// DO NOT MODIFY THE SETTINGS
    /// 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Directory for the input files
        /// </summary>
        public const string DataPath = @"input";

        public const string ProductFile = @"products.csv";

        public const string CustomerFile = @"customers.csv";

        public const string WarehouseFile = @"warehouses.csv";

        public const string WarehouseStockFile = @"warehouses-stocks.csv";

        public const string OrderLineFile = @"order-lines.csv";


        

        /// <summary>
        /// Directory where the output will be written
        /// </summary>
        public const string OutputPath = @"";

        /// <summary>
        /// Path where the source can be found
        /// </summary>
        public const string sourceDirectory = @"..\..\";

        /// <summary>
        /// Name of the results file
        /// </summary>
        public const string outResultFilename = @"result.csv";

        /// <summary>
        /// Name of the properties file
        /// </summary>
        public const string outPropertyFilename = @"KCC.properties";

        /// <summary>
        /// Name of the zip-file that is generated
        /// </summary>
        public const string outZipFilename = @"upload.zip";

        /// <summary>
        /// Name of the source directory within the zip file
        /// </summary>
        public const string zipSourceDirectory = "src\\";
    }
}
