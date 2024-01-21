using AddPost.Classes;
using AddPost.Classes.DataSet;
using AddPost.Classes.DownloaderDataSetPhoto;
using AddPost.Classes.VK;
using System.ComponentModel;

namespace AddPost
{
    public partial class Form1 : Form
    {

        private Int64 groupId;
        private readonly Authorize authorize;
        private readonly TagsLIst tagList = new();
        private readonly Date date;
        private float percentOriginalTag = 0.6f;
        private List<ImagesWithTag> imageList = [];
        private int imageIndex = 0;

        private struct ImagesWithTag
        {
            public Bitmap? image;
            public string? NeuralNetworkResultTag;
        }

        public Form1()
        {
            InitializeComponent();
            var accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            tagList.LoadDictionary();
            date = new Date(authorize.Api);
            groupId = Convert.ToInt64(tbGroupId.Text);
            cbTimeBetweenPost.SelectedIndex = 1;
            cbPercentOriginalTag.SelectedIndex = 5;

            WriteFindTag();
        }

        private void ClearInfAboutPost()
        {
            //Очитска полей после создания постаы
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
            var stack = tagList.Find(tbTag.Text);
            foreach (var elem in stack)
            {
                dgvDictionary.Rows.Add(elem);
            }
            dgvDictionary.Sort(dgvDictionary.Columns["tags"], ListSortDirection.Ascending);
        }

        private void AddInDataSet(List<ImagesWithTag> imageList, string tags)
        {
            if (!tagList.Add(tags) && tags.Split("#").Length - 1 < 3)
            {
                foreach (var image in imageList)
                {

                    if (image.NeuralNetworkResultTag != tbTag.Text)
                    {
                        DataSetPhoto.Add(image.image, tags);
                    }
                }
            }
        }

        private void WritePostTime()
        {
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
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
            }
        }

        private void ShowImage(int index)
        {
            if (-1 < index && index < imageList.Count)
            {
                if (index < imageList.Count)
                {
                    // Проверяем, требуется ли выполнить Invoke
                    if (pbImage.InvokeRequired)
                    {
                        // Если да, то выполняем Invoke с анонимным методом
                        pbImage.Invoke((MethodInvoker)delegate
                        {
                            // Установка изображения в PictureBox
                            pbImage.Image = imageList[index].image;
                            tbNeuralNetworkResult.Text = imageList[index].NeuralNetworkResultTag;
                            tbImageIndex.Text = (index + 1).ToString();
                        });
                    }
                    else
                    {
                        // Установка изображения в PictureBox
                        pbImage.Image = imageList[index].image;
                        tbNeuralNetworkResult.Text = imageList[index].NeuralNetworkResultTag;
                        tbImageIndex.Text = (index + 1).ToString();
                    }
                    imageIndex = index;
                }
            }
        }

        private async void bBuff_Click(object sender, EventArgs e)
        {
            // Проверка, содержит ли буфер обмена изображение
            if (Clipboard.ContainsImage())
            {
                var image = new Bitmap(Clipboard.GetImage());
                await Task.Run(() =>
                {
                    var tag = NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag);

                    AddImage(image, tag);

                    ShowImage(imageList.Count - 1);
                });

            }
        }

        private async void bSend_Click(object sender, EventArgs e)
        {
            if (imageList.Count>0)
            {
                string tags = tbTag.Text.Replace(" ", "");

                var index = cbTimeBetweenPost.SelectedIndex + 1;

                await Task.Run(() =>
                {
                    var post = new Post(authorize.Api);
                    //Создание поста
                    post.Publish(imageList.Select(x => x.image).ToArray(), tags, tbUrl.Text, date.ChangeTime(groupId, index), groupId);
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
                        // Если да, то выполняем Invoke с анонимным методом
                        bDownloadPhotos.Invoke((MethodInvoker)delegate
                        {
                            bDownloadPhotos.Enabled = false;
                        });
                    }
                    else
                    {
                        bDownloadPhotos.Enabled = false;
                    }

                    // Проверяем, требуется ли выполнить Invoke
                    if (tbShiftDownload.InvokeRequired)
                    {
                        // Если да, то выполняем Invoke с анонимным методом
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
                    var downloaderVK = new DownloaderDataSetPhotoFromVK(authorize.Api);
                    try
                    {
                        downloaderVK.SavePhotosIdFromNewsfeed(tbTag.Text, shift, count, groupId, percentOriginalTag);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (bDownloadPhotos.InvokeRequired)
                    {
                        // Если да, то выполняем Invoke с анонимным методом
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
                MessageBox.Show("Тег не указан", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}