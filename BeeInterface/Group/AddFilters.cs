using BeeCore.Algorithm;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Size = OpenCvSharp.Size;
using BeeCore.Algorithm;
namespace BeeUi.Unit
{
    public partial class AddFilters : UserControl
    {
//        List<FilterItem> filterCatalog = new List<FilterItem>()
//{
//    new FilterItem("Gaussian Blur",   Filters. GaussianBlur(new Size(5,5),1.5),Properties.Resources.icoGaussian),
//    new FilterItem("Median Blur",     Filters. MedianBlur(5),Properties.Resources.icoGaussian),
//    new FilterItem("Bilateral Filter", Filters.Bilateral(9,75,75),Properties.Resources.icoGaussian),
//    new FilterItem("Sharpen (Unsharp)",Filters.Sharpen(),Properties.Resources.icoGaussian),

//    new FilterItem("CLAHE",            Filters.Clahe(2.0,new Size(8,8)),Properties.Resources.icoGaussian),
//    new FilterItem ("Hist Eq",          Filters.HistEq(),Properties.Resources.icoGaussian),
//    new FilterItem ("Gamma 1.5",        Filters.Gamma(1.5),Properties.Resources.icoGaussian),

//    new FilterItem ("Canny Edge",       Filters.Canny(50,150),Properties.Resources.icoGaussian),
//    new FilterItem ("Sobel X",          Filters.Sobel(true),Properties.Resources.icoGaussian),
//    new FilterItem ("Sobel Y",          Filters.Sobel(false),Properties.Resources.icoGaussian),
//    new FilterItem ("Laplacian Edge",   Filters.Laplacian(3),Properties.Resources.icoGaussian),

//    new FilterItem ("Adapt Mean Th",    Filters.AdaptiveThresh(AdaptiveThresholdTypes.MeanC),Properties.Resources.icoGaussian),
//    new FilterItem ("Adapt Gauss Th",   Filters.AdaptiveThresh(AdaptiveThresholdTypes.GaussianC),Properties.Resources.icoGaussian),
//    new FilterItem ("Otsu Th",          Filters.OtsuThresh(),Properties.Resources.icoGaussian),

//    new FilterItem ("Erode 3×3",        Filters.ErodeDilate(true,new Size(3,3)),Properties.Resources.icoGaussian),
//    new FilterItem ("Dilate 3×3",       Filters.ErodeDilate(false,new Size(3,3)),Properties.Resources.icoGaussian),
//    new FilterItem ("Morph Open 3×3",   Filters.Morph(MorphTypes.Open,new Size(3,3)),Properties.Resources.icoGaussian),
//    new FilterItem ("Morph Close 3×3",  Filters.Morph(MorphTypes.Close,new Size(3,3)),Properties.Resources.icoGaussian),
//    new FilterItem ("Morph Grad 3×3",   Filters.Morph(MorphTypes.Gradient,new Size(3,3)),Properties.Resources.icoGaussian),

//    new FilterItem ("Gray‑World WB",    Filters.WhiteBalanceGrayWorld(),Properties.Resources.icoGaussian),
//    new FilterItem ("Simple WB",        Filters.WhiteBalanceSimple(),Properties.Resources.icoGaussian),
//    new FilterItem ("Keep Blue",        Filters.KeepColorRange(100,140),Properties.Resources.icoGaussian),
//    new FilterItem ("Resize 50%",       Filters.Resize(0.5,0.5),Properties.Resources.icoGaussian),
//    new FilterItem ("Convert Gray",     Filters.ConvertGray(),Properties.Resources.icoGaussian)

        public AddFilters()
        {
            InitializeComponent();
        }

        private void AddFilters_Load(object sender, EventArgs e)
        {
          //  listAddFilter.da
        }
    }
}
