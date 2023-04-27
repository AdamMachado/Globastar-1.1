using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

using System.IO; 
using databas;  //Database.

namespace databas
{    
    public partial class Principal : Form
    {
        #region VARIAVEIS_GLOBAIS

        int controlatempo;


        #endregion
        string vinte_minutos_str_con = "0";
        public Principal()
        {
            vinte_minutos v = new vinte_minutos();
            vinte_minutos_str_con = v.str_con();
           
            InitializeComponent();
            





        }
        

        //Carrega configuração principal.
        //---------------------
        private void Form1_Load(object sender, EventArgs e)
        {
           

            //Configuração padrão.
            N_Tempo.Value      = 10;
            L_Seg.Text         = "000";
            Lbl_Globalstar_Armazenamento_Total.Text    = "0";
            Lbl_Globalstar_Armazenamento_erro.Text     = "0";

            L_Data.Text = DateTime.Now.ToString();

            //Limpa tela e carrega configuração armazenada.
            novoGlobalstar();
            
        }
        //Sair.
        //*****
        private void Btn_Sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Painel de log.
        //**************
        private void Btn_Painel_log_Click(object sender, EventArgs e)
        {
            //Para Tempo.
            timer1.Stop();
            timer1.Enabled = false;

            log_auto.principal f = new log_auto.principal();

            f.ShowDialog();
        }

        //Configuração.
        //*************
        private void Btn_Config_Click(object sender, EventArgs e)
        {
            //Stop Tempo.
            timer1.Stop();
            timer1.Enabled = false;

            //Carrega configuração.
            Config_globalstar con = new Config_globalstar();

            if (con.ShowDialog() == DialogResult.Cancel)
            {
                //Carrega configuração novamente após fechar.
                novoGlobalstar();
            }
        }

        //Inicia processamento antes do tempo.
        //************************************
        private void B_Iniciar_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
                timer1.Start();
                Btn_Iniciar.BackColor = Btn_Config.BackColor;
            }
            else
            {
                timer1.Enabled = false;
                timer1.Stop();
                Btn_Iniciar.BackColor = System.Drawing.Color.Red;
            }

            if (timer1.Enabled == true)
            {
                InicioGlobalstar();
                pegar_str_vinte_minutos();

            }

        }
        
        //Controla tempo
        //---------------
        private void Tempo_Tick(object sender, EventArgs e)
        {

            //obtem os segundos atual do contador principal
            int tempoAtual = Convert.ToInt32(L_Seg.Text);

            //função calcular tempo atual
            if (tempoAtual > 0)
            {
                //calcula o intervalo atual -1
                L_Seg.Text = (tempoAtual - 1).ToString();
            }
            else
            {
                if (L_Seg.Text.Length > 0)
                {
                    //calcula o intervalo
                    L_Seg.Text = (N_Tempo.Value).ToString();

                    //stop tempo
                    timer1.Stop();

                    //Verifica condição e faz processamento.
                    if (Convert.ToInt32(L_Seg.Text) == N_Tempo.Value)
                    {
                        InicioGlobalstar();
                        pegar_str_vinte_minutos();
                    }

                    //inicia tempo
                    timer1.Start();
                }
            }




        }

        //Controla Tempo de processamento.
        //********************************
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                if (Btn_Config.BackColor == Btn_Iniciar.BackColor)
                {
                    Btn_Iniciar.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    Btn_Iniciar.BackColor = Btn_Config.BackColor;
                }
            }
        }
        






        //Controla painel de mensagem.
        //****************************
        public  void painel_GlobalstarArmazenagem(string strDados) 
        {
            if (strDados.Length == 16)
            {
                Lbl_Globalstar_Armazenamento_Total.Text      = (int.Parse(Lbl_Globalstar_Armazenamento_Total.Text) + 1).ToString();
                Lbl_Globalstar_Armazenamento_Ok.Text         = (int.Parse(Lbl_Globalstar_Armazenamento_Ok.Text)         + int.Parse(strDados.Substring(0, 1))).ToString();
                Lbl_Globalstar_Armazenamento_falha.Text      = (int.Parse(Lbl_Globalstar_Armazenamento_falha.Text)      + int.Parse(strDados.Substring(1, 1))).ToString();
                Lbl_Globalstar_Armazenamento_erro.Text       = (int.Parse(Lbl_Globalstar_Armazenamento_erro.Text)       + int.Parse(strDados.Substring(2, 1))).ToString();
            }
        }

        //Controla painel de mensagem.
        //****************************
        public  void painel_GlobalstarProcessamento(string strDados) 
        {
            if (strDados.Length == 16)
            {
                Lbl_Globalstar_Processamento_Total.Text = (int.Parse(Lbl_Globalstar_Processamento_Total.Text) + 1).ToString();
                Lbl_Globalstar_Processamento_Ok.Text    = (int.Parse(Lbl_Globalstar_Processamento_Ok.Text)    + int.Parse(strDados.Substring(0, 1))).ToString();
                Lbl_Globalstar_Processamento_falha.Text = (int.Parse(Lbl_Globalstar_Processamento_falha.Text) + int.Parse(strDados.Substring(1, 1))).ToString();
                Lbl_Globalstar_Processamento_erro.Text  = (int.Parse(Lbl_Globalstar_Processamento_erro.Text)  + int.Parse(strDados.Substring(2, 1))).ToString();
            }
        }
        
       




        //FUNÇÃO PRINCIPAL.
        //Inicio do processamento.
        public  void InicioGlobalstar()
        {
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, true,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************
                        
            #region PARAMETRO_ENTRADA

            /*
               //----------------------------------------------------------------------------------------------
              
               //verificar metodo salvaPadraoGlobalstar na class utilitarios 
               
               //----------------------------------------------------------------------------------------------
            */

            #endregion


            //-------------------------------------------------------------------------------------------------------------------------------------------------
            Globalstar.Globalstar p = new Globalstar.Globalstar();                                        //Instância objeto class Globalstar

            string[] _strParametro           = new string[700];                                           //Array com dados a ser enviado a função
            string[] _strRetornoSalvamento;                                                               //Array não inicializado, recebe o retorno do processamento. 
            string[] _strRetornoProcessamento;

            string   _exibeErro              = "";

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, false,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //----------------------------------------------------------------------------------------------

            #region carrega configuração

            if (utilitarios.ultilitarios.abrirArquivo("configGlobal.ini", Application.StartupPath, ref _strParametro, ref _exibeErro) == false)
            {

                MessageBox.Show("Erro em abrir arquivo.");
                this.Close();
                return;

            }
            else
            {
                var intLinha = 0;

                for (var i = 0; i < 700; i++)
                {
                    //Retira mensagem antes do =
                    if (_strParametro[i] != null)
                    {
                        if (_strParametro[i].ToString().Trim().Substring(0, 1) != "[" || _strParametro[i].ToString().Trim().Length > 0)
                        {
                            _strParametro[intLinha] = _strParametro[i].Substring(_strParametro[i].IndexOf('=', 0) + 1, _strParametro[i].Length - _strParametro[i].IndexOf('=', 0) - 1).ToString().Trim();
                            intLinha++;
                        }
                    }
                }
            }
            #endregion      
            
            //----------------------------------------------------------------------------------------------

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, false,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //Limpa conforme parametro de entrada.
            if (CH_Zera.Checked == true || Convert.ToInt32(Lbl_Globalstar_Armazenamento_Total.Text) >= int.Parse(_strParametro[14]))         //Limpa listbox onde as informações de processamento são exibidas.  
            {
                //Limpa Listbox.
                LI_ResultadoProcessamento.Items.Clear();

                //Mensagem total.
                Lbl_Globalstar_Armazenamento_Total.Text      = "0";

                //Quantidade de erros ocorridos no processamento.
                Lbl_Globalstar_Armazenamento_erro.Text       = "0";

                //Quantidade de mensagem processadas.
                Lbl_Globalstar_Armazenamento_Ok.Text         = "0";

                //Quantidade de falhas.
                Lbl_Globalstar_Armazenamento_falha.Text      = "0";
            }

            //************************************************************************************************************************

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, false,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            if (CH_Etapa_1.Checked)
            {
                //(1) ETAPA
                #region Salvamento da informação contida no xml.
                //************************************************************************************************************************
                //Retorno da mensagem.
                _strRetornoSalvamento = p.xmlFtp(_strParametro, Application.StartupPath, CH_Debug.Checked);

             
                //Detecta falha.    
                    if (_strRetornoSalvamento.Length > 0 && _strRetornoSalvamento[0] != "+OK")
                {
                    painel_GlobalstarArmazenagem("0100000000000000"); //Falha.
                }


                if (_strRetornoSalvamento[0].Trim().IndexOf("ERRO", 0) >= 0)
                {

                    //Separa mensagem de erro.
                    var tempDados = _strRetornoSalvamento[0].Split('|');


                    if (tempDados.Length > 3)
                    {


                        if (log_auto.log.permissaoExibirLog(Application.StartupPath, "conf-auto.ini", tempDados[1], tempDados[2]))
                        {
                            //Popula listbox.
                            LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + (_strRetornoSalvamento[0].ToString()).Substring(0, 60));
                        }

                    }
                    else
                    {
                        //Popula listbox.
                        LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + (_strRetornoSalvamento[0].ToString()).Substring(0, 60));
                    }

                }

                //----------------------------------------------------------------------------------------------
                //Processa retorno.
                if (_strRetornoSalvamento.Length > 0 && _strRetornoSalvamento[0] == "+OK")
                {
                    //Começa do 2 por causa de stattus da caixa e status da class.
                    for (var i = 2; i < _strRetornoSalvamento.Length; i++)
                    {

                        //Só ler diferente de Null.
                        if (_strRetornoSalvamento[i] != null)
                        {

                            //Detecta erro no processamento.    
                            if (_strRetornoSalvamento[i].Trim().IndexOf("ERRO", 0) >= 0)
                            {

                                //Separa mensagem de erro.
                                var tempDados = _strRetornoSalvamento[i].Split('|');

                                if (tempDados.Length > 3)
                                {

                                    if (log_auto.log.permissaoExibirLog(Application.StartupPath, "conf-auto.ini", tempDados[1], tempDados[2]))
                                    {
                                        //Popula listbox.
                                        LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + _strRetornoSalvamento[i].ToString().Substring(0, 60));
                                    }

                                }
                                else
                                {
                                    //Popula listbox.
                                    LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + _strRetornoSalvamento[i].ToString().Substring(0, 60));
                                }


                            }
                            else
                            {

                                #region verifica se segunda etapa está bloqueada
                                Globastar_1._0.Class.BloqEtapa2 txt = new Globastar_1._0.Class.BloqEtapa2();
                                string bloq2Etapa = "";
                                if (txt.verificaBloco(_strRetornoSalvamento[i].ToString().Substring(0, 9))==true)
                                {
                                    bloq2Etapa = "* ";
                                }
                                else
                                {
                                    bloq2Etapa = "";
                                }
                                #endregion

                                //Popula listbox
                                LI_ResultadoProcessamento.Items.Insert(0, (bloq2Etapa+_strRetornoSalvamento[i].ToString()).Substring(0, 60));

                            }



                            //Processamento da mensagem no painel.
                            if (i > 0)
                            {
                                painel_GlobalstarArmazenagem(_strRetornoSalvamento[i].Substring(60, 16));
                            }



                        }
                    }

                }
                //************************************************************************************************************************
                #endregion
            }


            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, false,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************


            if (CH_Etapa_2.Checked)
            {
                //(2) ETAPA
                #region Processamento das informações contidas no banco de dados.
                //************************************************************************************************************************
                //Retorno da mensagem.
                _strRetornoProcessamento = p.xmlExtracao(_strParametro, Application.StartupPath, CH_Debug.Checked);

                //Detecta falha.    
                if (_strRetornoProcessamento.Length > 0 && _strRetornoProcessamento[0] != "+OK")
                {
                    painel_GlobalstarProcessamento("0100000000000000"); //Falha.
                }


                if (_strRetornoProcessamento[0].Trim().IndexOf("ERRO", 0) >= 0)
                {

                    //Separa mensagem de erro.
                    var tempDados = _strRetornoProcessamento[0].Split('|');


                    if (tempDados.Length > 3)
                    {


                        if (log_auto.log.permissaoExibirLog(Application.StartupPath, "conf-auto.ini", tempDados[1], tempDados[2]))
                        {
                            //Popula listbox.
                            LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + (_strRetornoProcessamento[0].ToString()).Substring(0, 60));
                        }

                    }
                    else
                    {
                        //Popula listbox.
                        LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + (_strRetornoProcessamento[0].ToString()).Substring(0, 60));
                    }

                }


           
                //----------------------------------------------------------------------------------------------
                //Processa retorno.
                if (_strRetornoProcessamento.Length > 0 && _strRetornoProcessamento[0] == "+OK")
                {
                    //Começa do 1 por causa de status da class.
                    for (var i = 1; i < _strRetornoProcessamento.Length; i++)
                    {

                        
                        //Só ler diferente de Null.
                        if (_strRetornoProcessamento[i] != null)
                        {

                            //Detecta erro no processamento.    
                            if (_strRetornoProcessamento[i].Trim().IndexOf("ERRO", 0) >= 0)
                            {

                                //Separa mensagem de erro.
                                var tempDados = _strRetornoProcessamento[i].Split('|');

                                if (tempDados.Length > 3)
                                {

                                    if (log_auto.log.permissaoExibirLog(Application.StartupPath, "conf-auto.ini", tempDados[1], tempDados[2]))
                                    {
                                        //Popula listbox.
                                        LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + _strRetornoProcessamento[i].ToString().Substring(0, 60));
                                    }

                                }
                                else
                                {
                                    //Popula listbox.
                                    LI_ResultadoProcessamento.Items.Insert(0, DateTime.Now.ToString() + " " + _strRetornoProcessamento[i].ToString().Substring(0, 60));
                                }


                            }
                            else
                            {
                                //Popula listbox.
                                LI_ResultadoProcessamento.Items.Insert(0, (_strRetornoProcessamento[i].ToString()).Substring(0, 60));

                            }


                            //Processamento da mensagem no painel.
                            if (i > 0)
                            {
                                painel_GlobalstarProcessamento(_strRetornoProcessamento[i].Substring(60, 16));
                            }



                        }
                    }

                }
                //************************************************************************************************************************
                #endregion
            }


            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(CH_Debug.Checked, "debug.txt", Application.StartupPath, false,
            "PRINCIPAL.CS", "InicioGlobalstar()", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************


            //************************************************************************************************************************

            //Altera cor do label de erro.
            if (Convert.ToInt32(Lbl_Globalstar_Armazenamento_erro.Text) > 0)
            {
                Lbl_Globalstar_Armazenamento_erro.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Lbl_Globalstar_Armazenamento_erro.ForeColor = System.Drawing.Color.Black;
            }
            
            //Altera cor do label de erro.
            if (Convert.ToInt32(Lbl_Globalstar_Processamento_erro.Text) > 0)
            {
                Lbl_Globalstar_Processamento_erro.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Lbl_Globalstar_Processamento_erro.ForeColor = System.Drawing.Color.Black;
            }
            
            //************************************************************************************************************************


        }


        //Limpa dados da tela.
        //*********************
        public void novoGlobalstar()
        {
            string _exibeErro = "";

            string[] _strParametro = new string[700];

            //----------------------------------------------------------------------------------------------

            //Verifica-se arquivo existe.
            if (File.Exists(Application.StartupPath + "\\" + "configGlobal.ini") == false)
            {
                //Cria um arquivo contendo configuração padrão.
                if (utilitarios.ultilitarios.salvaConfiguracaoPadraoGlobalstar("configGlobal.ini", Application.StartupPath) == false)
                {
                    MessageBox.Show("Erro em criar arquivo", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            //----------------------------------------------------------------------------------------------

            #region carrega configuração

            if (utilitarios.ultilitarios.abrirArquivo("configGlobal.ini", Application.StartupPath, ref _strParametro, ref _exibeErro) == false)
            {

                MessageBox.Show("Erro em abrir arquivo.");
                this.Close();
                return;

            }
            else
            {
                var intLinha = 0;

                for (var i = 0; i < 700; i++)
                {
                    //Retira mensagem antes do =
                    if (_strParametro[i] != null)
                    {
                        if (_strParametro[i].ToString().Trim().Substring(0, 1) != "[" || _strParametro[i].ToString().Trim().Length > 0)
                        {
                            _strParametro[intLinha] = _strParametro[i].Substring(_strParametro[i].IndexOf('=', 0) + 1, _strParametro[i].Length - _strParametro[i].IndexOf('=', 0) - 1).ToString().Trim();
                            intLinha++;
                        }
                    }
                }


                //Após processamento.
                N_Tempo.Value = Convert.ToInt32(_strParametro[13]);
                L_Seg.Text = _strParametro[13];

            }
            #endregion

            //----------------------------------------------------------------------------------------------

        }








        //Metodo para controlar erro gerado pelo sistema.
        //-------------------------
        public void acessa_painel(string strTipo, string strInformacao, string strAviso, string strLocal, string StrMensagem)
        {

            string erroGeral = "";

            log_auto.log acessa_painel = new log_auto.log();

            if (acessa_painel.painelLog(strLocal, strTipo, strInformacao, strAviso + "\n Erro detalhado: " + StrMensagem, ref erroGeral))
            {
                //ERRO AO PROCESSAR MENSAGEM.
            }

        }

        //Metodo para controlar erro gerado pelo sistema.
        //-------------------------
        public void acessa_painel(string strTipo, string strInformacao, string strAviso, string strLocal)
        {

            string erroGeral = "";

            log_auto.log acessa_painel = new log_auto.log();


            if (acessa_painel.painelLog(strLocal, strTipo, strInformacao, strAviso, ref erroGeral))
            {
                //ERRO AO PROCESSAR MENSAGEM.
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
       
        }
         

        
        public void pegar_str_vinte_minutos()
        {
            vinte_minutos v = new vinte_minutos();
            string metodo_20_minutos = v.m_vinte_minutos(vinte_minutos_str_con).Trim();
            if (metodo_20_minutos != "")
            {
                lb_20_m.Items.Insert(0, metodo_20_minutos);
            }
            

        }
                              
   
    }
}
