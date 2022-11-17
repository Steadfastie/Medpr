using AutoMapper;
using MedprCore.Abstractions;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;

namespace MedprWebAPI.Utils;

/// <summary>
/// Class to check what info user is allowed to see
/// </summary>
public class WardedPeople
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyMemberService _familyMemberService;
    public WardedPeople(IFamilyService familyService,
        IFamilyMemberService familyMemberService)
    {
        _familyMemberService = familyMemberService;
        _familyService = familyService;
    }

    /// <summary>
    /// Method returns list of user IDs whose information user is allowed to see
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Guid>> GetWardedByUserPeople(Guid userId)
    {
        var families = await _familyService.GetFamiliesRelevantToUser(userId);
        HashSet<Guid> usersInAllFamilies = new()
        {
            userId
        };

        foreach (var family in families)
        {
            var membersDTO = await _familyMemberService.GetMembersRelevantToFamily(family.Id);
            var isCurrentUserAdmin = membersDTO
                .Where(member => member.UserId == userId)
                .ToList()[0]
                .IsAdmin;
            if (isCurrentUserAdmin)
            {
                var wardedPeople = membersDTO.Select(member => member.UserId).Where(member => member != userId);
                usersInAllFamilies.AddRange(wardedPeople);
            }
        }

        return usersInAllFamilies.ToList();
    }
}
