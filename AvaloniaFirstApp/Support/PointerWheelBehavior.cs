using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

public class PointerWheelBehavior : AvaloniaObject
{
    public static readonly AttachedProperty<Action<PointerWheelEventArgs>?> WheelChangedCommandProperty =
        AvaloniaProperty.RegisterAttached<PointerWheelBehavior, Control, Action<PointerWheelEventArgs>?>(
            "WheelChangedCommand"
        );

    public static void SetWheelChangedCommand(AvaloniaObject element, Action<PointerWheelEventArgs>? value)
    {
        element.SetValue(WheelChangedCommandProperty, value);
        if (element is Control control)
        {
            control.PointerWheelChanged -= Control_PointerWheelChanged;
            if (value != null)
            {
                control.PointerWheelChanged += Control_PointerWheelChanged;
            }
        }
    }

    public static Action<PointerWheelEventArgs>? GetWheelChangedCommand(AvaloniaObject element) =>
        element.GetValue(WheelChangedCommandProperty);

    private static void Control_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is AvaloniaObject obj)
        {
            var command = GetWheelChangedCommand(obj);
            command?.Invoke(e);
        }
    }
}
