﻿using System;

namespace BOA
{
    public class Commit
    {
        public int CommitId { get; set; }
        public int ProjectId { get; set; }
        public DateTimeOffset Date { get; set; }
        public Changes Changes { get; set; }
    }
}
