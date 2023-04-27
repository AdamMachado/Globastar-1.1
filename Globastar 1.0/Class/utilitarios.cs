using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace utilitarios  
{
    class ultilitarios 
    {
        //Metodos para decodificação de base64.
        //-------------------------
        static public string  decodificarBase64(string strTemp)
        {
            string _StrResult = "ERRO Decodificação_base64";

            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(strTemp);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                   _StrResult  = new String(decoded_char);

            }
            catch (Exception)
            {
                   _StrResult = "erro Decodificação_base64";
            }

            return _StrResult;

        }
        
        //Metodo converter string em hexadecimal.
        //-------------------------
        static public string hexadecimal(string strTemp)
        {
            string _Status = "ERRO decodificação_hexadecimal";
            string _hex = "";

            try
            {
                byte[] todecode_byte = Convert.FromBase64String(strTemp);

                foreach (byte c in todecode_byte)
                {

                        String hex2 = Convert.ToString(c, 16);

                        if (hex2.Length == 1)
                        {
                            _hex = _hex + "0" + hex2.ToUpper().ToString() + " ";
                        }
                        else
                        {
                            _hex = _hex + hex2.ToUpper().ToString() + " ";
                        }

                }

                _Status = _hex;

            }
            catch (Exception)
            {
                _Status = "ERRO decodificação_hexadecimal";
            }

            return _Status;

        }
        
        //Metodo converter string em binario.
        //-------------------------
        public string converterBinario(string strTemp)
        {

            try
            {

                string ret = "";


                foreach (char c in strTemp)
                {
                    int asc = (int)c;

                    ret += "0" + Convert.ToString(asc, 2) + " ";
                }

                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Error converter Binário" + e.Message);
            }

        }

        //Metodo codifica string em base64.
        //-------------------------
        public string codificarBase64(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Trim().Length];

                encData_byte = System.Text.Encoding.UTF8.GetBytes(data.Trim());

                string encodedData = Convert.ToBase64String(encData_byte);

                return encodedData;

            }
            catch (Exception e)
            {
                throw new Exception("Error converter Base64" + e.Message);
            }
        }


        //Abrir arquivo de texto retorna vetor.
        //-------------------------------
        static public bool abrirArquivo(string strArquivo,string strcaminho, ref string[] strInformacao, ref string strErro)
        {
            bool _status = true;

            string strMensagem     = "";

            string strFormatoTexto = "";

            int linha = 0;



            //Verifica se arquivo existe.
            if (File.Exists(strcaminho + "\\" + @strArquivo) == true)
            {

                try
                {
                    using (StreamReader conteudo = new StreamReader(strcaminho + "\\" + @strArquivo))
                    {

                        while ((strMensagem = conteudo.ReadLine()) != null)
                        {
                            if (strMensagem.Trim().Length != 0)
                            {

                                strInformacao[linha] = strMensagem;

                                strFormatoTexto = strFormatoTexto + strMensagem;

                                linha++;
                            }
                        }

                    }

                }
                catch (Exception)
                {
                    _status = false;
                    strErro = "ERRO EM PROCESSAR ARQUIVO ABERTO.";
                }
                

            }
            else
            {
                _status = false;
            }

            return _status;

        }

        //Abrir arquivo de texto retorna string.
        //----------------------------- 
        static public bool abrirArquivo(string strArquivo, string strcaminho, ref string strFormatoTexto, ref string strErro)
        {
            bool _status = true;

            string strMensagem = "";

            //Verifica se arquivo existe.
            if (File.Exists(strcaminho + "\\" + @strArquivo) == true)
            {

                try
                {


                    using (StreamReader conteudo = new StreamReader(strcaminho + "\\" + @strArquivo))
                    {
                        while ((strMensagem = conteudo.ReadLine()) != null)
                        {
                            if (strMensagem.Trim().Length != 0)
                            {
                                strFormatoTexto = strFormatoTexto + strMensagem;
                            }
                        }
                    }


                
                }
                catch (Exception)
                {
                    _status = false;
                    strErro = "ERRO EM PROCESSAR ARQUIVO ABERTO.";
                }



            }
            else
            {
                _status = false;
            }

            return _status;

        }




        //Backup de arquivo.
        //------------------
        static public bool backup(string strArquivo, string strcaminho, string strDiretorio)
        {
            bool _status = true;

            //Verifica se arquivo existe, faz copia e deleta original.
            if (File.Exists(strcaminho + @"\" + strArquivo))
            {
                try
                {

                    //Se o diretório não existir.
                    if (!Directory.Exists(strcaminho + "\\" + strDiretorio))
                    {
                        //Criamos um novo.
                        Directory.CreateDirectory(strcaminho + "\\" + strDiretorio);
                    }

                }
                catch (Exception)
                {
                    return false;
                }


                try
                {
                    File.Copy(strcaminho + @"\" + strArquivo, strcaminho + "\\" + strDiretorio + "\\" + strArquivo.Replace('.', ' ') + " " + DateTime.Now.ToString("dd-mm-yyyy hh-mm-ss") + ".bak");
                    File.Delete(strcaminho + @"\" + strArquivo);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return _status;
        }




        //Salva arquivo de texto.
        //-----------------------------
        static public bool salvaArquivo(string strArquivo,string strCaminho, ref string[] strInformacao,int intBackup, ref string strErro)
        {

            bool _status = true;
            

            //Faz backup do arquivo antes de salvar, ou editar.
            if (intBackup == 1)
            {

                if (backup(strArquivo, strCaminho, "BACKUP") == false)
                {
                    return false;
                }

            }

            //Apenas deleta antes de salvar.
            if (intBackup == 2)
            {

                try
                {
                    File.Delete(strCaminho + @"\" + strArquivo);
                }
                catch (Exception)
                {
                    return false;
                }

            }



            //Cria um novo arquivo caso não exista.
            if (File.Exists(strCaminho + @"\" + strArquivo) == false)
            {

                try
                {
                    //Cria arquivo.
                    using (FileStream arq = File.Create(strCaminho + @"\" + strArquivo))
                    {

                    }


                }
                catch (Exception e)
                {
                    strErro = "ERRO: " + e.Message;

                    return false;
                }

            }
            



            //Salva demais informações no arquivo.
            for (var x = 0; x < strInformacao.Length; x++)
            {
                if (strInformacao[x] != null)
                {

                    //Concatena
                    try
                    {
                        //Verifica-se arquivo existe.
                        if (File.Exists(strCaminho + @"\" + strArquivo))
                        {

                            //File.AppendText Concatena informação no arquivo.
                            using (StreamWriter escrever = File.AppendText(strCaminho + @"\" + strArquivo))
                            {
                                escrever.Write("\r\n" + strInformacao[x]);
                            }

                        }
                        else
                        {

                            strErro = "ERRO arquivo não encontrado.";
                            return false;
                        }

                    }
                    catch (Exception e)
                    {

                        strErro = "ERRO: " + e.Message;
                        return false;
                    }

                }

            }

            

            //Retorna resultado do processamento.
            return _status;
            


        }

        //Salva arquivo de texto.
        //-----------------------------
        static public bool salvaArquivo(string strArquivo, string strCaminho, ref string strInformacao, int intBackup, ref string strErro)
        {

            bool _status = true;


            //Faz backup do arquivo antes de salvar, ou editar.
            if (intBackup == 1)
            {

                if (backup(strArquivo, strCaminho, "BACKUP") == false)
                {
                    return false;
                }

            }

            //Apenas deleta antes de salvar.
            if (intBackup == 2)
            {

                try
                {
                    File.Delete(strCaminho + @"\" + strArquivo);
                }
                catch (Exception)
                {
                    return false;
                }

            }



            //Cria um novo arquivo caso não exista.
            if (File.Exists(strCaminho + @"\" + strArquivo) == false)
            {

                try
                {
                    //Cria arquivo.
                    using (FileStream arq = File.Create(strCaminho + @"\" + strArquivo))
                    {

                    }


                }
                catch (Exception e)
                {
                    strErro = "ERRO: " + e.Message;

                    return false;
                }

            }



            if (strInformacao != string.Empty)
            {

                //Concatena
                try
                {
                    //Verifica-se arquivo existe.
                    if (File.Exists(strCaminho + @"\" + strArquivo))
                    {

                        //File.AppendText Concatena informação no arquivo.
                        using (StreamWriter escrever = File.AppendText(strCaminho + @"\" + strArquivo))
                        {
                            escrever.Write("\r\n" + strInformacao);
                        }

                    }
                    else
                    {

                        strErro = "ERRO arquivo não encontrado.";
                        return false;
                    }

                }
                catch (Exception e)
                {

                    strErro = "ERRO: " + e.Message;
                    return false;
                }

            }



            //Retorna resultado do processamento.
            return _status;



        }
        



        //Cálcular datatime por unixTime.
        //********************************
        public static DateTime UnixTimeParaDateTime(double unixTimeSegundos)
        {
            //Data inicial 
            DateTime Dat = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //Adiciona segundos ao datetime atual.
            Dat = Dat.AddSeconds(unixTimeSegundos).ToLocalTime();

            //Resultado do datetime.
            return Dat;
        }

        //Suporte ao debug.
        //*****************
        static public void debug(bool status, string strArquivo, string strCaminho, bool novo, string strClass, string strMetodo, string strTipo, string informacao)
        {
            string strErro = "";

            if (status)
            {

                var strTemp = "|" + (strClass.PadRight(16, ' ')).Substring(0, 15) +
                              "|" + (strMetodo.PadRight(20, ' ')).Substring(0, 19) +
                              "|" + (strTipo.PadRight(3, ' ')).Substring(0, 2) +
                              "|" + informacao + "|";


                if (novo)
                {
                    if (salvaArquivo(strArquivo, strCaminho, ref strTemp, 2, ref strErro))
                    {

                    }
                }
                else
                {
                    if (salvaArquivo(strArquivo, strCaminho, ref strTemp, 3, ref strErro))
                    {

                    }
                }

            }


        }


        #region POP3
        //Salva Configuração Padrão.
        //---------------------------
        static public bool salvaconfiguracaoPadraoRawPop3(string strArquivo, string strCaminho)
        {

            bool     _status      = true;

            string   _strErro     = "";

            string[] _strEntrada  = new string[55];

            //*************************************************************************
            
            _strEntrada[0]  = ""; 

            _strEntrada[1]  = "[Conexão]";
            _strEntrada[2]  = ""; 
            _strEntrada[3]  = "Servidor pop3=192.168.100.8" ;
            _strEntrada[4]  = "Porta pop3=110";
            _strEntrada[5]  = "Usuario pop3=raw2";
            _strEntrada[6]  = "Senha pop3=019283";
            _strEntrada[7]  = ""; 
            _strEntrada[8]  = "[Processamento]";
            _strEntrada[9]  = ""; 
            _strEntrada[10] = "Msg número=0";
            _strEntrada[11] = "Msg quantidade=50";
            _strEntrada[12] = "Tempo de leitura Pop3=14";
            _strEntrada[13] = "Excluir msg=True";
            _strEntrada[14] = "Gravar msg banco=True";
            _strEntrada[15] = "Limpa tela=5000";
            _strEntrada[16] = ""; 
            _strEntrada[17] = "[String Conexão]";
            _strEntrada[18] = ""; 
            _strEntrada[19] = "Banco hostaname=192.168.100.6";
            _strEntrada[20] = "Banco usuario=webuser";
            _strEntrada[21] = "Banco senha=web-user";
            _strEntrada[22] = "Banco tabela=ras_raw_msg";
            _strEntrada[23] = "Banco de dados=Northwind";
            _strEntrada[24] = "Banco timeout=60";
            _strEntrada[25] = "Banco Encrypt=no";
            _strEntrada[26] = ""; 
            _strEntrada[27] = "[Permissão de processamento]";
            _strEntrada[28] = ""; 
            _strEntrada[29] = "Permissão_1=ARIASAT";
            _strEntrada[30] = "Permissão_2=DAMOS.NET";
            _strEntrada[31] = "Permissão_3=ORBCOMM.NET";
            _strEntrada[32] = "Permissão_4=";
            _strEntrada[33] = "Permissão_5=";
            _strEntrada[34] = "Permissão_6=";
            _strEntrada[35] = "Permissão_7=";
            _strEntrada[36] = ""; 
            _strEntrada[37] = "[Banco de dados, campo.]";
            _strEntrada[38] = ""; 
            _strEntrada[39] = "banco campo=codigo_vei";
            _strEntrada[40] = "banco campo=sat_data";
            _strEntrada[41] = "banco campo=mha_data";
            _strEntrada[42] = "banco campo=msg_data";
            _strEntrada[43] = "banco campo=raw_data";
            _strEntrada[44] = "banco campo=uidl";
            _strEntrada[45] = "banco campo=encoding";
            _strEntrada[46] = "banco campo=raw_data_hex";
            _strEntrada[47] = "banco campo=fl_decod";
            _strEntrada[48] = "banco campo=fl_seed";       

            //*************************************************************************

            if (salvaArquivo(strArquivo,strCaminho,ref _strEntrada,0, ref _strErro) == false)
            {
                _status = false;
            }


            return _status;


        }
        #endregion

        #region TELESEED
        //Salva Configuração Padrão.
        //---------------------------
        static public bool salvaconfiguracaoPadraoTelessed(string strArquivo, string strCaminho)
        {

            bool     _status     = true;
              
            string   _strErro    = "";

            string[] _strEntrada = new string[50];

            //*************************************************************************
 
            _strEntrada[0]  = "[Conexão]";
            _strEntrada[1]  = "";
            _strEntrada[2]  = "Banco hostname=192.168.100.6";
            _strEntrada[3]  = "Banco Usuario=webuser";
            _strEntrada[4]  = "Banco Senha=web-user";
            _strEntrada[5]  = "Banco Porta=0";
            _strEntrada[6]  = "";
            _strEntrada[7]  = "[Banco de dados]";
            _strEntrada[8]  = "";
            _strEntrada[9]  = "Dados Banco=Northwind";
            _strEntrada[10] = "Dados Tabela=ras_raw_msg";
            _strEntrada[11] = "Dados Dados=raw_data";
            _strEntrada[12] = "Dados Data=msg_data";
            _strEntrada[13] = "Dados id=id_raw";
            _strEntrada[14] = "Dados Nome=codigo_vei";
            _strEntrada[15] = "Dados status=fl_seed";
            _strEntrada[16] = "Dados Hex=raw_data_hex";
            _strEntrada[17] = "Dados Encrypt=no";
            _strEntrada[18] = "Dados Timeout=60";
            _strEntrada[19] = "";
            _strEntrada[20] = "[Banco Equipamento]";
            _strEntrada[21] = "";
            _strEntrada[22] = "Equipamento Banco=Northwind";
            _strEntrada[23] = "Equipamento Tabela=ras_teleseed";
            _strEntrada[24] = "Equipamento Codigo=codigo";
            _strEntrada[25] = "Equipamento Nome=nome";
            _strEntrada[26] = "Equipamento Tipo=programacao";
            _strEntrada[27] = "Equipamento Status=status";
            _strEntrada[28] = "Equipamento Encrypt=no";
            _strEntrada[29] = "Equipamento Timeout=60";
            _strEntrada[30] = "";
            _strEntrada[31] = "[Processamento]";
            _strEntrada[32] = "";
            _strEntrada[33] = "Tempo de processamento=10";
            _strEntrada[34] = "Quantidade=10";
            _strEntrada[35] = @"Localizacao txt=c:\txt_temp";
            _strEntrada[36] = "";
            _strEntrada[37] = "[Opções para processamento]";
            _strEntrada[38] = "";
            _strEntrada[39] = "Processar=0";
            _strEntrada[40] = "Apos Processar=1";
            _strEntrada[41] = "Apos Nao Processar=2";
            _strEntrada[42] = "Limpa Tela=5000";
            //*************************************************************************

            if (salvaArquivo(strArquivo, strCaminho, ref _strEntrada, 0, ref _strErro) == false)
            {
                _status = false;
            }

            //*************************************************************************



            return _status;


        }
        #endregion

        #region GLOBALSTAR
        //Salva Configuração Padrão.
        //---------------------------
        static public bool salvaConfiguracaoPadraoGlobalstar(string strArquivo, string strCaminho)
        {

            bool _status = true;

            string _strErro = "";

            string[] strEntrada = new string[110];

            //*************************************************************************

            strEntrada[0]  = "[Conexão]";
            strEntrada[1]  = "";
            strEntrada[2]  = "Servidor=192.168.100.6";
            strEntrada[3]  = "Usuario =webuser";
            strEntrada[4]  = "Senha   =web-user";
            strEntrada[5]  = "Banco   =Northwind";
            strEntrada[6]  = "Timeout =60";
            strEntrada[7]  = "Encrypt =no";
            strEntrada[8]  = "Porta   =0";
            strEntrada[10] = "reserva =";
            strEntrada[11] = "reserva =";
            strEntrada[12] = "reserva =";

            strEntrada[13] = "[Processamento]";
            strEntrada[14] = "";
            strEntrada[15] = "Quantidade=20";
            strEntrada[16] = "Tempo     =10";
            strEntrada[17] = "Limpa Tela=5000";
            strEntrada[18] = "reserva =";
            strEntrada[19] = "reserva =";
            strEntrada[20] = "reserva =";
            strEntrada[21] = "reserva =";
            strEntrada[22] = "reserva =";
            strEntrada[23] = "reserva =";
            strEntrada[24] = "reserva =";
            strEntrada[25] = "";

            strEntrada[26] = "[Procedure]";
            strEntrada[27] = "";
            strEntrada[28] = "T_Proc_Panico=sp_I_Panico1";
            strEntrada[29] = "T_Proc_Descricao=sp_S_Rastreamento_RetDesc";
            strEntrada[30] = "T_Proc_Rastreamento=sp_I_PosSat_Rastreamento";
            strEntrada[31] = "T_Proc_Online=sp_I_PosSat_Online";
            strEntrada[32] = "T_Proc_Cerca=p_cerca";
            strEntrada[33] = "T_Proc_Velocidade=p_vel_limite";
            strEntrada[34] = "reserva =";
            strEntrada[35] = "reserva =";
            strEntrada[36] = "reserva =";
            strEntrada[37] = "";

            strEntrada[38] = "[Tabela bruto]";
            strEntrada[39] = "";
            strEntrada[40] = "banco Tabela=ras_raw_msg";
            strEntrada[41] = "banco campo =raw_data";
            strEntrada[42] = "banco campo =raw_data_hex";
            strEntrada[43] = "banco campo =codigo_vei";
            strEntrada[44] = "banco campo =msg_data";
            strEntrada[45] = "banco campo =fl_seed";
            strEntrada[46] = "banco campo =id_raw";
            strEntrada[47] = "reserva=";
            strEntrada[48] = "reserva=";
            strEntrada[49] = "reserva=";
            strEntrada[50] = "";

            strEntrada[51] = "[Pasta ftp]";
            strEntrada[52] = "";
            strEntrada[53] = @"pasta FTP1=C:\xml_temp";
            strEntrada[54] = @"pasta FTP2=";
            strEntrada[55] = "";

            strEntrada[56] = "[Campo da tabela]";
            strEntrada[57] = "banco campo=codigo_vei";
            strEntrada[58] = "banco campo=sat_data";
            strEntrada[59] = "banco campo=mha_data";
            strEntrada[60] = "banco campo=msg_data";
            strEntrada[61] = "banco campo=raw_data";
            strEntrada[62] = "banco campo=uidl";
            strEntrada[63] = "banco campo=encoding";
            strEntrada[64] = "banco campo=raw_data_hex";
            strEntrada[65] = "banco campo=fl_decod";
            strEntrada[66] = "banco campo=fl_seed";
            strEntrada[67] = "reserva=";
            strEntrada[68] = "reserva=";
            strEntrada[69] = "";

            strEntrada[70] = "[Conexão procedure]";
            strEntrada[71] = "";
            strEntrada[72] = "Servidor=192.168.100.6";
            strEntrada[73] = "Usuario =webuser";
            strEntrada[74] = "Senha   =web-user";
            strEntrada[75] = "Banco   =Northwind";
            strEntrada[76] = "Timeout =120";
            strEntrada[77] = "Encrypt =no";
            strEntrada[78] = "Porta   =0";
            strEntrada[79] = "reserva =";
            strEntrada[80] = "reserva =";
            strEntrada[81] = "reserva =";
            strEntrada[82] = "";

            strEntrada[83] = "[Permissão de processamento]";
            strEntrada[84] = "";
            strEntrada[85] = "permissão=0-1";
            strEntrada[86] = "permissão=";
            strEntrada[87] = "permissão=";
            strEntrada[88] = "permissão=";
            strEntrada[89] = "permissão=";
            strEntrada[90] = "permissão=";
            strEntrada[91] = "permissão=";
            strEntrada[92] = "permissão=";
            strEntrada[93] = "permissão=";
            strEntrada[94] = "permissão=";
            strEntrada[95] = "reserva=";
            strEntrada[96] = "reserva=";
            strEntrada[97] = "reserva=";
            strEntrada[98] = "";

            strEntrada[99]  = "[Bloqueio no processamento]";
            strEntrada[100] = "";
            strEntrada[101] = "bloqueio =";
            strEntrada[102] = "";

            //*************************************************************************

            if (salvaArquivo(strArquivo, strCaminho, ref strEntrada, 0, ref _strErro) == false)
            {
                _status = false;
            }


            return _status;


        }
        #endregion

        #region CONVERTE

        //Salva Configuração Padrão.
        //---------------------------
        static public bool salvaconfiguracaoPadraoConverte(string strArquivo, string strCaminho)
        {

            bool _status = true;

            string _strErro = "";

            string[] _strEntrada = new string[100];

            //*************************************************************************

            _strEntrada[0]  = "[Conexão]";
            _strEntrada[1]  = "";
            _strEntrada[2]  = "Servidor=192.168.100.6";
            _strEntrada[3]  = "Usuario =webuser";
            _strEntrada[4]  = "Senha   =web-user";
            _strEntrada[5]  = "Banco   =Northwind";
            _strEntrada[6]  = "Timeout =60";
            _strEntrada[7]  = "Encrypt =no";
            _strEntrada[8]  = "Porta   =0";
            _strEntrada[9]  = "reserva=";
            _strEntrada[10] = "reserva=";
            _strEntrada[11] = "reserva=";
            _strEntrada[12] = "";

            _strEntrada[13] = "[Processamento]";
            _strEntrada[14] = "";
            _strEntrada[15] = "Quantidade=20";
            _strEntrada[16] = "Tempo     =10";
            _strEntrada[17] = "Limpa Tela=5000";
            _strEntrada[18] = "Excluir   =false";
            _strEntrada[19] = "Teleseed  =false";
            _strEntrada[20] = "Globalstar=false";
            _strEntrada[21] = "Satelital =false";
            _strEntrada[22] = "reserva=";
            _strEntrada[23] = "reserva=";
            _strEntrada[24] = "reserva=";
            _strEntrada[25] = "";

            _strEntrada[26] = "[Procedure]";
            _strEntrada[27] = "";
            _strEntrada[28] = "T_Proc_Panico      =sp_I_Panico1";
            _strEntrada[29] = "T_Proc_Descricao   =sp_S_Rastreamento_RetDesc";
            _strEntrada[30] = "T_Proc_Rastreamento=sp_I_PosSat_Rastreamento";
            _strEntrada[31] = "T_Proc_Online      =sp_I_PosSat_Online";
            _strEntrada[32] = "T_Proc_Cerca       =p_cerca";
            _strEntrada[33] = "T_Proc_Velocidade  =p_vel_limite";
            _strEntrada[34] = "reserva=";
            _strEntrada[35] = "reserva=";
            _strEntrada[36] = "reserva=";
            _strEntrada[37] = "";

            _strEntrada[38] = "[Tabela bruto]";
            _strEntrada[39] = "";
            _strEntrada[40] = "banco Tabela=ras_raw_msg";
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

            _strEntrada[51] = "[Tabela mensagem programada]";
            _strEntrada[52] = "";
            _strEntrada[53] = "banco Tabela=Rastreamento";
            _strEntrada[54] = "banco campo =Ras_Mensagem";
            _strEntrada[55] = "banco campo =teor_mensagem";
            _strEntrada[56] = "banco campo =id_mensagem";
            _strEntrada[57] = "reserva=";
            _strEntrada[58] = "reserva=";
            _strEntrada[59] = "reserva=";
            _strEntrada[60] = "";

            _strEntrada[61] = "[Permissão de processamento]";
            _strEntrada[62] = "";
            _strEntrada[63] = "permissão=TELEBOAT";
            _strEntrada[64] = "permissão=GERSON";
            _strEntrada[65] = "permissão=";
            _strEntrada[66] = "permissão=";
            _strEntrada[67] = "permissão=";
            _strEntrada[68] = "permissão=";
            _strEntrada[69] = "permissão=";
            _strEntrada[70] = "permissão=";
            _strEntrada[71] = "permissão=";
            _strEntrada[72] = "permissão=";
            _strEntrada[73] = "reserva=";
            _strEntrada[74] = "reserva=";
            _strEntrada[75] = "reserva=";
            _strEntrada[76] = "";

            _strEntrada[77] = "[Exceção de processamento]";
            _strEntrada[78] = "";
            _strEntrada[79] = "exceção=";
            _strEntrada[80] = "exceção=";
            _strEntrada[81] = "exceção=";
            _strEntrada[82] = "exceção=";
            _strEntrada[83] = "exceção=";
            _strEntrada[84] = "exceção=";
            _strEntrada[85] = "exceção=";
            _strEntrada[86] = "";

            _strEntrada[87] = "[Bloqueio no processamento]";
            _strEntrada[88] = "";
            _strEntrada[89] = "bloqueio =";
            _strEntrada[90] = "";

            //*************************************************************************

            if (salvaArquivo(strArquivo, strCaminho, ref _strEntrada, 0, ref _strErro) == false)
            {
                _status = false;
            }


            return _status;
        }

        #endregion

    }
}
