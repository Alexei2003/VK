using DataSet;

using DownloaderDataSetPhoto.Downloaders;

using Other.Tags;
using Other.Tags.Editors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using WinForms;

namespace DownloaderDataSetPhoto
{
    public partial class DownloaderDataSetPhoto : Form
    {
        private readonly TagsList tagList = new();

        public DownloaderDataSetPhoto()
        {
            InitializeComponent();
            HidePanels(pGelbooru);
            ListTagUI.WriteFindTag(dgvDictionary, tagList, tbTag.Text);
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
                using var clipboardImage = ConverterBmp.ConvertToImageSharp(imageBmp);

                var resulTag = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResult(clipboardImage);

                AddInDataSet(clipboardImage, tbTag.Text.Replace(" ", ""), resulTag);

                Clipboard.Clear();
            }
        }

        private void HidePanels(Panel panel)
        {
            pBackgroundImageCopy.Visible = false;
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

        private void bGelbooru_Click(object sender, EventArgs e)
        {
            HidePanels(pGelbooru);
        }

        private static readonly char[] separator = ['\r', '\n'];

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
                        var tag = BaseTagsEditor.FixTagString(tbTag.Text);
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

        private void tbTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbTag.Text.Length > 1)
            {
                ListTagUI.WriteFindTag(dgvDictionary, tagList, tbTag.Text);
            }
        }

        private void dgvDictionary_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ListTagUI.CellMouseClick(e, tbTag, dgvDictionary, tagList, tbTag.Text);
        }

        private void bSaveTag_Click(object sender, EventArgs e)
        {
            var text = BaseTagsEditor.FixTagString(tbTag.Text);
            tagList.Add(text);
            ListTagUI.WriteFindTag(dgvDictionary, tagList, tbTag.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tagList.Save();
        }
    }
}
