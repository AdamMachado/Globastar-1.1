using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using Microsoft.VisualBasic;

namespace databas
{
    public partial class Config_globalstar : Form
    {


        public Config_globalstar()
        {
            InitializeComponent();
        }
        


        //carrega configuração principal.
        //*******************************
        private void Config_Load(object sender, EventArgs e)
        {
            #region Painel bloq etapa2
            foreach (var varreConsultaTxt in consultaTxt())
            {
                cbExcluir.Items.Add(varreConsultaTxt);
            }
            #endregion


            string   _strExibeErro = "";

            string[] _parametro    = new string[700];

            //Conexão ao banco Sql.
            T_Banco_Hostname.Text  = "";
            T_Banco_Usuario.Text   = "";
            T_Banco_Senha.Text     = "";
            T_Banco_Banco.Text     = "";
            T_Banco_Encrypt.Text   = "";
            N_Porta.Value          = 0;

            //Processamento.
            N_Quantidade.Value     = 1;
            N_Tempo.Value          = 10;
            N_Limpa_Tela.Value     = 100;

            //limpa ListBox.
            LI_Bloqueio.Items.Clear();
            LI_Permissao.Items.Clear();

            //Configuração Banco de dados, dados Brutos.
            T_Banco_Tabela.Text = "";

            //carrega configuração, contida no arquivo .ini
            if (utilitarios.ultilitarios.abrirArquivo("configGlobal.ini", Application.StartupPath, ref _parametro, ref _strExibeErro) == false)
            {
                MessageBox.Show("Erro em abrir arquivo.");
                this.Close();
                return;
            }
            else
            {
                var intLinha = 0;

                for (var i = 0; i < 500; i++)
                {

                    //Retira mensagem antes do :
                    if (_parametro[i] != null)
                    {
                        if (_parametro[i].ToString().Trim().Substring(0, 1) != "[" || _parametro[i].ToString().Trim().Length > 0)
                        {
                            _parametro[intLinha] = _parametro[i].Substring(_parametro[i].IndexOf('=', 0) + 1, _parametro[i].Length - _parametro[i].IndexOf('=', 0) - 1).ToString().Trim();
                            intLinha++;
                        }
                    }

                }

                //*****************************************************************************

                try
                {

                    //Conexão ao banco Sql
                    T_Banco_Hostname.Text    = _parametro[1];
                    T_Banco_Usuario.Text     = _parametro[2];
                    T_Banco_Senha.Text       = _parametro[3];
                    T_Banco_Banco.Text       = _parametro[4];
                    N_Timeout.Value          = Convert.ToInt32(_parametro[5]);
                    T_Banco_Encrypt.Text     = _parametro[6];
                    N_Porta.Value            = Convert.ToInt32(_parametro[7]);

                    //Processamento ou tempo.
                    N_Quantidade.Value       = Convert.ToInt32(_parametro[12]);
                    N_Tempo.Value            = Convert.ToInt32(_parametro[13]);
                    N_Limpa_Tela.Value       = Convert.ToInt32(_parametro[14]);

                    //Procedure.
                    T_Proc_Panico.Text       = _parametro[23];
                    T_Proc_Descricao.Text    = _parametro[24];
                    T_Proc_Rastreamento.Text = _parametro[25];
                    T_Proc_Online.Text       = _parametro[26];
                    T_Proc_Cerca.Text        = _parametro[27];
                    T_Proc_Velocidade.Text   = _parametro[28];

                    //Configuração Banco de dados, dados Brutos.
                    T_Banco_Tabela.Text = _parametro[33];
                
                    //Caminho do arquivo xml.
                    T_Ftp_Caminho1.Text = _parametro[44];
                    T_Ftp_Caminho2.Text = _parametro[45];

                    //Conexão ao banco Sql, procedure.
                    T_Con_Servidor_Procedure.Text = _parametro[60];
                    T_Con_Usuario_Procedure.Text  = _parametro[61];
                    T_Con_Senha_Procedure.Text    = _parametro[62];

                    //Configuração Banco de dados, procedure.
                    T_Bruto_Banco_Procedure.Text     = _parametro[63];
                    N_Timeout_Procedure.Value        = Convert.ToInt32(_parametro[64]);
                    T_Bruto_C_Encrypt_Procedure.Text = _parametro[65];


                    //Permissão 71 - 83
                    if (_parametro[71].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[71]);
                    if (_parametro[72].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[72]);
                    if (_parametro[73].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[73]);
                    if (_parametro[74].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[74]);
                    if (_parametro[75].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[75]);
                    if (_parametro[76].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[76]);
                    if (_parametro[77].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[77]);
                    if (_parametro[78].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[78]);
                    if (_parametro[79].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[79]);
                    if (_parametro[80].Trim().Length > 0) LI_Permissao.Items.Add(_parametro[80]);

                    //***********************************************************************************************************
                    //Bloqueio no processamento.
                    for (var i = 85; i < 700; i++)
                    {
                        //Restrição > 85
                        if (_parametro[i] != null && _parametro[i] != string.Empty)
                        {
                            LI_Bloqueio.Items.Add(_parametro[i]);
                        }
                    }
                    //***********************************************************************************************************
                
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro em carregar dados de configuração.");
                    return;
                }

                //*****************************************************************************


            } //Read

        }
        



        private void Btn_Permissao_Novo_Click(object sender, EventArgs e)
        {
            //função não pertence a camada de dados e Class converte.

            string _strEntrada;

            _strEntrada = Interaction.InputBox("informe a sigla do equipamento para efetuar processamento:  telebot , telessed , globalstar");


            if (_strEntrada == string.Empty)
            {
                MessageBox.Show("Não foi localizado sigla do equipamento, tente novamente.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LI_Permissao.Items.Add(_strEntrada);

            MessageBox.Show("Alteração efetuada com sucesso.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Btn_Permissao_Remover_Click(object sender, EventArgs e)
        {
            //função não pertence a camada de dados e Class converte.
            if (LI_Permissao.Text == string.Empty)
            {
                MessageBox.Show("Não foi localizado o nome do equipamento, tente novamente.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LI_Permissao.Items.RemoveAt(LI_Permissao.SelectedIndex);
                MessageBox.Show("Exclusão efetuada com sucesso.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }            
        }




        private void Btn_Bloqueio_Novo_Click(object sender, EventArgs e)
        {
            //função não pertence a camada de dados e Class converte.

            string _strEntrada;

            _strEntrada = Interaction.InputBox("informe a nome do equipamento para efetuar bloqueio de processamento");

            if (_strEntrada == string.Empty)
            {
                MessageBox.Show("Não foi localizado o nome do equipamento, tente novamente.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Bloqueio efetuada com sucesso.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            LI_Bloqueio.Items.Add(_strEntrada);
        }

        private void Btn_Bloqueio_Remover_Click(object sender, EventArgs e)
        {

            //função não pertence a camada de dados e Class converte.
            if (LI_Bloqueio.Text == string.Empty)
            {
                MessageBox.Show("Não foi localizado o nome do equipamento, tente novamente.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LI_Bloqueio.Items.RemoveAt(LI_Bloqueio.SelectedIndex);
                MessageBox.Show("Exclusão efetuada com sucesso.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }




        //Salva e altera arquivo de configuralçao.
        //--------------------------------
        private void Btn_Salvar_Click(object sender, EventArgs e)
        {

            string[] _strEntrada = new string[700];

            string _strExibeErro = "";

            //validação dos campos

            //Conexão.
            if (T_Banco_Hostname.Text == string.Empty) { MessageBox.Show("Campo em branco, Endereço", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Banco_Usuario.Text == string.Empty) { MessageBox.Show("Campo em branco, Usuário", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Banco_Senha.Text == string.Empty) { MessageBox.Show("Campo em branco, Senha", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //Dados brutos.
            if (T_Banco_Banco.Text == string.Empty) { MessageBox.Show("Campo em branco, Banco", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Banco_Tabela.Text == string.Empty) { MessageBox.Show("Campo em branco, Tabela", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Banco_Encrypt.Text == string.Empty) { MessageBox.Show("Campo em branco, Encrypt", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //Caminho.
            if (T_Ftp_Caminho1.Text == string.Empty) { MessageBox.Show("Campo em branco, caminho-1.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //Procedimentos.
            if (T_Proc_Panico.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Pânico.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Proc_Descricao.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Descricao.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Proc_Rastreamento.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Rastreamento.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Proc_Online.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Online.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Proc_Cerca.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Cerca.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (T_Proc_Velocidade.Text == string.Empty) { MessageBox.Show("Campo em branco, procedimento Velocidade.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //**********************************************************************************************

            _strEntrada[0] = "[Conexão]";
            _strEntrada[1] = "";
            _strEntrada[2] = "Servidor=" + T_Banco_Hostname.Text;
            _strEntrada[3] = "Usuario =" + T_Banco_Usuario.Text;
            _strEntrada[4] = "Senha   =" + T_Banco_Senha.Text;
            _strEntrada[5] = "Banco   =" + T_Banco_Banco.Text;
            _strEntrada[6] = "Timeout =" + N_Timeout.Value;
            _strEntrada[7] = "Encrypt =" + T_Banco_Encrypt.Text;
            _strEntrada[8] = "Porta   =" + N_Porta.Value;
            _strEntrada[10] = "reserva =";
            _strEntrada[11] = "reserva =";
            _strEntrada[12] = "reserva =";

            _strEntrada[13] = "[Processamento]";
            _strEntrada[14] = "";
            _strEntrada[15] = "Quantidade=" + N_Quantidade.Value;
            _strEntrada[16] = "Tempo     =" + N_Tempo.Value;
            _strEntrada[17] = "Limpa Tela=" + N_Limpa_Tela.Value;
            _strEntrada[18] = "reserva   =";
            _strEntrada[19] = "reserva   =";
            _strEntrada[20] = "reserva   =";
            _strEntrada[21] = "reserva   =";
            _strEntrada[22] = "reserva   =";
            _strEntrada[23] = "reserva   =";
            _strEntrada[24] = "reserva   =";
            _strEntrada[25] = "";

            _strEntrada[26] = "[Procedure]";
            _strEntrada[27] = "";
            _strEntrada[28] = "T_Proc_Panico      =" + T_Proc_Panico.Text;
            _strEntrada[29] = "T_Proc_Descricao   =" + T_Proc_Descricao.Text;
            _strEntrada[30] = "T_Proc_Rastreamento=" + T_Proc_Rastreamento.Text;
            _strEntrada[31] = "T_Proc_Online      =" + T_Proc_Online.Text;
            _strEntrada[32] = "T_Proc_Cerca       =" + T_Proc_Cerca.Text;
            _strEntrada[33] = "T_Proc_Velocidade  =" + T_Proc_Velocidade.Text;
            _strEntrada[34] = "reserva =";
            _strEntrada[35] = "reserva =";
            _strEntrada[36] = "reserva =";
            _strEntrada[37] = "";

            _strEntrada[38] = "[Tabela bruto]";
            _strEntrada[39] = "";
            _strEntrada[40] = "banco Tabela=" + T_Banco_Tabela.Text;
            _strEntrada[41] = "banco campo =raw_data";
            _strEntrada[42] = "banco campo =raw_data_hex";
            _strEntrada[43] = "banco campo =codigo_vei";
            _strEntrada[44] = "banco campo =msg_data";
            _strEntrada[45] = "banco campo =fl_seed";
            _strEntrada[46] = "banco campo =id_raw";
            _strEntrada[47] = "reserva=";
            _strEntrada[48] = "reserva=";
            _strEntrada[49] = "reserva=";
            _strEntrada[50] = "";

            _strEntrada[51] = "[Pasta ftp]";
            _strEntrada[52] = "";
            _strEntrada[53] = "pasta FTP1=" + T_Ftp_Caminho1.Text;
            _strEntrada[54] = "pasta FTP2=" + T_Ftp_Caminho2.Text;
            _strEntrada[55] = "";

            _strEntrada[56] = "[Campo da tabela]";
            _strEntrada[57] = "banco campo=codigo_vei";
            _strEntrada[58] = "banco campo=sat_data";
            _strEntrada[59] = "banco campo=mha_data";
            _strEntrada[60] = "banco campo=msg_data";
            _strEntrada[61] = "banco campo=raw_data";
            _strEntrada[62] = "banco campo=uidl";
            _strEntrada[63] = "banco campo=encoding";
            _strEntrada[64] = "banco campo=raw_data_hex";
            _strEntrada[65] = "banco campo=fl_decod";
            _strEntrada[66] = "banco campo=fl_seed";
            _strEntrada[67] = "reserva=";
            _strEntrada[68] = "reserva=";
            _strEntrada[69] = "";

            _strEntrada[70] = "[Conexão dados Procedure]";
            _strEntrada[71] = "";
            _strEntrada[72] = "Servidor=" + T_Con_Servidor_Procedure.Text;
            _strEntrada[73] = "Usuario =" + T_Con_Usuario_Procedure.Text;
            _strEntrada[74] = "Senha   =" + T_Con_Senha_Procedure.Text;
            _strEntrada[75] = "Banco   =" + T_Bruto_Banco_Procedure.Text;
            _strEntrada[76] = "Timeout =" + N_Timeout_Procedure.Value;
            _strEntrada[77] = "Encrypt =" + T_Bruto_C_Encrypt_Procedure.Text;
            _strEntrada[78] = "Porta   =0";
            _strEntrada[79] = "reserva =";
            _strEntrada[80] = "reserva =";
            _strEntrada[81] = "reserva =";
            _strEntrada[82] = "";

            _strEntrada[83] = "[Permissão de processamento]";
            _strEntrada[84] = "";
            _strEntrada[85] = "permissão=";
            _strEntrada[86] = "permissão=";
            _strEntrada[87] = "permissão=";
            _strEntrada[88] = "permissão=";
            _strEntrada[89] = "permissão=";
            _strEntrada[90] = "permissão=";
            _strEntrada[91] = "permissão=";
            _strEntrada[92] = "permissão=";
            _strEntrada[93] = "permissão=";
            _strEntrada[94] = "permissão=";
            _strEntrada[95] = "reserva=";
            _strEntrada[96] = "reserva=";
            _strEntrada[97] = "reserva=";
            _strEntrada[98] = "";

            for (var i = 0; i < 98; i++)
            {
                try
                {
                    if (LI_Permissao.Items[i] != string.Empty)
                        _strEntrada[85 + i] = "permissão=" + LI_Permissao.Items[i];
                }
                catch (Exception)
                {

                }

            }

            _strEntrada[99] = "[Bloqueio no processamento]";
            _strEntrada[100] = "";
            _strEntrada[101] = "bloqueio =";
            _strEntrada[102] = "";



            //Loop. 88 - 700
            _strEntrada[101] = "bloqueio =";
            for (var i = 0; i < LI_Bloqueio.Items.Count; i++)
            {

                try
                {
                    if (LI_Bloqueio.Items[i] != string.Empty)
                        _strEntrada[101 + i] = "bloqueio=" + LI_Bloqueio.Items[i];
                }
                catch (Exception)
                {

                }

            }

            //**********************************************************************************************

            //Salva arquivo.

            //***********************************************************************************

            if (utilitarios.ultilitarios.salvaArquivo("configGlobal.ini", Application.StartupPath, ref _strEntrada, 1, ref _strExibeErro) == false)
            {
                MessageBox.Show("Erro em salvar configurações.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Alteração efetuada com sucesso.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //***********************************************************************************




        }
        
        //Sair.
        //*****
        private void Btn_Sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region painel bloqueia etapa2
        #region Consultar txt com os equipamento bloqueado para segunda etapa
        public ArrayList consultaTxt()
        {


            ArrayList lista = new ArrayList();

            StreamReader st = new StreamReader("bloqueioEtapa2.txt");
            string linha = st.ReadLine();

            while (linha != null)
            {

                lista.Add(linha);
                linha = st.ReadLine();

            }
            st.Close();
            return lista;

        }
        #endregion

        #region inserir bloqueio de equipamento no txt
        public string inserirTxt(string dadosInseri)
        {
            StreamWriter sw = new StreamWriter("bloqueioEtapa2.txt");

            sw.WriteLine(dadosInseri);
            sw.Close();

            return "Ação executada com sucesso";


        }
        #endregion
        private void btnSalva_Click(object sender, EventArgs e)
        {
            #region inseri txt
            string formataInserção = "";
            int i = 1;
            foreach (var varreCbl in cbExcluir.Items)
            {
                formataInserção += cbExcluir.Items[i - 1] + "\n";
                i++;
            }
            formataInserção += txtInseri.Text;

            MessageBox.Show("" + inserirTxt(formataInserção));
            #endregion

            #region inseri checkboxlist
            cbExcluir.Items.Add(txtInseri.Text);
            #endregion
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            #region inseri txt
            string formataInserção = "";
            int i = 0;
            ArrayList pulaLinha = new ArrayList();


            #region excluir txt
            foreach (var varreCbl in cbExcluir.Items)
            {
                #region lógica para pula linhas...
                pulaLinha.Insert(i, "\n");
                if (cbExcluir.Items.Count - 1 == i)
                {
                    pulaLinha.Insert(i, "");
                }
                #endregion

                #region Caso cbExcluir estiver chechado faça nada
                if (cbExcluir.GetItemChecked(i))
                {

                }
                #endregion

                #region Caso cbExcluir não estiver chechado recoloqueo no txt
                else
                {
                    formataInserção += cbExcluir.Items[i] + "" + pulaLinha[i];

                }
                #endregion

                i++;

            }
            #endregion

            MessageBox.Show("" + inserirTxt(formataInserção));


            #region zera checkboxlist... re-ler txt ... regrava chekcboxlist
            cbExcluir.Items.Clear();
            foreach (var varreConsultaTxt in consultaTxt())
            {

                cbExcluir.Items.Add(varreConsultaTxt);
            }
            #endregion

            #endregion
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }


}
