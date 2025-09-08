using DragonFarmApi.Models;

namespace DragonFarmApi.DTOs.Responses;
public class GetRolesResponse
{
    public List<Role> DragonFarmUserRoles { get; set; }
    public int TotalRoles { get; set; }
}

