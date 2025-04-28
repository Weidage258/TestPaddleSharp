
using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models.Local;

FullOcrModel model = LocalFullModels.ChineseV3;

//byte[] sampleImageData;
//string sampleImageUrl = @"https://www.tp-link.com.cn/content/images2017/gallery/4288_1920.jpg";
//using (HttpClient http = new HttpClient())
//{
//    Console.WriteLine("Download sample image from: " + sampleImageUrl);
//    sampleImageData = await http.GetByteArrayAsync(sampleImageUrl);
//}

byte[] sampleImageData;
//string sampleImageUrl = @"https://www.tp-link.com.cn/content/images2017/gallery/4288_1920.jpg";
string sampleImagePath = "D:\\Users\\Desktop\\Snipaste_2025-04-01_08-11-24.png";
using (HttpClient http = new HttpClient())
{
    Console.WriteLine("Download sample image from: " + sampleImagePath);
    sampleImageData = File.ReadAllBytes(sampleImagePath);
}

using (PaddleOcrAll all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
{
    AllowRotateDetection = true, /* 允许识别有角度的文字 */
    Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
})
{
    // Load local file by following code:
    // using (Mat src2 = Cv2.ImRead(@"C:\test.jpg"))
    using (Mat src = Cv2.ImDecode(sampleImageData, ImreadModes.Color))
    {
        PaddleOcrResult result = all.Run(src);
        Console.WriteLine("Detected all texts: \n" + result.Text);
        foreach (PaddleOcrResultRegion region in result.Regions)
        {
            Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize:    {region.Rect.Size}, Angle: {region.Rect.Angle}");
        }
    }
}
