using AddPost.Classes;

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
        public Form1()
        {
            InitializeComponent();
            accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            post = new Post(authorize.Api);
            tag = new Tag();
            tag.LoadTagsDictionary();
            date = new Date(authorize.Api);
            groupId = tbGroupId.Text;
            cbTimeBetweenPost.SelectedIndex = 1;

            foreach (var elem in tag.TagsList)
            {
                dgvDictionary.Rows.Add(elem);
            }
        }

        private void pbImage_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Проверка, содержит ли буфер обмена изображение
            if (Clipboard.ContainsImage())
            {
                // Получение изображения из буфера обмена
                Image clipboardImage = Clipboard.GetImage();

                // Установка изображения в PictureBox
                pbImage.Image = clipboardImage;
            }
        }

        private void bBuff_Click(object sender, EventArgs e)
        {
            // Проверка, содержит ли буфер обмена изображение
            if (Clipboard.ContainsImage())
            {
                // Получение изображения из буфера обмена
                Image clipboardImage = Clipboard.GetImage();

                // Установка изображения в PictureBox
                pbImage.Image = clipboardImage;
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            post.CreatePost(pbImage.Image, tbTag.Text, tbUrl.Text, postDate, groupId);
            tag.AddTag(tbTag.Text);
            if (cbClear1.Checked)
            {
                tbUrl.Text = "";
            }
            if (cbClear2.Checked)
            {
                tbTag.Text = "";
            }
            pbImage.Image = null;
            postDate = postDate.Value.AddHours(5);
            tbDate.Text = postDate.ToString();
        }

        private void tbTag_KeyUp(object sender, KeyEventArgs e)
        {
            dgvDictionary.Rows.Clear();
            if (tbTag.Text != "")
            {
                var stack = tag.FindTag(tbTag.Text);
                foreach (var elem in stack)
                {
                    dgvDictionary.Rows.Add(elem);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tag.SaveTagsDictionary();
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
            for(int i = str.Length - 1; i>-1 ; i--)
            {
                if (str[i] == '#')
                {
                    indexStartDel = i;
                    removeCount = str.Length - i; 
                    break;
                }
            }

            if(removeCount>-1  && indexStartDel > -1)
            {
                str = str.Remove(indexStartDel, removeCount);
            }

            tbTag.Text = str + dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
    }
}