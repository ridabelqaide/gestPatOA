﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATOA.APPLICATION.Dtos.Auth;

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
