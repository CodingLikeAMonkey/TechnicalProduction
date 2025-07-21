using Godot;
using System;

[Tool]
public partial class UI_HBoxContainer : HBoxContainer
{
    [Export]
    public bool UseSpaceBetween { get; set; } = true;

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (!UseSpaceBetween)
            return;

        if (what == NotificationResized || what == NotificationSortChildren)
        {
            UpdateChildrenPositions();
        }
    }

    private void UpdateChildrenPositions()
    {
        int count = GetChildCount();
        if (count == 0)
            return;

        Vector2 containerSize = Size;

        // Get widths of all children
        float[] widths = new float[count];
        for (int i = 0; i < count; i++)
        {
            Control child = GetChild<Control>(i);
            widths[i] = child.Size.X;
        }

        // Position first child on the left
        Control firstChild = GetChild<Control>(0);
        firstChild.Position = new Vector2(0, (containerSize.Y - firstChild.Size.Y) / 2);

        if (count == 1)
            return;

        // Position last child on the right
        Control lastChild = GetChild<Control>(count - 1);
        lastChild.Position = new Vector2(containerSize.X - widths[count - 1], (containerSize.Y - lastChild.Size.Y) / 2);

        if (count == 2)
            return;

        // Calculate total width of middle children
        float middleWidth = 0;
        for (int i = 1; i < count - 1; i++)
            middleWidth += widths[i];

        float availableSpace = lastChild.Position.X - (firstChild.Position.X + widths[0]);
        float totalGap = availableSpace - middleWidth;
        int gapsCount = count - 1;

        float gapSize = totalGap / gapsCount;

        float currentX = firstChild.Position.X + widths[0] + gapSize;
        for (int i = 1; i < count - 1; i++)
        {
            Control child = GetChild<Control>(i);
            child.Position = new Vector2(currentX, (containerSize.Y - child.Size.Y) / 2);
            currentX += widths[i] + gapSize;
        }
    }
}
