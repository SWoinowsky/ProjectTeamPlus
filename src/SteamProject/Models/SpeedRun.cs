﻿using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class SpeedRun
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public string? VideoId { get; set; }

    public int PlayerId { get; set; }
}