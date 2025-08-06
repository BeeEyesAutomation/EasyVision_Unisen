#include "ColorArea.h"
using namespace CvPlus;
string _toString(System::String^ STR)
{
    char cStr[1000] = { 0 };
    sprintf(cStr, "%s", STR);
    std::string s(cStr);
    return s;
}
void ColorArea::LoadTemp(System::String^ content)
{
    if (content == nullptr)return;
    std::string contentSTD = _toString(content);
   
    listColor.clear();
    for each (System::String ^ line in content->Split('\n'))
    {
        if (line->Trim() == "") continue;
        int R = 0, G = 0, B = 0;
        int i = 0;
        for each (System::String ^ color in line->Split(','))
        {
            switch (i)
            {
            case 0:R = Convert::ToInt32(color);
                break;
            case 1:G = Convert::ToInt32(color);
                break;
            case 2:B = Convert::ToInt32(color);
                break;
          
            }
            i++;
          
           
        }
        listColor.push_back(Scalar(R,G,B));
    }
}
System::String^ ColorArea::SaveTemp( )
{
    System::String^ list="";
    for each (Scalar scalar in listColor)
    {
        list += scalar.val[0]+"," + scalar.val[1] + "," + scalar.val[2]+"\n";
       
    }
    return list;
}
Mat matMask;
 void ColorArea::GetMask(Mat mat, int iAreaPixel)
{


     matMask = Mat(mat.rows, mat.cols, CV_8UC1, Scalar(0, 0, 0));
    Mat matHSV = Mat();
    if (mat.type() != CV_8UC3)
        return;
    int i = 0;
    if(StyleColor!=0)
        cvtColor(mat.clone(), matHSV, COLOR_BGR2RGB);
    else
        cvtColor(mat.clone(), matHSV, COLOR_BGR2HSV);
    //cv::imwrite("color2.png", matHSV);
    //cv::blur(matHSV, matHSV, cv::Size(3, 3));
    for each (Scalar scalar in listColor)
    {
        bool IsWhite = !ColorArea::GetLimitColor(scalar, iAreaPixel);

        Mat matInrange = Mat();
        Mat matGroup = Mat();

       
            
           /* if (StyleColor==2)
              cv::threshold(matHSV, matInrange, lower.val[0] + iAreaPixel,255, THRESH_BINARY_INV);
            else if (StyleColor == 1)
            cv::threshold(matHSV, matInrange, lower.val[0] - iAreaPixel, 255,THRESH_BINARY);
            else*/
            inRange(matHSV, lower, upper, matInrange);
            cv::erode(matInrange, matInrange, Mat(), cv::Point(-1, -1), 1, 1, 1);
            cv::dilate(matInrange, matInrange,Mat(), cv::Point(-1, -1),1, 1, 1);
          
        bitwise_or(matInrange, matMask, matGroup);

        matMask = matGroup.clone();

    }
    


}
 System::String^ ColorArea:: GetColor(System::IntPtr buffer, int width, int height, int Step, int image_type, int x, int y)
{
     try
     { // Chuyển IntPtr → uchar*
         uchar* uc = static_cast<uchar*>(buffer.ToPointer());

         // Tạo cv::Mat từ dữ liệu đó
         cv::Mat raw(height, width, image_type, uc, Step);
         if(raw.empty())return "";
         System::String^ sWrite = "";
        
         Mat matProccess=Mat();
         if (x > raw.cols || y > raw.rows)
             return   "";
       
         if (StyleColor == 0)
             cvtColor(raw, matProccess, COLOR_BGR2HSV);
         else
             cvtColor(raw, matProccess, COLOR_BGR2RGB);
        
         Mat mat = matProccess(cv::Rect(x - 1, y - 1, 2, 2));
         H = 0, S = 0, V = 0;
         for (int k = 0; k < mat.rows; k++)
         {
             for (int j = 0; j < mat.cols; j++)
             {
                 Vec3b color = mat.at<Vec3b>(k, j);
                 H += color[0];
                 S += color[1];
                 V += color[2];
             }
         }
         H = (int)H / 4;
         S = (int)S / 4;
         V = (int)V / 4;
        
         if (StyleColor == 0)
             cvtColor(raw, matProccess, COLOR_BGR2RGB);
         else
             return   H + "," + S + "," + V;
          mat = matProccess(cv::Rect(x - 1, y - 1, 2, 2));
        float R = 0, G = 0, B = 0;
         for (int k = 0; k < mat.rows; k++)
         {
             for (int j = 0; j < mat.cols; j++)
             {
                 Vec3b color = mat.at<Vec3b>(k, j);
                 R += color[0];
                 G += color[1];
                 B += color[2];

             }
         }
         R = (int)R / 4;
         G= (int)G / 4;
         B = (int)B / 4;
         return  R + "," + G + "," + B;
     }
     catch (...)
     {

     }
}
 void  ColorArea::AddColor()
 {
     
     listColor.push_back(Scalar(H, S, V));
 }
Mat RotateImge(Mat raw, RotatedRect rot)
{
    Mat matRs, matR = getRotationMatrix2D(rot.center, rot.angle, 1);

    float fTranslationX = (rot.size.width - 1) / 2.0f - rot.center.x;
    float fTranslationY = (rot.size.height - 1) / 2.0f - rot.center.y;
    matR.at<double>(0, 2) += fTranslationX;
    matR.at<double>(1, 2) += fTranslationY;
    warpAffine(raw, matRs, matR, rot.size, INTER_LINEAR, BORDER_CONSTANT);
    return matRs;
}
uchar* MatToBytes2(cv::Mat image)
{
    int image_size = image.total() * image.elemSize();
    uchar* image_uchar = new uchar[image_size];
    //image_uchar is a class data member of uchar*
    std::memcpy(image_uchar, image.data, image_size * sizeof(uchar));
    return image_uchar;
}

Mat equalize(const Mat& img)
{
	Mat res(img.size(), img.type());
	Mat imgB(img.size(), CV_8UC1);
	Mat imgG(img.size(), CV_8UC1);
	Mat imgR(img.size(), CV_8UC1);
	Vec3b pixel;

	

	for (int r = 0; r < img.rows; r++)
	{
		for (int c = 0; c < img.cols; c++)
		{
			pixel = img.at<Vec3b>(r, c);
			imgB.at<uchar>(r, c) = pixel[0];
			imgG.at<uchar>(r, c) = pixel[1];
			imgR.at<uchar>(r, c) = pixel[2];
		}
	}

	equalizeHist(imgB, imgB);
	equalizeHist(imgG, imgG);
	equalizeHist(imgR, imgR);

	for (int r = 0; r < img.rows; r++)
	{
		for (int c = 0; c < img.cols; c++)
		{
			pixel = Vec3b(imgB.at<uchar>(r, c), imgG.at<uchar>(r, c), imgR.at<uchar>(r, c));
			res.at<Vec3b>(r, c) = pixel;
		}
	}

	return res;
}
float ColorArea::CheckColor(int iAreaPixel) {
    double d1 = clock();
  
    Mat matBilate = Mat();
   // matCrop = RotateImge(matRaw.clone(), RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));
   // cv::imwrite("colorCrop.png", matRaw);
   // cv::bilateralFilter(matCrop, matBilate, 9, 75, 75);
    cv::medianBlur(matRaw, matBilate, 5);
    matResult = matRaw.clone();

    //cv::imwrite("color1.png", matRaw);
        GetMask(matBilate, iAreaPixel);
    Mat matRS = matMask.clone();
   
    pxMathching = countNonZero(matRS);
    if(matRS.type()==CV_8UC3)
        cvtColor(matRS, matRS, COLOR_BGR2GRAY);
    Mat mask = Mat(matRS.rows, matRS.cols, CV_8UC1, Scalar(255,255,255));
   
    bitwise_and(mask, matRS, matResult);
 //   cycle = int(clock() - d1);
    return pxMathching;
    //if (pxMathching>(pxTemp* Score) / 100)
    //{
    //  //  mask = Mat(matRS.rows, matRS.cols, CV_8UC3, Scalar(255, 255,255));
    //    bitwise_and(mask, matRS, matResult);
    //    cycle = int(clock() - d1);
    //    return true;
    //}
    //else
    //{
    //    //mask = Mat(matRS.rows, matRS.cols, CV_8UC3, Scalar(255, 0, 255));
    //    bitwise_and(mask, matRS, matResult);
    //    cycle = int(clock() - d1);
    // 
    //    return false;
    //}
    //    return false;
}
bool ColorArea::GetLimitColor(Scalar color,int iAreaPixel)
{
    if (StyleColor != 0)
    {
        int H = color[0] - (iAreaPixel*2 );
        int S = color[1] - (iAreaPixel * 2);
        int V = color[2] - (iAreaPixel * 2);
        int H2 = color[0] + (iAreaPixel * 2);
        int S2 = color[1] + (iAreaPixel * 2);
        int V2 = color[2] + (iAreaPixel * 2);
        lower = Scalar(H, S, V);
        upper = Scalar(H2, S2, V2);
        return false;
  }
    int H = color[0] ;
    int S = color[1] ;
    int V = color[2];
    if (H >= 165)
    {
        lower = Scalar(H - iAreaPixel, 100, 100);
        upper = Scalar(180, 255, 255);
    }
    else if (H <= 35)
    {
        lower = Scalar(0, 100, 100);
        upper = Scalar(H + iAreaPixel, 255, 255);
    }

    else
    {
     int H = color[0]-(iAreaPixel/100.0)*180;
    int S = color[1] - (iAreaPixel / 100.0) * 255;
    int V = color[2] - (iAreaPixel / 100.0) * 255;
    int H2 = color[0] + (iAreaPixel / 100.0) *180;
    int S2 = color[1] + (iAreaPixel / 100.0) * 255;
    int V2 = color[2] + (iAreaPixel / 100.0) * 255;
    if (S < 0)S = 0; if (S2 < 0)S2 = 0;
    if (H < 0)H = 0; if (H2 < 0)H2 = 0;
    lower = Scalar(H,S,V);
    upper = Scalar(H2, S2, V2);
        return false;
    }
    return true;
}

bool ColorArea::Undo(int iAreaPixel)
{
    int sz= listColor.size();
    if (sz == 0) return false;
    listColor.resize(listColor.size()-1);
    return true;
}

int  ColorArea::SetColorArea(int iAreaPixel)
{
   
    if (listColor.size() == 0)
    {
        matRsTemp = Mat(matSetTemp.rows, matSetTemp.cols, CV_8UC1 ,Scalar(0,0,0));// matRaw.clone();
        return false;
    }
    Mat matResult = Mat();
   // cv::GaussianBlur(matRaw.clone(), matResult, cv::Size(5, 5), 0);
      cv::medianBlur(matSetTemp.clone(), matResult, 5);
   // cv::bilateralFilter(matRaw.clone(), matResult, 9, 75, 75);
   // cv::imshow("mat", matResult);
    GetMask(matResult, iAreaPixel);
    matRsTemp = matMask.clone();
   int pxTemp = countNonZero(matMask);
   
    cvtColor(matRsTemp, matRsTemp, COLOR_GRAY2BGR);
    return pxTemp;

}
