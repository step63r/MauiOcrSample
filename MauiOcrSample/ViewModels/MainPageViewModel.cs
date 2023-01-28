using MauiOcrSample.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiOcrSample.ViewModels
{
    /// <summary>
    /// MainPage.xamlのViewModelクラス
    /// </summary>
    internal class MainPageViewModel
    {
        #region プロパティ
        /// <summary>
        /// Modelオブジェクト
        /// </summary>
        public ReactiveProperty<MauiOcr> Model { get; } = new ReactiveProperty<MauiOcr>(new MauiOcr());
        #endregion

        #region コマンド
        /// <summary>
        /// 「ファイルを開く」コマンド
        /// </summary>
        public ReactiveCommand OpenFileCommand { get; }
        /// <summary>
        /// 「OCR実行」コマンド
        /// </summary>
        public ReactiveCommand OcrCommand { get; }
        #endregion

        #region コンストラクタ
        public MainPageViewModel()
        {
            OpenFileCommand = new ReactiveCommand();
            OpenFileCommand.Subscribe(_ => ExecuteOpenFileCommand());

            OcrCommand = new ReactiveCommand();
            OcrCommand.Subscribe(_ => ExecuteOcrCommand());
        }
        #endregion

        #region コマンドの実装
        /// <summary>
        /// 
        /// </summary>
        private async void ExecuteOpenFileCommand()
        {
            try
            {
                var options = new PickOptions()
                {
                    PickerTitle = "画像ファイルを選択",
                    FileTypes = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                            { DevicePlatform.Android, new[] { "image/bmp", "image/jpeg", "image/png" } },
                            { DevicePlatform.WinUI, new[] { ".bmp", ".jpg", ".jpeg", ".png" } },
                        })
                };

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    Model.Value.FilePath = result.FullPath;
                    Model.Value.ResultImage = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception)
            {
                // 
            }
        }

        private async void ExecuteOcrCommand()
        {
            await Model.Value.ExecuteOcr();
        }
        #endregion
    }
}
