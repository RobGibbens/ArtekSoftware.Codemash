using System.Drawing;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class HLabel : UILabel
	{ 
		public enum VerticalAlignments
		{ 
			Middle = 0,    //the default (what standard UILabels do) 
			Top,
			Bottom
		} 

        #region VerticalAlignment 
		private VerticalAlignments m_eVerticalAlignment;

		public VerticalAlignments VerticalAlignment { 
			get { return m_eVerticalAlignment; } 
			set { 
				if (m_eVerticalAlignment != value) { 
					m_eVerticalAlignment = value; 
					SetNeedsDisplay ();    //redraw if value changed 
				} 
			} 
		} 
        #endregion 
        #region construction 
		public HLabel ()
		{
		}

		public HLabel (RectangleF rF) : base(rF)
		{
		} 
		//add other constructors if needed 
        #endregion 
        #region overrides (DrawText, TextRectForBounds) 
		//normally it uses full size of the control - we change this 
		public override void DrawText (RectangleF rect)
		{ 
			RectangleF rErg = TextRectForBounds (rect, Lines); 
			base.DrawText (rErg); 
		} 
		//calculate the rect for text output - depending on VerticalAlignment 
		public override RectangleF TextRectForBounds (RectangleF rBounds, int nNumberOfLines)
		{
			RectangleF rCalculated = base.TextRectForBounds (rBounds, nNumberOfLines); 
			if (m_eVerticalAlignment != VerticalAlignments.Top) {    //no special handling for top
				if (m_eVerticalAlignment == VerticalAlignments.Bottom) { 
					rBounds.Y += rBounds.Height - rCalculated.Height;    //move down by difference
				} else {    //middle == nothing set == somenthing strange ==> act like standard UILabel
					rBounds.Y += (rBounds.Height - rCalculated.Height) / 2; 
				} 
			} 
			rBounds.Height = rCalculated.Height;    //always the calculated height 
			return (rBounds); 
		} 
        #endregion 
	}
}

