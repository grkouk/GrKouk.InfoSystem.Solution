using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace GrKouk.InfoSystem
{
   public  class DataGridCustomStyle : DataGridStyle
    {
        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.LightGray;
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.Blue;
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.White;
        }
    }
}
