namespace Сестричка_парсер.Core.Vuz
{
    class VuzSettings : IParserSettings
    {
        public VuzSettings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set; }
        public string Prefix { get; set; } = "{CurrentId}";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}
