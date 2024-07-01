using HNG.Abstractions.Helpers;
using HNG.Abstractions.Models;
using IPinfo;
using IPinfo.Models;
using Microsoft.Extensions.Caching.Memory;
using OpenWeatherMap;
using OpenWeatherMap.Models;

namespace HNG.Api.Client.Extensions
{
    /// <summary>
    /// IpInfo Service  Extension
    /// </summary>
    public class IpInfoService
    {
        private readonly AppSettings _appSettings;
        private readonly IPinfoClient _ipInfoClient;
        private readonly IOpenWeatherMapService _openWeatherMapService;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromDays(30);
        private readonly TimeSpan _cacheWeatherDuration = TimeSpan.FromHours(1);
        private const int PrefixLength = 24;

        /// <summary>
        /// constructor
        /// </summary>
        public IpInfoService(IPinfoClient ipInfoClient, IMemoryCache cache, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _openWeatherMapService = new OpenWeatherMapService(new OpenWeatherMapOptions
            {
                ApiKey = _appSettings.OpenWeatherMap.ApiKey,
                ApiEndpoint = _appSettings.OpenWeatherMap.ApiEndpoint,
                Language = _appSettings.OpenWeatherMap.Language,
                UnitSystem = _appSettings.OpenWeatherMap.UnitSystem
            });
            _ipInfoClient = ipInfoClient;
            _cache = cache;
        }

        /// <summary>
        /// Get ip address information based on provided ip address
        /// </summary>
        public async Task<IPResponse?> GetIpInfoAsync(string ip)
        {
            var ipRange = IpRangeHelper.GetIpRange(ip, PrefixLength);

            if (_cache.TryGetValue(ipRange, out IPResponse? cachedResponse))
            {
                return cachedResponse;
            }

            //localhost range
            if (ipRange == "0.0.0.0")
            {
                return new IPResponse();
            }

            var response = await _ipInfoClient.IPApi.GetDetailsAsync(ip);
            _cache.Set(ipRange, response, _cacheDuration);
            return response;
        }

        /// <summary>
        /// Get weather information for Ip
        /// </summary>
        public async Task<WeatherInfo?> GetWeatherInfoAsync(string Ip, string? Lat, string? Lon)
        {
            var ipRange = IpRangeHelper.GetIpRange(Ip, PrefixLength);
            if (_cache.TryGetValue(ipRange, out WeatherInfo? cachedResponse))
            {
                return cachedResponse;
            }

            if (string.IsNullOrWhiteSpace(Lat) || string.IsNullOrWhiteSpace(Lon))
            {
                return null;
            }

            var weatherInfo = await _openWeatherMapService.GetCurrentWeatherAsync(Convert.ToDouble(Lat), Convert.ToDouble(Lon));
            _cache.Set(ipRange, weatherInfo, _cacheWeatherDuration);
            return weatherInfo;
        }
    }
}
