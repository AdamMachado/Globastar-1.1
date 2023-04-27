using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;


namespace log_auto
{

    public partial class principal : Form
    {

        //Usa Api do windows para executa programa padrão windows(notepad,mspaint.etc).
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd, 
            string lpVerb,
            string lpFile, 
            string lpParameters, 
            string lpDirectory,
            int nShowCmd );


        public principal()
        {
            InitializeComponent();
        }

        public void acessa_painel(string informacao,string tipo, string aviso)
        { 

            string erroGeral      = "";

            log acessa_painel = new log();

            if (acessa_painel.painelLog(Application.StartupPath,tipo,informacao,aviso, ref erroGeral))
            {
               // MessageBox.Show("ok");
            }
            else
            {
                MessageBox.Show("ERRO: " +  erroGeral);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            for (var x = 0; x < 10; x++)
            {
                acessa_painel("cad9", "banco", "erro ao tentar converte base64 do equipamento TELESEED41.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            log_configuracao f = new log_configuracao();

            log.intTipo = 1; // Tipo novo.

            log.dadosTemp = null;

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                string[] dadosTemp;

                //Verifica se já existe e altera.
                //Caso seja novo é adicionado.
                //verifica se já existe.

                if (log.dadosTemp != null)
                {

                    dadosTemp = log.dadosTemp.Split('|');

                    for (var i = Li_Listagem.Items.Count - 1; i >= 0; i--)
                    {


                        if (Li_Listagem.Items[i].ToString().IndexOf(dadosTemp[0] + "|" + dadosTemp[1]) >= 0)
                        {
                            Li_Listagem.Items.RemoveAt(i);
                        }


                    }

                    Li_Listagem.Items.Add(log.dadosTemp);

                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {


            if (Li_Listagem.Text == string.Empty)
            {
                MessageBox.Show("É necessário selecionar a configuração.","Aviso.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                Li_Listagem.Items.RemoveAt(Li_Listagem.SelectedIndex);
            }


        }

        private void principal_Load(object sender, EventArgs e)
        {

            string   _strErro = "";

            string[] _strDados = new string[1000];


            string   _strDadosTemp = "";

            Li_Listagem.Items.Clear();

            

            //Carrega configurações do arquivo===> config-auto.ini
            if (utilitarios.ultilitarios.abrirArquivo("conf-auto.ini", Application.StartupPath, ref _strDados, ref _strErro))
            {
                if (_strDados != null)
                {


                    for (var i = 0; i < _strDados.Length - 1; i++)
                    {

                        //Retira mensagem antes do :
                        if (_strDados[i] != null)
                        {
                            _strDados[i] = _strDados[i].Substring(_strDados[i].IndexOf(':', 0) + 1, _strDados[i].Length - _strDados[i].IndexOf(':', 0) - 1);
                        }

                    }




                    for (var i = 0; i < _strDados.Length -1; i++)
                    {

                        if (_strDados[i] != null)
                        {

                            if (i == 0)
                            {
                                _strDadosTemp = _strDados[i].ToString().Substring(1, _strDados[i].ToString().Length - 1) + "|";
                            }
                            else
                            {
                                if (_strDados[i].ToString().Trim().Length > 0)
                                {

                                    if (_strDados[i].ToString().Substring(0, 1) == "#")
                                    {
                                        Li_Listagem.Items.Insert(0,_strDadosTemp);

                                        _strDadosTemp = _strDados[i].ToString().Substring(1, _strDados[i].ToString().Length - 1) + "|";
                                    }
                                    else
                                    {
                                        _strDadosTemp = _strDadosTemp + _strDados[i].ToString() + "|";
                                    }

                                }
                                else
                                {
                                    _strDadosTemp = _strDadosTemp + "|" + _strDados[i].ToString();
                                }

                            }

                        }
                    }

                }
            }



        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (Li_Listagem.Text == string.Empty)
            {
                MessageBox.Show("É necessário selecionar a configuração.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                log.intTipo = 2; // Tipo Edição.
                
                log.dadosTempEntrada = Li_Listagem.Text.Split('|');
                
                log_configuracao f = new log_configuracao();

                if (f.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    string[] dadosTemp;

                    //Verifica se já existe e altera.
                    //Caso seja novo é adicionado.
                    //verifica se já existe.

                    if (log.dadosTemp != null)
                    {

                        dadosTemp = log.dadosTemp.Split('|');

                        for (var i = Li_Listagem.Items.Count - 1; i >= 0; i--)
                        {


                            if (Li_Listagem.Items[i].ToString().IndexOf(dadosTemp[0] + "|" + dadosTemp[1]) >= 0)
                            {
                                Li_Listagem.Items.RemoveAt(i);
                            }


                        }

                        Li_Listagem.Items.Add(log.dadosTemp);

                    }
                }

            
            }


            




        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Salva configuração do listbox.
            string[] strDados;

            string strErro = "";

            if (Li_Listagem.Items.Count == 0)
            {
                MessageBox.Show("Não existe item para salvar como configuração.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                strErro = "erro";
                
                return;
            }


            //Faz backup do arquivo.
            if (utilitarios.ultilitarios.backup("conf-auto.ini", Application.StartupPath, "BACKUP") == false)
            {
                MessageBox.Show("Erro ao efetuar backup.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                strErro = "erro";
                
                return;
            }



            for (var i = Li_Listagem.Items.Count - 1; i >= 0; i--)
            {
                
                if (Li_Listagem.Items[i].ToString().Length > 0)
                {
                    strDados = null;
                    strDados = Li_Listagem.Items[i].ToString().Split('|');

                    if (strDados != null)
                    {

                        strDados[0]  = "TIPO:#"             + strDados[0];
                        strDados[1]  = "INFORMAÇÃO:"        + strDados[1];
                        strDados[2]  = "MODO:"              + strDados[2];
                        strDados[3]  = "SMTP-ENDEREÇO:"     + strDados[3];
                        strDados[4]  = "SMTP-PORTA:"        + strDados[4];
                        strDados[5]  = "SMTP-USUARIO:"      + strDados[5];
                        strDados[6]  = "SMTP-SENHA:"        + strDados[6];
                        strDados[7]  = "SMTP-SSL:"          + strDados[7];
                        strDados[8]  = "SMTP-REMETENTE:"    + strDados[8];
                        strDados[9]  = "SMTP-DESTINATARIO:" + strDados[9];
                        strDados[10] = "ALERTA:"            + strDados[10];
                        strDados[11] = "ALERTA-MAX:"        + strDados[11];
                        strDados[12] = "ALERTA-ATUAL:"      + strDados[12];
                        strDados[13] = "TEMPO-DE-ALERTA:"   + strDados[13];
                        strDados[14] = "AVISO-TELA:"        + strDados[14];
                        strDados[15] = "OBSERVACAO:"        + strDados[15];
                        strDados[16] = "COPIA:"             + strDados[16];

                        if (i == 0)
                        {
                            strDados[17] = "FIM:#";
                        }

                        //Salva mensagem.
                        if (utilitarios.ultilitarios.salvaArquivo("conf-auto.ini", Application.StartupPath, ref strDados, 0 , ref strErro) == false)
                        {
                            MessageBox.Show("Erro ao salvar arquivo de configuração, restaure o backup.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            strErro = "erro";

                            break;
                        }

                    }

                }

            }

            
            if (strErro.Length == 0)
            {
                MessageBox.Show("Alteração salva com sucesso no arquivo [conf-auto.ini].", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (log.atualizaConfiguracaoCampo(Application.StartupPath, "conf-auto.ini", "banco", "mensagem", "ALERTA:", "20"))
            {

            MessageBox.Show("ok");
            
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string strRetorno = "";

            
            if (log.localizaConfiguracaoCampo(Application.StartupPath, "conf-auto.ini", "banco", "mensagem", "ALERTA:", ref strRetorno))
            {
                MessageBox.Show(strRetorno);
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {

            string[] strRetorno = new string[50];


            if (log.localizaConfiguracao(Application.StartupPath, "conf-auto.ini", "banco", "mensagem", ref strRetorno))
            {
                MessageBox.Show("ok");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(log.permissaoExibirLog(Application.StartupPath, "conf-auto.ini", "banco", "mensagem").ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            acessa_painel("mensagem", "pop3", "erro ao tentar converte base64 do equipamento TELESEED41.");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Li_Listagem.Text == string.Empty)
            {
                MessageBox.Show("É necessário selecionar a configuração.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var strTemp = Li_Listagem.Text.Trim().Split('|');

                var strCaminho = Application.StartupPath + @"\" + strTemp[0] + "-" + strTemp[1] + ".txt";

                if (File.Exists(strCaminho) && strTemp.Length > 1)
                    {
                        ShellExecute(this.Handle, "open", "notepad.exe", strCaminho, "", 1);
                    }

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            acessa_painel("cax", "banco", "erro ao tentar converte base64 do equipamento TELESEED41.");


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //Teste envio de alerta.
            acessa_painel("mensagem", "pop3", "erro ao tentar converte base64 do equipamento TELESEED-tango01-64alfa98.");
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            //Teste envio de alerta.
            acessa_painel("teste", "teste-1", "erro ao tentar converte base64 do equipamento TELESEED-tango01.");
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            //Teste envio de alerta.
            acessa_painel("teste", "teste-2", "erro ao tentar converte base64 do equipamento TELESEED-tango02.");
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //Teste envio de alerta.
            acessa_painel("teste", "teste-3", "erro ao tentar converte base64 do equipamento TELESEED-tango03.");
        }

    }
}
