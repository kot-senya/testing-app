using Avalonia;
using Avalonia.Controls;
using System;

namespace EntranseTesting.Models
{
    public class LayoutChangeEventArgs : EventArgs
    {
        public readonly Rect Rect;
        public LayoutChangeEventArgs(Rect rect) => Rect = rect;
    }

    public class LayoutWatcher
    {
        public void ChangeTarget(Control target, Control origin = null)
        {
            if (this.target != null)
                this.target.LayoutUpdated -= OnLayoutUpdate;

            this.target = target;
            this.origin = origin;
            OnLayoutUpdate(null, null);

            if (this.target != null)
                this.target.LayoutUpdated += OnLayoutUpdate;
        }

        void OnLayoutUpdate(object sender, EventArgs e)
        {
            var newRenderRect = GetRenderRect();
            if (newRenderRect != currRenderRect)
            {
                currRenderRect = newRenderRect;
                FireChanged();
            }
        }

        Control target, origin;
        Rect currRenderRect = new Rect();

        public static Rect ComputeRenderRect(Control target, Control origin)
        {
            Point? translatedPoint = target.TranslatePoint(new Point(), origin);
            Point point = translatedPoint.HasValue ? translatedPoint.Value : new Point();
            return new Rect(point, target.Bounds.Size);
        }
            

        Rect GetRenderRect() => ComputeRenderRect(target, origin);

        void FireChanged() =>
            Changed?.Invoke(target, new LayoutChangeEventArgs(currRenderRect));

        public event EventHandler<LayoutChangeEventArgs> Changed;
    }
}
