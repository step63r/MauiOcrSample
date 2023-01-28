using MauiOcrSample.Extends;
using System.Drawing;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

namespace MauiOcrSample.Models
{
    /// <summary>
    /// OCRモデルクラス
    /// </summary>
    internal class MauiOcr : BindableBase
    {
        #region プロパティ
        private string _filepath = string.Empty;
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get => _filepath;
            set => SetProperty(ref _filepath, value);
        }

        private OcrResult _ocrResult = null;
        /// <summary>
        /// OCR結果
        /// </summary>
        public OcrResult OcrResult
        {
            get => _ocrResult;
            set => SetProperty(ref _ocrResult, value);
        }

        private ImageSource _resultImage = null;
        /// <summary>
        /// 元画像にOCR結果を被せた画像
        /// </summary>
        public ImageSource ResultImage
        {
            get => _resultImage;
            set => SetProperty(ref _resultImage, value);
        }

        private List<Rect> _detectRects = new List<Rect>();
        /// <summary>
        /// 検出した行を囲む四角形
        /// </summary>
        public List<Rect> DetectRects
        {
            get => _detectRects;
            set => SetProperty(ref _detectRects, value);
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// WinRT APIのOCRエンジン
        /// </summary>
        private readonly OcrEngine _engine = OcrEngine.TryCreateFromUserProfileLanguages();
        #endregion

        /// <summary>
        /// OCRを実行する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task ExecuteOcr()
        {
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
            {
                throw new ArgumentException(nameof(FilePath));
            }

            var bitmap = new Bitmap(FilePath);

            SoftwareBitmap softwareBitmap = null;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                using var stream = ms.AsRandomAccessStream();
                var decoder = await BitmapDecoder.CreateAsync(stream);
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            }

            if (softwareBitmap is null)
            {
                throw new Exception("Failed to convert Bitmap to SoftwareBitmap.");
            }

            OcrResult =  await _engine.RecognizeAsync(softwareBitmap);

            if (OcrResult is not null)
            {
                DetectRects = GetDetectRects();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<Rect> GetDetectRects()
        {
            var rects = new List<Rect>();

            // X座標, Y座標の最大値と最小値を求める
            foreach (var line in OcrResult.Lines)
            {
                double minX = line.Words.Min(item => item.BoundingRect.X);
                double maxX = line.Words.Max(item => item.BoundingRect.X);

                double minY = line.Words.Min(item => item.BoundingRect.Y);
                double maxY = line.Words.Max(item => item.BoundingRect.Y);

                rects.Add(new Rect(minX, minY, maxX - minX, maxY - minY));
            }

            return rects;
        }
    }
}
