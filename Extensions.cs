﻿using HoYoLabApi.Enums;

namespace HoYoLabApi.HonkaiStarRail;

public static class Extensions
{
	public static string GetHsrLang(this Language language)
	{
		return language switch
		{
			Language.English => "en",
			Language.Russian => "ru",
			_ => "en"
		};
	}
	
	public static string GetHsrRegion(this Region region)
	{
		return region switch
		{
			Region.Europe => "prod_official_eur",
			_ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
		};
	}
}