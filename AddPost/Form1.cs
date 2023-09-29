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
        private readonly PhotoDataSet photoDataSet;
        public Form1()
        {
            InitializeComponent();
            accessToken = File.ReadAllText("AccessToken.txt");
            authorize = new Authorize(accessToken);
            post = new Post(authorize.Api);
            tag = new Tag();
            tag.LoadDictionary();
            date = new Date(authorize.Api);
            photoDataSet = new PhotoDataSet();   
            groupId = tbGroupId.Text;
            cbTimeBetweenPost.SelectedIndex = 1;

            foreach (var elem in tag.TagsList)
            {
                dgvDictionary.Rows.Add(elem);
            }
        }

        private void bBuff_Click(object sender, EventArgs e)
        {
            // ��������, �������� �� ����� ������ �����������
            if (Clipboard.ContainsImage())
            {
                // ��������� ����������� �� ������ ������
                var clipboardImage = new Bitmap(Clipboard.GetImage());

                // ��������� ����������� � PictureBox
                pbImage.Image = clipboardImage;
            }
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            var postDate = date.ChangeTime(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            string tags = tbTag.Text.Replace(" ", "");
            var image = new Bitmap(pbImage.Image);
            
            //�������� �����
            post.Publish(image, tags, tbUrl.Text, postDate, groupId);

            if (!tag.Add(tags) && tags.Split("#").Length < 3)
            {
                PhotoDataSet.Add(image, tags);
            }

            //������� ����� ����� �������� ������
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
    }
}