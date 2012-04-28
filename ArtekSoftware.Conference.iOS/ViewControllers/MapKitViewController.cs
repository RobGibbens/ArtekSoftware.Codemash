using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;  // required
using MonoTouch.CoreLocation;
using System.Linq;  // required

namespace ArtekSoftware.Codemash
{
	public class MapKitViewController : UIViewController
	{
		private MKMapView mapView;
		public UILabel labelDistance;
		public CLLocationCoordinate2D ConferenceLocation;
		private CLLocationManager locationManager;

		private IMapFlipViewController _mfvc;
		private RectangleF _segmentedControlFrame;
		
		public MapKitViewController (IMapFlipViewController mfvc, RectangleF segmentedControlFrame):base()
		{
			_mfvc = mfvc;
			_segmentedControlFrame = segmentedControlFrame;
			ConferenceLocation = new CLLocationCoordinate2D(41.38651,-82.640533);
		}

		public void SetLocation(CLLocationCoordinate2D toLocation)
		{
			Console.WriteLine("SetLocation to {0},{1}", toLocation.Latitude, toLocation.Longitude);
			if (toLocation.Equals(new CLLocationCoordinate2D(0,0)))
			{
				toLocation = locationManager.Location.Coordinate;
			}
			mapView.CenterCoordinate = toLocation;
		}

		public override void ViewDidLoad ()
        {
			// no XIB !
			mapView = new MKMapView()
			{
				ShowsUserLocation = true
			};
			
			var distanceWidth = this.View.Frame.Width;
			var distanceHeight = 44;
			var distanceX = this.View.Frame.X;
			var distanceY = this.View.Frame.Y;
			
			labelDistance = new UILabel()
			{
				Frame = new RectangleF (distanceX, distanceY, distanceWidth, distanceHeight),
				Lines = 2,
				BackgroundColor = UIColor.Black,
				TextColor = UIColor.White
			};

			var segmentedControl = new UISegmentedControl(_segmentedControlFrame);			
			
			segmentedControl.InsertSegment("Map", 0, false);
			segmentedControl.InsertSegment("Satellite", 1, false);
			segmentedControl.InsertSegment("Hybrid", 2, false);
			segmentedControl.SelectedSegment = 0;
			segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			segmentedControl.TintColor = UIColor.DarkGray;

			segmentedControl.ValueChanged += delegate {
				if (segmentedControl.SelectedSegment == 0)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Standard;
				else if (segmentedControl.SelectedSegment == 1)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Satellite;
				else if (segmentedControl.SelectedSegment == 2)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Hybrid;
			};

			mapView.Delegate = new MapViewDelegate(this);  // RegionChanged, GetViewForAnnotation 

			// Set the web view to fit the width of the app.
            mapView.SizeToFit();

            // Reposition and resize the receiver

            mapView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);

			//mapView.SetCenterCoordinate(confLoc, true); 	
			MKCoordinateSpan span = new MKCoordinateSpan(0.2,0.2);
			MKCoordinateRegion region = new MKCoordinateRegion(ConferenceLocation,span);
			mapView.SetRegion(region, true);

			ConferenceAnnotation a = new ConferenceAnnotation(ConferenceLocation
                                , "Kalahari Resort"
                                , "7000 Kalahari Drive, Sandusky, Ohio 44870‎"
                              );
			Console.WriteLine("This adds a custom placemark for the Conference Venue");
			mapView.AddAnnotationObject(a); 

			locationManager = new CLLocationManager();
			locationManager.Delegate = new LocationManagerDelegate(mapView, this);
			locationManager.StartUpdatingLocation();


            // Add the table view as a subview
			this.View.InsertSubview(mapView, 0);
            //this.View.AddSubview(mapView);
			//this.View.AddSubview(labelDistance);
			this.View.AddSubview(segmentedControl);

			// Add the 'info' button to flip
			Console.WriteLine("make flip button");
			var flipButton = UIButton.FromType(UIButtonType.InfoLight);
			var flipButtonWidth = 20;
			var flipButtonHeight = 20;
			var flipButtonY = this.View.Frame.Top + 17;
			var flipButtonX = this.View.Frame.Right - 30;
			flipButton.Frame = new RectangleF(flipButtonX, flipButtonY, flipButtonWidth, flipButtonHeight);
			flipButton.Title (UIControlState.Normal);
			flipButton.TouchDown += delegate {
				_mfvc.Flip();
			};
			Console.WriteLine("flipbutton ready to add");
			//this.View.AddSubview(flipButton);
			
            base.ViewDidLoad ();
			
		}	
		
	
//		
//		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
//		{
//			var segmentedControlWidth = 282;
//			var segmentedControlHeight = 30;
//			var segmentedControlX = (this.View.Frame.Width - segmentedControlWidth) / 2;
//			var segmentedControlY = (this.View.Frame.Height - 120);
//			Console.WriteLine("DR FRAME HEIGHT " + this.View.Frame.Height);
//			Console.WriteLine("DR BOUNDS HEIGHT" + this.View.Bounds.Height);
//			
//			var segmentedControl = (UISegmentedControl)this.View.Subviews[1];
//			Console.WriteLine("segmentedControlX = " + segmentedControlX);
//			Console.WriteLine("segmentedControlY = " + segmentedControlY);
//			
//			this.View.Subviews[1].Frame.X = 20; //segmentedControlX;
//			this.View.Subviews[1].Frame.Y = 100; //segmentedControlY;
//			this.View.Subviews[1].Frame.Height = segmentedControlHeight;
//			this.View.Subviews[1].Frame.Width = segmentedControlWidth;
//			this.View.Subviews[1].SetNeedsDisplay();
//			this.View.Subviews[1].SetNeedsLayout();
//			this.View.Subviews[1].LayoutIfNeeded();
//			
//			var mapView = (MKMapView)this.View.Subviews[0];
//			mapView.Frame.X = 0;
//			mapView.Frame.Y = 0;
//			mapView.Frame.Height = this.View.Frame.Height;
//			mapView.Frame.Width = this.View.Frame.Width;
//			
//			this.View.SetNeedsDisplay();
//			this.View.SizeToFit();
//			this.View.SetNeedsLayout();
//			this.View.LayoutIfNeeded();
//			
//			Console.WriteLine("MKVC.DidRotate");
//			
//			//base.DidRotate (fromInterfaceOrientation);
//		}
//		
		public class MapViewDelegate : MKMapViewDelegate
		{
			private MapKitViewController _mvc;
			public MapViewDelegate (MapKitViewController controller):base()
			{
				_mvc = controller;
			}
			/// <summary>
			/// When user moves the map, update lat,long text in label
			/// </summary>
			public override void RegionChanged (MKMapView mapView, bool animated)
			{
				Console.WriteLine("Region did change");
			}
			/// <summary>
			/// Seems to work in the Simulator now
			/// </summary>
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				Console.WriteLine("attempt to get view for MKAnnotation "+annotation);
				try
				{
					var anv = mapView.DequeueReusableAnnotation("thislocation");
					if (anv == null)
					{
						Console.WriteLine("creating new MKAnnotationView");
						var pinanv = new MKPinAnnotationView(annotation, "thislocation");
						pinanv.AnimatesDrop = true;
						pinanv.PinColor = MKPinAnnotationColor.Green;
						pinanv.CanShowCallout = true;
						anv = pinanv;
					}
					else 
					{
						anv.Annotation = annotation;
					}
					return anv;
				} catch (Exception ex)
				{
					Console.WriteLine("GetViewForAnnotation Exception " + ex);
					return null;
				}
			}
		}
		/// <summary>
		/// MonoTouch definition seemed to work without too much trouble
		/// </summary>
		private class LocationManagerDelegate: CLLocationManagerDelegate
		{
			private MKMapView _mapview;
			private MapKitViewController _appd;
			private int _count = 0;
			public LocationManagerDelegate(MKMapView mapview, MapKitViewController mapvc):base()
			{
				_mapview = mapview;
				_appd=mapvc;
				Console.WriteLine("Delegate created");
			}
			/// <summary>
			/// Whenever the GPS sends a new location, update text in label
			/// and increment the 'count' of updates AND reset the map to that location 
			/// </summary>
			public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
			{
				//MKCoordinateSpan span = new MKCoordinateSpan(0.2,0.2);
				//MKCoordinateRegion region = new MKCoordinateRegion(newLocation.Coordinate,span);
				//_appd.mylocation = newLocation;
				//_mapview.SetRegion(region, true);
				double distanceToConference = MapHelper.Distance (new Coordinate(_appd.ConferenceLocation), new Coordinate(newLocation.Coordinate), UnitsOfLength.Miles);

				_appd.labelDistance.Text = String.Format("{0} miles from CodeMash!", Math.Round(distanceToConference,0));
				Console.WriteLine("Distance: {0}", distanceToConference);

				//Console.WriteLine("Location updated");
			}
			public override void Failed (CLLocationManager manager, NSError error)
			{
#if !SIMULATOR
				//_appd.labelInfo.Text = "Failed to find location";
				Console.WriteLine("Failed to find location");
				base.Failed (manager, error);
#endif
			}
		}
	}


	/// <summary>
	/// MKAnnotation is an abstract class (in Objective C I think it's a protocol).
	/// Therefore we must create our own implementation of it. Since all the properties
	/// are read-only, we have to pass them in via a constructor.
	/// </summary>
	public class ConferenceAnnotation : MKAnnotation
	{
		private CLLocationCoordinate2D _coordinate;
		private string _title, _subtitle;
		public override CLLocationCoordinate2D Coordinate {
			get {
				return _coordinate;
			}
			set {
				_coordinate = value;
			}
		}
		public override string Title {
			get {
				return _title;
			}
		}
		public override string Subtitle {
			get {
				return _subtitle;
			}
		}
		/// <summary>
		/// custom constructor
		/// </summary>
		public ConferenceAnnotation (CLLocationCoordinate2D coord, string t, string s) : base()
		{
			_coordinate=coord;
		 	_title=t; 
			_subtitle=s;
		}
	}
}