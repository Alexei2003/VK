using DataSet;

using NeuralNetwork;

namespace NeuralNetworkAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                using var predImage = pbImage.Image;
                var imageBmp = (Bitmap)Clipboard.GetImage();
                pbImage.Image = imageBmp;

                var image = ConverterBmp.ConvertToImageSharp(imageBmp);

                Task.Run(() =>
                {
                    var labels = NeuralNetworkWorker.NeuralNetworkResultKTopCountAndPercent(image, 20);

                    this.Invoke((MethodInvoker)(() =>
                    {
                        dvgPercent.Rows.Clear();
                        for (var i = 0; i < labels.Length; i++)
                        {
                            dvgPercent.Rows.Add(new DataGridViewRow { Cells = { new DataGridViewTextBoxCell() { Value = labels[i].Name }, new DataGridViewTextBoxCell() { Value = labels[i].Value.ToString("F2") + "%" } } });
                        }
                    }));
                });
            }
        }
    }
}
