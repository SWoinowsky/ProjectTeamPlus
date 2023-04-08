using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class AdminUser
{
    public int Id { get; set; }

    public string? AspnetIdentityId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
