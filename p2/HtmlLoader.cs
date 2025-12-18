using System;
using System.Collections.Generic;
using System.Text;

namespace p2
{
    public class HtmlLoader
    {

        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

    }
}
