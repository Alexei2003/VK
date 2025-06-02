using DataSet;

using DownloaderDataSetPhoto.Downloaders;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VKClasses;
using VKClasses.Tags;
using VKClasses.VK;

namespace DownloaderDataSetPhoto
{
    public partial class DownloaderDataSetPhoto : Form
    {
        private Int64 groupId;
        private readonly TagsList tagList = new();
        private readonly VkApiCustom api;

        public DownloaderDataSetPhoto()
        {
            InitializeComponent();
            var accessTokens = GosUslugi.GetAccessTokens();
            api = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));
            groupId = 220199532;
            HidePanels(pGelbooru);
        }


        private void AddInDataSet(Image<Rgb24> image, string tags, string resulTag)
        {
            if (!tagList.Add(tags) && tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 3 && resulTag != tbTag.Text)
            {
                DataSetImage.Save(image, tags);
            }
        }

        private void BackgroundImageCopy()
        {
            if (Clipboard.ContainsImage())
            {
                using var imageBmp = (Bitmap)Clipboard.GetImage();
                using var clipboardImage = Converter.ConvertToImageSharp(imageBmp);

                var resulTag = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResult(clipboardImage);

                AddInDataSet(clipboardImage, tbTag.Text.Replace(" ", ""), resulTag);

                Clipboard.Clear();
            }
        }

        private void HidePanels(Panel panel)
        {
            pBackgroundImageCopy.Visible = false;
            pVK.Visible = false;
            pGelbooru.Visible = false;

            panel.Visible = true;
        }

        private void bBackgroundImageCopyOn_Click(object sender, EventArgs e)
        {
            bBackgroundImageCopyOn.Enabled = false;
            bBackgroundImageCopyOff.Enabled = true;

            tBackgroundImageCopy.Start();
        }

        private void bBackgroundImageCopyOff_Click(object sender, EventArgs e)
        {
            bBackgroundImageCopyOff.Enabled = false;
            bBackgroundImageCopyOn.Enabled = true;

            tBackgroundImageCopy.Stop();
        }

        private void tBackgroundImageCopy_Tick(object sender, EventArgs e)
        {
            BackgroundImageCopy();
        }

        private void bBackgroundImageCopy_Click(object sender, EventArgs e)
        {
            HidePanels(pBackgroundImageCopy);
        }

        private void bVK_Click(object sender, EventArgs e)
        {
            HidePanels(pVK);
        }

        private void bGelbooru_Click(object sender, EventArgs e)
        {
            HidePanels(pGelbooru);
        }

        private static readonly char[] separator = ['\r', '\n'];

        private async void bDownloadPhotosVK_Click(object sender, EventArgs e)
        {
            if (tbTag.Text.Length > 0)
            {
                int shift = 0;
                int count = 0;
                await Task.Run(() =>
                {
                    if (bDownloadPhotosVK.InvokeRequired)
                    {
                        bDownloadPhotosVK.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosVK.Enabled = false;
                        });
                    }
                    else
                    {
                        bDownloadPhotosVK.Enabled = false;
                    }

                    if (tbShiftDownload.InvokeRequired)
                    {
                        tbShiftDownload.Invoke((MethodInvoker)delegate
                        {
                            shift = Convert.ToInt32(tbShiftDownload.Text);
                            count = Convert.ToInt32(tbCountDownload.Text);
                        });
                    }
                    else
                    {
                        shift = Convert.ToInt32(tbShiftDownload.Text);
                        count = Convert.ToInt32(tbCountDownload.Text);
                    }
                    var downloaderVK = new DownloaderDataSetPhotoFromVK(api, tagList);

                    try
                    {
                        var tags = tbTag.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        Parallel.For(0, tags.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                        {
                            downloaderVK.SavePhotosFromNewsfeed(tags[i], shift, count, groupId, $"DataSet_{i}");
                        });
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotosVK.InvokeRequired)
                    {
                        bDownloadPhotosVK.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosVK.Enabled = true;
                            tbTag.Text = "";
                        });
                    }
                    else
                    {
                        bDownloadPhotosVK.Enabled = true;
                        tbTag.Text = "";
                    }
                });
            }
            else
            {
                MessageBox.Show("Tag is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void bDownloadPhotosGelbooru_Click(object sender, EventArgs e)
        {
            if (tbTag.Text.Length > 0)
            {
                await Task.Run(() =>
                {
                    if (bDownloadPhotosGelbooru.InvokeRequired)
                    {
                        bDownloadPhotosGelbooru.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosGelbooru.Enabled = false;
                        });
                    }
                    else
                    {
                        bDownloadPhotosGelbooru.Enabled = false;
                    }

                    try
                    {
                        var tag = tbTag.Text.Trim();
                        var url = tbGelbooruUrl.Text;
                        DownloaderDataSetPhotoFromGelbooru.SavePhotos(url, tag, "DataSet_");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotosGelbooru.InvokeRequired)
                    {
                        bDownloadPhotosGelbooru.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosGelbooru.Enabled = true;
                            tbTag.Text = "";
                            tbGelbooruUrl.Text = "";
                        });
                    }
                    else
                    {
                        bDownloadPhotosGelbooru.Enabled = true;
                        tbTag.Text = "";
                        tbGelbooruUrl.Text = "";
                    }
                });
            }
        }
    }
}
