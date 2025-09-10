using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Auth
{
    public DateTime? CreatedAt { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int RespondentId { get; set; }

    public string? Role { get; set; }

    public virtual Respondent Respondent { get; set; } = null!;
}
