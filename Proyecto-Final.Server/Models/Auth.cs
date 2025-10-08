using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Auth
{
    public int RespondentId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Respondent Respondent { get; set; } = null!;
}
