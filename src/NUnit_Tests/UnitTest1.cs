using SteamProject.Controllers;
using SteamProject.Services;
using SteamProject.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using Moq;
namespace NUnit_Tests
{
    public class Tests
    {
        //IUserRepository, ISteamService, IGameRepository, IUserGameInfoRepository
        // private Mock<ISteamService> _mockSteamService;
        // private Mock<IUserRepository> _mockUserRepo;
        // private Mock<IGameRepository> _mockGameRepo;
        // private Mock<IUserGameInfoRepository> _mockUserGameInfoRepo;
        // private Mock<UserManager<IdentityUser>> _mockUserManager;

        // private SteamController _mockController;
        [SetUp]
        public void Setup()
        {
        
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    //     public void TestControllerTest()
    //     {
    //         _mockUserManager = new Mock<UserManager<IdentityUser>>();
    //         _mockUserRepo = new Mock<IUserRepository>();
    //         _mockSteamService = new Mock<ISteamService>();
    //         _mockGameRepo = new Mock<IGameRepository>();
    //         _mockUserGameInfoRepo = new Mock<IUserGameInfoRepository>();
    //         _mockController = new SteamController(_mockUserManager, _mockUserRepo, _mockSteamService, _mockGameRepo, _mockUserGameInfoRepo);
    //     }
    // }
    }
}