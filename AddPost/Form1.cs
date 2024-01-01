using AddPost.Classes;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace AddPost
{
    public partial class Form1 : Form
    {

        private string groupId;
        private readonly string accessToken;
        private readonly Authorize authorize;
        private readonly Post post;
        private readonly Tags tagList = new();
        private readonly Date date;
        private string? resulTag = null;
        public Form1()
        {
            InitializeComponent();
            accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            post = new Post(authorize.Api);
            tagList.LoadDictionary();
            date = new Date(authorize.Api);
            groupId = tbGroupId.Text;
            cbTimeBetweenPost.SelectedIndex = 1;

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
                    PhotoDataSet.Add(image, tags);
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
            image = PhotoDataSet.ChangeResolution(image);

            // Преобразуйте изображение в байты
            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                // Сохраните изображение в том же формате, в котором оно находится в буфере обмена
                image.Save(stream, ImageFormat.Jpeg);
                imageBytes = stream.ToArray();
            }

            // Подача данных в модель
            var sampleData = new ComputerVision.ModelInput()
            {
                ImageSource = imageBytes,
            };

            //Load model and predict output
            var resultArts = ComputerVision.Predict(sampleData);

            var scores = resultArts.Score;

            if (cbClear2.Checked)
            {
                resulTag = resultArts.PredictedLabel;
                if (scores.Max() < 0.6 || resulTag.Contains(".#Original"))
                {
                    resulTag = "#Original";
                }

                tbTag.Text = resulTag;
            }
        }

        private void bBuff_Click(object sender, EventArgs e)
        {
            // Проверка, содержит ли буфер обмена изображение
            if (Clipboard.ContainsImage())
            {
                // Получение изображения из буфера обмена
                var clipboardImage = new Bitmap(Clipboard.GetImage());

                // Установка изображения в PictureBox
                pbImage.Image = clipboardImage;

                NeuralNetworkResult(clipboardImage);
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            if (pbImage.Image != null)
            {
                string tags = tbTag.Text.Replace(" ", "");
                var image = new Bitmap(pbImage.Image);

                //Создание поста
                post.Publish(image, tags, tbUrl.Text, date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1), groupId);

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
            groupId = tbGroupId.Text;
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
    }
}