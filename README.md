PaddleSharp 图片文字识别简介
本项目使用 PaddleSharp 与 OpenCvSharp 结合实现图像中的文字识别功能。该项目通过加载本地图片并应用 PaddlePaddle 的 OCR 模型进行识别，支持中文文字的识别，能够处理带有旋转角度的文本。

环境要求
.NET 环境（推荐 .NET Core 或 .NET 5/6）
OpenCvSharp：用于图像处理和解码。
PaddleOCR：基于 PaddlePaddle 的 OCR 引擎，支持多种语言的文字识别。
安装依赖
在项目中使用 NuGet 包管理器安装必要的依赖：

OpenCvSharp4：图像处理库。
Sdcb.PaddleInference：PaddlePaddle 推理引擎。
Sdcb.PaddleOCR：PaddlePaddle OCR 库，用于执行图像文字识别。
bash
Install-Package OpenCvSharp4
Install-Package Sdcb.PaddleInference
Install-Package Sdcb.PaddleOCR
代码实现
1. 导入所需命名空间
csharp
using OpenCvSharp;  // OpenCV 用于图像处理
using Sdcb.PaddleInference;  // PaddlePaddle 推理引擎
using Sdcb.PaddleOCR.Models;  // OCR 模型
using Sdcb.PaddleOCR;  // PaddleOCR 包
using Sdcb.PaddleOCR.Models.Local;  // 本地 OCR 模型
2. 选择 OCR 模型
我们使用本地的 ChineseV3 OCR 模型来进行中文文字识别。

csharp
FullOcrModel model = LocalFullModels.ChineseV3;
3. 加载图片数据
可以通过 URL 或本地文件路径加载图像。这里演示了从本地加载图像：

csharp
byte[] sampleImageData;
string sampleImagePath = "D:\\Users\\Desktop\\Snipaste_2025-04-01_08-11-24.png";
sampleImageData = File.ReadAllBytes(sampleImagePath);
4. 初始化 OCR 引擎
使用 PaddleOcrAll 初始化 OCR 引擎，并设置是否允许旋转文本的检测。

csharp
using (PaddleOcrAll all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
{
    AllowRotateDetection = true,  // 允许识别有角度的文字
    Enable180Classification = false  // 禁用识别超过 90 度的文字
})
{
    // 处理图像并进行 OCR 识别
}
5. 加载并解码图像
使用 Cv2.ImDecode 从字节数组解码图像，并进行 OCR 识别。

csharp
using (Mat src = Cv2.ImDecode(sampleImageData, ImreadModes.Color))
{
    // 运行 OCR 识别
    PaddleOcrResult result = all.Run(src);
    
    // 输出识别结果
    Console.WriteLine("Detected all texts: \n" + result.Text);

    // 输出每个文本区域的详细信息
    foreach (PaddleOcrResultRegion region in result.Regions)
    {
        Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize: {region.Rect.Size}, Angle: {region.Rect.Angle}");
    }
}
6. 识别结果
PaddleOcrResult 包含所有识别出的文本信息以及对应的位置信息（矩形框和旋转角度等）。Regions 属性存储了每个文本区域的详细信息。

运行结果示例
假设输入的图片包含中文文本，程序将输出识别到的文本内容。例如：

plaintext
Detected all texts: 
欢迎使用PaddleOCR，中文识别准确率高！
Text: 欢迎使用PaddleOCR，中文识别准确率高！, Score: 0.95, RectCenter: (300, 200), RectSize: (500, 50), Angle: 0
在此示例中：

Text：识别出的文本内容。
Score：文本识别的置信度（0 到 1 之间）。
RectCenter：文本所在矩形的中心坐标。
RectSize：文本矩形框的大小。
Angle：文本区域的旋转角度。
配置说明
旋转文本支持
AllowRotateDetection 设置为 true 时，OCR 引擎将能够处理和识别有角度的文本。默认情况下，PaddleOCR 可以自动检测和校正旋转角度。

180 度文本支持
Enable180Classification 设置为 false 时，禁止识别旋转超过 90 度的文本。可以根据需要调整此设置。

总结
该项目通过结合 PaddleOCR 和 OpenCV，利用 PaddlePaddle 强大的 OCR 模型实现高效的文字识别。通过简单的代码，您可以轻松处理并提取图像中的文本信息，支持中文识别以及旋转文本的处理。

您可以将以上内容保存为 README.md 文件，并放置在项目根目录下，便于其他开发人员查看和使用。
