using AddPost.Classes;

using DataSet;

using Other.Tags;
using Other.Tags.Editors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VKClasses;
using VKClasses.VK;

using WinForms;

namespace AddPost
{
    public partial class AddPost : Form
    {
        private string groupShortUrl = "@anime_art_for_every_day";

        private Int64 groupId;
        private readonly TagsList tagList = new();
        private List<ImageWithTag> imageList = [];
        private int imageIndex = -1;
        private readonly VkApiCustom api;

        private struct ImageWithTag
        {
            public Image<Rgb24> image;
            public string? NeuralNetworkResultTag;
        }

        public AddPost()
        {
            InitializeComponent();
            var accessTokens = GosUslugi.GetAccessTokens();
            api = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));
            groupId = Convert.ToInt64(tbGroupId.Text);
            cbTimeBetweenPost.SelectedIndex = 1;

            ListTagUI.WriteFindTag(dgvDictionary, tagList, tbTag.Text);
        }

        private void ClearInfAboutPost()
        {
            if (cbClear1.Checked)
            {
                tbUrl.Text = "";
            }
            if (cbClear2.Checked)
            {
                tbTag.Text = "";
            }

            pbImage.Image = null;
            imageList = [];
            tbNeuralNetworkResult.Text = "";
        }

        private void AddInDataSet(List<ImageWithTag> imageList, string tags)
        {
            if (tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 3)
            {
                foreach (var image in imageList)
                {
                    if (image.NeuralNetworkResultTag != tbTag.Text)
                    {
                        DataSetImage.Save(image.image, tags);
                    }
                }
            }
        }

        private void WritePostTime()
        {
            var date = new Date(api);
            var postDate = date.ChangeTimeNewPostUseLastPost(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
        }

        private void AddImage(Image<Rgb24> image, string tag)
        {
            if (imageList.Count < 10)
            {
                imageList.Add(new ImageWithTag()
                {
                    image = image,
                    NeuralNetworkResultTag = tag
                });

                ShowImage(imageList.Count - 1);
            }
        }

        private void RemoveImage(int index)
        {
            if (imageList.Count > 0)
            {
                using var image = imageList[index].image;
                imageList.RemoveAt(index);

                ShowImage(imageIndex - 1);
            }
        }

        private void ShowImage(int index)
        {
            if (-1 < index)
            {
                if (index < imageList.Count)
                {
                    if (pbImage.InvokeRequired)
                    {
                        pbImage.Invoke((MethodInvoker)delegate
                        {
                            pbImage.Image = ConverterBmp.ConvertToBitmap(imageList[index].image);
                            tbNeuralNetworkResult.Text = imageList[index].NeuralNetworkResultTag;
                            tbImageIndex.Text = (index + 1).ToString();
                        });
                    }
                    else
                    {
                        pbImage.Image = ConverterBmp.ConvertToBitmap(imageList[index].image);
                        tbNeuralNetworkResult.Text = imageList[index].NeuralNetworkResultTag;
                        tbImageIndex.Text = (index + 1).ToString();
                    }
                    imageIndex = index;
                }
            }
            else
            {
                if (imageList.Count == 0)
                {
                    if (pbImage.InvokeRequired)
                    {
                        pbImage.Invoke((MethodInvoker)delegate
                        {
                            pbImage.Image = null;
                            tbNeuralNetworkResult.Text = "";
                            tbImageIndex.Text = "0";
                        });
                    }
                    else
                    {
                        pbImage.Image = null;
                        tbNeuralNetworkResult.Text = "";
                        tbImageIndex.Text = "0";
                    }
                    imageIndex = -1;
                }
            }
        }

        private async void bBuff_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                using var imageBmp = (Bitmap)Clipboard.GetImage();
                var image = ConverterBmp.ConvertToImageSharp(imageBmp);
                await Task.Run(() =>
                {
                    var tag = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResult(image);

                    AddImage(image, tag);
                });
            }
        }

        private async void bSend_Click(object sender, EventArgs e)
        {
            if (imageList.Count > 0)
            {
                var tags = BaseTagsEditor.FixTagString(tbTag.Text);

                var index = cbTimeBetweenPost.SelectedIndex + 1;

                bSend.Enabled = false;

                await Task.Run(() =>
                {
                    var date = new Date(api);
                    var post = new Post(api);
                    try
                    {
                        post.Publish(imageList.Select(x => x.image).ToArray(), tags, tbUrl.Text, date.ChangeTimeNewPostUseLastPost(groupId, index), groupId, groupShortUrl);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                });

                bSend.Enabled = true;

                AddInDataSet(imageList, tags);

                WritePostTime();

                ClearInfAboutPost();
            }
        }

        private void tbTag_KeyUp(object sender, KeyEventArgs e)
        {
            ListTagUI.WriteFindTag(dgvDictionary, tagList, tbTag.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tagList.Save();
        }

        private void cbTimeBetweenPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            WritePostTime();
        }

        private void tbGroupId_Leave(object sender, EventArgs e)
        {
            groupId = Convert.ToInt64(tbGroupId.Text);
            WritePostTime();
        }

        private void bDataSet_Click(object sender, EventArgs e)
        {
            if (imageList.Count > 0)
            {
                string tags = tbTag.Text.Replace(" ", "");
                AddInDataSet(imageList, tags);

                ClearInfAboutPost();
            }
        }

        private void dgvDictionary_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ListTagUI.CellMouseClick(e, tbTag, null, dgvDictionary, tagList, tbTag.Text);
        }

        private void bImageLeft_Click(object sender, EventArgs e)
        {
            ShowImage(imageIndex - 1);
        }

        private void bImageRight_Click(object sender, EventArgs e)
        {
            ShowImage(imageIndex + 1);
        }

        private void bImageDelete_Click(object sender, EventArgs e)
        {
            RemoveImage(imageIndex);
        }

        private void bTbTagFix_Click(object sender, EventArgs e)
        {
            tbTag.Text = BaseTagsEditor.FixTagString(tbTag.Text);
        }
    }
}
