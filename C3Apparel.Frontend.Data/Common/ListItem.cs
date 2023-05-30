namespace C3Apparel.Frontend.Data.Common;

public class ListItem
{
    public string Text { get;  }
    public string Value { get;  }
    public bool IsSelected { get; }

    public ListItem(string text, string value, bool isSelected)
    {
        Text = text;
        Value = value;
        IsSelected = isSelected;
    }
}