using DataSet;
using DownloaderDataSetPhoto.Downloaders;
using MyCustomClasses;
using MyCustomClasses.Tags;
using MyCustomClasses.VK;

namespace DownloaderDataSetPhoto
{
    public partial class DownloaderDataSetPhoto : Form
    {
        private Int64 groupId;
        private readonly TagsLIst tagList = new();
        private float percentOriginalTag = 0.6f;
        private readonly VkApiCustom api;

        public DownloaderDataSetPhoto()
        {
            InitializeComponent();
            var accessTokens = GosUslugi.GetAccessTokens();
            api = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));
            tagList.LoadDictionary();
            groupId = 220199532;
            HidePanels(pRule34);

            cbPercentOriginalTag.SelectedIndex = 5;
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
            pRule34.Visible = false;

            panel.Visible = true;
        }
        private void cbPercentOriginalTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            percentOriginalTag = (cbPercentOriginalTag.SelectedIndex + 1) * 0.1f;
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

        private void bRule34_Click(object sender, EventArgs e)
        {
            HidePanels(pRule34);
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
                            downloaderVK.SavePhotosFromNewsfeed(tags[i], shift, count, groupId, percentOriginalTag, $"DataSet_{i}", lockNeuralNetworkResult);
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

        private async void bDownloadPhotosDanbooru_Click(object sender, EventArgs e)
        {
            if (tbTag.Text.Length > 0)
            {
                await Task.Run(() =>
                {
                    if (bDownloadPhotosDanbooru.InvokeRequired)
                    {
                        bDownloadPhotosDanbooru.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosDanbooru.Enabled = false;
                        });
                    }
                    else
                    {
                        bDownloadPhotosDanbooru.Enabled = false;
                    }

                    try
                    {
                        var lockNeuralNetworkResult = new object();
                        var tag = tbTag.Text;
                        var url = tbRule34Url.Text;
                        DownloaderDataSetPhotoFromRule34.SavePhotos(url, tag, percentOriginalTag, $"DataSet_{0}", lockNeuralNetworkResult);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotosDanbooru.InvokeRequired)
                    {
                        bDownloadPhotosDanbooru.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotosDanbooru.Enabled = true;
                            tbTag.Text = "";
                            tbRule34Url.Text = "";
                        });
                    }
                    else
                    {
                        bDownloadPhotosDanbooru.Enabled = true;
                        tbTag.Text = "";
                        tbRule34Url.Text = "";
                    }
                });
            }
        }
    }
}
