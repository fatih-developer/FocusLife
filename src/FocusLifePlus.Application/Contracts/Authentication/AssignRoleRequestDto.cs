using System;

namespace FocusLifePlus.Application.Contracts.Authentication;

public class AssignRoleRequestDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
} 