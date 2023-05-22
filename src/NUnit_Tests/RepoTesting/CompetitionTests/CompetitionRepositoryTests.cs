using Microsoft.EntityFrameworkCore;
using Moq;
using SteamProject.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using System;
namespace NUnit_Tests.RepoTesting.CompetitionTests
{
    public class CompetitionRepositoryTests
    {
        private Mock<SteamInfoDbContext> _mockContext;
        private Mock<DbSet<Competition>> _mockCompetitionDbSet;
        private Mock<ICompetitionVoteRepository> _mockCompetitionVoteRepository;
        private Mock<IUserGameInfoRepository> _mockUserGameInfoRepository;
        private Mock<IUserRepository> _mockUserRepository; // Add this line

        private CompetitionPlayer _compPlayer = new CompetitionPlayer { CompetitionId = 1, SteamId = "1" };
        private List<Competition> _competitions;

        [SetUp]
        public void Setup()
        {
            _competitions = new List<Competition>
            {
                new Competition { Id = 1, GameId = 1, StartDate = new DateTime(2001, 03, 08), EndDate = new DateTime(2001, 03, 09) },
                new Competition { Id = 2, GameId = 2, StartDate = new DateTime(2001, 03, 10), EndDate = new DateTime(2001, 03, 11) },
                new Competition { Id = 3, GameId = 3, StartDate = new DateTime(2001, 03, 12), EndDate = new DateTime(2001, 03, 13) }
            };
            _competitions[0].CompetitionPlayers.Add(_compPlayer);

            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Competitions).Returns(_mockCompetitionDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);

            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();
            _mockCompetitionVoteRepository = new Mock<ICompetitionVoteRepository>();
            _mockUserGameInfoRepository = new Mock<IUserGameInfoRepository>();
            _mockUserRepository = new Mock<IUserRepository>(); // Initialize the mock

            // Provide the mocks when setting up CompetitionRepository
            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);
        }




        [Test]
        public void GetCompetitionById_IfNoMatch_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();


            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var testComp = compRepository.GetCompetitionById(50);

            Assert.True( testComp == null );
        }

        [Test]
        public void GetCompetitionById_IfYesMatch_ReturnsMatch()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);


            var testComp = compRepository.GetCompetitionById(1);
            var comparisonComp = new Competition { Id = 1, GameId = 1, StartDate = new DateTime(2001, 03, 08), EndDate = new DateTime(2001, 03, 09) };

            Assert.True( testComp.Id.Equals( comparisonComp.Id ));
        }

        [Test]
        public void GetCompetitionByCompPlayerAndGameId_IfNoMatch_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var testComp = compRepository.GetCompetitionByCompPlayerAndGameId( _compPlayer, 0 );

            Assert.True( testComp == null );
        }

        [Test]
        public void GetCompetitionByCompPlayerAndGameId_IfYesMatch_ReturnsMatch()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var testComp = compRepository.GetCompetitionByCompPlayerAndGameId( _compPlayer, 1 );

            Assert.True( testComp == _competitions[0] );
        }

        [Test]
        public void GetAllCompetitionsForUser_IfNullInput_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var testComps = compRepository.GetAllCompetitionsForUser(null);

            Assert.True( testComps == null );
        }

        [Test]
        public void GetAllCompetitionsForUser_IfEmptyList_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var testComps = compRepository.GetAllCompetitionsForUser( new List<CompetitionPlayer>() );

            Assert.True( testComps == null );
        }

        [Test]
        public void GetAllCompetitionsForUser_IfListContainsProperComps_ReturnsCompetitions()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            var mockCompetitionPlayerRepository = new Mock<ICompetitionPlayerRepository>();

            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object, mockCompetitionPlayerRepository.Object, _mockCompetitionVoteRepository.Object, _mockUserGameInfoRepository.Object, _mockUserRepository.Object);

            var compPlayerList = new List<CompetitionPlayer>();
            compPlayerList.Add( _compPlayer );

            var testComps = compRepository.GetAllCompetitionsForUser( compPlayerList );

            var goalList = new List<Competition>();
            goalList.Add( _competitions[0] );

            Assert.True( testComps.SequenceEqual( goalList ) );
        }
    }
}