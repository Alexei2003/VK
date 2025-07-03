using System.ComponentModel;

using Other;
using Other.Tags;

namespace WinForms
{
    public static class ListTagUI
    {
        public static void WriteFindTag(DataGridView dgvDictionary, TagsList tagList, string text)
        {
            dgvDictionary.Rows.Clear();
            if (!text.Contains("#"))
            {
                text = "#";
            }
            var stack = tagList.FindLast(text);

            dgvDictionary.Rows.AddRange(stack.Select(elem => new DataGridViewRow { Cells = { new DataGridViewTextBoxCell { Value = 0 }, new DataGridViewTextBoxCell { Value = elem } } }).ToArray());

            dgvDictionary.Sort(dgvDictionary.Columns["tags"], ListSortDirection.Ascending);

            int red = ChangeRGB(0);
            int green = ChangeRGB(0);
            int blue = ChangeRGB(0);

            int groupIndex = 0;
            string groupName = "";
            string tmpGroupName = "";

            for (int i = 0; i < dgvDictionary.Rows.Count; i++)
            {
                if (dgvDictionary.Rows[i].Cells["tag"].Value == null)
                {
                    break;
                }

                dgvDictionary.Rows[i].Cells["index"] = new DataGridViewTextBoxCell { Value = i + 1 };
                tmpGroupName = dgvDictionary.Rows[i].Cells["tags"].Value.ToString().Split('#', StringSplitOptions.RemoveEmptyEntries).First();

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

        private static int ChangeRGB(int value)
        {
            const int MIN_RGB = 150;

            if (value > MIN_RGB)
            {
                return value - (RandomStatic.Rand.Next(30) + 20);
            }
            else
            {
                return MIN_RGB + RandomStatic.Rand.Next(50);
            }
        }

        public static void CellMouseClick(DataGridViewCellMouseEventArgs e, TextBox textBox , DataGridView dgvDictionary, TagsList tagList, string text)
        {
            if (e.Button == MouseButtons.Left)
            {
                int removeCount = -1;
                int indexStartDel = -1;
                for (int i = text.Length - 1; i > -1; i--)
                {
                    if (text[i] == '#')
                    {
                        indexStartDel = i;
                        removeCount = text.Length - i;
                        break;
                    }
                }
                if (removeCount > -1 && indexStartDel > -1)
                {
                    text = text.Remove(indexStartDel, removeCount);
                }

                textBox.Text = dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else if (e.Button == MouseButtons.Right)
            {
                DialogResult result = MessageBox.Show($"Удаление тега {dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}", "Подтвердите действие для ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    tagList.Remove(dgvDictionary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }

                WriteFindTag(dgvDictionary, tagList, text);
            }
        }
    }
}
