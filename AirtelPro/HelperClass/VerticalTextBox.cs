using System;using CardPrintingApplication;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DG_Tool
{
	public class VerticalTextBox : Control
	{
		public VerticalTextBox()
		{
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

			textBox = new CustomTextBox();
			textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.Controls.Add(textBox);

			//Init
			Text = "";
			BackColor = SystemColors.Window;
			LeftRightPadding = 10;
			BorderColor = Color.Gray;
			BorderRadius = 10;
			BorderWidth = 2;
		}

		private CustomTextBox textBox;
		public override string Text
		{
			get { return textBox.Text; }
			set { textBox.Text = value; }
		}
		public override Color ForeColor { get { return textBox.ForeColor; } set { textBox.ForeColor = value; } }
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				textBox.BackColor = base.BackColor = value;
				Invalidate();
			}
		}
		public HorizontalAlignment TextAlign { get { return textBox.TextAlign; } set { textBox.TextAlign = value; } }

		private int leftRightPadding;
		public uint LeftRightPadding
		{
			get { return Convert.ToUInt32(leftRightPadding); }
			set
			{
				leftRightPadding = Convert.ToInt32(value);
				textBox.Location = new Point(leftRightPadding, textBox.Location.Y);
			}
		}

		private Color borderColor;
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		private int borderRadius;
		public int BorderRadius
		{
			get { return borderRadius; }
			set { borderRadius = value; Invalidate(); }
		}

		private int borderWidth;
		public int BorderWidth
		{
			get { return borderWidth; }
			set { borderWidth = value; Invalidate(); }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			Rectangle rect = ClientRectangle;
			rect.Inflate(-BorderWidth / 2, -BorderWidth / 2);

			using (GraphicsPath path = GetRoundedRectanglePath(rect, BorderRadius))
			{
				using (Pen pen = new Pen(BorderColor, BorderWidth))
				{
					e.Graphics.DrawPath(pen, path);
				}
			}
		}

		private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
		{
			int diameter = radius * 2;
			Size size = new Size(diameter, diameter);
			Rectangle arc = new Rectangle(rect.Location, size);
			GraphicsPath path = new GraphicsPath();

			// top left arc
			if (radius > 0)
			{
				path.AddArc(arc, 180, 90);
			}
			else
			{
				path.AddLine(rect.Left, rect.Top, rect.Left, rect.Top);
			}

			// top right arc
			arc.X = rect.Right - diameter;
			if (radius > 0)
			{
				path.AddArc(arc, 270, 90);
			}
			else
			{
				path.AddLine(rect.Right, rect.Top, rect.Right, rect.Top);
			}

			// bottom right arc
			arc.Y = rect.Bottom - diameter;
			if (radius > 0)
			{
				path.AddArc(arc, 0, 90);
			}
			else
			{
				path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);
			}

			// bottom left arc
			arc.X = rect.Left;
			if (radius > 0)
			{
				path.AddArc(arc, 90, 90);
			}
			else
			{
				path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Bottom);
			}

			path.CloseFigure();
			return path;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			int textTop = (this.Height / 2) - (textBox.ClientSize.Height / 2);
			textBox.Location = new Point(leftRightPadding, textTop);
			textBox.Width = this.Width - (leftRightPadding * 2) - 2;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			if (!textBox.Focused)
			{
				textBox.Focus();
			}
		}

		public class CustomTextBox : TextBox
		{
			public CustomTextBox()
			{
				this.BorderStyle = BorderStyle.None;
				this.ReadOnly = true;
			}

			protected override void OnFontChanged(EventArgs e)
			{
				base.OnFontChanged(e);

				int textTop = (this.Parent.Height / 2) - ((this.ClientSize.Height + 2) / 2);
				this.Location = new Point(this.Location.X, textTop);
			}

			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				if (e.KeyChar == (char)Keys.Return)
				{
					e.Handled = true;
				}
				base.OnKeyPress(e);
			}
		}
	}
}
