using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NoName.Controls
{
	public class ManipulableControl : ContentControl
	{
		public ManipulableControl()
		{
			DefaultStyleKey = typeof(ManipulableControl);
		}

		#region IsSelected Property

		public static readonly DependencyProperty IsSelectedProperty =
		  DependencyProperty.Register("IsSelected", typeof(bool), typeof(ManipulableControl), new PropertyMetadata(OnIsSelectedPropertyChanged));

		private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdateView();
		}

		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		#endregion

		#region TranslateX Property

		public static readonly DependencyProperty TranslateXProperty =
		  DependencyProperty.Register("TranslateX", typeof(double), typeof(ManipulableControl), new PropertyMetadata(OnTranslateXPropertyChanged));

		private static void OnTranslateXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdatePosition();
		}

		public double TranslateX
		{
			get { return (double)GetValue(TranslateXProperty); }
			set { SetValue(TranslateXProperty, value); }
		}

		#endregion

		#region TranslateY Property

		public static readonly DependencyProperty TranslateYProperty =
		  DependencyProperty.Register("TranslateY", typeof(double), typeof(ManipulableControl), new PropertyMetadata(OnTranslateYPropertyChanged));

		private static void OnTranslateYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdatePosition();
		}

		public double TranslateY
		{
			get { return (double)GetValue(TranslateYProperty); }
			set { SetValue(TranslateYProperty, value); }
		}

		#endregion

		#region Rotation Property

		public static readonly DependencyProperty RotationProperty =
		  DependencyProperty.Register("Rotation", typeof(double), typeof(ManipulableControl), new PropertyMetadata(OnRotationPropertyChanged));

		private static void OnRotationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdatePosition();
		}

		public double Rotation
		{
			get { return (double)GetValue(RotationProperty); }
			set { SetValue(RotationProperty, value); }
		}

		#endregion

		#region AllowResize Property

		public static readonly DependencyProperty AllowResizeProperty =
		  DependencyProperty.Register("AllowResize", typeof(bool), typeof(ManipulableControl), new PropertyMetadata(true, OnAllowResizePropertyChanged));

		private static void OnAllowResizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdateView();
		}

		public bool AllowResize
		{
			get { return (bool)GetValue(AllowResizeProperty); }
			set { SetValue(AllowResizeProperty, value); }
		}

		#endregion

		#region DragHandlerMode Property

		public static readonly DependencyProperty DragHandlerModeProperty =
		  DependencyProperty.Register("DragHandlerMode", typeof(DragHandlerMode), typeof(ManipulableControl), new PropertyMetadata(OnDragHandlerModePropertyChanged));

		private static void OnDragHandlerModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdatePosition();
		}

		public DragHandlerMode DragHandlerMode
		{
			get { return (DragHandlerMode)GetValue(DragHandlerModeProperty); }
			set { SetValue(DragHandlerModeProperty, value); }
		}

		#endregion

		public static readonly DependencyProperty ClipTranslationToParentProperty =
			DependencyProperty.Register("ClipTranslationToParent", typeof(bool), typeof(ManipulableControl), new PropertyMetadata(default(bool)));

		public bool ClipTranslationToParent
		{
			get { return (bool)GetValue(ClipTranslationToParentProperty); }
			set { SetValue(ClipTranslationToParentProperty, value); }
		}

		public static readonly DependencyProperty AllowManipulateProperty =
			DependencyProperty.Register("AllowManipulate", typeof(bool), typeof(ManipulableControl), new PropertyMetadata(true, OnAllowManipulatePropertyChanged));

		private static void OnAllowManipulatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ManipulableControl)d).UpdateView();
		}

		public bool AllowManipulate
		{
			get { return (bool)GetValue(AllowManipulateProperty); }
			set { SetValue(AllowManipulateProperty, value); }
		}

		public event EventHandler ManipulationStarting;

		private bool isMouseOver;
		private bool IsMouseOver
		{
			get { return isMouseOver; }
			set { isMouseOver = value; UpdateView(); }
		}

		private bool isFocused;
		private bool IsFocused
		{
			get { return isFocused; }
			set { isFocused = value; UpdateView(); }
		}

		private Point startPoint;
		private Point startLocalPoint;
		private double currentWidth;
		private double currentHeight;
		private CompositeTransform currentCompositeTransform;
		private Primitives.ManipulatorHandleType manipulatorHandleType;

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			Focus();

			if (!AllowManipulate)
				return;

			CaptureMouse();
			currentHeight = ActualHeight;
			currentWidth = ActualWidth;
			currentCompositeTransform = RenderTransform as CompositeTransform ?? new CompositeTransform();
			startPoint = e.GetPosition(null);
			startLocalPoint = e.GetPosition(this);
			var ri = ControlHelper.GetParent<Primitives.ManipulatorHandle>(e.OriginalSource as FrameworkElement);
			if (ri != null)
			{
				manipulatorHandleType = ri.Type;

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.W || manipulatorHandleType == Primitives.ManipulatorHandleType.E)
				{
					ManipulationMode = ManipulationMode.HorizontalResize;
				}
				else if (manipulatorHandleType == Primitives.ManipulatorHandleType.N || manipulatorHandleType == Primitives.ManipulatorHandleType.S)
				{
					ManipulationMode = ManipulationMode.VerticalResize;
				}
				else if (manipulatorHandleType == Primitives.ManipulatorHandleType.ES || manipulatorHandleType == Primitives.ManipulatorHandleType.NE
					|| manipulatorHandleType == Primitives.ManipulatorHandleType.SW || manipulatorHandleType == Primitives.ManipulatorHandleType.WN)
				{
					ManipulationMode = ManipulationMode.BothResize;
				}
				else if (manipulatorHandleType == Primitives.ManipulatorHandleType.Rotate)
				{
					ManipulationMode = ManipulationMode.Rotate;
				}
				else
				{
					ManipulationMode = ManipulationMode.Translate;
				}
			}
			else if (DragHandlerMode == DragHandlerMode.UseWholeControl)
			{
				ManipulationMode = ManipulationMode.Translate;
			}

			base.OnMouseLeftButtonDown(e);
			e.Handled = true;
		}

		protected void OnManipulationStarting()
		{
			if (ManipulationStarting != null)
			{
				ManipulationStarting(this, new EventArgs());
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var point = e.GetPosition(null);
			var parent = (FrameworkElement)VisualTreeHelper.GetParent(this);

			var relativeToParentPoint = e.GetPosition(parent);

			if (ManipulationMode != ManipulationMode.None)
			{
				OnManipulationStarting();
			}

			if (ManipulationMode == ManipulationMode.Translate)
			{
				var translateXTemp = (point.X - startPoint.X) + currentCompositeTransform.TranslateX;

				if (!ClipTranslationToParent)
				{
					TranslateX = translateXTemp;
				}
				else if (startLocalPoint.X + translateXTemp <= 0)
					TranslateX = -startLocalPoint.X;
				else if (relativeToParentPoint.X > parent.ActualWidth)
					TranslateX = parent.ActualWidth - startLocalPoint.X;
				else
					TranslateX = translateXTemp;

				var translateYTemp = (point.Y - startPoint.Y) + currentCompositeTransform.TranslateY;

				if (!ClipTranslationToParent)
				{
					TranslateY = translateYTemp;
				}
				else if (startLocalPoint.Y + translateYTemp <= 0)
					TranslateY = -startLocalPoint.Y;
				else if (relativeToParentPoint.Y > parent.ActualHeight)
					TranslateY = parent.ActualHeight - startLocalPoint.Y;
				else
					TranslateY = translateYTemp;

				Rotation = currentCompositeTransform.Rotation;
			}
			else if (ManipulationMode == ManipulationMode.Rotate)
			{
				var center = new Point(ActualWidth / 2, ActualHeight / 2);
				var localPoint = new Point(startLocalPoint.X + (point.X - startPoint.X),
										   startLocalPoint.Y + (point.Y - startPoint.Y));

				var rotation = (180 / Math.PI) * Math.Atan((localPoint.Y - center.Y) / (localPoint.X - center.X));

				if (localPoint.X < center.X)
				{
					rotation = 180 + rotation;
				}

				TranslateX = currentCompositeTransform.TranslateX;
				TranslateY = currentCompositeTransform.TranslateY;
				Rotation = currentCompositeTransform.Rotation + rotation + 90;
			}
			else if (ManipulationMode != ManipulationMode.None)
			{
				if (manipulatorHandleType == Primitives.ManipulatorHandleType.W)
				{
					TranslateX = (point.X - startPoint.X) + currentCompositeTransform.TranslateX;
					TranslateY = currentCompositeTransform.TranslateY;
					Rotation = currentCompositeTransform.Rotation;

					var width = currentWidth - (point.X - startPoint.X);
					Width = width < MinWidth ? MinWidth : width;
				}
				else if (manipulatorHandleType == Primitives.ManipulatorHandleType.E)
				{
					var width = currentWidth + (point.X - startPoint.X);
					Width = width < MinWidth ? MinWidth : width;
				}

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.N)
				{
					TranslateX = currentCompositeTransform.TranslateX;
					TranslateY = (point.Y - startPoint.Y) + currentCompositeTransform.TranslateY;
					Rotation = currentCompositeTransform.Rotation;

					var height = currentHeight - (point.Y - startPoint.Y);
					Height = height < MinHeight ? MinHeight : height;
				}
				else if (manipulatorHandleType == Primitives.ManipulatorHandleType.S)
				{
					var height = currentHeight + (point.Y - startPoint.Y);
					Height = height < MinHeight ? MinHeight : height;
				}

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.ES || manipulatorHandleType == Primitives.ManipulatorHandleType.SW)
				{
					var height = currentHeight + (point.Y - startPoint.Y);
					Height = height < MinHeight ? MinHeight : height;
				}
				if (manipulatorHandleType == Primitives.ManipulatorHandleType.ES || manipulatorHandleType == Primitives.ManipulatorHandleType.NE)
				{
					var width = currentWidth + (point.X - startPoint.X);
					Width = width < MinWidth ? MinWidth : width;
				}

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.WN)
				{
					TranslateX = (point.X - startPoint.X) + currentCompositeTransform.TranslateX;
					TranslateY = (point.Y - startPoint.Y) + currentCompositeTransform.TranslateY;
					Rotation = currentCompositeTransform.Rotation;

					var height = currentHeight - (point.Y - startPoint.Y);
					var width = currentWidth - (point.X - startPoint.X);
					Height = height < MinHeight ? MinHeight : height;
					Width = width < MinWidth ? MinWidth : width;
				}

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.NE)
				{
					TranslateX = currentCompositeTransform.TranslateX;
					TranslateY = (point.Y - startPoint.Y) + currentCompositeTransform.TranslateY;
					Rotation = currentCompositeTransform.Rotation;

					var height = currentHeight - (point.Y - startPoint.Y);
					Height = height < MinHeight ? MinHeight : height;
				}

				if (manipulatorHandleType == Primitives.ManipulatorHandleType.SW)
				{
					TranslateX = (point.X - startPoint.X) + currentCompositeTransform.TranslateX;
					TranslateY = currentCompositeTransform.TranslateY;
					Rotation = currentCompositeTransform.Rotation;

					var width = currentWidth - (point.X - startPoint.X);
					Width = width < MinWidth ? MinWidth : width;
				}
			}
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			ReleaseMouseCapture();
			ManipulationMode = ManipulationMode.None;
			base.OnMouseLeftButtonUp(e);
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			IsMouseOver = true;
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			IsMouseOver = false;
			base.OnMouseLeave(e);
		}

		public override void OnApplyTemplate()
		{
			deleteButton = GetTemplateChild(DeleteButtonElementName) as Button;
			deleteButton.Click += (s, e) => { (this.Parent as Panel).Children.Remove(this); };

			base.OnApplyTemplate();
			UpdateView();
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			IsFocused = true;
			IsSelected = true;
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			IsFocused = false;
			if (GetBindingExpression(IsSelectedProperty) == null)
			{
				IsSelected = false;
			}

		}

		void UpdatePosition()
		{
			RenderTransform = new CompositeTransform() { TranslateX = TranslateX, TranslateY = TranslateY, Rotation = Rotation };
		}

		void UpdateView()
		{
			if (IsMouseOver)
			{
				VisualStateManager.GoToState(this, EditMouseOverStateName, true);
			}
			else
			{
				VisualStateManager.GoToState(this, NormalStateName, true);
			}

			if (!AllowManipulate)
			{
				VisualStateManager.GoToState(this, UnfocusedStateName, true);
			}
			else if (IsSelected && !IsFocused)
			{
				VisualStateManager.GoToState(this, SelectedUnfocusedStateName, true);
			}
			else if (IsSelected)
			{
				VisualStateManager.GoToState(this, SelectedStateName, true);
			}
			else
			{
				VisualStateManager.GoToState(this, UnselectedStateName, true);
			}

			if (IsFocused)
			{
				VisualStateManager.GoToState(this, FocusedStateName, true);
			}
			else
			{
				VisualStateManager.GoToState(this, UnfocusedStateName, true);
			}

			if (AllowResize)
			{
				VisualStateManager.GoToState(this, AllowResizeStateName, true);
			}
			else
			{
				VisualStateManager.GoToState(this, DenyResizeStateName, true);
			}
		}

		private ManipulationMode ManipulationMode { get; set; }

		private Button deleteButton;

		private const string EditMouseOverStateName = "MouseOver";
		private const string NormalStateName = "Normal";
		private const string SelectedStateName = "Selected";
		private const string UnselectedStateName = "Unselected";
		private const string SelectedUnfocusedStateName = "SelectedUnfocused";
		private const string FocusedStateName = "Focused";
		private const string UnfocusedStateName = "Unfocused";
		private const string AllowResizeStateName = "AllowResize";
		private const string DenyResizeStateName = "DenyResize";

		private const string DeleteButtonElementName = "DeleteButton";
	}

	enum ManipulationMode
	{
		None,
		HorizontalResize,
		VerticalResize,
		BothResize,
		Translate,
		Rotate
	}
}
