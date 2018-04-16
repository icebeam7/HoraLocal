namespace HoraLocal.Modelos
{
    public class TimeZoneInfo
    {
        public long dstOffset { get; set; }
        public long rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
}
