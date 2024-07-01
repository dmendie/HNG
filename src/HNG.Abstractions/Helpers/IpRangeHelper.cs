using System.Net;

namespace HNG.Abstractions.Helpers
{
    public static class IpRangeHelper
    {
        public static string GetIpRange(string ipAddress, int prefixLength = 24)
        {
            var address = IPAddress.Parse(ipAddress);
            var bytes = address.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            uint ip = BitConverter.ToUInt32(bytes, 0);
            uint mask = uint.MaxValue << (32 - prefixLength);
            uint ipRange = ip & mask;

            var rangeBytes = BitConverter.GetBytes(ipRange);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(rangeBytes);
            }

            return new IPAddress(rangeBytes).ToString();
        }
    }
}
