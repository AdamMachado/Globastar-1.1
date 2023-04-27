using System;
using System.Collections.Generic;
using System.Text;
using System.IO;             //http://msdn.microsoft.com/pt-br/library/system.io(v=vs.90).aspx  
using System.Xml;            //http://msdn.microsoft.com/en-us/library/system.xml.xmldocument.aspx
using System.Globalization;  //Tratamento de data, em formato americano.
using Microsoft.VisualBasic; //http://msdn.microsoft.com/pt-br/library/microsoft.visualbasic.aspx

using System.Data;
using System.Data.SqlClient;


namespace Globalstar
{
    class Globalstar 
    {
        #region  VARIAVEIS

      public string strLocalLog;

      //Controla tipo e erros do processamento.
      int _int_pr_ok           = 0;
      int _int_pr_tipo         = 1; 
      int _int_pr_falha        = 0;
      int _int_pr_erro         = 0;
      int _int_pr_texto        = 0;
      int _int_pr_binario      = 0;

      int _int_pr_semp_aviso   = 0;
      int _int_pr_aviso        = 0;
      int _int_pr_aviso_cod    = 0;
      int _int_pr_aviso_8_cod  = 0;
      int _int_pr_sem_destino  = 0;
      int _int_pr_erro_formato = 0;
      int _int_pr_normal       = 0;
      int _int_pr_panico       = 0;
      int _int_pr_msg          = 0;
      int _int_pr_teclado      = 0;

      #endregion
     
        //(2) - Tratamento de mensagem XML.
        //-------------------------
        public void tratamentoXml(ref string[] strDados, ref string strHexadecimal, bool bDebug)
        {
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //ROTINA DESENVOLVIDA CONFORME XML DA EMPRESA GLOBALSTAR.

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE stuMessages SYSTEM "http://cody.glpconnect.com/DTD/StuMessage_Rev6.dtd">
            <stuMessages timeStamp="01/06/2012 12:22:39 GMT" messageID="540737181ced100586bfffc1907fa316">
            <stuMessage>
            <esn>0-1102570</esn>
            <unixTime>1338553358</unixTime>
            <gps>N</gps>
            <payload length="9" source="pc" encoding="hex">0x00DE87E6DED12A0A00</payload>
            </stuMessage>
            </stuMessages>
            */

            double   _dUnixTime;        //unixTime. 

            string[] _strTemp;          //String temporária. 
            string   _strHex;           //Hexadecimal.        
            string   _strXmlTemp;       //XmlTemporário.
            string   _strUnixTime;      //UnixTime calculado.


            //Cria objeto xml, através do XmlDocument.
            XmlDocument xml = new XmlDocument();

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //Verifica quantidade de caracteres no anexo.
            if (strDados[1].Length == 0)
            {
                acessa_painel("RAW-POP3", "XML", "|ERRO ANEXO DO XML É INVALIDO|1|" + DateTime.Now.ToString(), strLocalLog);

                strDados[0] = "ERRO|RAW-POP3|XML|ERRO ANEXO DO XML É INVALIDO|1|";

                return;
            }

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //Localiza o início do Xml na string strDados[1].
            var posicao1 = strDados[1].IndexOf("<?xml", 0);

            //Posição final.
            var posicao2 = 0;

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            //Verifica se existe inicio do XML.
            if ((int)posicao1 > 0)
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO PROCESSAMENTO.");
                //*********************************************************************************************************************************

                //Proteção para nó não posicionado.
                try
                {
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                    "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO TRATAMENTO.");
                    //*********************************************************************************************************************************


                    //Carrega arquivo xml na memória, conforme posição indicada.
                    _strXmlTemp = strDados[1].Substring(posicao1, strDados[1].Length - posicao1);

                    //Obtem posição da final tag </stuMessages>
                    posicao2 = _strXmlTemp.IndexOf("</stuMessages>", 0);

                    //Carrega xml na variavel temporária.
                    _strXmlTemp = _strXmlTemp.Substring(0, posicao2 + 14);
                    
                    //Xml a ser carregada não deve ser validado pelo DTD.
                    xml.XmlResolver = null;

                    //Carrega xml na memória.
                    xml.LoadXml(_strXmlTemp);

                    //Data + Hora.  
                    _strTemp     = xml.SelectSingleNode("stuMessages").Attributes[0].InnerText.Split(' ');

                    //Equipamento.
                    strDados[4]  = xml.SelectSingleNode("/stuMessages").ChildNodes[0].ChildNodes[0].InnerText.ToString();

                    //UnixTime 
                    _dUnixTime   = double.Parse(xml.SelectSingleNode("/stuMessages").ChildNodes[0].ChildNodes[1].InnerText.ToString());

                    //UnixTime Calculado.
                    _strUnixTime = utilitarios.ultilitarios.UnixTimeParaDateTime(_dUnixTime).ToString();

                    //Proteção para converte data.
                    try
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                        "GLOBALSTAR.CS", "tratamentoXml", "0", "INICIO DO TRATAMENTO DA UNIXTIME.");
                        //*********************************************************************************************************************************

                        //Obtem data/hora através do UNIXTIME.
                        strDados[10] = Convert.ToDateTime(_strUnixTime).ToString();

                        //Data.         
                        strDados[5] = Convert.ToDateTime(_strUnixTime).ToString("dd/MM/yyyy");

                        //Hora.         
                        strDados[6]  = Convert.ToDateTime(_strUnixTime).ToString("HH:mm:ss");

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                        "GLOBALSTAR.CS", "tratamentoXml", "0", "FIM DO TRATAMENTO DA UNIXTIME.");
                        //*********************************************************************************************************************************
                        
                    }
                    catch
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                        "GLOBALSTAR.CS", "tratamentoXml", "2", "ERRO, DATA INVALIDA DA MENSAGEM.");
                        //*********************************************************************************************************************************

                        strDados[10] = "01/01/2000 01:01:01";                            //Detecta possível erro.
                        strDados[5]  = "01/01/2000";                                     //Detecta possível erro.
                        strDados[6]  = "01:01:01";                                       //Detecta possível erro.

                        acessa_painel("RAW-POP3", "XML", "|ERRO, DATA INVALIDA DA MENSAGEM|2|" + DateTime.Now.ToString(), strLocalLog, strDados[1]);
                    }


                    //Id_mensagem id + unixTime.  
                    strDados[7]  = xml.SelectSingleNode("stuMessages").Attributes[3].InnerText + "-" + _dUnixTime;

                    //Gps.          
                    strDados[15] = xml.SelectSingleNode("/stuMessages").ChildNodes[0].ChildNodes[2].InnerText.ToString();
                    
                    //Encoding.     
                    strDados[13] = xml.SelectSingleNode("/stuMessages").ChildNodes[0].ChildNodes[3].Attributes[2].InnerText;
                    
                    //Hexadecimal.
                    _strHex      = xml.SelectSingleNode("/stuMessages").ChildNodes[0].ChildNodes[3].InnerText.ToString();
                                        
                    //Tratamento de Hexadecimal, separando em dupla.
                    strDados[16] = "";

                    //Separa valor Hexadecimal encontrado, colocando espaço entre eles.
                    for (var i = 0; i <= _strHex.Trim().Length - 2; i += 2)
                    {
                        strDados[16] += _strHex.Substring(i, 2) + " ";
                    }


                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                    "GLOBALSTAR.CS", "tratamentoXml", "0", "FIM DO TRATAMENTO.");
                    //*********************************************************************************************************************************


                }
                catch (Exception e)
                {

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                    "GLOBALSTAR.CS", "tratamentoXml", "3", "ERRO EM TRATA XML.");
                    //*********************************************************************************************************************************

                    acessa_painel("RAW-POP3", "XML", "|ERRO EM TRATA XML|3|" + DateTime.Now.ToString(), strLocalLog, strDados[1]);  //erro com mensagem.

                    strDados[0] = "ERRO|RAW-POP3|XML|ERRO EM TRATA XML|3|" + e.Message;    //Detecta possível erro.
                }


                //Troca anexo.
                strDados[1] = "Globalstar";                                                //Não precisa de anexo, apenas o nome Globalstar.

                //Sta.
                strDados[17] = "0";                                                        //Sta = 0 

                //Mha
                strDados[18] = "0";                                                        //Mha = 0 

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
                "GLOBALSTAR.CS", "tratamentoXml", "0", "FIM DO PROCESSAMENTO.");
                //*********************************************************************************************************************************

            }

        }

        //(6) - Regras de processamento.
        //-------------------------
        public bool permissaoProcessamento(string strEquipamento, ref string[] strParametro, bool bDebug)
        {
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "permissaoProcessamento", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            bool _status = false;

            strEquipamento = " " + strEquipamento;

            //Permissão da sigla. 
            for (var i = 71; i <= 83; i++)
            {
                if (strParametro[i] != null)
                {
                    if (strEquipamento.ToUpper().IndexOf(strParametro[i].ToUpper().ToString(), 0) > 0)
                    {
                        _status = true;
                    }
                }
            }

            strEquipamento = " " + strEquipamento;

            //Bloqueio individual.
            for (var i = 85; i < 700; i++)
            {
                if (strParametro[i] != null)
                {
                    if (string.Compare(strParametro[i].Trim(), strEquipamento.Trim(), true) == 0)
                    {
                        _status = false;
                    }
                }
            }

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocalLog, false,
            "GLOBALSTAR.CS", "permissaoProcessamento", "0", "FIM DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            return _status;
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
                var x = 0;
            }

        }
        
 



        //(1) - Metodo principal.
        //------------------------------------------------
        public string[] xmlFtp(string[] strParametro, string strPasta, bool bDebug)
        {

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************


            //*******************************************************

            #region DETALHE DO CONTROLE DE PROCESSAMENTO

            //1-TIPO  ==> 1-SATELITAL 2-GLOBALSTAR 3-TELESEED 
            //2-OK
            //3-FALHA
            //4-ERRO
            //5-MENSAGEM DE TEXTO
            //6-MENSAGEM BINÁRIA
            //7-SEM POSIÇÃO
            //8-AVISO EM TEXTO
            //9-AVISO EM CODÍGO
            //10-AVISO EM CÓDIGO 8 BYTES
            //11-SEM DESTINO
            //12-ERRO NO FORMATO
            //16-NORMAL
            //14-PÂNICO
            //15-MSG
            //16-MSG TECLADO

            //  "00000000000000000"

            #endregion

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "0", "DECLARAÇÃO DE VARIÁVEIS E INSTÂNCIAS.");
            //*********************************************************************************************************************************

            //(1)
            #region VARIÁVEIS E INSTÂNCIAS

            string[] _strDados         = new string[1001]; //Dados dos arquivos.

            string[] _strArquivo       = new string[501];  //lista de Arquivos.

            string[] _strRetorno       = new string[1001]; //Retorna resultado do processamento.
             
            string[] _strInformacao    = new string[30];   //Resultado do processamento do XML.

            string[] _strTempBruto     = new string[20];   //Vetor temporário, salvar dados dat.salvaDadosPop3.

            string[] _strTempParametro = new string[50];   //Vetor temporário, parametros, dat.salvaDados.

            string[] _strTempProcedure = new string[50];   //Vetor temporário, parametros, procedures. 

            string   _strErro          = "";               //Controle de erro geral.

            string   _strTemp          = "";               //String temporária.  

            string   _strHex           = "";               //String para armazenamento do xml.
          
            string   _dbErroConexao    = "";               //Controle de erro para conexão.

            int      _intArquivoQuant  = 0;                //Quantidade de arquivo.
            int      _intXmlQuant      = 0;                //Quantidade de xml por arquivo.
            int      _intProcessamento = 2;                //Processamento.

            //*******************************************************

            //Instânciamento da classe dataBase.
            database.database dat   = new database.database();


            //Inicia processamento +OK
            _strRetorno[0]          = "+OK";

            //Caminho da aplicação.
            strLocalLog = strPasta;

            //*******************************************************

            #endregion


            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "1", "INICIO LOCALIZAÇÃO DOS ARQUIVOS XML.");
            //*********************************************************************************************************************************
            
            //(2)
            #region Localiza arquivo e retorna string[] contendo arquivos a serem processados.
            
            var strDiretorioEscolhido = "";
            
            #region PRIMEIRO CAMINHO
            if (strParametro[44] != null && strParametro[44].Trim().Length > 0)
            {

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "LOCALIZA ARQUIVO NO PRIMEIRO LOCAL.");
                //*********************************************************************************************************************************

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "Verifica-se diretório existir.");
                //*********************************************************************************************************************************

                //Verifica-se diretório existir.
                if (Directory.Exists(strParametro[44]))
                {

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "2", "Marca o diretório a ser listado.");
                    //*********************************************************************************************************************************

                    //Marca o diretório a ser listado.
                    DirectoryInfo diretorio1 = new DirectoryInfo(strParametro[44]);

                    try
                    {

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "2", "Executa metodo GetFile(Lista os arquivos desejados de acordo com o parametro).");
                        //*********************************************************************************************************************************
                        
                        //Executa metodo GetFile(Lista os arquivos desejados de acordo com o parametro)
                        FileInfo[] Arquivos1 = diretorio1.GetFiles("*.xml");

                        strDiretorioEscolhido = strParametro[44];

                    }
                    catch (Exception)
                    { 
                    
                    }

                }

            }
            #endregion
            

            #region SEGUNDO CAMINHO
            if (strParametro[45].Trim().Length > 0)
            {

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "LOCALIZA ARQUIVO NO SEGUNDO LOCAL.");
                //*********************************************************************************************************************************


                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "Verifica-se diretório existir.");
                //*********************************************************************************************************************************


                //Verifica-se diretório existir.
                if (Directory.Exists(strParametro[45]))
                {
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "2", "Marca o diretório a ser listado.");
                    //*********************************************************************************************************************************

                    //Marca o diretório a ser listado.
                    DirectoryInfo diretorio2 = new DirectoryInfo(strParametro[45]);

                    try
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "2", "Executa função GetFile(Lista os arquivos desejados de acordo com o parametro).");
                        //*********************************************************************************************************************************

                        //Executa função GetFile(Lista os arquivos desejados de acordo com o parametro)
                        FileInfo[] Arquivos2 = diretorio2.GetFiles("*.xml");

                        if (Arquivos2.Length > 0)
                        {
                            strDiretorioEscolhido = strParametro[45];
                        }

                    }
                    catch (Exception)
                    {

                    }

                }

            }
            #endregion

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "2", "LOCAL ESCOLHIDO:  " + strDiretorioEscolhido);
            //*********************************************************************************************************************************

            //Marca o diretório a ser listado.
            DirectoryInfo diretorio = new DirectoryInfo(strDiretorioEscolhido);


            try
            {

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "Executa função GetFile(Lista os arquivos desejados de acordo com o parametro).");
                //*********************************************************************************************************************************


                //Executa função GetFile(Lista os arquivos desejados de acordo com o parametro)
                FileInfo[] Arquivos = diretorio.GetFiles("*.xml");

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "Zera quantidade de arquivos.");
                //*********************************************************************************************************************************

                //Zera quantidade de arquivos.
                _intArquivoQuant = 0;

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "2", "Começamos a listar os arquivos.");
                //*********************************************************************************************************************************


                //Começamos a listar os arquivos.
                foreach (FileInfo fileinfo in Arquivos)
                {

                    if (_intArquivoQuant < 501)
                    {


                        if (fileinfo.Name.ToString().IndexOf("tmp") == -1)
                        {

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "2", "ARQUIVO: " + fileinfo.Name.ToString());
                            //*********************************************************************************************************************************

                            _strArquivo[_intArquivoQuant] = fileinfo.Name.ToString();

                            _intArquivoQuant++;
                        }


                    }
                    else
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "2", "OCORREU UM (BREAK), QUANTIDADE MAIOR QUE 500.");
                        //*********************************************************************************************************************************


                        break;
                    }

                }


            }
            catch (Exception)
            {

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "4", "ERRO EM ACESSAR PASTA FTP");
                //*********************************************************************************************************************************
                
              acessa_painel("GLOBALSTAR", "XML", "|ERRO EM ACESSAR PASTA FTP|4|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

              _strRetorno[0] = "ERRO|GLOBALSTAR|XML|ERRO EM ACESSAR PASTA FTP|4|";

              return _strRetorno;

            }

            #endregion


            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "2", "FIM DA LOCALIZAÇÃO DOS ARQUIVOS XML.");
            //*********************************************************************************************************************************

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "2", "Aguarda alguns segundos, as vezes o arquivo ainda esta sendo processado pelo ftp.");
            //*********************************************************************************************************************************
            
            //Aguarda alguns segundos, as vezes o arquivo ainda esta sendo processado pelo ftp.
            System.Threading.Thread.Sleep(1000);

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "3", "INICIO DO PROCESSAMENTO DE EXTRAÇÃO.");
            //*********************************************************************************************************************************

            //(3)
            #region Faz abertura do arquivo e efetua a extração dos dados para processamento.
            //Abre arquivo e efetua a extração dos dados para processamento.
            //Obtem quantidade de registros que será o loop.

            //(INICIO) "> após timeStamp=
            //(FIM)    </stuMessages>

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "3", "Verifica-se existe arquivo a ser processado.");
            //*********************************************************************************************************************************
            
            //Verifica-se existe arquivo a ser processado.
            if (_intArquivoQuant > 0)
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "3", "existe arquivo a ser processado.");
                //*********************************************************************************************************************************

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "3", "Criar um loop dos arquivos a serem processados.");
                //*********************************************************************************************************************************
                
                //Criar um loop dos arquivos a serem processados.
                for (var i = 0; i < _intArquivoQuant; i++)
                {

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "3", "Verifica limite de processamento, aguarda processar arquivo.");
                    //*********************************************************************************************************************************

                    //Verifica limite de processamento, aguarda processar arquivo.
                    if (_intXmlQuant >= int.Parse(strParametro[12]))
                    break;


                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "3", "Controla limite imposto pela configuração.");
                    //*********************************************************************************************************************************


                    //Controla limite imposto pela configuração.
                    if (_intXmlQuant <= int.Parse(strParametro[12]))
                    {
                        
                        //Limpa Objeto.
                        _strTemp = null;

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "3", "Faz abertura do arquivo retornando seu conteúdo no formato de string[].");
                        //*********************************************************************************************************************************
                        
                        //Faz abertura do arquivo retornando seu conteúdo no formato de string[].
                        if (utilitarios.ultilitarios.abrirArquivo(_strArquivo[i].ToString(), strDiretorioEscolhido, ref _strTemp, ref _strErro))
                        {
                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "3", "Aqui começa a função que vai retirar as informações dos arquivos.");
                            //*********************************************************************************************************************************
 
                            //Aqui começa a função que vai retirar as informações dos arquivos,
                            //sendo que após a extração individual, será criado novo xml dentro do vetor _strDados
                            //contendo informação indiviual.

                            //Controla loop.
                            var temp = true;


                            var p1 = 0; //Posição inicial.
                            var p2 = 0; //Posição final.

                            var cl = 0; //Controle para evitar loop infinito.


                            try
                            {

                                #region LOOP INTERNO, ONDE DEVE OCORRER MUDANÇAS CONFORME XML.

                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "3", "Loop da string contendo xml individual ou em lote.");
                                //*********************************************************************************************************************************
     
                                //Loop da string contendo xml individual ou em lote.
                                while (temp)
                                {
                                    
                                    //Controle para evitar loop infinito.
                                    cl++;

                                    

                                    if (cl > 200)
                                    {

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlFtp", "3", "ocorreu um loop infinito, porque tag final de xml não foi encontrado.");
                                        //*********************************************************************************************************************************
                                   
                                        //ocorreu um loop infinito, porque tag final de xml não foi encontrado.
                                        _strDados[_intXmlQuant] = "ERRO TAG FINAL DE XML, NÃO ENCONTRADO.";

                                        _intXmlQuant++;

                                        //Sai do loop
                                        temp = false; 
                                    
                                    }

                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlFtp", "3", "Localiza posição, após <stuMessage>");
                                    //*********************************************************************************************************************************
 
                                    //Localiza posição, após <stuMessage>
                                    p1 = _strTemp.IndexOf("<stuMessage>", p2);


                                    //Posição foi encontrada.
                                    if (p1 >= 0)
                                    {

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlFtp", "3", "Posição foi encontrada.");
                                        //*********************************************************************************************************************************

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlFtp", "3", "Localiza posição </stuMessage>, após p1.");
                                        //*********************************************************************************************************************************
                                                                                
                                        //Localiza posição </stuMessage>, após p1.
                                        p2 = _strTemp.IndexOf("</stuMessage>", p1);


                                        //*****************************************************************************************

                                        //Posição p2 foi encontrada, então recolho intervalo.
                                        if (p2 >= 0)
                                        {

                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlFtp", "3", "Posição p2 foi encontrada, então recolho intervalo.");
                                            //*********************************************************************************************************************************
 
                                                //Recolho do inicio(0) do xml até  o final da tag.
                                                _strDados[_intXmlQuant] = _strTemp.Substring(0, _strTemp.IndexOf(((char)34) + ">", _strTemp.IndexOf("timeStamp="))) + ((char)34) + ">";

                                                //Recolho as informações conforme posição p1 e p2.
                                                _strDados[_intXmlQuant] = _strDados[_intXmlQuant] + _strTemp.Substring(p1, p2 - p1);

                                                //Monto final do xml.
                                                _strDados[_intXmlQuant] = _strDados[_intXmlQuant] + "</stuMessage> </stuMessages>";

                                                //Concatena _strDados com nome do arquivo e seu respectivo conteúdo.
                                                _strDados[_intXmlQuant] = _strArquivo[i] + _strDados[_intXmlQuant];

                                                _intXmlQuant++;

                                        }
                                        else
                                        {

                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlFtp", "3", "Termina loop p2.");
                                            //*********************************************************************************************************************************
 
                                            //Termina loop p2.
                                            temp = false;
                                        }
                                        //*****************************************************************************************


                                    }
                                    else
                                    {

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlFtp", "3", "Termina loop p1.");
                                        //*********************************************************************************************************************************
                                                                                
                                        //Termina loop p1.
                                        temp = false;
                                    }

                                }
                                #endregion
                            
                            }
                            catch (Exception)
                            {

                                //*********************************************************************************************

                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "5", "ERRO EM RETIRAR XML");
                                //*********************************************************************************************************************************
 
                                //Lista arquivo para que seja deletado.
                                _strDados[_intXmlQuant] = "ERRO EM RETIRAR XML.|5|" + _strArquivo[i].ToString();

                                //Add.
                                _intXmlQuant++;

                                //Controla erro.
                                acessa_painel("GLOBALSTAR", "XML", "|ERRO EM RETIRAR XML|5|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                                //Tratamento para retorna processamento.
                                _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|ERRO EM RETIRAR XML.|5|".PadRight(60, ' ')).Substring(0, 60) + "0010000000000000"; //Erro.

                                //Add.
                                _intProcessamento++;

                                //*********************************************************************************************

                           }



                        }
                        else
                        {
                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "7", "Erro em abrir arquivo, move para pasta erro.");
                            //*********************************************************************************************************************************
 

                            #region Erro em abrir arquivo, move para pasta erro.

                            #region DIRETÓRIO

                            //Se o diretório não existir.
                            if (!Directory.Exists(strDiretorioEscolhido + "\\backupErro"))
                            {
                                try
                                {
                                    //Criamos um novo.
                                    Directory.CreateDirectory(strDiretorioEscolhido + "\\backupErro");
                                }
                                catch (Exception e)
                                {

                                    //*********************************************************************************************

                                    //Controla erro.
                                    acessa_painel("GLOBALSTAR", "XML", "|ERRO EM CRIAR DIRETÓRIO|6|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                                    //Tratamento para retorna processamento.
                                    _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|ERRO EM CRIAR DIRETÓRIO|6|".PadRight(60, ' ')).Substring(0, 60) + "2" + "0100000000000000"; //Falha.

                                    //Add.
                                    _intProcessamento++;

                                    //*********************************************************************************************

                                }

                            }

                            #endregion

                            if (utilitarios.ultilitarios.backup(_strArquivo[i], strDiretorioEscolhido,"backupErro") == false)
                            {
                                //Tratamento de erro.
                            }
                            

                            //*********************************************************************************************

                            //Controla erro.
                            acessa_painel("GLOBALSTAR", "XML", "|ERRO EM ABRIR ARQUIVO XML|7|" + DateTime.Now.ToString(), strLocalLog);

                            //Tratamento para retorna processamento.
                            _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|ERRO EM ABRIR ARQUIVO XML|7|".PadRight(60, ' ')).Substring(0, 60) + "2" + "0010000000000000"; //Erro.

                            //Add.
                            _intProcessamento++;

                            //*********************************************************************************************

                            #endregion

                        }



                    } //Controla limite do intXmlQuant e decrementa arquivo a ler.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "7", "Fim do loop.");
                    //*********************************************************************************************************************************


                } //Fim do loop.


            }//Fim do verifica-se existe arquivo.

            #endregion

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "3", "FIM DA PROCESSAMENTO DE EXTRAÇÃO.");
            //*********************************************************************************************************************************

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "4", "INICIO DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
            //*********************************************************************************************************************************
            
            //(4)
            #region Tratamento do xml - Salvamento - Move arquivo.
            if (_intArquivoQuant > 0)
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "4", "INICIO DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
                //*********************************************************************************************************************************

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "4", "INICIO DO LOOP CONTENDO INFORMAÇÕES.");
                //*********************************************************************************************************************************
                
                for (var i = 0; i < _intXmlQuant; i++)
                {
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4A", "INICIO DO OBTEM XML DO ARQUIVO.");
                    //*********************************************************************************************************************************

                    //4-A
                    #region OBTEM XML DO ARQUIVO.

                    //Nome do arquivo xml a ser processado.
                    var arquivoNameTemp = "";

                    //Localiza posição <?xml
                    var posicao = _strDados[i].IndexOf("<?xml");

                    //Posição encontrada.
                    if (posicao >= 0)
                    {

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4A", "Posição encontrada.");
                        //*********************************************************************************************************************************

                        //Obtem arquivo que rerente ao conteúdo processado.
                        arquivoNameTemp = _strDados[i].Substring(0, posicao);

                        //Obtem Dados XML.
                        _strDados[i] = "   " + _strDados[i].Substring(posicao, _strDados[i].Length - posicao);

                        _strInformacao[0] = "+OK";

                    }
                    else
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "8", "ERRO NO FORMATO DO XML.");
                        //*********************************************************************************************************************************

                            //Nâo foi possível obter XML.
                            _strInformacao[0] = "ERRO FORMATO DO XML";
                            
                            //Tenta obter nome do arquivo para deletar, porque arquivo é invalido.
                            arquivoNameTemp = _strDados[i].Substring(0, _strDados[i].IndexOf(".xml") + 4);

                            //*********************************************************************************************

                            acessa_painel("GLOBALSTAR", "XML", "|ERRO NO FORMATO DO XML|8|" + DateTime.Now.ToString(), strLocalLog);

                            _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|XML|ERRO NO FORMATO DO XML|8|".PadRight(60, ' ')).Substring(0, 60) + "0010000000000000"; //Erro.

                            _intProcessamento++;

                            //*********************************************************************************************

                    }

                    #endregion
                    
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4A", "FIM DO OBTEM XML DO ARQUIVO.");
                    //*********************************************************************************************************************************
                    

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4B", "INICIO DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
                    //*********************************************************************************************************************************
                    
                    //4-B
                    #region TRATAMENTO DO XML RECEBIDO.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4B", "tratamento do erro.");
                    //*********************************************************************************************************************************


                    //tratamento do erro.
                    if (_strInformacao[0] == "+OK")
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4B", "ZERA DADOS, ANTES DE ENVIAR PARA METODO TRATAMENTO.");
                        //*********************************************************************************************************************************

                        _strInformacao[0]  = "+OK";
                        _strInformacao[1]  = _strDados[i];
                        _strInformacao[2]  = "";
                        _strInformacao[3]  = "";
                        _strInformacao[4]  = "";
                        _strInformacao[5]  = "";
                        _strInformacao[6]  = "";
                        _strInformacao[7]  = "";
                        _strInformacao[8]  = "";
                        _strInformacao[9]  = "";
                        _strInformacao[10] = "";
                        _strInformacao[11] = "";
                        _strInformacao[12] = "";
                        _strInformacao[13] = "";
                        _strInformacao[14] = "";
                        _strInformacao[15] = "";
                        _strInformacao[16] = "";
                        _strInformacao[17] = "";
                        _strInformacao[18] = "";
                        _strInformacao[19] = "";
                        _strInformacao[20] = "";

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4B", "INICIO DO METODO TRATAMENTO.");
                        //*********************************************************************************************************************************

                        //Retornando dados necessários.
                        tratamentoXml(ref _strInformacao, ref _strHex, bDebug);

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4B", "FIM DO METODO TRATAMENTO.");
                        //*********************************************************************************************************************************
                        
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4B", "VALIDAÇÃO DAS INFORMAÇÕES RETORNADAS PELO METODO TRATAMENTO.");
                        //*********************************************************************************************************************************
                        
                        //tratamento do erro.
                        if (_strInformacao[0] != "+OK")
                        {
                            //*********************************************************************************************

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "9", "ERRO EM FAZER TRATAMENTO DO ARQUIVO XML.");
                            //*********************************************************************************************************************************


                            acessa_painel("GLOBALSTAR", "XML", "|ERRO EM FAZER TRATAMENTO DO ARQUIVO XML|9|" + _strInformacao[4] + "|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                            _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|XML|ERRO EM FAZER TRATAMENTO DO ARQUIVO XML|9|" + _strInformacao[4] + "|".PadRight(60, ' ')).Substring(0, 60) + "0010000000000000"; //Erro.

                            _intProcessamento++;

                            //*********************************************************************************************
                        }
                        else
                        {
                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4B", "VALIDAÇÃO OK.");
                            //*********************************************************************************************************************************

                        }

                    }

                    #endregion

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4B", "FIM DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
                    //*********************************************************************************************************************************



                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4C", "INICIO DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
                    //*********************************************************************************************************************************

                    //4-C
                    #region SALVAMENTO E RETORNO.

                    #region EXEMPLO DE PARAMETRO do _strInformacao

                    //strInformacao[0]   Status {"+OK", "ERRO"}.
                    //strInformacao[1]   Anexo.
                    //strInformacao[2]   Decodificado.
                    //strInformacao[3]   Hexadecimal.
                    //strInformacao[4]   Veículo ou equipamento.
                    //strInformacao[5]   Data da mensagem em formato 00/00/0000.
                    //strInformacao[6]   Hora da mensagem em formato 00:00:00.
                    //strInformacao[7]   Id da mensagem bruta.

                    //strInformacao[8]   Retorna longitude.
                    //strInformacao[9]   Retorna latitude.
                    //strInformacao[10]  Retorna data + hora.

                    //strInformacao[11] = "";  Retorna informação adicional exemplo: mensagem globalstar.

                    //strInformacao[12] = "";  Panico.
                    //strInformacao[13] = "";  Mensagem.
                    //strInformacao[14] = "";  Teclado.

                    #endregion

                    //Metodo para salvar dados brutos.

                    #region Troca posição dos vetores (dados),     para adaptar-se ao metodo dat.salvaDadosPop3.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4C", "Troca posição dos vetores (dados),     para adaptar-se ao metodo dat.salvaDadosPop3.");
                    //*********************************************************************************************************************************


                    try
                       {

                        _strTempBruto[1]  = _strInformacao[1];  //Anexo.
                        _strTempBruto[3]  = _strInformacao[4];  //Equipamento.
                        _strTempBruto[17] = "0";                //Sat_data.
                        _strTempBruto[18] = "0";                //Mha_data.
                        _strTempBruto[7]  = _strInformacao[7];  //uidl.
                        _strTempBruto[13] = _strInformacao[13]; //Encondig. 
                        _strTempBruto[16] = _strInformacao[16]; //Hexadecimal.
                        _strTempBruto[5]  = _strInformacao[5];  //Data em formato americano.
                        _strTempBruto[6]  = _strInformacao[6];  //Hora.
                        _strTempBruto[19] = Convert.ToDateTime(_strInformacao[5]).ToString("MM/dd/yyyy") + " " + _strInformacao[6]; //DateTime em formato americano.


                       }
                       catch (Exception)
                       {

                           //*********************************************************************************************************************************
                           //Debug.
                           utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                           "GLOBALSTAR.CS", "xmlFtp", "10", "ERRO EM TROCAR POSIÇÃO DOS VETORES");
                           //*********************************************************************************************************************************

                        //*********************************************************************************************
                        acessa_painel("GLOBALSTAR", "BANCO", "|ERRO EM TROCAR POSIÇÃO DOS VETORES|10|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                        _strRetorno[_intProcessamento] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|10|";

                        _intProcessamento++;

                        _strInformacao[0] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|10|";

                        //*********************************************************************************************

                       }
                        
                        
                        #endregion


                    #region Troca posição dos vetores (parametro), para adaptar-se ao metodo dat.salvaDadosPop3.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4C", "Troca posição dos vetores (parametro), para adaptar-se ao metodo dat.salvaDadosPop3.");
                    //*********************************************************************************************************************************


                       try
                        {
                            _strTempParametro[9]  = strParametro[1];   //Servidor.
                            _strTempParametro[11] = strParametro[2];   //Usuário.
                            _strTempParametro[12] = strParametro[3];   //Senha.
                            _strTempParametro[13] = strParametro[4];   //banco. 
                            _strTempParametro[27] = strParametro[5];   //Timeout.
                            _strTempParametro[8]  = strParametro[6];   //Encrypt.
                            _strTempParametro[15] = strParametro[33];  //Tabela.

                            _strTempParametro[7]  = "1";               //Conexão Sql Server.

                            _strTempParametro[16] = strParametro[47];  //Equipamento.
                            _strTempParametro[17] = strParametro[48];  //Data_data.
                            _strTempParametro[18] = strParametro[49];  //Mha_data.
                            _strTempParametro[19] = strParametro[50];  //DateTime.
                            _strTempParametro[20] = strParametro[51];  //Anexo.
                            _strTempParametro[21] = strParametro[52];  //UIDL.
                            _strTempParametro[22] = strParametro[53];  //Enconding.
                            _strTempParametro[23] = strParametro[54];  //Raw_Hex.
                            _strTempParametro[24] = strParametro[55];  //fl_decod.
                            _strTempParametro[25] = strParametro[56];  //fl_seed.
                        }
                        catch (Exception)
                        {

                            //*********************************************************************************************

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "11", "ERRO EM TROCAR POSIÇÃO DOS VETORES.");
                            //*********************************************************************************************************************************

                            acessa_painel("GLOBALSTAR", "BANCO", "|ERRO EM TROCAR POSIÇÃO DOS VETORES|11|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                            _strRetorno[_intProcessamento] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|11|";

                            _intProcessamento++;

                            _strInformacao[0] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|11|";

                            //*********************************************************************************************

                        }

                        #endregion


                    #region Troca posição dos vetores (parametro), para adaptar-se ao metodo dat.salvaDados.

                       //*********************************************************************************************************************************
                       //Debug.
                       utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                       "GLOBALSTAR.CS", "xmlFtp", "4C", "Troca posição dos vetores (parametro), para adaptar-se ao metodo dat.salvaDados.");
                       //*********************************************************************************************************************************


                       try
                       {

                           _strTempProcedure[1]  = strParametro[1];  //Servidor.
                           _strTempProcedure[2]  = strParametro[2];  //Usuário.
                           _strTempProcedure[3]  = strParametro[3];  //Senha.
                           _strTempProcedure[4]  = strParametro[4];  //Banco.
                           _strTempProcedure[5]  = strParametro[5];  //Timeout.
                           _strTempProcedure[6]  = strParametro[6];  //Encrypt.
                           
                           _strTempProcedure[23] = strParametro[23]; //Pânico.
                           _strTempProcedure[24] = strParametro[24]; //Descrição.
                           _strTempProcedure[25] = strParametro[25]; //Rastreamento.
                           _strTempProcedure[26] = strParametro[26]; //Online.
                           _strTempProcedure[27] = strParametro[27]; //Cerca.
                           _strTempProcedure[28] = strParametro[28]; //Velocidade.


                       }
                       catch (Exception)
                       {
                           //*********************************************************************************************

                           //*********************************************************************************************************************************
                           //Debug.
                           utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                           "GLOBALSTAR.CS", "xmlFtp", "11B", "ERRO EM TROCAR POSIÇÃO DOS VETORES.");
                           //*********************************************************************************************************************************

                           acessa_painel("GLOBALSTAR", "BANCO", "|ERRO EM TROCAR POSIÇÃO DOS VETORES|11-b|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                           _strRetorno[_intProcessamento] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|11-b|";

                           _intProcessamento++;

                           _strInformacao[0] = "ERRO|GLOBALSTAR|BANCO|ERRO EM TROCAR POSIÇÃO DOS VETORES|11-b|";

                           //*********************************************************************************************
                       }

                       #endregion

                       //*********************************************************************************************************************************
                       //Debug.
                       utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                       "GLOBALSTAR.CS", "xmlFtp", "4C", "VALIDAÇÃO ANTES DE SALVAR DADOS BRUTOS.");
                       //*********************************************************************************************************************************

                        //Dados Brutos.
                        if (_strInformacao[0] == "+OK")
                        {   
                            #region DADOS BRUTOS

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4C", "VALIDAÇÃO OK.");
                            //*********************************************************************************************************************************

                            //(39) FLUXO DE DADOS.
                            //Retorna status do salvamento, com descrição caso ocorra alguma falha no salvamento, não permite salvamento em duplicidade.

                            //Zera erro.
                            _dbErroConexao = "";

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4C", "INICIA SALVAMENTO.");
                            //*********************************************************************************************************************************

                            //9 bytes=  29 caracteres
                            //if (_strTempBruto[16].Length > 29) 
                            //{ System.Windows.Forms.MessageBox.Show("Ops"); 
                            //}

                           // _strTempParametro[18] = "2";
                            if (dat.salvaDadosPop3(_strTempParametro, _strTempBruto, ref _dbErroConexao))
                            {

                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "4C", "SALVAMENTO CONCLUÍDO.");
                                //*********************************************************************************************************************************


                                //                                   Equipamento                       dateTime                 Hexadecimal
                                _strRetorno[_intProcessamento] = (_strInformacao[4].Trim() + " " + _strInformacao[10] + " " + _strInformacao[3].PadRight(60, ' ')).Substring(0, 60) + "1000000000000000"; //OK.

                                _intProcessamento++;

                            }
                            else
                            {
                                //*********************************************************************************************

                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "12", "ERRO EM SALVAR INFORMAÇÃO.");
                                //*********************************************************************************************************************************

                                
                                acessa_painel("GLOBALSTAR", "BANCO", "|ERRO EM SALVAR INFORMAÇÃO|12|" + _strInformacao[4] + "|" + DateTime.Now.ToString() + "|" + _dbErroConexao, strLocalLog);  //erro com mensagem.

                                _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|BANCO|ERRO EM SALVAR INFORMAÇÃO|12|" + _strInformacao[4] + "|" + _dbErroConexao + "|".PadRight(60, ' ')).Substring(0, 60) + "0010000000000000"; //Erro.

                                _intProcessamento++;

                                _strInformacao[0]              =DateTime.Now+ "ERRO|GLOBALSTAR|BANCO|ERRO EM SALVAR INFORMAÇÃO|12|" + _strInformacao[4] + "|" + _dbErroConexao + "|";

                                //*********************************************************************************************
                            }

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4C", "Fecha todas as conexões abertas na classe.");
                            //*********************************************************************************************************************************


                            //Fecha todas as conexões abertas na classe.
                            dat.fechaSql();

                            #endregion
                        }

                    #endregion

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4C", "INICIO DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
                    //*********************************************************************************************************************************


                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4D", "INICIO DO MOVE ARQUIVO PROCESSADO.");
                    //*********************************************************************************************************************************

                    //4-D
                    #region MOVE ARQUIVO, APÓS PRIMEIRO REGISTRO PERTENCER AO ARQUIVO.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4D", "VALIDAÇÃO DO SALVAMENTO.");
                    //*********************************************************************************************************************************

                    if (_strInformacao[0] == "+OK")
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4D", "VALIDAÇÃO OK.");
                        //*********************************************************************************************************************************

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4D", "FAZ BACKUP DO ARQUIVO ANTES DE MOVER.");
                        //*********************************************************************************************************************************

                        //**********************************************************************************
                        if (utilitarios.ultilitarios.backup(arquivoNameTemp.Trim(), strDiretorioEscolhido, "backup") == false)
                        {

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "14", "FALHA AO GUARDAR ARQUIVO PROCESSADO.");
                            //*********************************************************************************************************************************
                            
                            acessa_painel("GLOBALSTAR", "XML", "|FALHA AO GUARDAR ARQUIVO PROCESSADO|14|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                            _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|FALHA AO GUARDAR ARQUIVO PROCESSADO|14|".PadRight(60, ' ')).Substring(0, 60) + "0100000000000000"; //Falha.

                            _intProcessamento++;

                        }
                        else
                        {
                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4D", "BACKUP EFETUADA COM SUCESSO.");
                            //*********************************************************************************************************************************
                        }
                        //**********************************************************************************


                    }
                    else
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                        "GLOBALSTAR.CS", "xmlFtp", "4D", "FALHA NA VALIDAÇÃO.");
                        //*********************************************************************************************************************************

                        
                        //**********************************************************************************
                        if (_strInformacao[0].IndexOf("ERRO|GLOBALSTAR|MENSAGEM NÃO PADRÃO.|15|") != -1)
                        {
                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4D", "VALIDAÇÃO OK.");
                            //*********************************************************************************************************************************

                            _strRetorno[_intProcessamento] = (_strInformacao[0].PadRight(60, ' ')).Substring(0, 60) + "0010000000000000"; //Erro.

                            _intProcessamento++;

                            #region DIRETÓRIO

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlFtp", "4D", "VERIFICA-SE DIRETÓRIO EXISTE.");
                            //*********************************************************************************************************************************

                            //Se o diretório não existir.
                            if (!Directory.Exists(strDiretorioEscolhido + "\\backupErro"))
                            {
                                try
                                {
                                    //Criamos um novo.
                                    Directory.CreateDirectory(strDiretorioEscolhido + "\\backupErro");

                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlFtp", "4D", "CRIA UM NOVO DIRETÓRIO.");
                                    //*********************************************************************************************************************************


                                }
                                catch (Exception)
                                {
                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlFtp", "14", "ERRO EM CRIAR DIRETÓRIO.");
                                    //*********************************************************************************************************************************

                                    acessa_painel("GLOBALSTAR", "XML", "|ERRO EM CRIAR DIRETÓRIO|14|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                                    _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|ERRO EM CRIAR DIRETÓRIO|14|".PadRight(60, ' ')).Substring(0, 60) + "0100000000000000"; //Falha.

                                    _intProcessamento++;

                                }

                            }

                            #endregion


                            //**********************************************************************************
                            if (utilitarios.ultilitarios.backup(_strArquivo[i], strDiretorioEscolhido, "backupErro") == false)
                            {
                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "14", "FALHA NO BACKUP DO ARQUIVO PROCESSADO.");
                                //*********************************************************************************************************************************
                                
                                acessa_painel("GLOBALSTAR", "XML", "|FALHA EM GUARDAR ARQUIVO PROCESSADO|14|" + DateTime.Now.ToString(), strLocalLog);  //erro com mensagem.

                                _strRetorno[_intProcessamento] = ("ERRO|GLOBALSTAR|ARQUIVO|FALHA EM GUARDAR ARQUIVO PROCESSADO|14|".PadRight(60, ' ')).Substring(0, 60) + "0100000000000000"; //Falha.

                                _intProcessamento++;

                            }
                            else
                            {

                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlFtp", "4D", "BACKUP EFETUADA COM SUCESSO.");
                                //*********************************************************************************************************************************
                            
                            }
                            //**********************************************************************************

                        }
                        //**********************************************************************************

                                        
                    }

                    
                    #endregion

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlFtp", "4D", "FIM DO MOVE ARQUIVO PROCESSADO.");
                    //*********************************************************************************************************************************


                }

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlFtp", "4", "FIM DO LOOP CONTENDO INFORMAÇÕES.");
                //*********************************************************************************************************************************


            }
            #endregion
            
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "4", "FIM DO TRATAMENTO E SALVAMENTO DAS INFORMAÇÕES.");
            //*********************************************************************************************************************************

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlFtp", "5", "FIM DO METODO xmlFtp.");
            //*********************************************************************************************************************************
            
            //(5)
            return _strRetorno;  //Retorno do processamento é de seus status.

            //*******************************************************

        }




        //(3) - Metodo para processar registro salvo.
        //-------------------------
        public string[] xmlExtracao     (string[] strParametro, string strPasta, bool bDebug)
        {
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlExtracao", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            #region VARIÁVEIS

            string[] _strInformacao = new string[25];
            string[] _strRetorno    = new string[500];

            string _dbErroRecord    = "";    //Controle de erro para record.               
            string _dbErroConexao   = "";    //Controle de erro para conexão.

            int _intProcessamento   = 2;     //Processamento.

            int _intProc            = 0;    //Quantidade max de processamento.

            SqlDataReader _record_1 = null;  

            SqlConnection _banco_1  = null;  
            //*******************************************************

            //Instânciamento da classe dataBase.
            database.database dat = new database.database();
            
            //Inicia processamento +OK
            _strRetorno[0] = "+OK";

            //*******************************************************

            #endregion



             
 
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlExtracao", "0", "Conexão ao banco de dados.");
            //*********************************************************************************************************************************

            //Conexão ao banco de dados.
            _banco_1 = dat.conexaoBancoSql(strParametro[1], strParametro[2], strParametro[3], strParametro[4], ref _dbErroConexao, Convert.ToInt32(strParametro[5]), strParametro[6]);
                        
            //Conexão com registros do banco 1.
            //SubSelect
            //var select1 = "*, (select b.Codigo_vei from RAS_Veiculo b where Codigo_vei = a.codigo_vei ) as resultado";
            //_record_1 = dat.conexaoRecordSql("select " + select1 + " from [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "] a  with(nolock) where a." + strParametro[38].Trim() + " = 0", _banco_1, ref _dbErroRecord);

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlExtracao", "0", "Seleciona registros.");
            //*********************************************************************************************************************************
            
            //Seleciona registros.
            _record_1 = dat.conexaoRecordSql("select * from [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "] a  with(nolock) where codigo_vei like '0-%' and a." + strParametro[38].Trim() + " = 0", _banco_1, ref _dbErroRecord);
            
            //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlExtracao", "3", "Detecta erro.");
            //*********************************************************************************************************************************

            //(3) Detecta erro.
            if (_dbErroConexao.Length > 0 || _dbErroRecord.Length > 0)
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlExtracao", "3", "ERRO, SELECIONAR REGISTRO BRUTO.");
                //*********************************************************************************************************************************

                //Retorna erro detalhado.
                _strRetorno[0] = "|ERRO, SELECIONAR REGISTRO BRUTO." + _dbErroConexao + "|" + _dbErroRecord + "|1|";

                //Retorna erro detalhado no painel de log.
                acessa_painel("GLOBALSTAR", "BANCO", "|ERRO, SELECIONAR REGISTRO BRUTO." + _dbErroConexao + "|" + _dbErroRecord + "|1|", strLocalLog);

                //Controla erro no painel.
                _int_pr_falha = 1;

            }
            else
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                "GLOBALSTAR.CS", "xmlExtracao", "3", "INICIO DO PROCESSAMENTO.");
                //*********************************************************************************************************************************

                try
                {
                    //string aa = "aa";
                    //int bb = Convert.ToInt32(aa);
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlExtracao", "4", "INICIO DO PROCESSAMENTO, PARTE-2.");
                    //*********************************************************************************************************************************

                  #region (4) Loop dos dados brutos encontrados.

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlExtracao", "4", "INICIO DO LOOP CONTENDO REGISTROS.");
                    //*********************************************************************************************************************************

                    //(4) Loop dos dados brutos encontrados.
                    //#region 
                    //while (_record_1.Read())
                    //        {

                    //            string teste = _record_1["fl_seed"] as string;
                    //            _record_1.Close();
                    //               //dat.testandoConect();
                    //        }
                    #endregion

                    while (_record_1.Read())
                            {

                               
                       
                                                              
                                //*********************************************************************************************************************************
                                //Debug.
                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                "GLOBALSTAR.CS", "xmlExtracao", "4", "Permissão para processar + ERRO DE DECODIFICAÇÃO / ERRO DE DADOS.");
                                //*********************************************************************************************************************************
                                
                                //(7) Permissão para processar + ERRO DE DECODIFICAÇÃO / ERRO DE DADOS.
                                if (permissaoProcessamento(_record_1[strParametro[36]].ToString(), ref strParametro, bDebug) == true)
                                {
                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlExtracao", "4", "PERMISSÃO OK.");
                                    //*********************************************************************************************************************************

                                    //Controla quantidade de processamento.
                                    if (_intProc >= int.Parse(strParametro[12]))
                                        break;

                                    //(5) Zera controle do painel.
                                    _int_pr_ok           = 0;
                                    _int_pr_tipo         = 1;
                                    _int_pr_falha        = 0;
                                    _int_pr_erro         = 0;
                                    _int_pr_texto        = 0;
                                    _int_pr_binario      = 0;

                                    _int_pr_semp_aviso   = 0;
                                    _int_pr_aviso_cod    = 0;
                                    _int_pr_aviso_8_cod  = 0;
                                    _int_pr_sem_destino  = 0;
                                    _int_pr_erro_formato = 0;
                                    _int_pr_normal       = 0;
                                    _int_pr_panico       = 0;
                                    _int_pr_msg          = 0;
                                    _int_pr_teclado      = 0;

                                    #region (6) Montagem dos dados.
                                    //(6) Montagem dos dados dos registros.
                                    try
                                    {
                                        
                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "6", "INICIO DA MONTAGEM DE DADOS.");
                                        //*********************************************************************************************************************************

                                        //Montagem do vetor informações.
                                        _strInformacao[0] = "+OK";
                                        _strInformacao[1] = _record_1[strParametro[34]].ToString();                                               //Mensagem
                                        _strInformacao[2] = utilitarios.ultilitarios.decodificarBase64(_record_1[strParametro[34]].ToString());   //Decofificado

                                        //Carrega informação.
                                        _strInformacao[3] = _record_1[strParametro[35]].ToString();                                               //Hexadecimal.
                                        _strInformacao[4] = _record_1[strParametro[36]].ToString();                                               //Veículo ou equipamento.
                                        _strInformacao[5] = Convert.ToDateTime(_record_1[strParametro[37]]).ToString("yyyy-MM-dd HH:mm:ss");      //Data mensagem / formato 00/00/0000.
                                        _strInformacao[6] = Convert.ToDateTime(_record_1[strParametro[37]]).ToString("yyyy-MM-dd HH:mm:ss");      //Hora mensagem / formato 00:00:00.
                                        _strInformacao[7] = _record_1[strParametro[39]].ToString();                                               //Id mensagem.
                                        _strInformacao[8] = " ";                                                                                  //Retorno longitude.
                                        _strInformacao[9] = " ";                                                                                  //Retorno latitude.
                                        _strInformacao[10] = " ";                                                                                  //Retorno data + hora Atualizada.
                                        _strInformacao[11] = "";                                                                                   //Retorna informação adicional exemplo: mensagem globalstar.
                                        _strInformacao[12] = "";                                                                                   //Panico.
                                        _strInformacao[13] = "";                                                                                   //Mensagem.
                                        _strInformacao[14] = "";                                                                                   //Teclado.
                                        _strInformacao[15] = "";   //Status PREPS                                                                  //Teclado.
                                        _strInformacao[16] = "";   //NORMAL,BATERIA FRACA,PANICO,BATERIA.                                          //Teclado.
                                        _strInformacao[17] = "";                                                                                   //Teclado.
                                        _strInformacao[18] = "";                                                                                   //Teclado.
                                        _strInformacao[19] = "";                                                                                   //Teclado.
                                        _strInformacao[20] = "";                                                                                   //Teclado.
                                        _strInformacao[21] = "";                                                                                   //Teclado.
                                        _strInformacao[22] = "";                                                                                   //Teclado.
                                        _strInformacao[23] = "";                                                                                   //Teclado.
                                        _strInformacao[24] = "";                                                                                   //Teclado.

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "6", "FIM DA MONTAGEM DE DADOS.");
                                        //*********************************************************************************************************************************
                                    }
                                    catch (Exception e)
                                    {
                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "2", "ERRO, MONTAGEM DE DADOS DO REGISTRO.");
                                        //*********************************************************************************************************************************

                                        //Controla erro no painel.
                                        _int_pr_falha = 1;

                                        //Retorna erro detalhado.
                                        _strRetorno[_intProc] = ("|ERRO, MONTAGEM DE DADOS DO REGISTRO.|2|" + e.ToString() + "|").Substring(0, 50)

                                                + _int_pr_tipo         + ""
                                                + _int_pr_ok           + ""
                                                + _int_pr_falha        + ""
                                                + _int_pr_erro         + ""
                                                + _int_pr_texto        + ""
                                                + _int_pr_binario      + ""
                                                + _int_pr_semp_aviso   + ""
                                                + _int_pr_aviso        + ""
                                                + _int_pr_aviso_cod    + ""
                                                + _int_pr_aviso_8_cod  + ""
                                                + _int_pr_sem_destino  + ""
                                                + _int_pr_erro_formato + ""
                                                + _int_pr_normal       + ""
                                                + _int_pr_panico       + ""
                                                + _int_pr_msg          + ""
                                                + _int_pr_teclado;

                                        //Retorna erro detalhado no painel de log.
                                        acessa_painel("GLOBALSTAR", "PARAMETRO", "|ERRO, MONTAGEM DE DADO DO REGISTRO.|2|" + e.ToString() + "|", strLocalLog);

                                    }
                                    #endregion

                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlExtracao", "5", "Detecta erro ou mensagem do equipamento, antes de continuar o processamento.");
                                    //*********************************************************************************************************************************
                                    
                                    //Detecta erro ou mensagem do equipamento, antes de continuar o processamento.
                                    if (_strInformacao[0] != "+OK")
                                    {
                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "5", "FALHA NA MENSAGEM.");
                                        //*********************************************************************************************************************************

                                        //Contador de equipamentos.
                                        _intProc++;

                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "7B", "(7-b) Atualiza status = 3, significa processamento não efetuado.");
                                        //*********************************************************************************************************************************

                                        #region (7-b) Atualiza status = 3, significa processamento não efetuado.
                                        //Atualiza status = 3, significa processamento não efetuado.
                                        
                                        if (dat.atualizaDadosBrutosGlobalstar(ref _strInformacao, ref strParametro, ref _dbErroConexao, ref _dbErroRecord, 3))
                                        {
                                            
                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "7B", "MENSAGEM DE EQUIPAMENTO.");
                                            //*********************************************************************************************************************************

                                            //Controla erro no painel.
                                            _int_pr_erro = 0;
                                            _int_pr_sem_destino = 0;
                                            _int_pr_normal = 1;

                                            //Alteração do status = 3 evita futuro re-processamento.
                                            _strRetorno[_intProc] = ("|MSG:|" + _strInformacao[1] + "|" + _strInformacao[2] + "|" + _strInformacao[4] + "|7|".PadRight(60, ' ')).Substring(0, 60)

                                            + _int_pr_ok           + ""
                                            + _int_pr_falha        + ""
                                            + _int_pr_erro         + ""
                                            + _int_pr_texto        + ""
                                            + _int_pr_binario      + ""
                                            + _int_pr_semp_aviso   + ""
                                            + _int_pr_aviso        + ""
                                            + _int_pr_aviso_cod    + ""
                                            + _int_pr_aviso_8_cod  + ""
                                            + _int_pr_sem_destino  + ""
                                            + _int_pr_erro_formato + ""
                                            + _int_pr_normal       + ""
                                            + _int_pr_panico       + ""
                                            + _int_pr_msg          + ""
                                            + _int_pr_teclado      + "0";

                                            //Troca posição.     //Hora da mensagem.
                                            _strInformacao[10] = _strInformacao[5];

                                            //Pânico de mensagem.
                                            dat.execProcedureSqlPanicoGlobalstar(ref strParametro, _strInformacao[4], ref _strInformacao, 10, 1, _strInformacao[1]);

                                            //Retorna erro detalhado no painel de log.
                                            acessa_painel("GLOBALSTAR", "MENSAGEM-EQUIPAMENTO", "|MENSAGEM DE EQUIPAMENTO|" + _strInformacao[1] + "|" + _strInformacao[2] + "|" + _strInformacao[4] + "|7|", strLocalLog);

                                        }
                                        else
                                        {
                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "8", "ERRO, EM ALTERAR DADOS BRUTOS.");
                                            //*********************************************************************************************************************************

                                            //Controla erro no painel.
                                            _int_pr_falha = 1;
                                            _int_pr_sem_destino = 1;
                                            _int_pr_normal = 1;

                                            //Retorna erro detalhado.
                                            _strRetorno[_intProc] = ("|ERRO, EM ALTERAR DADOS BRUTOS |8|" + _strInformacao[2] + "|" + _strInformacao[4] + "|" + _strRetorno[_intProc].PadRight(60, ' ')).Substring(0, 60)

                                            + _int_pr_ok           + ""
                                            + _int_pr_falha        + ""
                                            + _int_pr_erro         + ""
                                            + _int_pr_texto        + ""
                                            + _int_pr_binario      + ""
                                            + _int_pr_semp_aviso   + ""
                                            + _int_pr_aviso        + ""
                                            + _int_pr_aviso_cod    + ""
                                            + _int_pr_aviso_8_cod  + ""
                                            + _int_pr_sem_destino  + ""
                                            + _int_pr_erro_formato + ""
                                            + _int_pr_normal       + ""
                                            + _int_pr_panico       + ""
                                            + _int_pr_msg          + ""
                                            + _int_pr_teclado      + "0";

                                            //Retorna erro detalhado no painel de log.
                                            acessa_painel("GLOBALSTAR", "BANCO", "|ERRO, EM ALTERAR DADOS BRUTOS |8|" + _strInformacao[2] + "|" + _strInformacao[4] + "|" + _strRetorno[_intProc], strLocalLog);

                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        //*********************************************************************************************************************************
                                        //Debug.
                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                        "GLOBALSTAR.CS", "xmlExtracao", "0", "Processamento.");
                                        //*********************************************************************************************************************************

                                        #region (7) Processamento.

                                        //Não esta cadastrado.   
                                        //if (_record_1["resultado"].ToString() == string.Empty)

                                        if (1 == 2)
                                        {
                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "7", "ERRO, Equipamento não cadastrado.");
                                            //*********************************************************************************************************************************

                                            //Contador de equipamentos.
                                            _intProc++;

                                            //Controla erro no painel.
                                            _int_pr_erro = 0;
                                            _int_pr_sem_destino = 1;

                                            //Alteração do status = 3 evita futuro re-processamento.
                                            _strRetorno[_intProc] = ("|ERRO, Equipamento não cadastrado.|" + _strInformacao[4] + "|" + "|7|".PadRight(60, ' ')).Substring(0, 60)

                                            + _int_pr_ok           + ""
                                            + _int_pr_falha        + ""
                                            + _int_pr_erro         + ""
                                            + _int_pr_texto        + ""
                                            + _int_pr_binario      + ""
                                            + _int_pr_semp_aviso   + ""
                                            + _int_pr_aviso        + ""
                                            + _int_pr_aviso_cod    + ""
                                            + _int_pr_aviso_8_cod  + ""
                                            + _int_pr_sem_destino  + ""
                                            + _int_pr_erro_formato + ""
                                            + _int_pr_normal       + ""
                                            + _int_pr_panico       + ""
                                            + _int_pr_msg          + ""
                                            + _int_pr_teclado      + "0";

                                        }


                                        else
                                        {
                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "8", "processamento das informações.");
                                            //*********************************************************************************************************************************

                                            //Contador de equipamentos.
                                            _intProc++;

                                            #region (8) processamento das informações.

                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "8", "INICIO DO XML TRATAMENTO.");
                                            //*********************************************************************************************************************************

                                            // public bool globalStar(ref string[] strParametro, string strHexadecimal, ref string[] strInformacao)

                                            //(8) processamento das informações, fornecendo o vetor _strInformacao por referencia..
                                            //if (processaDados(ref _strInformacao, ref _dbErroProcessamento, ref strParametro))        
                                      
                                            if (xmlProcessamento(ref strParametro, ref _strInformacao, strPasta, bDebug))
                                            {
                                                
                                              
                                                //*********************************************************************************************************************************
                                                //Debug.
                                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                "GLOBALSTAR.CS", "xmlExtracao", "8", "INICIO DA CONDIÇÃO XMLPROCESSAMENTO.");
                                                //*********************************************************************************************************************************

                                                //*********************************************************************************************************************************
                                                //Debug.
                                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                "GLOBALSTAR.CS", "xmlExtracao", "8", "XML TRATAMENTO OK.");
                                                //*********************************************************************************************************************************

                                                _dbErroRecord  = "";    //Controle de erro para record,  evita propagação do erro para demais registros.               
                                                _dbErroConexao = "";    //Controle de erro para conexão, evita propagação do erro para demais registros.

                                                //*********************************************************************************************************************************
                                                //Debug.
                                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                "GLOBALSTAR.CS", "xmlExtracao", "9", "INICIO DO SALVAMENTO DOS DADOS.");
                                                //*********************************************************************************************************************************
                                                
                                                 
                                                //(9) Salva dados convertidos.  
                                                string teste= "" + _record_1;                                           
                                                if (dat.salvaDadosGlobalstar(ref _strInformacao, ref strParametro, ref _dbErroConexao, ref _dbErroRecord))
                                                {
                                                    teste = "" + _record_1;
                                                    
                                                   
                                                    //*********************************************************************************************************************************
                                                    //Debug.
                                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                    "GLOBALSTAR.CS", "xmlExtracao", "9", "DADOS SALVO.");
                                                    //*********************************************************************************************************************************


                                                    #region Atualiza e Controla painel.

                                                    //*********************************************************************************************************************************
                                                    //Debug.
                                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                    "GLOBALSTAR.CS", "xmlExtracao", "10", "ATUALIZADA DADOS.");
                                                    //*********************************************************************************************************************************

                                                    //(10) Atualiza campo para status = 1- equipamento processado com sucesso.| 2- Não tem | 3- Erro: | 5- Dados(não executa aqui somente na primeira etapa) 
                                                    if (dat.atualizaDadosBrutosGlobalstar(ref _strInformacao, ref strParametro, ref _dbErroConexao, ref _dbErroRecord, 1))
                                                    {

                                                        //*********************************************************************************************************************************
                                                        //Debug.
                                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                        "GLOBALSTAR.CS", "xmlExtracao", "10", "DADOS ATUALIZADO.");
                                                        //*********************************************************************************************************************************


                                                        //Controla erro no painel.
                                                        _int_pr_ok = 1;

                                                        //**********************************************************************************************************************
                                                            if (_strInformacao[16].Trim().Length == 0)
                                                            {

                                                                //Alteração do status = 1,evita futuro re-processamento.  
                                                                _strRetorno[_intProc] = ((_strInformacao[4].Trim() + "                                     ").Substring(0, 13) + "," + (_strInformacao[5] + "                           ").Substring(0, 16) + ",Lat:" + (_strInformacao[9] + "                ").Substring(0, 10) + " Log:" + (_strInformacao[8] + "                ").Substring(0, 10).PadRight(60, ' ')).Substring(0, 60)

                                                               + _int_pr_ok           + ""
                                                               + _int_pr_falha        + ""
                                                               + _int_pr_erro         + ""
                                                               + _int_pr_texto        + ""
                                                               + _int_pr_binario      + ""
                                                               + _int_pr_semp_aviso   + ""
                                                               + _int_pr_aviso        + ""
                                                               + _int_pr_aviso_cod    + ""
                                                               + _int_pr_aviso_8_cod  + ""
                                                               + _int_pr_sem_destino  + ""
                                                               + _int_pr_erro_formato + ""
                                                               + _int_pr_normal       + ""
                                                               + _int_pr_panico       + ""
                                                               + _int_pr_msg          + ""
                                                               + _int_pr_teclado      + "0";

                                                            }
                                                            else
                                                            {

                                                                //Alteração do status = 1,evita futuro re-processamento.
                                                                _strRetorno[_intProc] = (((_strInformacao[16]).Substring(0, 3) + ":" + _strInformacao[4].Trim() + "                                ").Substring(0, 13) + "," + (_strInformacao[5] + "                           ").Substring(0, 16) + ",Lat:" + (_strInformacao[9] + "                ").Substring(0, 10) + " Log:" + (_strInformacao[8] + "                ").Substring(0, 10).PadRight(60, ' ')).Substring(0, 60)

                                                               + _int_pr_ok           + ""
                                                               + _int_pr_falha        + ""
                                                               + _int_pr_erro         + ""
                                                               + _int_pr_texto        + ""
                                                               + _int_pr_binario      + ""
                                                               + _int_pr_semp_aviso   + ""
                                                               + _int_pr_aviso        + ""
                                                               + _int_pr_aviso_cod    + ""
                                                               + _int_pr_aviso_8_cod  + ""
                                                               + _int_pr_sem_destino  + ""
                                                               + _int_pr_erro_formato + ""
                                                               + _int_pr_normal       + ""
                                                               + _int_pr_panico       + ""
                                                               + _int_pr_msg          + ""
                                                               + _int_pr_teclado      + "0";

                                                            }
                                                        //**********************************************************************************************************************
                                                           

                                                    }
                                                        
                                                    else
                                                    {
                                                        
                                                        #region //Controla erro no painel.

                                                        //*********************************************************************************************************************************
                                                        //Debug.
                                                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                        "GLOBALSTAR.CS", "xmlExtracao", "3", "ERRO, ATUALIZAR DADOS BRUTOS.");
                                                        //*********************************************************************************************************************************

                                                        //Controla erro no painel.
                                                        _int_pr_falha = 1;

                                                        //Retorna erro detalhado.
                                                        _strRetorno[_intProc] = ("|ERRO, ATUALIZAR DADOS BRUTOS|3|" + _strInformacao[4].Trim() + "|" + _strInformacao[10] + "|".PadRight(60, ' ')).Substring(0, 60)

                                                        + _int_pr_ok           + ""
                                                        + _int_pr_falha        + ""
                                                        + _int_pr_erro         + ""
                                                        + _int_pr_texto        + "" 
                                                        + _int_pr_binario      + ""
                                                        + _int_pr_semp_aviso   + ""
                                                        + _int_pr_aviso        + "" 
                                                        + _int_pr_aviso_cod    + ""
                                                        + _int_pr_aviso_8_cod  + ""
                                                        + _int_pr_sem_destino  + "" 
                                                        + _int_pr_erro_formato + ""
                                                        + _int_pr_normal       + ""
                                                        + _int_pr_panico       + "" 
                                                        + _int_pr_msg          + "" 
                                                        + _int_pr_teclado      + "0";

                                                        //Retorna erro detalhado no painel de log.
                                                        acessa_painel("GLOBALSTAR", "a", "|ERRO, ATUALIZAR DADOS BRUTOS|3|" + _strInformacao[4].Trim() + "|" + _strInformacao[10] + "|", strLocalLog);

                                                        #endregion

                                                    }
                                                   


                                                    #endregion

                                                }
                                                else
                                                {
                                                    //*********************************************************************************************************************************
                                                    //Debug.
                                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                    "GLOBALSTAR.CS", "xmlExtracao", "5", "ERRO, EM SALVAR DADOS .");
                                                    //*********************************************************************************************************************************


                                                    #region //Controla erro no painel.

                                                    //Controla erro no painel.
                                                    _int_pr_falha = 1;

                                                    //(13-c) Retorna erro detalhado.
                                                    _strRetorno[_intProc] = ("|ERRO, EM SALVAR DADOS |5|" + (_strInformacao[4].Trim() + "                ").Substring(0, 10) + "|" + (_strInformacao[10] + "                           ").Substring(0, 15) + "|" + _dbErroConexao + "|" + _dbErroRecord + "|".PadRight(60, ' ')).Substring(0, 60)

                                                     + _int_pr_ok           + "" 
                                                     + _int_pr_falha        + "" 
                                                     + _int_pr_erro         + "" 
                                                     + _int_pr_texto        + "" 
                                                     + _int_pr_binario      + ""
                                                     + _int_pr_semp_aviso   + ""
                                                     + _int_pr_aviso        + "" 
                                                     + _int_pr_aviso_cod    + ""
                                                     + _int_pr_aviso_8_cod  + ""
                                                     + _int_pr_sem_destino  + "" 
                                                     + _int_pr_erro_formato + "" 
                                                     + _int_pr_normal       + ""
                                                     + _int_pr_panico       + "" 
                                                     + _int_pr_msg          + "" 
                                                     + _int_pr_teclado      + "0";


                                                    //Retorna erro detalhado no painel de log.
                                                    acessa_painel("GLOBALSTAR", "BANCO", "|ERRO, EM SALVAR DADOS |5|" + _strInformacao[4].Trim() + "|" + _strInformacao[10] + "|" + _dbErroConexao + "|" + _dbErroRecord + "|", strLocalLog);

                                                    #endregion

                                                }

                                                //*********************************************************************************************************************************
                                                //Debug.
                                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                "GLOBALSTAR.CS", "xmlExtracao", "0", "FIM DA CONDIÇÃO XMLPROCESSAMENTO.");
                                                //*********************************************************************************************************************************


                                            }
                                            else
                                            {

                                                //*********************************************************************************************************************************
                                                //Debug.
                                                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                                "GLOBALSTAR.CS", "xmlExtracao", "6", "ERRO, NO PROCESSAMENTO.");
                                                //*********************************************************************************************************************************

                                                #region //Controla erro no painel.

                                                //Controla erro no painel.
                                                _int_pr_falha = 1;

                                                //Retorna erro detalhado.
                                                _strRetorno[_intProc] = ("|ERRO, NO PROCESSAMENTO.|6|" + _strInformacao[4].Trim() + "|" + _strInformacao[10] + "|".PadRight(60, ' ')).Substring(0, 60)

                                                + _int_pr_ok           + "" 
                                                + _int_pr_falha        + ""
                                                + _int_pr_erro         + ""
                                                + _int_pr_texto        + "" 
                                                + _int_pr_binario      + ""
                                                + _int_pr_semp_aviso   + ""
                                                + _int_pr_aviso        + "" 
                                                + _int_pr_aviso_cod    + ""
                                                + _int_pr_aviso_8_cod  + ""
                                                + _int_pr_sem_destino  + ""
                                                + _int_pr_erro_formato + "" 
                                                + _int_pr_normal       + ""
                                                + _int_pr_panico       + "" 
                                                + _int_pr_msg          + "" 
                                                + _int_pr_teclado      + "0";

                                                //Retorna erro detalhado no painel de log.
                                                acessa_painel("GLOBALSTAR", "BANCO", "|ERRO, NO PROC.|6|" + _strInformacao[4].Trim() + "|" + _strInformacao[10] + "|", strLocalLog);


                                                #endregion

                                            }

                                            //*********************************************************************************************************************************
                                            //Debug.
                                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                            "GLOBALSTAR.CS", "xmlExtracao", "8", "FIM DO XML TRATAMENTO..");
                                            //*********************************************************************************************************************************

                                            #endregion

                                        }

                                        #endregion

                                    }


                                }
                                   
                                else
                                {

                                    //*********************************************************************************************************************************
                                    //Debug.
                                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                                    "GLOBALSTAR.CS", "xmlExtracao", "0", "INICIO DO PROCESSAMENTO.");
                                    //*********************************************************************************************************************************
                                    
                                }

                              
                            }

                            //*********************************************************************************************************************************
                            //Debug.
                            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                            "GLOBALSTAR.CS", "xmlExtracao", "4", "FIM DO LOOP CONTENDO REGISTROS.");
                            //*********************************************************************************************************************************

                           

                }
                catch (Exception e)
                {
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlExtracao", "9", "ERRO, CONEXÃO.");
                    //*********************************************************************************************************************************

                    //(4-b) Controla erro no painel.
                    _int_pr_falha = 1;

                    //(4-c) Retorna erro detalhado.
                    _strRetorno[_intProc] = ( "|ERRO, CONEXÃO |9|" + e.ToString() +"|".PadRight(60, ' ')).Substring(0, 60)

                    + _int_pr_ok           + "" 
                    + _int_pr_falha        + "" 
                    + _int_pr_erro         + "" 
                    + _int_pr_texto        + "" 
                    + _int_pr_binario      + ""
                    + _int_pr_semp_aviso   + ""
                    + _int_pr_aviso        + "" 
                    + _int_pr_aviso_cod    + "" 
                    + _int_pr_aviso_8_cod  + ""
                    + _int_pr_sem_destino  + "" 
                    + _int_pr_erro_formato + "" 
                    + _int_pr_normal       + ""
                    + _int_pr_panico       + "" 
                    + _int_pr_msg          + "" 
                    + _int_pr_teclado      + "0";

                    //(4-d) Retorna erro detalhado no painel de log.
                    acessa_painel("GLOBALSTAR", "BANCO", "|ERRO, CONEXÃO |9|" + e.ToString() + "\n \rEquip:" + _strInformacao[4] + "\n \r Data:" + DateTime.Now + "|", strLocalLog);

                }


                finally
                {
                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
                    "GLOBALSTAR.CS", "xmlExtracao", "0", "finally, FECHA TODAS AS CONEXÕES ABERTAS PELA CLASSE.");
                    //*********************************************************************************************************************************

                    //fecha recordset
                    if (_record_1 != null)
                    {
                        _record_1.Close();
                        _record_1.Dispose();
                    }

                    //fecha Banco de dados
                    dat.fechaSql();

                }


            }

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strPasta, false,
            "GLOBALSTAR.CS", "xmlExtracao", "0", "FIM DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            return _strRetorno;
        
        }


        //(5) - Metodo para processar LAT: e LOG retorna dados por ref.
        //-------------------------
        public bool xmlProcessamento(ref string[] strParametro, ref string[] strInformacao, string strLocal, bool bDebug)
        {
            
            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
            "GLOBALSTAR.CS", "xmlProcessamento", "0", "INICIO DO PROCESSAMENTO.");
            //*********************************************************************************************************************************

            bool _status            = true;

            double _latitude        = 0;
            double _longitude       = 0;

            SqlConnection _banco_1  = null; //Conexão ao banco de dados, Sql Server.
            SqlDataReader _record_1 = null; //Conexão ao registro.

            string _dbErroConexao   = "";   //Controla retorno da conexão.
            string _dbErroRecord    = "";   //Controla retorno de seleção de registros.

            //Loop dos registros não processados, que seja globalstar.
            string[] _hexTratado = strInformacao[3].Trim().Split(' ');

            database.database dat        = new database.database();

            CultureInfo culturaAmericana = new CultureInfo("en-US");
            
            try
            {

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                "GLOBALSTAR.CS", "xmlProcessamento", "0", "INICIO DA ROTINA DE TRATAMENTO DE DADOS");
                //*********************************************************************************************************************************

                var t = (Convert.ToInt32(_hexTratado[1], 16) & 3);
                
              
           
            
                //Verifica-se a mensagem é padrão.
                if ((Convert.ToInt32(_hexTratado[1], 16) & 3) == 0)
                {
                 

                    //Obtem latitude.
                    _latitude = ((double)Convert.ToInt32(_hexTratado[2] + _hexTratado[3] + _hexTratado[4], 16) / 8388608) * 90;

                    //Calcula latitude acima de 90º
                    if (_latitude > 90)
                    {
                        _latitude = _latitude - 180;
                    }


                    //Obtem longitude.
                    _longitude = ((double)Convert.ToInt32(_hexTratado[5] + _hexTratado[6] + _hexTratado[7], 16) / 8388608) * 180;

                    //Calcula longitude acima de 180º
                    if (_longitude > 180)
                    {
                        _longitude = _longitude - 360;
                    }


                    //Apenas formata a string para 0.00000000

                    var latTemp = string.Format("{0:0.000000}", _latitude);
                    var logTemp = string.Format("{0:0.000000}", _longitude);

                    //Troca virgula(,) pelo ponto(.)

                    strInformacao[8] = logTemp.Replace(',', '.');
                    strInformacao[9] = latTemp.Replace(',', '.');


                    //***************************************************************************************************
                    //VERIFICO-SE STATUS DA BATERIA E GPS-DATA.
                    //Vou gerar um pânico para bateria fraca.

                    var bateria = 0;
                    var gps     = 0;

                    
                    // Menssagem diagnostico
                    int diag = Convert.ToInt32(_hexTratado[1], 16) / 4;//21
                    if ((Convert.ToInt32(_hexTratado[1], 16) & 4) != 0)
                    {
                        bateria = 1;//pilha interna ruim
                    }

                    //Verifica Gps-data
                    if ((Convert.ToInt32(_hexTratado[1], 16) & 8) != 0)
                    {
                        gps = 1;
                    }

                    //***********************************************************************************
                    
                    var strTemp = Convert.ToString(Convert.ToInt32(_hexTratado[8], 16), 2);

                    acessa_painel("GLOBALSTAR", "INPUT", "(".PadRight(8 - strTemp.Length, '0') + strTemp + ")  Equipamento: " + strInformacao[4] + "|" + DateTime.Now.ToString() + "|" + strInformacao[3], strLocal);

                    //***********************************************************************************

                    strInformacao[15] = "";
                    strInformacao[16] = "";

                    //Aviso de bateria fraca.
                    if (bateria == 1)
                    {

                    //**********************************************************************************************************************************************************

                        //Obtem Valor Hexadecimal.
                        strInformacao[11] = _hexTratado[1];
                        
                        //Aviso tela.
                        strInformacao[16] = "BAF:BATERIA FRACA";

                        //PÂNICO DE NÍVEL DE BATERIA.
                        dat.execProcedureSqlPanicoGlobalstar(ref strParametro, strInformacao[4].Trim(), ref strInformacao, 1, 1, "BATERIA FRACA: " + strInformacao[11]);

                    //**********************************************************************************************************************************************************

                    }

                    //**********************************************************************************************************************************************************
                    //Geração de Pânico e Bateria para o PREPS.

                    try
                    {
                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                        "GLOBALSTAR.CS", "xmlProcessamento", "0", "INICIO DA ROTINA DE LOCALIZAÇÃO DO TIPO DE ALERTA.");
                        //*********************************************************************************************************************************

                        //Conexão ao banco de dados.
                        _banco_1 = dat.conexaoBancoSql(strParametro[60], strParametro[61], strParametro[62], strParametro[63], ref _dbErroConexao, Convert.ToInt32(strParametro[64]), strParametro[65]);

                        //verifica retorno.
                        if (_dbErroConexao.Length > 0)
                        {
                            return false;
                        }
                        

                        //Seleciona registros.
                        _record_1 = dat.conexaoRecordSql("select top 1 * from [" + strParametro[63] + "" + "].[dbo].[ras_alerta_preps]  with(nolock) where codigo = " + Convert.ToInt32(_hexTratado[8], 16), _banco_1, ref _dbErroRecord);

                        //verifica retorno.
                        if (_dbErroRecord.Length > 0)
                        {
                            return false;
                        }

                        //**********************************************************************************************************************************************************

                        //Verifica retorno.
                        if (_record_1.HasRows)
                        {
                            while (_record_1.Read())
                            {
                                strInformacao[15] = _record_1["gera"].ToString().Trim();   //Código PREPS.
                                strInformacao[16] = (strInformacao[16].Length > 0 ? strInformacao[16] : _record_1["alerta"].ToString().Trim()); //Alerta PREPS.
                            }

                        }

                        //Verifica retorno.
                        if (strInformacao[15].Trim().Length == 0)
                        {
                            strInformacao[15] = "#" + _hexTratado[8];
                        }

                        //**********************************************************************************************************************************************************

                        if (strInformacao[15].Trim() == "/1")
                        {
                            //Pânico aviso usuário
                            dat.execProcedureSqlPanicoGlobalstar(ref strParametro, strInformacao[4].Trim(), ref strInformacao, 1, 1, "PÂNICO");

                            //Pânico preps.
                            //strInformacao[15] 

                            //Aviso tela.
                            //strInformacao[16] = "";

                        }

                        if (strInformacao[15].Trim() == "/2")
                        {
                            //Bateria aviso usuário.
                            dat.execProcedureSqlPanicoGlobalstar(ref strParametro, strInformacao[4].Trim(), ref strInformacao, 1, 1, "BATERIA");

                            //Bateria preps.
                            //strInformacao[15] 

                            //Aviso tela.
                            //strInformacao[16] = "";

                        }

                        //*********************************************************************************************************************************
                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                        "GLOBALSTAR.CS", "xmlProcessamento", "0", "FIM DA ROTINA DE LOCALIZAÇÃO DO TIPO DE ALERTA.");
                        //*********************************************************************************************************************************

                        //**********************************************************************************************************************************************************


                    }
                    catch (Exception)
                    {

                        //*********************************************************************************************************************************

                        //Debug.
                        utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                        "GLOBALSTAR.CS", "xmlProcessamento", "16B", "ERRO, PROCESSAR DADOS.");
                        //*********************************************************************************************************************************
                     
                        //Retorna erro detalhado.
                        strInformacao[8] = "-0.00";
                        strInformacao[9] = "-0.00";
                        strInformacao[10] = "2000-01-01 01:01:00";

                        //Retorna erro detalhado no painel de log.
                        acessa_painel("GLOBALSTAR", "MENSAGEM", "|ERRO, PROCESSAR DADOS.|16B|" + strInformacao[4] + "|" + DateTime.Now.ToString(), strParametro[13]);

                        _status = false;

                        //*********************************************************************************************************************************
                    }

                    finally
                    {
                        //Fecha objeto record.
                        _record_1.Close();

                        //Limpa objeto.
                       _record_1.Dispose();
                        

                        //Fecha conexão.
                        _banco_1.Close();

                        //Limpa objeto.
                       _banco_1.Dispose();
                    
                    }

                    //**********************************************************************************************************************************************************




                }
                else
                {
                   

                    //*********************************************************************************************************************************
                    //Debug.
                    utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                    "GLOBALSTAR.CS", "xmlProcessamento", "15", "ERRO, MENSAGEM NÃO PADRÃO.");
                    //*********************************************************************************************************************************

                    

                    //Mensagem não é padrão vou gerar um log.
               

                    //Retorna erro detalhado.
                    strInformacao[10] = "2000-01-01 01:01:00";
                    strInformacao[8]  = "-0.00";
                    strInformacao[9]  = "-0.00";
                    strInformacao[5]  = "2000-01-01 01:01:00";

                    //Retorna erro detalhado no painel de log.
                    acessa_painel("GLOBALSTAR", "MENSAGEM NÃO PADRÃO.", "|ERRO, MENSAGEM NÃO PADRÃO.|15|" + strInformacao[4] + "|" + DateTime.Now.ToString(), strParametro[13]);

                    strInformacao[0] = "ERRO|GLOBALSTAR|MENSAGEM NÃO PADRÃO.|15|" + strInformacao[4] + "|";
                    
                }

                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                "GLOBALSTAR.CS", "xmlProcessamento", "0", "FIM DA ROTINA DE TRATAMENTO DE DADOS");
                //*********************************************************************************************************************************

            }
            catch (Exception)
            {
                //*********************************************************************************************************************************
                //Debug.
                utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
                "GLOBALSTAR.CS", "xmlProcessamento", "16", "ERRO, PROCESSAR DADOS.");
                //*********************************************************************************************************************************

                //Retorna erro detalhado.
                strInformacao[8]  = "-0.00";
                strInformacao[9]  = "-0.00";
                strInformacao[10] = "2000-01-01 01:01:00";
                
                //Retorna erro detalhado no painel de log.
                acessa_painel("GLOBALSTAR", "MENSAGEM", "|ERRO, PROCESSAR DADOS.|16|" + strInformacao[4] + "|" + DateTime.Now.ToString(), strParametro[13]);

                _status = false;

            }

            //*********************************************************************************************************************************
            //Debug.
            utilitarios.ultilitarios.debug(bDebug, "debug.txt", strLocal, false,
            "GLOBALSTAR.CS", "xmlProcessamento", "0", "FIM DO PROCESSAMENTO.");
            //*********************************************************************************************************************************
            
            return _status;
           
        }

    }
}
