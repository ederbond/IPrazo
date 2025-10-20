using HtmlAgilityPack;

namespace IPrazo.Crowler;

public static class ProxyParser
{
    // Parses the first table in the document and finds columns by header names (IP Address, Port, Protocol)
    public static List<Data> ParseProxyTable(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var table = doc.DocumentNode.SelectSingleNode("//table[.//th]");
        if (table == null)
            return new List<Data>();

        var headerNodes = table.SelectNodes(".//thead//th");
        if (headerNodes == null)
            return new List<Data>();

        int ipIndex = -1, portIndex = -1, protocolIndex = -1;

        for (int i = 0; i < headerNodes.Count; i++)
        {
            var text = headerNodes[i].InnerText?.Trim() ?? string.Empty;
            if (text.IndexOf("IP Address", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("IP", StringComparison.OrdinalIgnoreCase) >= 0)
                ipIndex = i;
            else if (text.IndexOf("Port", StringComparison.OrdinalIgnoreCase) >= 0)
                portIndex = i;
            else if (text.IndexOf("Protocol", StringComparison.OrdinalIgnoreCase) >= 0)
                protocolIndex = i;
        }

        // If required columns not found, return empty list
        if (ipIndex == -1 || portIndex == -1 || protocolIndex == -1)
            return new List<Data>();

        var rows = table.SelectNodes(".//tbody//tr");
        if (rows == null)
            return new List<Data>();

        var result = new List<Data>();

        foreach (var row in rows)
        {
            var cols = row.SelectNodes("./td");
            if (cols == null || cols.Count <= Math.Max(Math.Max(ipIndex, portIndex), protocolIndex))
                continue;

            // IP: prefer anchor text inside the cell
            var ipCell = cols[ipIndex];
            string ip = null;
            var a = ipCell.SelectSingleNode(".//a");
            if (a != null)
                ip = HtmlEntity.DeEntitize(a.InnerText).Trim();
            else
                ip = HtmlEntity.DeEntitize(ipCell.InnerText).Trim();

            // Port: prefer data-port attribute on span if present, otherwise plain text
            var portCell = cols[portIndex];
            string port = null;
            var span = portCell.SelectSingleNode(".//span[@data-port]");
            if (span != null)
                port = span.GetAttributeValue("data-port", string.Empty).Trim();
            if (string.IsNullOrEmpty(port))
                port = HtmlEntity.DeEntitize(portCell.InnerText).Trim();

            // Protocol
            var protocolCell = cols[protocolIndex];
            string protocol = HtmlEntity.DeEntitize(protocolCell.InnerText).Trim();

            result.Add(new Data { Ip = ip, Port = port, Protocol = protocol });
        }

        return result;
    }
}