using System.Diagnostics.CodeAnalysis;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var l = new List<AggregationGroup>
        {
            new AggregationGroup
            {
                Key = new AggregationKey
                {
                    { "instrument", new AggregationValue { StringValue = "instrument1" } },
                    { "subtype", new AggregationValue { StringValue = "subtype1" } }
                },
                Value = new Dictionary<string, AggregationValue>
                {
                    { "position", new AggregationValue { DecimalValue = 6 } }
                }
            },
            new AggregationGroup
            {
                Key = new AggregationKey
                {
                    { "instrument", new AggregationValue { StringValue = "instrument1" } },
                    { "subtype", new AggregationValue { StringValue = "subtype1" } }
                },
                Value = new Dictionary<string, AggregationValue>
                {
                    { "position", new AggregationValue { DecimalValue = 5 } }
                }
            },
        };

        var g1 = l.GroupBy(x => x.Key, new AggregationKeyComparer());
            //.ToDictionary(x => x.Key, x => x.Select(x => x.Value), new AggregationKeyComparer());


    }
}

public class AggregationGroup
{
    public AggregationKey Key { get; set; }

    public Dictionary<string, AggregationValue> Value { get; set; }
}

public class AggregationKey : Dictionary<string, AggregationValue>
{
}

public class AggregationValue
{
    public string StringValue { get; set; }

    public DateTime? DateValue { get; set; }

    public decimal? DecimalValue { get; set; }
}

public class AggregationKeyComparer : IEqualityComparer<AggregationKey>
{
	public bool Equals(AggregationKey x, AggregationKey y)
	{
        if (x.Count != y.Count)
            return false;

        foreach (var xItem in x)
        {
            AggregationValue yValue;
            if (!y.TryGetValue(xItem.Key, out yValue) ||
                !string.Equals(xItem.Value.StringValue, yValue.StringValue, StringComparison.InvariantCultureIgnoreCase) ||
                xItem.Value.DateValue != yValue.DateValue ||
                xItem.Value.DecimalValue != yValue.DecimalValue)
            {
                return false;
            }
        }

        return true;
	}

	public int GetHashCode(AggregationKey obj)
	{
        var sb = new StringBuilder();

        foreach (var item in obj.OrderBy(x => x.Key))
        {
            sb.Append(item.Key);
            if (item.Value.StringValue != null)
                sb.Append(item.Value.StringValue);

			if (item.Value.DecimalValue.HasValue)
				sb.Append(item.Value.DecimalValue);

			if (item.Value.DateValue.HasValue)
				sb.Append(item.Value.DateValue);
		}

        return sb.ToString().ToUpper().GetHashCode();
    }
}