using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATOA.INFRA.Settings;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public AdminTokenSettings AdminToken { get; set; }
    public PublicTokenSettings PublicToken { get; set; }
}

public class AdminTokenSettings
{
    public string SecretKey { get; set; }
    public int ExpirationMinutes { get; set; } = 30; // 30 minutes par défaut
}

public class PublicTokenSettings
{
    public string SecretKey { get; set; }
    public int ExpirationMinutes { get; set; } = 2; // 2 minutes par défaut
}
