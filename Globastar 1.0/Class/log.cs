using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;
using System.Net.Mime;
using System.Net.Configuration;
using System.Net;

using System.IO;

using database;

namespace log_auto
{
    class log
    {
        
        //Apenas id para uso da interface.
        static public int intId;
        static public int intTipo;
        
        //Apenas para uso da interface.
        static public string   dadosTemp;
        static public string[] dadosTempEntrada;  
        
        //Comunicação entre modulos.
        //-----------------------------
        public  bool painelLog(string strCaminho,string strTipo, string strInformacao,string strAviso, ref string erroGeral)
        {
            //Status inicia positivo.
            bool _status  = true;

            //Valida Caminho.
            if (strCaminho == string.Empty)
            return false;

            
            //Valida tipo.
            if (strTipo == string.Empty)
            return false;


            //Valida Informação.
            if (strInformacao == string.Empty)
            return false;


            //Valida Aviso.
            if (strAviso == string.Empty)
            return false;


            //Efetua cálculo, para efetuar disparo de alerta.
            if (calcular_Alerta(strCaminho, strTipo, strInformacao, strAviso, ref erroGeral) == false)
            return false;
            

            return _status;        
        }

        //Envia mensagem.
        //-----------------------------
        private bool send_Mail(ref string[] strParametro, string strCorpo, ref string erroGeral)
        {

            bool _status = true;          
            
            SmtpClient cliente = new SmtpClient();

            try
            {

                
                cliente.Host        = strParametro[3];   //Endereço do servidor smtp.
                cliente.EnableSsl   = Convert.ToBoolean(strParametro[7]); //Conexão SSL.

                cliente.Credentials = new NetworkCredential(strParametro[5], strParametro[6]); //Usuário e senha.

                MailMessage message = new MailMessage();

             // message.Sender      = new MailAddress(strParametro[8], "Painel de Log"); //De.
                message.From        = new MailAddress(strParametro[8], "Painel de Log"); //Quem envia.

                message.To.Add(new MailAddress(strParametro[9]));  //Para.

                if (strParametro[16].Trim().Length > 0)
                {
                    message.To.Add(strParametro[16]);             //Copia, separa com virgula.
                }
                
                message.Body       = strCorpo;                    //Corpo da mensagem.
                message.Subject    = "Controle de log.";          //Texto.
                message.IsBodyHtml = true;                        //Mensagem do corpo e html.
                message.Priority   = MailPriority.High;           //Prioridade.

                cliente.Send(message);                            //Envia mensagem.
            }
            catch (Exception e)
            {
                erroGeral = "ERRO: E-MAIL: " + e.Message;
                _status = false;
            }

          return _status;   
        }

        //Zera alerta principal.
        //-----------------------------
        public  bool  zeraAlerta(string strCaminho, string strTipo, string strInformacao, ref string erroGeral)
        {

            bool _status = true;   

            //Pede para localizar configuração conforme parametros, e pede para zera.
            if (atualizaConfiguracaoCampo(strCaminho, "conf-auto.ini", strTipo, strInformacao, "ALERTA:", "0") == false)
            {
                _status = false;
            }

            return _status;
        } 

        //Calcular alerta.
        //-----------------------------
        private bool calcular_Alerta(string strCaminho, string strTipo, string strInformacao, string strAviso, ref string erroGeral)
        {
            //Contém informações sobre configuração.
            string[] informacaoConf = new string[20];

            int _intAlert         = 0; // Alerta atual.
            int _intAlerta_Mim    = 0; // Alerta max.
            int _intTemp          = 0; // Tempo de alerta , tempo em minutos.

            string _strEmail;          //E-mail a ser enviado.
           
            //Verifica-se já existe alguma configuração cadastrada.
            //-----------------------------------------------------
            if (existeConfiguracaoAlerta(strCaminho, strTipo, strInformacao, ref erroGeral) == false)
            return false;


            //Atualiza ---> informacaoConf.
            //-----------------------------
            if (existeAlerta(strCaminho, strTipo, strInformacao, ref informacaoConf, ref erroGeral) == false)
            return false;


            //Salva alerta atual.
            //---------------------
            if (atualiza_Alerta(strCaminho, strTipo, strInformacao, informacaoConf[12], int.Parse(informacaoConf[13]), int.Parse(informacaoConf[10]), ref erroGeral) == false)
            return false;


            //Atualiza --> strInformacao com novos alertas.
            //--------------------------------------------
            if (existeAlerta(strCaminho, strTipo, strInformacao, ref informacaoConf, ref erroGeral))
            {

                try
                {

                    _intAlert            = int.Parse(informacaoConf[10]); // Alerta atual
                    _intAlerta_Mim       = int.Parse(informacaoConf[11]); // Alerta max
                    _intTemp             = int.Parse(informacaoConf[13]); // Tempo de alerta , tempo em minutos.
                    _strEmail            = informacaoConf[9];             //e-mail Destinatário.

                }
                catch (Exception)
                {
                    return false;
                }

                //Verifica-se existe alerta a ser enviado.
                if (_intAlert >= _intAlerta_Mim)
                {

                    //Zera alerta.
                    if (zeraAlerta(strCaminho, strTipo, strInformacao, ref erroGeral) == false)
                    {
                        return false;
                    }

                    //Verifica se é pra salvar.
                    if (informacaoConf[2] == "1")
                    {
                        if (salvar_Alerta(strCaminho, strTipo, strInformacao, strAviso , ref erroGeral) == false)
                        {
                            return false;
                        }
                    }

                    //Verifica se é pra salva.
                    if (informacaoConf[2] == "1" || informacaoConf[2] == "3")
                    {
                        if (salvar_Alerta(strCaminho, strTipo, strInformacao, strAviso, ref erroGeral) == false)
                        {
                            return false;
                        }
                    }

                    //Apenas envia e-mail.
                    if (informacaoConf[2] == "2" || informacaoConf[2] == "3")
                    {
                            //Montagem do detalhe + historico.
                        if (send_Mail(ref informacaoConf, monta_Mensagem_Principal(ref informacaoConf, strAviso, ref erroGeral), ref erroGeral) == false)
                            {
                                return false;
                            }
                        
                    }

                }
                else
                {
                    //Apenas salva o alerta-detalhado.
                    if (salvar_Alerta(strCaminho, strTipo, strInformacao, strAviso, ref erroGeral) == false)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        //Verifica-se já existe essa configuração.
        //-----------------------------
        private bool existeConfiguracaoAlerta(string strCaminho, string strTipo, string strInformacao, ref string erroGeral)
        {
            bool _status   = true;

            string[] _strDados = new string[50];


            //Verifica retorno
            if (log.localizaConfiguracao(strCaminho, "conf-auto.ini", strTipo, strInformacao, ref _strDados) == false)
            {


                try
                {

                    //Cria uma nova configuração PADRÃO.           

                    _strDados[0]  = "TIPO:#"      + strTipo; 
                    _strDados[1]  = "INFORMAÇÃO:" + strInformacao; 
                    _strDados[2]  = "MODO:3" ;
                    _strDados[3]  = "SMTP-ENDEREÇO:192.168.100.8";
                    _strDados[4]  = "SMTP-PORTA:25";
                    _strDados[5]  = "SMTP-USUARIO:silas@ariasat.com.br";
                    _strDados[6]  = "SMTP-SENHA:123456";
                    _strDados[7]  = "SMTP-SSL:false";
                    _strDados[8]  = "SMTP-REMETENTE:silas@ariasat.com.br";
                    _strDados[9]  = "SMTP-DESTINATARIO:silas@ariasat.com.br";
                    _strDados[10] = "ALERTA:0";
                    _strDados[11] = "ALERTA-MAX:5";
                    _strDados[12] = "ALERTA-ATUAL:01/01/2012 12:12:33";
                    _strDados[13] = "TEMPO-DE-ALERTA:1";
                    _strDados[14] = "AVISO-TELA:true";
                    _strDados[15] = "OBSERVACAO:";
                    _strDados[16] = "SMTP-COPIA:helio@ariasat.com.br,suporte@ariasat.com.br";                  //EXISTE UM PULO AQUI.

                }
                catch (Exception)
                {
                    return false;
                }


                //Cria um novo arquivo.
                if (File.Exists(strCaminho + @"\" + "conf-auto.ini") == false)
                {
                    _strDados[17] = "FIM:#";

                    //Cria um nova configuração.
                    if (utilitarios.ultilitarios.salvaArquivo("conf-auto.ini", strCaminho, ref _strDados,0, ref erroGeral) == false)
                    {
                        _status = false;
                    }

                }
                else
                {

                    //Cria um nova configuração sobre arquivo já existente.
                    if (salvaNovaConfiguracao(strTipo, strInformacao, strCaminho, "conf-auto.ini", ref _strDados) == false)
                    {
                        _status = false;
                    }
                
                }

                
            }

           

            return _status;
        
        }

        //Verifica-se já existe alerta.
        //-----------------------------
        private bool existeAlerta(string strCaminho, string strTipo, string strInformacao, ref string[] strInformacaoConf, ref string erroGeral) 
        {

            bool _status   = true;

            string[] _strDados = new string[50];

            //Localiza configuracao, conforme parametro.
            if (localizaConfiguracao(strCaminho, "conf-auto.ini", strTipo, strInformacao, ref _strDados))
            {
                //Pega erro.
                try
                {

                    //Loop dos registros retornados.
                    for (var i = 0; i < _strDados.Length - 1; i++)
                    {

                        //Retira mensagem antes do :
                        if (_strDados[i] != null)
                        {
                            _strDados[i] = _strDados[i].Substring(_strDados[i].IndexOf(':', 0) + 1, _strDados[i].Length - _strDados[i].IndexOf(':', 0) - 1);
                        }

                    }


                    strInformacaoConf[0]  = _strDados[0].Substring(1,_strDados[0].ToString().Length - 1);      // Tipo.
                    strInformacaoConf[1]  = _strDados[1];      // informação.
                    strInformacaoConf[2]  = _strDados[2];      // Tipo/modo.
                    strInformacaoConf[3]  = _strDados[3];      // Endereço SMTP.
                    strInformacaoConf[4]  = _strDados[4];      // Porta SMTp.
                    strInformacaoConf[5]  = _strDados[5];      // Usuário SMTP.
                    strInformacaoConf[6]  = _strDados[6];      // Senha SMTP. 
                    strInformacaoConf[7]  = _strDados[7];      // SSL.
                    strInformacaoConf[8]  = _strDados[8];      // Remetente.
                    strInformacaoConf[9]  = _strDados[9];      // Destinatário.
                    strInformacaoConf[10] = _strDados[10];     // Alerta.
                    strInformacaoConf[11] = _strDados[11];     // Alerta Max.
                    strInformacaoConf[12] = _strDados[12];     // Alerta Atual.
                    strInformacaoConf[13] = _strDados[13];     // Intervalo de alerta.
                    strInformacaoConf[14] = _strDados[14];     // Aviso na Tela.
                    strInformacaoConf[15] = _strDados[15];     // Observação.
                    strInformacaoConf[16] = _strDados[16];     // Copia.

                }
                catch (Exception)
                {
                    _status = false;
                }

            }

            return _status;


        }
        
        //Salva alerta detalhado.
        //-----------------------------
        private bool salvar_Alerta(string strCaminho, string strTipo, string strInformacao, string strAviso, ref string erroGeral) 
        {
            bool _status = true;

            string _strTemp;

            //Pega erro.
            try
            {

                //Monta string conforme parametro.
                _strTemp = "|" + strTipo + "|" + strInformacao + "|" + strAviso + "|";

            }
            catch (Exception)
            {
                return false;
            }

 

            //Salva registro de configuração.
            if (salvaTxtDetalhado(strCaminho, strTipo + "-" + strInformacao + ".txt", _strTemp, ref erroGeral) == false)
            {
                _status = false;
            }



            return _status;
       
        }

        //Atualiza alerta principal.
        //-----------------------------
        private bool atualiza_Alerta(string strCaminho, string strTipo, string strInformacao,string strDataUltAlerta, int intTempoAlerta,int intAlertaAtual,  ref string erroGeral) 
        {
            bool _status   = true;

            //Obtem  intervalo em minutos pela subtração de duas datas.
            TimeSpan intMinuto = DateTime.Now - Convert.ToDateTime(strDataUltAlerta);


            //Verifica-se total da subtração e maior que total de minutos definidos na configuração.
            if (intMinuto.TotalMinutes > intTempoAlerta)
            {

                //Atualiza configuração ALERTA.
                if (log.atualizaConfiguracaoCampo(strCaminho, "conf-auto.ini", strTipo, strInformacao, "ALERTA:", (intAlertaAtual + 1).ToString()) == false)
                {
                    _status = false;
                }

                //Atualiza configuração DATA.
                if (log.atualizaConfiguracaoCampo(strCaminho, "conf-auto.ini", strTipo, strInformacao, "ALERTA-ATUAL:", DateTime.Now.ToString()) == false)
                {
                    _status = false;
                }



            }


            return _status;
        
        }
        
        //Montagem de aviso simples.
        //-----------------------------
        private string monta_Mensagem_Principal(ref string[] strInformacao,string strAviso, ref string erroGeral)
        {
            //Concatenação de string.
            StringBuilder _strHtml = new StringBuilder();

            try
            {

                _strHtml.Append("<html>");
                _strHtml.Append("<body>");
                _strHtml.Append("<p> TIPO:" + strInformacao[0] + "</p>");
                _strHtml.Append("<p> INFORMAÇÃO:" + strInformacao[1]  + "</p>");
                _strHtml.Append("<p> DATA/HORA: " + strInformacao[12] + "</p>");
                _strHtml.Append("<p> ALERTA:    " + strInformacao[11] + "</p>");
                _strHtml.Append("<p> TEMPO:     " + strInformacao[13] + " Minutos</p>");
                _strHtml.Append("<p> AVISO:     " + strAviso + "</p>");
                _strHtml.Append("</body>");
                _strHtml.Append("</html>");
            }
            catch (Exception)
            {

                erroGeral = "Erro em montagem do HTML.";
            
                return erroGeral;
            
            }


            return _strHtml.ToString();

        }
        
        //Calcula quantidade de registros em uma determinada tabela.
        //-----------------------------
        public  void controleLimiteRegistro() 
        { 
            //Metodo para verifica a quantidade de registro de uma determinada tabela, verifica somente se o intervalo for atingido..

            //Montagem da string select com top definido pelo usuário.
            //string _strSql = "select * from  [log_quantidade_registro]";

            //OleDbDataReader _record_1 = null;

            //Concatenação de string.
            StringBuilder _strHtml = new StringBuilder();

            //Instância classe database.
            database.database dat = new database.database();
                        

        }

        //Salva quantidade de registros encontrados.
        //-----------------------------
        public  bool inserirControle(string strEndereco, string strUsuario, string strSenha, string strBanco, string strTabela,int intervalo_dia,string strEmail,int limite)
        {
            bool _status = true;

            //Metodo para inserir informação para controle de registro.




            return _status;
        }

        //Valida Parametro de entrada.
        //-----------------------------
        private bool valida_Parametro(ref string[] strParametro, ref string strErro)
        {
            bool _status = true;

            int retorno = 0;

            try
            {

                //Localização de campo em branco.
                for (var i = 0; i < strParametro.Length; i++)
                {
                    if (strParametro.Length == 0)
                    {
                        strErro = "ERRO, INFORMAÇÃO INCOMPLETA NA POSIÇÃO: " + i.ToString();
                        return false;
                    }
                }

                //Tipo.
                if (int.TryParse(strParametro[1], out retorno) == false)
                {
                    strErro = "ERRO, POSIÇÃO [1] NÃO É NUMERICA: ";
                    return false;
                }
                else
                {
                    //Tipo 1,2,3.
                    if ((int.Parse(strParametro[1]) > 0 && int.Parse(strParametro[1]) < 4) == false)
                    {
                        strErro = "ERRO, TIPO NÃO ENCONTRADO. ";
                        return false;
                    }

                }

                //Porta
                if (int.TryParse(strParametro[3], out retorno) == false)
                {
                    strErro = "ERRO, POSIÇÃO [3] NÃO É NUMERICA: ";
                    return false;
                }


                //Alerta
                if (int.TryParse(strParametro[9], out retorno) == false)
                {
                    strErro = "ERRO, POSIÇÃO [9] NÃO É NUMERICA: ";
                    return false;
                }


                //Tempo
                if (int.TryParse(strParametro[11], out retorno) == false)
                {
                    strErro = "ERRO, POSIÇÃO [11] NÃO É NUMERICA: ";
                    return false;
                }

                //Historico
                if (int.TryParse(strParametro[14], out retorno) == false)
                {
                    strErro = "ERRO, POSIÇÃO [14] NÃO É NUMERICA: ";
                    return false;
                }
            }
            catch (Exception)
            {
                _status = false;
            }

            return _status;

        }
        
        //Atualiza config-auto.ini
        //-------------------------
        static  public bool atualizaConfiguracaoCampo(string strCaminho, string strArquivo, string strTipo, string StrInformacao,string strCampo,string strCampoInformacao)
        {
            bool     _status = true;

            string[] _strDados = new string[1000];

            string   _strErro ="";

            //Carrega configuração encontrada no arquivo.
            if (utilitarios.ultilitarios.abrirArquivo(strArquivo, strCaminho, ref _strDados, ref _strErro))
            {


                //POsição inicial igual zero.
                var ativo = 0;


                //Loop dos registros encontrados.
                for (var i = 0; i < _strDados.Length; i++)
                {
                    if (_strDados[i] != null)
                    {

                        //Inicia após 1.
                        if (i > 0)
                        {

                            //Localiza registro.
                            if (_strDados[i - 1].ToUpper() == "TIPO:#" + strTipo.ToUpper() && _strDados[i].ToUpper() == "INFORMAÇÃO:" + StrInformacao.ToUpper())
                            {
                                ativo = 1;
                            }



                            //Atualiza configuração + informação.
                            if (ativo == 1)
                            {
                                if (_strDados[i].ToString().IndexOf(strCampo, 0) >= 0)
                                {
                                    _strDados[i] = strCampo.Trim() + strCampoInformacao.Trim();

                                    //Salva arquivo com alteração.
                                    if (utilitarios.ultilitarios.salvaArquivo(strArquivo, strCaminho, ref _strDados,2, ref _strErro) == false)
                                    {
                                        _status = false;
                                    }

                                   
                                    break;
                                }
                            }

                        }


                    }



                }

            }


            return _status;


        }
       
        //Localiza determinado campo
        //---------------------------
        static  public bool localizaConfiguracaoCampo(string strCaminho, string strArquivo, string strTipo, string StrInformacao, string strCampo, ref string strCampoInformacao)
        {
            bool _status = true;

            string[] _strDados = new string[1000];

            string _strErro = "";


            if (utilitarios.ultilitarios.abrirArquivo(strArquivo, strCaminho, ref _strDados, ref _strErro))
            {


                //Inicia com posição zero.
                var ativo = 0;


                //Loop dos registros encontrados.
                for (var i = 0; i < _strDados.Length; i++)
                {
                    if (_strDados[i] != null)
                    {

                        //Inicia após 1.
                        if (i > 0)
                        {


                            //Pega erro.
                            try
                            {

                                //Localiza informação.
                                if (_strDados[i - 1].ToUpper() == "TIPO:#" + strTipo.ToUpper() && _strDados[i].ToUpper() == "INFORMAÇÃO:" + StrInformacao.ToUpper())
                                {
                                    ativo = 1;
                                }


                            }
                            catch (Exception)
                            {
                                _status = false;

                                break;
                            
                            }



                            //Pega erro.
                            try
                            {

                                //Localização dados.
                                if (ativo == 1)
                                {
                                    if (_strDados[i].ToString().IndexOf(strCampo, 0) >= 0)
                                    {

                                        strCampoInformacao = _strDados[i].ToString().Substring(strCampo.Length, _strDados[i].ToString().Length - strCampo.Length);

                                        _status = true;

                                        break;
                                    }
                                }


                            }
                            catch (Exception)
                            {

                                _status = true;

                                break;
                            }




                        }


                    }



                }

            }

            return _status;
        }

        //Localiza determinado campo Config.
        //---------------------------
        static  public string localizaConfiguracaoCampoConfig(string strCaminho, string strArquivo, string strCampo, string strVazio)
        {
            string   _status = strVazio;
             
            string[] _strDados = new string[1000];

            string   _strErro = "";

            //Carrega arquivo.
            if (utilitarios.ultilitarios.abrirArquivo(strArquivo, strCaminho, ref _strDados, ref _strErro))
            {



                var ativo = 0;


                //Loop dos registros encontrados.
                for (var i = 0; i < _strDados.Length; i++)
                {
                    //Diferente de Null.
                    if (_strDados[i] != null)
                    {

                        //Inicia após 1.
                        if (i > 0)
                        {


                            //Pega erro.
                            try
                            {
                                //Localiza informação dentro do vetor.
                                if (_strDados[i].ToUpper().IndexOf(strCampo.Trim().ToUpper()) >= 0)
                                {
                                    ativo = 1;
                                }

                            }
                            catch (Exception)
                            {
                                _status = strVazio;

                                break;

                            }



                            //Pega erro.
                            try
                            {
                                //Após localiza configuração localiza dados.
                                if (ativo == 1)
                                {
                                    if (_strDados[i].ToString().IndexOf(strCampo, 0) >= 0)
                                    {
                                        //Localizar configuração, retira informação.
                                        _status = _strDados[i].ToString().Substring(strCampo.Length, _strDados[i].ToString().Length - strCampo.Length);

                                        break;
                                    }
                                }

                            }
                            catch (Exception)
                            {

                                _status = strVazio;

                                break;
                            }




                        }


                    }



                }

            }

            return _status;
        }

        //Localiza configuração completa.
        //------------------------------
        static  public bool localizaConfiguracao(string strCaminho, string strArquivo, string strTipo, string StrInformacao, ref string[] strCampoInformacao)
        {
            bool     _status = false;

            string[] _strDados = new string[1000];

            string   _strErro    = "";


            //Carrega dados encontrados, conforme parametro.
            if (utilitarios.ultilitarios.abrirArquivo(strArquivo, strCaminho, ref _strDados, ref _strErro))
            {



                //Loop dos registros encontrados.
                for (var i = 0; i < _strDados.Length; i++)
                {
                    if (_strDados[i] != null)
                    {
                        if (i > 0)
                        {

                            //Localiza dados.
                            if (_strDados[i - 1].ToUpper() == "TIPO:#" + strTipo.ToUpper() && _strDados[i].ToUpper() == "INFORMAÇÃO:" + StrInformacao.ToUpper())
                            {

                                try
                                {
                                    strCampoInformacao[0]  = _strDados[i - 1].ToString(); //Tipo.
                                    strCampoInformacao[1]  = _strDados[i + 0].ToString(); //Informação.
                                    strCampoInformacao[2]  = _strDados[i + 1].ToString(); //Tipo.
                                    strCampoInformacao[3]  = _strDados[i + 2].ToString();
                                    strCampoInformacao[4]  = _strDados[i + 3].ToString();
                                    strCampoInformacao[5]  = _strDados[i + 4].ToString();
                                    strCampoInformacao[6]  = _strDados[i + 5].ToString();
                                    strCampoInformacao[7]  = _strDados[i + 6].ToString();
                                    strCampoInformacao[8]  = _strDados[i + 7].ToString();
                                    strCampoInformacao[9]  = _strDados[i + 8].ToString();
                                    strCampoInformacao[10] = _strDados[i + 9].ToString();
                                    strCampoInformacao[11] = _strDados[i + 10].ToString();
                                    strCampoInformacao[12] = _strDados[i + 11].ToString();
                                    strCampoInformacao[13] = _strDados[i + 12].ToString();
                                    strCampoInformacao[14] = _strDados[i + 13].ToString();
                                    strCampoInformacao[15] = _strDados[i + 14].ToString();
                                    strCampoInformacao[16] = _strDados[i + 15].ToString();

                                    _status = true;

                                }
                                catch (Exception)
                                {
                                    _status = false;
                                }

                            }

                        }
                    }
                }
            }


            
            return _status;


        }

        //Salva arquivo TXT detalhado.
        //----------------------------
        static  public bool salvaTxtDetalhado(string strCaminho, string strArquivo, string StrInformacao, ref string strErro)
        {
            bool     _status   = true;

            string[] _strDados = new string[2];

            _strDados[0] = StrInformacao;

            //Salva informação conformerme parametro.
            if (utilitarios.ultilitarios.salvaArquivo(strArquivo, strCaminho + "\\", ref _strDados,0, ref strErro) == false)
            {
                _status = false;
            } 

            return _status;
        }

        //Salva nova configuração.
        //------------------------
        static  public bool salvaNovaConfiguracao(string strTipo, string strInformacao,string strCaminho, string strArquivo, ref string[] strCampoInformacao)
        {
            bool    _status = true;

            string[] _strDados = new string[50];

            string[] _strConfiguracaoAtual = new string[1000];

            string   _strErro = "";


            //Verifica-se não existe essa configuração.
            if (localizaConfiguracao(strCaminho, strArquivo, strTipo, strInformacao, ref _strDados) == false)
            {


                //Abre configuração atual.
                if (utilitarios.ultilitarios.abrirArquivo(strArquivo, strCaminho, ref _strConfiguracaoAtual, ref _strErro))
                {
                    var fim = 0;

                    //Verifica fim do arquivo #fim e adiciona uma nova configuração ao final do arquivo.
                    for (var i = 0; i < _strConfiguracaoAtual.Length; i++)
                    {

                        if (_strConfiguracaoAtual[i].Trim() == "FIM:#")
                        {
                            fim = i;
                            break;
                        }
                    
                    }

                    //Inicia após 0.
                    if (fim > 0)
                    {

                        //Pega erro.
                        try
                        {

                            _strConfiguracaoAtual[fim + 0] = strCampoInformacao[0];
                            _strConfiguracaoAtual[fim + 1] = strCampoInformacao[1];
                            _strConfiguracaoAtual[fim + 2] = strCampoInformacao[2];
                            _strConfiguracaoAtual[fim + 3] = strCampoInformacao[3];
                            _strConfiguracaoAtual[fim + 4] = strCampoInformacao[4];
                            _strConfiguracaoAtual[fim + 5] = strCampoInformacao[5];
                            _strConfiguracaoAtual[fim + 6] = strCampoInformacao[6];
                            _strConfiguracaoAtual[fim + 7] = strCampoInformacao[7];
                            _strConfiguracaoAtual[fim + 8] = strCampoInformacao[8];
                            _strConfiguracaoAtual[fim + 9] = strCampoInformacao[9];
                            _strConfiguracaoAtual[fim + 10] = strCampoInformacao[10];
                            _strConfiguracaoAtual[fim + 11] = strCampoInformacao[11];
                            _strConfiguracaoAtual[fim + 12] = strCampoInformacao[12];
                            _strConfiguracaoAtual[fim + 13] = strCampoInformacao[13];
                            _strConfiguracaoAtual[fim + 14] = strCampoInformacao[14];
                            _strConfiguracaoAtual[fim + 15] = strCampoInformacao[15];
                            _strConfiguracaoAtual[fim + 16] = strCampoInformacao[16];
                            _strConfiguracaoAtual[fim + 17] = "FIM:#";


                        }
                        catch (Exception)
                        {
                            _status = false;
                        }



                        //Salva nova configuração.
                        if (utilitarios.ultilitarios.salvaArquivo(strArquivo, strCaminho, ref _strConfiguracaoAtual,1, ref _strErro))
                        {
                            _status = true;
                        }
                        else
                        {
                            _status = false;
                        }

                    

                    }
                
                }
                else
                {
                    _status = false;
                }



            }
            else
            {
                _status = false;
            }

            return _status;
        }

        //Permissão para exibir log de erro na tela.
        //-------------------------------------
        static  public bool permissaoExibirLog(string strCaminho, string strArquivo, string strTipo, string StrInformacao)
        {
            bool   _status      = true;

            string _strRetorno  = "";

            //Permissão para exibir esse erro na tela.
            if (localizaConfiguracaoCampo(strCaminho, strArquivo, strTipo, StrInformacao, "AVISO-TELA:", ref _strRetorno))
            {

                //Compara valores.
                if (string.Compare(_strRetorno, "true", true) == 0)
                {
                    _status = true;
                }
                else
                {
                    _status = false;
                }


            }
            else
            {
                _status = false;
            }



            return _status;
        }


    }
}
