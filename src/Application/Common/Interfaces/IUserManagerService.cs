﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserManagerService
    {
        public Task<List<Guid>> GetUserIdsInRole(string role);
    }
}