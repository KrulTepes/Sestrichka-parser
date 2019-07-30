using AngleSharp.Html.Dom;

namespace Сестричка_парсер.Core
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
