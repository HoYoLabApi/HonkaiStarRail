﻿using HoYoLabApi.Enums;
using HoYoLabApi.interfaces;
using HoYoLabApi.Static;

namespace HoYoLabApi.HonkaiStarRail;

public class HonkaiStarRailService : HonkaiStarRailServiceBase
{
	public HonkaiStarRailService(IHoYoLabClient client) : base(client)
	{
	}

	public async Task DailiesClaimAsync(ICookies[] cookies)
	{
		await foreach (var _ in DailiesClaimAsync(cookies, null))
		{
		}
	}

	public async Task CodesClaimAsync(ICookies cookies, string[] codes)
	{
		await foreach (var _ in CodesClaimAsync(cookies, codes, null))
		{
		}
	}

	public Task<IDailyClaimResult> DailyClaimAsync(string cookies)
	{
		return base.DailyClaimAsync(cookies.ParseCookies());
	}

	public Task<IDailyClaimResult> DailyClaimAsync()
	{
		return base.DailyClaimAsync(Client.Cookies!);
	}

	public Task<ICodeClaimResult> CodeClaimAsync(string code)
	{
		return base.CodeClaimAsync(Client.Cookies!, code);
	}

	public IAsyncEnumerable<IDailyClaimResult> DailiesClaimAsync(string[] cookies, CancellationToken? cancellationToken)
	{
		return base.DailiesClaimAsync(cookies.Select(x => x.ParseCookies()).ToArray(), cancellationToken);
	}

	public Task CodesClaimAsync(string cookies, string[] codes)
	{
		return CodesClaimAsync(cookies.ParseCookies(), codes);
	}

	public Task DailiesClaimAsync(string[] cookies)
	{
		return DailiesClaimAsync(cookies.Select(x => x.ParseCookies()).ToArray());
	}

	public IAsyncEnumerable<ICodeClaimResult> CodesClaimAsync(
		string[] codes,
		string? cookies = null,
		Region? region = null,
		CancellationToken? cancellationToken = null)
	{
		return base.CodesClaimAsync(cookies?.ParseCookies() ?? Client.Cookies!, codes, region, cancellationToken);
	}
}