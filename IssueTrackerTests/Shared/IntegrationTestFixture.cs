//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IssueTrackerTests
//{
//    [TestClass]
//    public class IntegrationTestFixture
//    {
//        public static IntegrationTestDatabase Database { get; private set; } = null!;

//        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
//        public static async Task Initialize(TestContext context)
//        {
//            var connectionString =
//                Environment.GetEnvironmentVariable("ConnectionStrings__IntegrationTest");

//            Assert.IsNotNull(connectionString);

//            Database = await IntegrationTestDatabase.CreateAsync(connectionString);
//        }

//        [TestInitialize]
//        public async Task ResetDatabase()
//        {
//            await Database.ResetAsync();
//        }
//    }
//}
