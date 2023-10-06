using Microsoft.Maui.Appium;
using NUnit.Framework;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue4734 : _IssuesUITest
	{
		public Issue4734(TestDevice device)
		: base(device)
		{ }

		public override string Issue => "Gestures in Label Spans not working";

		[Test]
		public void Issue4734Test()
		{
			if (Device == TestDevice.Windows)
			{
				Assert.Ignore("This test is failing, likely due to product issue");
			}
			else
			{
				App.WaitForElement("WaitForStubControl");

				var label = App.WaitForElement("TargetSpanControl");
				var location = label[0].Rect;
				var lineHeight = location.Height / 5;
				var lineCenterOffset = lineHeight / 2;
				var y = location.Y;

				App.TapCoordinates(location.X + 10, y + lineCenterOffset);
				App.TapCoordinates(location.X + 10, y + (lineHeight * 2) + lineCenterOffset);
				App.TapCoordinates(location.X + 10, y + (lineHeight * 4) + lineCenterOffset);

				var textAfterTap = App.Query("TapResultControl").First().Text;
				Assert.False(string.IsNullOrEmpty(textAfterTap));
			}
		}
	}
}