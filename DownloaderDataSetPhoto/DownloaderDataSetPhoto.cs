using DataSet;
using DataSet.DataStruct;
using DownloaderDataSetPhoto.Downloader;

namespace DownloaderDataSetPhoto
{
    public partial class DownloaderDataSetPhoto : Form
    {
        private Int64 groupId;
        private readonly TagsLIst tagList = new();
        private float percentOriginalTag = 0.6f;
        private readonly VkApiCustom.VkApiCustom api;

        public DownloaderDataSetPhoto()
        {
            InitializeComponent();
            var accessToken = File.ReadAllText("AccessToken.txt");
            api = new VkApiCustom.VkApiCustom(accessToken);
            tagList.LoadDictionary();
            groupId = 220199532;
        }


        private void AddInDataSet(Bitmap image, string tags, string resulTag)
        {
            if (!tagList.Add(tags) && tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 3)
            {
                if (resulTag != tbTag.Text)
                {
                    DataSetPhoto.Save(image, tags);
                }
            }
        }

        private void BackgroundImageCopy()
        {
            if (Clipboard.ContainsImage())
            {
                var clipboardImage = new Bitmap(Clipboard.GetImage());

                var resulTag = NeuralNetwork.NeuralNetwork.NeuralNetworkResult(clipboardImage, percentOriginalTag);

                AddInDataSet(clipboardImage, tbTag.Text.Replace(" ", ""), resulTag);

                Clipboard.Clear();
            }
        }

        private void HidePanels(Panel panel)
        {
            pBackgroundImageCopy.Visible = false;
            pVK.Visible = false;
            pDanbooru.Visible = false;

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
                        var lockNeuralNetworkResult = new object();
                        var tags = tbTag.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        Parallel.For(0, tags.Length, i =>
                        {
                            downloaderVK.SavePhotosIdFromNewsfeed(tags[i], shift, count, groupId, percentOriginalTag, $"DataSet_{i}", lockNeuralNetworkResult);
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotosVK.InvokeRequired)
                    {
                        bDownloadPhotosVK.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosVK.Enabled = true;
                        });
                    }
                    else
                    {
                        bDownloadPhotosVK.Enabled = true;
                    }
                });
            }
            else
            {
                MessageBox.Show("Tag is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bBackgroundImageCopy_Click(object sender, EventArgs e)
        {
            HidePanels(pBackgroundImageCopy);
        }

        private void bVK_Click(object sender, EventArgs e)
        {
            HidePanels(pVK);
        }

        private void bDanbooru_Click(object sender, EventArgs e)
        {
            HidePanels(pDanbooru);
        }
    }
}
