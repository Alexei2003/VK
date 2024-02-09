using AddPost.Classes;
using AddPost.Classes.DataSet;
using AddPost.Classes.DownloaderDataSetPhoto;
using AddPost.Classes.VK;
using System.ComponentModel;
using WorkWithPost;

namespace AddPost
{
    public partial class Form1 : Form
    {

        private Int64 groupId;
        private readonly Authorize authorize;
        private readonly TagsLIst tagList = new();
        private float percentOriginalTag = 0.6f;
        private List<ImagesWithTag> imageList = [];
        private int imageIndex = -1;
        private readonly Random rand = new();

        private struct ImagesWithTag
        {
            public Bitmap image;
            public string? NeuralNetworkResultTag;
        }

        public Form1()
        {
            InitializeComponent();
            var accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            tagList.LoadDictionary();
            groupId = Convert.ToInt64(tbGroupId.Text);
            cbTimeBetweenPost.SelectedIndex = 1;
            cbPercentOriginalTag.SelectedIndex = 5;

            WriteFindTag();
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

        private void WriteFindTag()
        {
            dgvDictionary.Rows.Clear();
            var stack = tagList.FindLast(tbTag.Text);

            dgvDictionary.Rows.AddRange(stack.Select(elem => new DataGridViewRow { Cells = { new DataGridViewTextBoxCell { Value = elem } } }).ToArray());

            dgvDictionary.Sort(dgvDictionary.Columns["tags"], ListSortDirection.Ascending);

            int red = ChangeRGB(0);
            int green = ChangeRGB(0);
            int blue = ChangeRGB(0);

            int groupIndex = 0;
            string groupName = "";
            string tmpGroupName = "";

            for (int i = 0; i < dgvDictionary.Rows.Count; i++)
            {
                if (dgvDictionary.Rows[i].Cells[0].Value == null)
                {
                    break;
                }

                tmpGroupName = dgvDictionary.Rows[i].Cells[0].Value.ToString().Split('#')[1];

                if (tmpGroupName != groupName)
                {
                    groupName = tmpGroupName;
                    switch (groupIndex % 3)
                    {
                        case 0:
                            red = ChangeRGB(red);
                            break;
                        case 1:
                            green = ChangeRGB(green);
                            break;
                        case 2:
                            blue = ChangeRGB(blue);
                            break;
                    }
                    groupIndex++;
                }

                dgvDictionary.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(red, green, blue);
            }
        }

        private int ChangeRGB(int value)
        {
            const int MIN_RGB = 150;

            if (value > MIN_RGB)
            {
                return value - (rand.Next(30) + 20);
            }
            else
            {
                return MIN_RGB + rand.Next(50);
            }
        }

        private void AddInDataSet(List<ImagesWithTag> imageList, string tags)
        {
            if (!tagList.Add(tags) && tags.Split("#").Length < 4)
            {
                foreach (var image in imageList)
                {
                    if (image.NeuralNetworkResultTag != tbTag.Text)
                    {
                        DataSetPhoto.Save(image.image, tags);
                    }
                }
            }
        }

        private void WritePostTime()
        {
            var date = new Date(authorize.Api);
            var postDate = date.ChangeTimeNewPostUseLastPost(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
        }

        private void BackgroundImageCopy()
        {
            if (Clipboard.ContainsImage())
            {
                var clipboardImage = new Bitmap(Clipboard.GetImage());

                var resulTag = NeuralNetwork.NeuralNetworkResult(clipboardImage, percentOriginalTag);

                AddInDataSet([new ImagesWithTag { image = clipboardImage, NeuralNetworkResultTag = resulTag }], tbTag.Text.Replace(" ", ""));

                Clipboard.Clear();
            }
        }

        private void AddImage(Bitmap image, string tag)
        {
            if (imageList.Count < 10)
            {
                imageList.Add(new ImagesWithTag()
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
                            pbImage.Image = imageList[index].image;
                            tbNeuralNetworkResult.Text = imageList[index].NeuralNetworkResultTag;
                            tbImageIndex.Text = (index + 1).ToString();
                        });
                    }
                    else
                    {
                        pbImage.Image = imageList[index].image;
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
                var image = new Bitmap(Clipboard.GetImage());
                await Task.Run(() =>
                {
                    var tag = NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag);

                    AddImage(image, tag);
                });
            }
        }

        private async void bSend_Click(object sender, EventArgs e)
        {
            if (imageList.Count > 0)
            {
                string tags = tbTag.Text.Replace(" ", "");

                var index = cbTimeBetweenPost.SelectedIndex + 1;

                await Task.Run(() =>
                {
                    var date = new Date(authorize.Api);
                    var post = new Post(authorize.Api);
                    post.Publish(imageList.Select(x => x.image).ToArray(), tags, tbUrl.Text, date.ChangeTimeNewPostUseLastPost(groupId, index), groupId);
                });

                AddInDataSet(imageList, tags);

                WritePostTime();

                ClearInfAboutPost();
            }
        }

        private void tbTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbTag.Text.Length > 1)
            {
                WriteFindTag();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tagList.SaveDictionary();
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

        private void dgvDictionary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var str = tbTag.Text;
            int removeCount = -1;
            int indexStartDel = -1;
            for (int i = str.Length - 1; i > -1; i--)
            {
                if (str[i] == '#')
                {
                    indexStartDel = i;
                    removeCount = str.Length - i;
                    break;
                }
            }

            if (removeCount > -1 && indexStartDel > -1)
            {
                str = str.Remove(indexStartDel, removeCount);
            }

            tbTag.Text = str + dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
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
            if (e.Button == MouseButtons.Right)
            {
                tagList.Remove(dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                WriteFindTag();
            }
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

        private void cbPercentOriginalTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            percentOriginalTag = (cbPercentOriginalTag.SelectedIndex + 1) * 0.1f;
        }

        private async void bDownloadPhotos_Click(object sender, EventArgs e)
        {

            if (tbTag.Text.Length > 0)
            {
                int shift = 0;
                int count = 0;
                await Task.Run(() =>
                {
                    if (bDownloadPhotos.InvokeRequired)
                    {
                        bDownloadPhotos.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotos.Enabled = false;
                        });
                    }
                    else
                    {
                        bDownloadPhotos.Enabled = false;
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
                    var downloaderVK = new DownloaderDataSetPhotoFromVK(authorize.Api, tagList);
                    try
                    {
                        downloaderVK.SavePhotosIdFromNewsfeed(tbTag.Text, shift, count, groupId, percentOriginalTag);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotos.InvokeRequired)
                    {
                        bDownloadPhotos.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotos.Enabled = true;
                        });
                    }
                    else
                    {
                        bDownloadPhotos.Enabled = true;
                    }
                });
            }
            else
            {
                MessageBox.Show("Tag is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
    }
}
