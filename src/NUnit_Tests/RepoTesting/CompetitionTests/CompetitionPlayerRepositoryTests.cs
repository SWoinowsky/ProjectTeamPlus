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
using NUnit_Tests.RepoTesting;

namespace NUnit_Tests.RepoTesting.CompetitionTests;

public class CompetitionPlayerRepositoryTests
{
    private Mock<SteamInfoDbContext> _mockContext;
    private Mock<DbSet<CompetitionPlayer>> _mockCompetitionPlayerDbSet;

    private List<CompetitionPlayer> _competitionPlayers; 

    [SetUp]
    public void Setup()
    {
        _competitionPlayers = new List<CompetitionPlayer>
        {
            new CompetitionPlayer { CompetitionId = 1, SteamId = "1" },
            new CompetitionPlayer { CompetitionId = 2, SteamId = "2" },
            new CompetitionPlayer { CompetitionId = 3, SteamId = "3" },
            new CompetitionPlayer { CompetitionId = 4, SteamId = "1" },
            new CompetitionPlayer { CompetitionId = 5, SteamId = "1" },
        };

        _mockContext = new Mock<SteamInfoDbContext>();
        _mockCompetitionPlayerDbSet = MockHelpers.GetMockDbSet(_competitionPlayers.AsQueryable());
        _mockContext.Setup(ctx => ctx.CompetitionPlayers).Returns(_mockCompetitionPlayerDbSet.Object);
        _mockContext.Setup(ctx => ctx.Set<CompetitionPlayer>()).Returns(_mockCompetitionPlayerDbSet.Object);
    }

    [Test]
    public void GetCompetitionIdsBySteamId_IfNoMatch_ReturnsNull()
    {
        _mockContext = new Mock<SteamInfoDbContext>();
        _mockCompetitionPlayerDbSet = MockHelpers.GetMockDbSet(_competitionPlayers.AsQueryable());
        _mockContext.Setup(ctx => ctx.Set<CompetitionPlayer>()).Returns(_mockCompetitionPlayerDbSet.Object);
        ICompetitionPlayerRepository compPlayRepository = new CompetitionPlayerRepository(_mockContext.Object);

        var listFound = compPlayRepository.GetCompetitionIdsBySteamId("0");
        
        Assert.True( listFound == null );
    }

    [Test]
    public void GetCompetitionsBySteamId_IfYesMatch_ReturnsMatchingList()
    {
        _mockContext = new Mock<SteamInfoDbContext>();
        _mockCompetitionPlayerDbSet = MockHelpers.GetMockDbSet(_competitionPlayers.AsQueryable());
        _mockContext.Setup(ctx => ctx.Set<CompetitionPlayer>()).Returns(_mockCompetitionPlayerDbSet.Object);
        ICompetitionPlayerRepository compPlayRepository = new CompetitionPlayerRepository(_mockContext.Object);

        var foundList = compPlayRepository.GetCompetitionIdsBySteamId("1");
        var compareList = new List<CompetitionPlayer>
        { 
            _competitionPlayers[0],
            _competitionPlayers[3],
            _competitionPlayers[4]
        };

        Assert.True( foundList.SequenceEqual(compareList) );
    }
}
