using HoYoLabApi.Enums;

namespace HoYoLabApi.HonkaiStarRail;

public static class Extensions
{
	public static string GetHsrRegion(this Region region)
	{
		return region switch
		{
			Region.Europe => "prod_official_eur",
			Region.America => "prod_official_usa",
			Region.Asia => "prod_official_asia",
			Region.Cht => "prod_official_cht",
			_ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
		};
	}
}