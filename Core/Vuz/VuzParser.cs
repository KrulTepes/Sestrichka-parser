using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;

namespace Сестричка_парсер.Core.Vuz
{
    class VuzParser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            List<string> list = new List<string>();
            var items = document.QuerySelectorAll("td").Where(item => item.ClassName != null && item.ClassName.Contains("center"));
            var itemsName = document.QuerySelectorAll("td").Where(item => item.ClassName != null && item.ClassName.Contains("link_name"));

            foreach (var item in items)
            {
                list.Add(item.TextContent);
            }

            int i = 1;

            foreach (var item in itemsName)
            {
                list.Insert(i, item.TextContent);
                i += 13;
            }

            return list.ToArray();
        }
    }
}
