using System.Drawing;

namespace SteamProject.Data
{
    public class UserInfoData
    {
        public string UserName { get; set;}
        public string Email { get; set;}
        public bool EmailConfirmed { get; set; } = true;
    }

    public class SeedData 
    {
        public static readonly UserInfoData[] UserSeedData = new UserInfoData[]
        {
            new UserInfoData { UserName = "TestUser", Email = "TestUser@mail.com" },
            new UserInfoData { UserName = "TestUser2", Email = "TestUser2@mail.com" },
        };
    }
}
