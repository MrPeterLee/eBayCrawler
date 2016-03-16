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
using System.Xml;
using System.IO;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
#endregion
namespace AllTestsSuite.T_120_SellingManagerTestsSuite
{
	/// <summary>
	/// Summary description for T_070_SetSellingManagerFeedbackOptions.
	/// </summary>
	[TestFixture]
	public class T_070_SetSellingManagerFeedbackOptions : SOAPTestBase
	{
		[Test]
		public void SetSellingManagerFeedbackOptions()
		{	
			SetSellingManagerFeedbackOptionsCall api = new SetSellingManagerFeedbackOptionsCall(apiContext);
			api.AutomatedLeaveFeedbackEvent=AutomatedLeaveFeedbackEventCodeType.PaymentReceived;
			StringCollection storedComments =new StringCollection(new string[]{"Paid quickly! Thank you for choosing to buy from Sales-a-lot. "});
			api.StoredCommentList=storedComments;
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
		}
	}
}
