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
        }

        [Test]
        public void GetCompetitionById_IfNoMatch_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object);

            var testComp = compRepository.GetCompetitionById(50);

            Assert.True( testComp == null );
        }

        [Test]
        public void GetCompetitionById_IfYesMatch_ReturnsMatch()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object);

            var testComp = compRepository.GetCompetitionById(1);
            var comparisonComp = new Competition { Id = 1, GameId = 1, StartDate = new DateTime(2001, 03, 08), EndDate = new DateTime(2001, 03, 09) };


            Assert.True( testComp.Equals( comparisonComp ) );
        }

        [Test]
        public void GetCompetitionByCompPlayerAndGameId_IfNoMatch_ReturnsNull()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object);

            var testComp = compRepository.GetCompetitionByCompPlayerAndGameId( _compPlayer, 0 );

            Assert.True( testComp == null );
        }

        [Test]
        public void GetCompetitionByCompPlayerAndGameId_IfYesMatch_ReturnsMatch()
        {
            _mockContext = new Mock<SteamInfoDbContext>();
            _mockCompetitionDbSet = MockHelpers.GetMockDbSet(_competitions.AsQueryable());
            _mockContext.Setup(ctx => ctx.Set<Competition>()).Returns(_mockCompetitionDbSet.Object);
            ICompetitionRepository compRepository = new CompetitionRepository(_mockContext.Object);

            var testComp = compRepository.GetCompetitionByCompPlayerAndGameId( _compPlayer, 1 );

            Assert.True( testComp == _competitions[0] );
        }
    }
}