﻿using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class Concept : BaseEntity, INameable
    {
        public string Name { get; set; }
    }
}