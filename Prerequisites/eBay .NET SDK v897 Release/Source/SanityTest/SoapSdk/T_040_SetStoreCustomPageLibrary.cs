#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

#region Namespaces
using System;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
#endregion

namespace AllTestsSuite.T_100_StoreTestsSuite
{
	[TestFixture]
	public class T_040_SetStoreCustomPageLibrary : SOAPTestBase
	{
		[Test]
		public void SetStoreCustomPage()
		{
			if( TestData.StoreCustomPages == null )
			return;
			StoreCustomPageTypeCollection pages = TestData.StoreCustomPages;
			if( pages == null || pages.Count == 0 )
			return;
			SetStoreCustomPageCall api = new SetStoreCustomPageCall(this.apiContext);
			// Set input parameter.
			api.CustomPage = pages[0];
			// Make API call.
			api.Execute();
			
		}
	}
}