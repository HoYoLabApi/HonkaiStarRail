using HoYoLabApi.interfaces;
using HoYoLabApi.Models;
using HoYoLabApi.Static;

namespace HoYoLabApi.HonkaiStarRail;

public sealed class HonkaiStarRailService
{
	private readonly IHoYoLabClient m_client;

	public HonkaiStarRailService(IHoYoLabClient client)
		=> m_client = client;
	
	public async Task<GameData> GetGameAccountAsync(ICookies cookies)
	{
		return (await m_client.GetGamesArrayAsync(new Request(
			"api-account-os",
			"account/binding/api/getUserGameRolesByCookieToken",
			cookies,
			new Dictionary<string, string>()
			{
				{ "game_biz", "hkrpg_global" },
				{ "uid", cookies.AccountId.ToString() },
				{ "sLangKey", cookies.Language.GetLanguageString() },
			}
		)).ConfigureAwait(false)).Data.GameAccounts.FirstOrDefault()!;
	}
	
	public Task<GameData> GetGameAccountAsync(string cookies)
		=> GetGameAccountAsync(cookies.ParseCookies());
	
	public Task<GameData> GetGameAccountAsync()
		=> GetGameAccountAsync(m_client.Cookies);
	
	public async Task<IDailyClaimResult> DailyClaimAsync(ICookies cookies)
	{
		return await m_client.DailyClaimAsync(new Request(
			"sg-public-api",
			"event/luna/os/sign",
			cookies,
			new Dictionary<string, string>
			{
				{ "act_id", "e202303301540311" },
				{ "lang", cookies.Language.GetLanguageString() }
			}
		)).ConfigureAwait(false);
	}
	
	private async IAsyncEnumerable<IDailyClaimResult> DailysClaimAsync(ICookies[] cookies, CancellationToken? cancellationToken = null)
	{
		cancellationToken ??= CancellationToken.None;
		foreach (var cookie in cookies)
		{
			if (cancellationToken.Value.IsCancellationRequested)
				yield break;
			
			yield return await DailyClaimAsync(cookie).ConfigureAwait(false);
		}
	}
	
	public IAsyncEnumerable<IDailyClaimResult> DailysClaimAsync(string[] cookies, CancellationToken? cancellationToken = null)
		=> DailysClaimAsync(cookies.Select(x => x.ParseCookies()).ToArray());
	
	public Task<IDailyClaimResult> DailyClaimAsync()
		=> DailyClaimAsync(m_client.Cookies);

	public Task<IDailyClaimResult> DailyClaimAsync(string cookies)
		=> DailyClaimAsync(cookies.ParseCookies());

	public async Task<ICodeClaimResult> CodeClaimAsync(ICookies cookies, string code)
	{
		var gameAcc = await GetGameAccountAsync(cookies).ConfigureAwait(false);
		
		return await m_client.CodeClaimAsync(new CodeClaimRequest(
			"sg-hkrpg-api",
			"common/apicdkey/api/webExchangeCdkey",
			cookies,
			new Dictionary<string, string>()
			{
				{ "uid", gameAcc.Uid.ToString() },
				{ "region", gameAcc.Region.GetHsrRegion() },
				{ "game_biz", gameAcc.Game },
				{ "cdkey", code },
				{ "lang", cookies.Language.GetHsrLang() },
			},
			new Dictionary<string, string>()
			{
				{ "Referer", "https://hsr.hoyoverse.com" },
				{ "Orig", "https://hsr.hoyoverse.com" }
			}
		)).ConfigureAwait(false);
	}
	
	public Task<ICodeClaimResult> CodeClaimAsync(string code)
		=> CodeClaimAsync(m_client.Cookies, code);

	public async IAsyncEnumerable<ICodeClaimResult> CodesClaimAsync(
		string[] codes,
		CancellationToken? cancellationToken = null)
	{
		cancellationToken ??= CancellationToken.None;
		foreach (var code in codes)
		{
			if (cancellationToken.Value.IsCancellationRequested)
				yield break;
			
			yield return await CodeClaimAsync(code).ConfigureAwait(false);
		}
	}
}