using System.Windows;
using System.Windows.Controls;

namespace NoName.Controls.Primitives
{
    public class ManipulatorHandle : ContentControl
    {
        public ManipulatorHandle()
        {
            DefaultStyleKey = typeof(ManipulatorHandle);
        }

        #region Type Property

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ManipulatorHandleType), typeof(ManipulatorHandle), new PropertyMetadata(OnTypePropertyChanged));

        private static void OnTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ManipulatorHandle)d).UpdateView();
        }

        public ManipulatorHandleType Type
        {
            get { return (ManipulatorHandleType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        private bool isMouseOver;
        private bool IsMouseOver
        {
            get { return isMouseOver; }
            set { isMouseOver = value; UpdateView(); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateView();
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            IsMouseOver = true;
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            IsMouseOver = false;
        }

        void UpdateView()
        {
            if (Type == ManipulatorHandleType.W)
            {
                VisualStateManager.GoToState(this, WestStateName, true);
            }
            else if (Type == ManipulatorHandleType.N)
            {
                VisualStateManager.GoToState(this, NorthStateName, true);
            }
            else if (Type == ManipulatorHandleType.E)
            {
                VisualStateManager.GoToState(this, EastStateName, true);
            }
            else if (Type == ManipulatorHandleType.S)
            {
                VisualStateManager.GoToState(this, SouthStateName, true);
            }
            else if (Type == ManipulatorHandleType.WN)
            {
                VisualStateManager.GoToState(this, WeastNorthStateName, true);
            }
            else if (Type == ManipulatorHandleType.NE)
            {
                VisualStateManager.GoToState(this, NorthEastStateName, true);
            }
            else if (Type == ManipulatorHandleType.ES)
            {
                VisualStateManager.GoToState(this, EastSouthStateName, true);
            }
            else if (Type == ManipulatorHandleType.SW)
            {
                VisualStateManager.GoToState(this, SouthWestStateName, true);
            }
            else if (Type == ManipulatorHandleType.Rotate)
            {
                VisualStateManager.GoToState(this, RotateStateName, true);
            }

            if(IsMouseOver)
            {
                VisualStateManager.GoToState(this, MouseOverStateName, true);
            }
            else
            {
                VisualStateManager.GoToState(this, NormalStateName, true);
            }
        }

        private const string WestStateName = "West";
        private const string NorthStateName = "North";
        private const string EastStateName = "East";
        private const string SouthStateName = "South";
        private const string WeastNorthStateName = "WestNorth";
        private const string NorthEastStateName = "NorthEast";
        private const string EastSouthStateName = "EastSouth";
        private const string SouthWestStateName = "SouthWest";
        private const string RotateStateName = "Rotate";

        private const string MouseOverStateName = "MouseOver";
        private const string NormalStateName = "Normal";

    }


}
