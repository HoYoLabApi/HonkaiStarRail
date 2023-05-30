using HoYoLabApi.Classes;
using HoYoLabApi.interfaces;
using HoYoLabApi.Models;

namespace HoYoLabApi.HonkaiStarRail;

public abstract class HonkaiStarRailServiceBase
{
	protected readonly IHoYoLabClient Client;

	private static readonly Func<GameData, ClaimRequest> s_codeClaim = (gameAcc) => new ClaimRequest(
		"sg-hkrpg-api", "", null, gameAcc.Region.GetHsrRegion(), gameAcc
	);
	
	private static readonly ClaimRequest s_dailyClaim = new(
		"sg-public-api",
		"event/luna/os/sign",
		"e202303301540311"
	);

	private readonly AccountSearcher m_accountSearcher;
	private readonly DailyClaimer m_dailyClaimer;
	private readonly CodesClaimer m_codesClaimer;
	
	protected HonkaiStarRailServiceBase(IHoYoLabClient client)
	{
		Client = client;
		m_accountSearcher = new AccountSearcher(client);
		m_dailyClaimer = new DailyClaimer(client, s_dailyClaim);
		m_codesClaimer = new CodesClaimer(client, m_accountSearcher);
	}

	public Task<GameData> GetGameAccountAsync(ICookies? cookies = null)
	{
		return m_accountSearcher.GetGameAccountAsync(cookies ?? Client.Cookies!, "hkrpg_global");
	}

	public Task<IDailyClaimResult> DailyClaimAsync(ICookies cookies)
	{
		return m_dailyClaimer.DailyClaimAsync(cookies);
	}

	public IAsyncEnumerable<IDailyClaimResult> DailiesClaimAsync(ICookies[] cookies, CancellationToken? cancellationToken = null)
	{
		return m_dailyClaimer.DailiesClaimAsync(cookies, cancellationToken);
	}
	
	public async Task<ICodeClaimResult> CodeClaimAsync(ICookies cookies, string code)
	{
		var gameAcc = await GetGameAccountAsync(cookies).ConfigureAwait(false);
		return await m_codesClaimer.CodeClaimAsync(cookies, code, s_codeClaim(gameAcc));
	}
	
	public async IAsyncEnumerable<ICodeClaimResult> CodesClaimAsync(
		ICookies cookies,
		string[] codes,
		CancellationToken? cancellationToken = null)
	{
		cancellationToken ??= CancellationToken.None;
		var gameAcc = await GetGameAccountAsync(cookies).ConfigureAwait(false);
		await foreach (var codeClaimResult in m_codesClaimer.CodesClaimAsync(cookies, codes, s_codeClaim(gameAcc), cancellationToken))
		{
			yield return codeClaimResult;
		}
	}
}