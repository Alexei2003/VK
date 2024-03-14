using AddPost.Classes;
using DataSet;
using DataSet.DataStruct;
using System.ComponentModel;

namespace AddPost
{
    public partial class AddPost : Form
    {

        private Int64 groupId;
        private readonly TagsLIst tagList = new();
        private float percentOriginalTag = 0.6f;
        private List<ImagesWithTag> imageList = [];
        private int imageIndex = -1;
        private readonly Random rand = new();
        private readonly VkApiCustom.VkApiCustom api;

        private struct ImagesWithTag
        {
            public Bitmap image;
            public string? NeuralNetworkResultTag;
        }

        public AddPost()
        {
            InitializeComponent();
            var accessToken = File.ReadAllText("AccessToken.txt");
            api = new VkApiCustom.VkApiCustom(accessToken);
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

                tmpGroupName = dgvDictionary.Rows[i].Cells[0].Value.ToString().Split('#', StringSplitOptions.RemoveEmptyEntries).First();

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
            if (!tagList.Add(tags) && tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 3)
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
            var date = new Date(api);
            var postDate = date.ChangeTimeNewPostUseLastPost(groupId, cbTimeBetweenPost.SelectedIndex + 1);
            postDate = postDate.Value.AddHours(3);
            tbDate.Text = postDate.ToString();
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

        private static string FixTagString(string tagsStr)
        {
            // Удаление пробелов по краям
            tagsStr = tagsStr.Trim(' ');

            // Замена пробелов на _
            var tagsArr = tagsStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            tagsStr = tagsArr[0];
            for (var i = 1; i < tagsArr.Length; i++)
            {
                if (!string.IsNullOrEmpty(tagsArr[i]))
                {
                    tagsStr += '_' + tagsArr[i];
                }
            }

            // Удаление без # тегов
            tagsArr = tagsStr.Split('#', StringSplitOptions.RemoveEmptyEntries);
            tagsStr = "";
            for (var i = 0; i < tagsArr.Length; i++)
            {
                if (!string.IsNullOrEmpty(tagsArr[i]))
                {
                    tagsStr += '#' + tagsArr[i];
                }
            }

            return tagsStr;
        }

        private async void bBuff_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var image = new Bitmap(Clipboard.GetImage());
                await Task.Run(() =>
                {
                    var tag = NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag);

                    AddImage(image, tag);
                });
            }
        }

        private async void bSend_Click(object sender, EventArgs e)
        {
            if (imageList.Count > 0)
            {
                var tags = FixTagString(tbTag.Text);

                var index = cbTimeBetweenPost.SelectedIndex + 1;

                await Task.Run(() =>
                {
                    var date = new Date(api);
                    var post = new Post(api);
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
            //Удаление тега для замены
            var tagStr = tbTag.Text;
            int removeCount = -1;
            int indexStartDel = -1;
            for (int i = tagStr.Length - 1; i > -1; i--)
            {
                if (tagStr[i] == '#')
                {
                    indexStartDel = i;
                    removeCount = tagStr.Length - i;
                    break;
                }
            }
            if (removeCount > -1 && indexStartDel > -1)
            {
                tagStr = tagStr.Remove(indexStartDel, removeCount);
            }

            var findTagStr = dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            var tagArr1 = tagStr.Split('#', StringSplitOptions.RemoveEmptyEntries);
            var tagArr2 = findTagStr.Split('#', StringSplitOptions.RemoveEmptyEntries);

            ///////////////////


            tbTag.Text = tagStr + findTagStr;
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
                DialogResult result = MessageBox.Show($"Удаление тега {dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}", "Подтвердите действие для ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    tagList.Remove(dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }

                WriteFindTag();
            }
        }

        private void cbPercentOriginalTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            percentOriginalTag = (cbPercentOriginalTag.SelectedIndex + 1) * 0.1f;
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
            tbTag.Text = FixTagString(tbTag.Text);
        }
    }
}
