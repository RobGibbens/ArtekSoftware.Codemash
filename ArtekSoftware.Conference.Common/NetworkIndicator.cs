//using System;
//using MonoTouch.UIKit;

//namespace ArtekSoftware.Codemash
//{
//    public class NetworkIndicator : IDisposable
//    {
//        public NetworkIndicator ()
//        {
//            if (UIApplication.SharedApplication !=  null)
//            {
//                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
//            }
//        }

//        public void Dispose ()
//        {
//            if (UIApplication.SharedApplication !=  null)
//            {
//                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
//                GC.SuppressFinalize (this);
//            }
//        }
//    }
//}