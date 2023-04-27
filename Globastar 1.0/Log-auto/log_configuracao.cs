using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace log_auto
{
    public partial class log_configuracao : Form
    {
        public log_configuracao()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

         string strDados;

            
            if (Com_Tipo.Text == string.Empty)
            {
                MessageBox.Show("Tipo não selecionado", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Txt_Smtp_Endereco.Text == string.Empty)
            {
                MessageBox.Show("Servidor SMTP em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Txt_Smtp_Usuario.Text == string.Empty)
            {
                MessageBox.Show("Usuário do servidor SMTP em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Txt_Smtp_Senha.Text == string.Empty)
            {
                MessageBox.Show("Senha do servidor SMTP em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Txt_Smtp_Remetente.Text == string.Empty)
            {
                MessageBox.Show("Remente do servidor SMTP em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Txt_Smtp_Destinatario.Text == string.Empty)
            {
                MessageBox.Show("Destinatário do servidor SMTP em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Txt_Informacao.Text == string.Empty)
            {
                MessageBox.Show("Informação em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Txt_Tipo.Text == string.Empty)
            {
                MessageBox.Show("Tipo em branco.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            strDados  =             Txt_Informacao.Text.Trim() + "|";
            strDados  = strDados  + Txt_Tipo.Text.Trim() + "|";

            strDados  = strDados  + Com_Tipo.Text + "|";
            strDados  = strDados  + Txt_Smtp_Endereco.Text + "|";
            strDados  = strDados  + N_Smtp_Porta.Value.ToString() + "|";
            strDados  = strDados  + Txt_Smtp_Usuario.Text + "|";
            strDados  = strDados  + Txt_Smtp_Senha.Text + "|";
            strDados  = strDados  + Ch_Ssl.Checked.ToString() + "|";
            strDados  = strDados  + Txt_Smtp_Remetente.Text + "|";
            strDados  = strDados  + Txt_Smtp_Destinatario.Text + "|";
            strDados  = strDados  + N_Alerta.Value.ToString() + "|";
            strDados  = strDados  + N_Alerta_Max.Value.ToString() + "|";
            strDados  = strDados  + Txt_Alerta_Atual.Text + "|";
            strDados  = strDados  + N_Tempo.Value.ToString() + "|";
            strDados  = strDados  + CH_Exibir_Erro.Checked.ToString() + "|";
            strDados  = strDados  + Txt_Aviso.Text + "|";
            strDados  = strDados  + Txt_Copia.Text + "|";
            strDados  = strDados  + "" + "|";

            log.dadosTemp = strDados;


            this.Close();

        }

        public void novo()
        {

            //Tipo de salvamento 1-salva txt, 2- envia e-mail e 3- salva e envia.
            Com_Tipo.Items.Clear();
            
            Com_Tipo.Items.Add("1");
            Com_Tipo.Items.Add("2");
            Com_Tipo.Items.Add("3");

            Com_Tipo.Text = "3";

            Txt_Smtp_Endereco.Text     = "";
            N_Smtp_Porta.Value         = 0;
            Txt_Smtp_Usuario.Text      = "";
            Txt_Smtp_Senha.Text        = "";
            Ch_Ssl.Checked             = false;
            Txt_Smtp_Remetente.Text    = "";
            Txt_Smtp_Destinatario.Text = "";
            Txt_Informacao.Text        = "";
            Txt_Tipo.Text              = "";
            N_Alerta.Value             = 0;
            N_Alerta_Max.Value         = 1;
            Txt_Alerta_Atual.Text      = DateTime.Now.ToString();
            N_Tempo.Value              = 1;
            CH_Exibir_Erro.Checked     = true;
            Txt_Aviso.Text             = "";
            Txt_Copia.Text             = "";

        }

        private void Btn_Novo_Click(object sender, EventArgs e)
        {
            novo();
        }

        private void log_configuracao_Load(object sender, EventArgs e)
        {
            novo();

            //Carrega configuração na tela.
            if (log.intTipo == 2)
            {

                Txt_Informacao.Text        = log.dadosTempEntrada[0];
                Txt_Tipo.Text              = log.dadosTempEntrada[1];
                Com_Tipo.Text              = log.dadosTempEntrada[2];
                Txt_Smtp_Endereco.Text     = log.dadosTempEntrada[3];
                N_Smtp_Porta.Value         = int.Parse(log.dadosTempEntrada[4]);
                Txt_Smtp_Usuario.Text      = log.dadosTempEntrada[5];
                Txt_Smtp_Senha.Text        = log.dadosTempEntrada[6];
                Ch_Ssl.Checked             = Convert.ToBoolean(log.dadosTempEntrada[7]);
                Txt_Smtp_Remetente.Text    = log.dadosTempEntrada[8];
                Txt_Smtp_Destinatario.Text = log.dadosTempEntrada[9];
                N_Alerta.Value             = int.Parse(log.dadosTempEntrada[10]);
                N_Alerta_Max.Value         = int.Parse(log.dadosTempEntrada[11]);
                Txt_Alerta_Atual.Text      = log.dadosTempEntrada[12];
                N_Tempo.Value              = int.Parse(log.dadosTempEntrada[13]);
                CH_Exibir_Erro.Checked     = Convert.ToBoolean(log.dadosTempEntrada[14]);
                Txt_Aviso.Text             = log.dadosTempEntrada[15];
                Txt_Copia.Text             = log.dadosTempEntrada[16];

            }



        }
    }
}
