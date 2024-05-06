namespace Scavenger.Core.Utility;

public static class FilterComboBox
{
    public static void Execute<T>(IEnumerable<T> items, Func<T, string> selector, Action<string> addItem)
    {
        foreach (var item in items.GroupBy(selector).Select(group => group.Key).OrderBy(key => key))
        {
            addItem(item);
        }
    } 
}