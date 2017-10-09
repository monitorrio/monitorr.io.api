using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Infrastructure.Repositories;

namespace Tests.Infrastructure
{
    /// <summary>
    /// Summary description for AutopocoTests
    /// </summary>
    [TestClass]
    public class AutopocoTests
    {
        public static IAutopocoRepository AutopocoRepository;

        public AutopocoTests()
        {
            AutopocoRepository = new AutopocoRepository();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        #endregion

    }
}
