using HoYoLabApi.Classes;
using HoYoLabApi.interfaces;
using HoYoLabApi.Models;
using HoYoLabApi.Static;

namespace HoYoLabApi.HonkaiStarRail;

public class HonkaiStarRailService : ServiceBase
{
	private const string gameCode = "hkrpg_global";
	
	private static readonly Func<GameData, ClaimRequest> s_codeClaim = (GameData? gameAcc)
		=> ClaimRequest.FromData(gameAcc, "sg-hkrpg-api", gameAcc?.Region.GetHsrRegion());

	private static readonly ClaimRequest s_dailyClaim = new(
		"sg-public-api",
		"event/luna/os/sign",
		"e202303301540311"
	);
	
	public HonkaiStarRailService(IHoYoLabClient client) : base(client, s_codeClaim, s_dailyClaim)
	{
	}
	
	public Task<IGameResponse> GetGameAccountAsync(string? cookies = null)
	{
		return base.GetGameAccountAsync(cookies?.ParseCookies() ?? Client.Cookies!, gameCode);
	}
}