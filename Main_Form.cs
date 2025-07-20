using System;
using System.IO;
using System.Windows.Forms;

namespace OrganizadorArquivos
{
    public class MainForm : Form
    {
        private Button btnSelecionarPasta;
        private Button btnOrganizar;
        private FolderBrowserDialog seletorPasta;
        private string caminhoSelecionado = "";

        public MainForm()
        {
            this.Text = "Organizador de Arquivos";
            this.Size = new System.Drawing.Size(400, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            btnSelecionarPasta = new Button
            {
                Text = "Selecionar Pasta",
                Size = new System.Drawing.Size(150, 30),
                Location = new System.Drawing.Point(50, 40)
            };
            btnSelecionarPasta.Click += BtnSelecionarPasta_Click;

            btnOrganizar = new Button
            {
                Text = "Organizar Arquivos",
                Size = new System.Drawing.Size(150, 30),
                Location = new System.Drawing.Point(200, 40),
                Enabled = false
            };
            btnOrganizar.Click += BtnOrganizar_Click;

            Controls.Add(btnSelecionarPasta);
            Controls.Add(btnOrganizar);
        }

        private void BtnSelecionarPasta_Click(object sender, EventArgs e)
        {
            seletorPasta = new FolderBrowserDialog();
            seletorPasta.Description = "Selecione a pasta a ser organizada: ";

            if (seletorPasta.ShowDialog() == DialogResult.OK)
            {
                caminhoSelecionado = seletorPasta.SelectedPath;
                btnOrganizar.Enabled = true;
            }
        }

        private void BtnOrganizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(caminhoSelecionado))
                return;

            string[] arquivos = Directory.GetFiles(caminhoSelecionado);

            foreach (string arquivo in arquivos)
            {
                string extensao = Path.GetExtension(arquivo).TrimStart('.').ToLower();

                if (string.IsNullOrWhiteSpace(extensao))
                    extensao = "Sem Extensão";

                string pastaDestino = Path.Combine(caminhoSelecionado, extensao);
                Directory.CreateDirectory(pastaDestino);

                string novoCaminho = Path.Combine(pastaDestino, Path.GetFileName(arquivo));

                try
                {
                    File.Move(arquivo, novoCaminho);
                }
                catch
                {
                    // Arquivo pode já existir ou estar em uso
                }
            }

            MessageBox.Show("Organização concluída!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
