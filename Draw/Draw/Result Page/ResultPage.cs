using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Draw.Result_Page
{
	public class ResultPage : ContentPage
	{
		public ResultPage ()
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = "Hello Page" }
				}
			};
		}
	}
}
