namespace GUI.Components.OmniButton
{
    public class OmniButtonState
    {
        public readonly string Text;
        public readonly string? Tooltip;
        public readonly Action? Action;
        public readonly bool IsDisabled;
        public readonly bool IsWaiting = false;

        public OmniButtonState(string text, Action? action)
        {
            Text = text;
            Action = action;
            IsDisabled = Action == null;
        }
        public OmniButtonState(string text, string? tooltip, Action? action)
        {
            Text = text;
            Tooltip = tooltip;
            Action = action;
            IsDisabled = Action == null;
        }

        public OmniButtonState(string text, string? tooltip, Action? action, bool isWaiting)
        {
            Text = text;
            Tooltip = tooltip;
            Action = action;
            IsDisabled = Action == null;
            IsWaiting = isWaiting;
        }
    }
}
