#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

namespace ArtekSoftware.Conference.Common.Http
{
	public class HttpWebRequestMethod {
		private string method;
		private static readonly string SGET = "GET";
		private static readonly string SPOST = "POST";
    private static readonly string SPUT = "PUT";
		public static readonly HttpWebRequestMethod GET = new HttpWebRequestMethod(SGET);
		public static readonly HttpWebRequestMethod POST = new HttpWebRequestMethod(SPOST);
    public static readonly HttpWebRequestMethod PUT = new HttpWebRequestMethod(SPUT);

		private HttpWebRequestMethod(string m) {
			method = m;
		}

		public override string ToString() {
			return method;
		}

		public static HttpWebRequestMethod Parse(string httpMethod) {
      if (string.Compare(httpMethod, SGET, StringComparison.OrdinalIgnoreCase) == 0)
			{
			  return GET;
			}
      else if (string.Compare(httpMethod, SPOST, StringComparison.OrdinalIgnoreCase) == 0)
      {
        return POST;
      }
      else if (string.Compare(httpMethod, SPUT, StringComparison.OrdinalIgnoreCase) == 0)
      {
        return PUT;
      }

		  return POST;
		}
	}
}