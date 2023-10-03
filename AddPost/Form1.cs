using AddPost.Classes;
using System.Drawing.Imaging;

namespace AddPost
{
    public partial class Form1 : Form
    {

        private string groupId;
        private readonly string accessToken;
        private readonly Authorize authorize;
        private readonly Post post;
        private readonly Tag tag;
        private readonly Date date;
        ComputerVision.ModelOutput resultArts;


        public Form1()
        {
            InitializeComponent();
            accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            post = new Post(authorize.Api);
            tag = new Tag();
            tag.LoadDictionary();
            date = new Date(authorize.Api);
            groupId = tbGroupId.Text;
            cbTimeBetweenPost.SelectedIndex = 1;

            foreach (var elem in tag.TagsList)
            {
                dgvDictionary.Rows.Add(elem);
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

                // Преобразуйте изображение в байты
                byte[] imageBytes;
                using (var stream = new MemoryStream())
                {
                    // Сохраните изображение в том же формате, в котором оно находится в буфере обмена
                    clipboardImage.Save(stream, ImageFormat.Jpeg);
                    imageBytes = stream.ToArray();
                }

                // Подача данных в модель
                var sampleData = new ComputerVision.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                //Load model and predict output
                resultArts = ComputerVision.Predict(sampleData);

                var scores = resultArts.Score;

                if (scores.Max() > 0.5)
                {
                    tbTag.Text = resultArts.PredictedLabel;
                }
                else
                {
                    tbTag.Text = "#Original";
                }
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            string tags = tbTag.Text.Replace(" ", "");
            var image = new Bitmap(pbImage.Image);

            //Создание поста
            post.Publish(image, tags, tbUrl.Text, postDate, groupId);

            if (!tag.Add(tags) && tags.Split("#").Length - 1 < 3 && !tags.Contains("#Original") && tbTag.Text != resultArts.PredictedLabel)
            {
                PhotoDataSet.Add(image, tags);
            }

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
            postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
        }

        private void tbTag_KeyUp(object sender, KeyEventArgs e)
        {
            dgvDictionary.Rows.Clear();
            if (tbTag.Text != "")
            {
                var stack = tag.Find(tbTag.Text);
                foreach (var elem in stack)
                {
                    dgvDictionary.Rows.Add(elem);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tag.SaveDictionary();
        }

        private void cbTimeBetweenPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
        }

        private void tbGroupId_Leave(object sender, EventArgs e)
        {
            groupId = tbGroupId.Text;
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
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
            string tags = tbTag.Text.Replace(" ", "");
            var image = new Bitmap(pbImage.Image);
            if (!tag.Add(tags) && tags.Split("#").Length - 1 < 3 && !tags.Contains("#Original"))
            {
                PhotoDataSet.Add(image, tags);
            }
            if (cbClear2.Checked)
            {
                tbTag.Text = "";
            }
            pbImage.Image = null;
        }
    }
}