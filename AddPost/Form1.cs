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
        private string? resulTag = null;
        private float percentOriginalTag = 0.6f;

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

        public void ClearInfAboutPost()
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
        }

        public void WriteFindTag()
        {
            dgvDictionary.Rows.Clear();
            var stack = tagList.Find(tbTag.Text);
            foreach (var elem in stack)
            {
                dgvDictionary.Rows.Add(elem);
            }
            dgvDictionary.Sort(dgvDictionary.Columns["tags"], ListSortDirection.Ascending);
        }

        public void AddInDataSet(Bitmap image, string tags)
        {
            if (!tagList.Add(tags) && tags.Split("#").Length - 1 < 3)
            {
                if (resulTag != null && resulTag != tbTag.Text)
                {
                    DataSetPhoto.Add(image, tags);
                }
            }
        }

        public void WritePostTime()
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

                NeuralNetworkResult(clipboardImage);

                AddInDataSet(clipboardImage, tbTag.Text.Replace(" ", ""));

                Clipboard.Clear();
            }
        }

        private void NeuralNetworkResult(Bitmap image)
        {
            resulTag = NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag);

            // Проверяем, требуется ли выполнить Invoke
            if (tbTag.InvokeRequired)
            {
                // Если да, то выполняем Invoke с анонимным методом
                tbTag.Invoke((MethodInvoker)delegate
                {
                    if (cbClear2.Checked)
                    {
                        tbTag.Text = resulTag;
                    }
                });
            }
            else
            {
                if (cbClear2.Checked)
                {
                    tbTag.Text = resulTag;
                }
            }
        }

        private async void bBuff_Click(object sender, EventArgs e)
        {
            // Проверка, содержит ли буфер обмена изображение
            if (Clipboard.ContainsImage())
            {
                // Получение изображения из буфера обмена
                var clipboardImage = new Bitmap(Clipboard.GetImage());

                await Task.Run(() =>
                {
                    NeuralNetworkResult(clipboardImage);

                    // Проверяем, требуется ли выполнить Invoke
                    if (pbImage.InvokeRequired)
                    {
                        // Если да, то выполняем Invoke с анонимным методом
                        pbImage.Invoke((MethodInvoker)delegate
                        {
                            // Установка изображения в PictureBox
                            pbImage.Image = clipboardImage;
                        });
                    }
                    else
                    {
                        // Установка изображения в PictureBox
                        pbImage.Image = clipboardImage;
                    }

                });

            }
        }

        private async void bSend_Click(object sender, EventArgs e)
        {
            if (pbImage.Image != null)
            {
                string tags = tbTag.Text.Replace(" ", "");
                var image = new Bitmap(pbImage.Image);

                var index = cbTimeBetweenPost.SelectedIndex + 1;

                await Task.Run(() =>
                {
                    var post = new Post(authorize.Api);
                    //Создание поста
                    post.Publish(image, tags, tbUrl.Text, date.ChangeTime(groupId, index), groupId);
                });

                AddInDataSet(image, tags);

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
            if (pbImage.Image != null)
            {
                string tags = tbTag.Text.Replace(" ", "");
                var image = new Bitmap(pbImage.Image);
                AddInDataSet(image, tags);

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

        private void bDownloadPhotos_Click(object sender, EventArgs e)
        {

            if (tbTag.Text.Length > 0)
            {
                int shift = 0;
                int count = 0;
                Task.Run(() =>
                {
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
                    downloaderVK.SavePhotosIdFromNewsfeed(tbTag.Text, tagList, shift, count, groupId, percentOriginalTag);
                });
            }
            else
            {

            }
        }
    }
}