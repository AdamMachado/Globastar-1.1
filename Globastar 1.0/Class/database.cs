using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;              //Conexão sql.
using System.Data.Odbc;                   //Conexão odbc.
using System.Data.OleDb;                  //Conexão access.
using System.IO;                                 //Ler e escrever txt

namespace database  
{
    class database 
    {

        OleDbConnection conAccessGlobal;  //Conexão sql.
        SqlConnection   conSqlGlobal;     //Conexão odbc.
        OdbcConnection  conOdbcGlobal;    //Conexão access.

        //*****************************************************************************************
        //Informação.
      

        //Conexão geral.
        #region CONEXÃO_BANCO

        //(Sql Server)
        //Efetua conexão e abre banco de dados retorna null em caso de erro.
        public SqlConnection conexaoBancoSql(string dbConexao, string dbUsuario, string dbSenha, string dbBanco, ref string dbErro, int timeout, string strEncrypt)
        {
            //montagem da string de conexão
            string ConnectionString = "Data Source="     + dbConexao  + ";";
            ConnectionString       += "User ID="         + dbUsuario  + ";";
            //ConnectionString       += "Password="        + dbSenha    + ";";
            ConnectionString       += "Initial Catalog=" + dbBanco    + ";Connection Timeout=" + timeout.ToString() + ";";

            ConnectionString += "Integrated Security=True;";
            ConnectionString += "Encrypt ="        + strEncrypt + ";";

             //ConnectionString = "Data Source=ADAM\\SQLEXPRESS;Initial Catalog=Telemetria;Integrated Security=True";


            try
            {
                conSqlGlobal = new SqlConnection(ConnectionString);
                
                conSqlGlobal.Open();
            }
            catch (Exception e)
            {
                dbErro       = e.Message;
                conSqlGlobal = null;            
            }

            return conSqlGlobal;
        }
        
        //(Access)
        //Efetua conexão e abre banco de dados retorna null em caso de erro.
        public OleDbConnection conexaoBancoAccess(string dbCaminho, string dbUsuario, string dbSenha, ref string dbErro)
        {
            //montagem da string de conexão
            //em banco web ,recomendo retirar:  Mode=Share Deny None;
            //string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbCaminho + ";Mode=Share Deny None; User ID= " + dbUsuario + ";Password=" + dbSenha;
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbCaminho + ";Mode=Share Deny None; User ID= " + dbUsuario + ";Password=" + dbSenha;

            conAccessGlobal = new OleDbConnection(ConnectionString);

            try
            {
                conAccessGlobal.Open();
            }
            catch (Exception e)
            {
                dbErro = e.Message;
                conAccessGlobal = null;
            }

            return conAccessGlobal;
        }
        
        //(Odbc)
        //Efetua conexão e abre banco de dados retorna null em caso de erro.
        public OdbcConnection  conexaoBancoOdbc(string dbSource, string dbUsuario, string dbSenha, string dbBanco, ref string dbErro)
        {
            //montagem da string de conexão
            string ConnectionString = "dsn=" + dbSource + ";UID=" + dbUsuario + ";PWD=" + dbSenha + ";";

            conOdbcGlobal = new OdbcConnection(ConnectionString);

            try
            {
                conOdbcGlobal.Open();
            }
            catch (Exception e)
            {
                dbErro = e.Message;
                conOdbcGlobal = null;
            }

            return conOdbcGlobal;
        }




        #endregion


        //Executa geral.
        #region EXECUTA SQL

        //(Sql Server)
        //Retorna resultado do processamento.
        public bool conexaoExecutaSql   (string stringSql, SqlConnection con, ref string dbErro) 
        {

           bool _status = false;  //Começa verdadeiro
           int  _Proc   = 0;      //_Proc = Quantidade retornada pelo executeNonQuery.

           if (con == null)
           {
               return _status;
           }


            try
            {
                SqlCommand sqlc = new SqlCommand(stringSql,con);

                _Proc = sqlc.ExecuteNonQuery();

                //Retorna a quantidade de processamento, caso seja maior que 0, processamento foi efetuado.
                if (_Proc > 0)
                {
                    _status = true;    //Caso o processamento foi efetuado com sucesso.
                }
                else
                {
                    dbErro = "Informação não inserida na fonte de dados." + dbErro;
                    _status = false;    //caso o processamento não foi concluido.
                }                


            }
            catch (Exception e)
            {
                 dbErro = e.Message;
                _status = false;
            }
                      
            finally
            {
               //Fecha conexão.
               con.Close();
               con.Dispose();
            }

            return _status;

        }
        
        //(Access)
        //retorna OleDbDataReader
        public bool conexaoExecutaAccess(string stringSql, OleDbConnection con, ref string dbErro)
        {

            bool _status = false;  //Começa verdadeiro
            int _Proc = 0;      //_Proc = Quantidade retornada pelo executeNonQuery.

            if (con == null)
            {
                return _status;
            }



            try
            {
                OleDbCommand sqlc = new OleDbCommand(stringSql, con);

                _Proc = sqlc.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                dbErro = e.Message;
                _status = false;
            }

            //Retorna a quantidade de processamento, caso seja maior que 0, processamento foi efetuado.
            if (_Proc > 0)
            {
                _status = true;    //Caso o processamento foi efetuado com sucesso.
            }
            else
            {
                dbErro = "Informação não inserida na fonte de dados.";
                _status = false;    //caso o processamento não foi concluido.
            }

            //Fecha conexão.
            con.Close();
            con.Dispose();


            return _status;

        }
        
        //(Odbc)
        //Retorna resultado do processamento
        public bool conexaoExecutaOdbc  (string stringSql, OdbcConnection con, ref string dbErro)
        {


            bool _status = false;  //Começa verdadeiro
            int _Proc = 0;      //_Proc = Quantidade retornada pelo executeNonQuery.

            if (con == null)
            {
                return _status;
            }


            try
            {
                OdbcCommand sqlc = new OdbcCommand(stringSql, con);

                _Proc = sqlc.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                dbErro = e.Message;
                _status = false;
            }


            //Retorna a quantidade de processamento, caso seja maior que 0, processamento foi efetuado.
            if (_Proc > 0)
            {
                _status = true;    //Caso o processamento foi efetuado com sucesso.
            }
            else
            {
                dbErro = "Informação não inserida na fonte de dados.";
                _status = false;    //caso o processamento não foi concluido.
            }

            //Fecha conexão.
            con.Close();
            con.Dispose();


            return _status;

        }
        
        #endregion


        //Recordset geral.
        #region RECORDSET

        //(Sql Server)
        //Retorna resultado do processamento.
        public SqlDataReader   conexaoRecordSql(string stringSql, SqlConnection con, ref string dbErro)
        {

            SqlDataReader record;

            SqlCommand command = new SqlCommand(stringSql, con);

            try
            {
                record = command.ExecuteReader();
            }
            catch (Exception e)
            {
                record = null;
                dbErro = e.Message;
            }

            return record;

        }
       
        //(Access)
        //Retorna resultado do processamento.
        public OleDbDataReader conexaoRecordAccess(string SelectSql, OleDbConnection con, ref string dbErro)
        {
            OleDbDataReader record;

            OleDbCommand command = new OleDbCommand(SelectSql, con);

            try
            {
                record = command.ExecuteReader();
            }
            catch (Exception e)
            {
                record = null;
                dbErro = e.Message;
            }

            return record;
        }


        #endregion
       

        //Fecha Conexão geral.
        #region FECHAR_CONEXÃO

        //(Sql Server)
        //Fecha Conexão sql
        public void fechaSql()
        {
            if (conSqlGlobal != null)
            {
                if (conSqlGlobal.State == System.Data.ConnectionState.Open)
                {
                    conSqlGlobal.Close();
                    conSqlGlobal.Dispose();
                }
            }
        }        
        
        //(Access)
        //Fecha conexão acess.
        public void fechaAccess() 
       {
           if (conAccessGlobal != null)
           {
               if (conAccessGlobal.State == System.Data.ConnectionState.Open)
               {
                   conAccessGlobal.Close();
                   conAccessGlobal.Dispose();
               }
           }
       }
       
        //(Odbc)
        //Fecha Conexão sql
        public void fechaOdbc()
       {
           if (conOdbcGlobal != null)
           {
               if (conOdbcGlobal.State == System.Data.ConnectionState.Open)
               {
                   conOdbcGlobal.Close();
                   conOdbcGlobal.Dispose();
               }
           }
       }

        #endregion

        //*****************************************************************************************
        //Aplicativos.

        #region POP3

        //Montagem de Insert pop3.
        //
        public string montSqlInsertPop3(string[] strCampo, string[] strDados)  
        {

          
         
            
            
            //montagem da string principal, para inserir informação
            string _strSqlMontado = "insert into [" + strCampo[13] + "].[dbo].[" + strCampo[15] + "] (";

            //montagem da string com nome dos dados.
            string _strSqlCampo = "";

            //montagem da string contendo dados.
            string _strSqlDados = "  values (";

            //percorre campo e dados, para montagem da INSERT INTO
            for (var i = 16; i < 26; i++)
            {
                if (strCampo[i] != null && strCampo[i].Trim().Length > 0)
                {
                    _strSqlCampo += " [" + strCampo[i] + "],";
                    /*monta string inserção (values(...))*/
                    if (i == 16) { _strSqlDados += "'" + strDados[3].Trim()  + "',"; }
                    if (i == 17) { _strSqlDados += "'" + strDados[17].Trim() + "',"; }
                    if (i == 18) { _strSqlDados += "'" + strDados[18].Trim() + "',"; }
                    if (i == 19) { _strSqlDados += "'" + strDados[19].Trim() + "',"; }
                    if (i == 20) { _strSqlDados += "'" + strDados[1].Trim()  + "',"; }
                    if (i == 21) { _strSqlDados += "'" + strDados[7].Trim()  + "',"; }
                    if (i == 22) { _strSqlDados += "'" + strDados[13].Trim() + "',"; }
                    if (i == 23) { _strSqlDados += "'" + strDados[16].Trim() + "',"; }                   
                    if (i == 24) { _strSqlDados += "'" + "0" + "',"; }
                    #region   (i == 25 fl_seed |||----->>> 0 não lido | 1 lido e ok | 5 lido e não na executado segundo etapa | 9 string maior  do que deve

           Globastar_1._0.Class.BloqEtapa2 txt= new Globastar_1._0.Class.BloqEtapa2();
      
                //Caso retorno da consulta do bloco de notas for=true siginifica que contém equipamento no txt então inseri 5 para não executar a segunda etapa
                 if (txt.verificaBloco(strDados[3]) ==true)
                    {
                    if (i == 25) _strSqlDados += "'" + "5" + "',"; 
                    }
                  //Caso retorna da consulta do bloco de notas for=false
                      else
                    {
                        //Caso 0x 00 DE 87 DB DE D1 25 0A 00.legth>29 significa maior que 9 byts então não pode executar...
                        #region fl_seed 0 ou 2   | 0 ok | 2 string maior do que deve
                        string tiraEspacosHex = strDados[16].Trim();
                        if (i == 25 && tiraEspacosHex.Length <= 29) { _strSqlDados += "'" + "0" + "',"; }
                        if (i == 25 && tiraEspacosHex.Length > 29) { _strSqlDados += "'" + "9" + "',"; }
                        #endregion
                    }

                    #endregion
                }
            }

            return _strSqlMontado += _strSqlCampo.Substring(1, _strSqlCampo.Trim().Length - 1) + ")" + _strSqlDados.Substring(1, _strSqlDados.Trim().Length) + ")";
        }
        
        //montagem de Select para procurar id semelhante.
        //--------------------------------
        public string montSqlSelectPop3(string[] strCampo, string[] strDados)
        {

            string _strSqlMontado = "";

            if (strCampo[21] != null && strDados[7] != null)
            {
                //montagem da string principal, para selecionar id
                _strSqlMontado = "select  " + strCampo[21] + " from  [" + strCampo[13] + "].[dbo].[" + strCampo[15] + "]  with(nolock)   where " + strCampo[21] + " = '" + strDados[7] + "'";
            }
            
            return _strSqlMontado;  //retorna em branco quando não é possível montar select, por falta de informações.

        }
        
        //Salva dados pop3.
        //--------------------------------
        public bool salvaDadosPop3(string[] strParametro, string[] strDados, ref string dbErro) 
        {
          bool   _status         = false;
            
          string _dbErroConexao  = "";

          SqlDataReader record_1 = null;

          try
          {

              //Tipo de conexão: Sql Server
              if (Convert.ToInt32(strParametro[7]) == 1)
              {
                  //Conexão ao banco de dados.
                  conSqlGlobal = conexaoBancoSql(strParametro[9], strParametro[11], strParametro[12], strParametro[13], ref _dbErroConexao, int.Parse(strParametro[27]), strParametro[8]);

                  //Verifica-se todos os campos estão preenchidos.
                  if (strParametro[16] != null || strParametro[17] != null || strParametro[18] != null || strParametro[19] != null || strParametro[20] != null || strParametro[21] != null || strParametro[22] != null || strParametro[23] != null)
                  {

                      //Carrega dados.
                      record_1 = conexaoRecordSql(montSqlSelectPop3(strParametro, strDados), conSqlGlobal, ref dbErro);

                      if (dbErro.Trim().Length > 0)
                      {
                          //Acumula erro.
                          dbErro = dbErro + " " + _dbErroConexao;

                          return false;
                      }


                      //Função verifica se já existe id no banco de dados, evita duplicidade de dados.
                      if (record_1 != null)
                      {

                          if (record_1.HasRows)
                          {
                              //Já existe esse ID, não precisa salvar.
                              _status = true;
                          }
                          else
                          {
                              //Fecha record_1 porque esta associado a essa conexão e não permite o uso por outro objeto.
                              record_1.Close();

                              //Não existe esse ID. 
                              _status = conexaoExecutaSql(montSqlInsertPop3(strParametro, strDados), conSqlGlobal, ref dbErro);

                              //Concatena erro.
                              dbErro = dbErro + " " + _dbErroConexao;

                              //Fecha banco de dados.
                              fechaSql();

                          }
                      }
                      else
                      {

                          //Já existe esse ID, não precisa salvar.
                          _status = true;

                      }



                  }
                  else
                  {
                      _status = false;
                  }



              }

          }
          catch (Exception)
          {


          }

          finally
          {
              //Fecha banco de dados.
              fechaSql();          
          }

            return _status;
        }
        
        #endregion 

        #region CONVERTE

        //Executa procedimento online.
        //--------------------------------
        public string execProcedureSqlOnlineConverte      (ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {

            try
            {
                SqlCommand comm = new SqlCommand(strParametro[26].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@codigovei",  SqlDbType.Text).Value  = strInformacao[4].Trim();
                comm.Parameters.Add("@longitude",  SqlDbType.Text).Value  = strInformacao[8].Trim();
                comm.Parameters.Add("@latitude",   SqlDbType.Text).Value  = strInformacao[9].Trim();
                comm.Parameters.Add("@velo",       SqlDbType.Text).Value  = "0";
                comm.Parameters.Add("@datahora",   SqlDbType.Text).Value  = Convert.ToDateTime(strInformacao[10]).ToString("MM/dd/yyyy HH:mm:ss");
                comm.Parameters.Add("@status_vei", SqlDbType.Text).Value   = "0";

                var x = comm.ExecuteScalar();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return "";
        }

        //Executa procedimento rastreamento.
        //--------------------------------
        public string execProcedureSqlRastreamentoConverte(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao, string _strDescricao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[25].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CdVeiculoID", SqlDbType.Text).Value          = strInformacao[4];
                comm.Parameters.Add("@DtHrRastreamento", SqlDbType.DateTime).Value = strInformacao[10];
                comm.Parameters.Add("@NrLong", SqlDbType.Text).Value               = strInformacao[8];
                comm.Parameters.Add("@NrLat", SqlDbType.Text).Value                = strInformacao[9];
                comm.Parameters.Add("@Desc", SqlDbType.Text).Value                 = _strDescricao + strInformacao[15];
                comm.Parameters.Add("@Velo", SqlDbType.Int).Value                  = "0";

                _strRetorno = comm.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento descricao.
        //--------------------------------
        public string execProcedureSqlDescricaoConverte   (ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[24].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@NrLong", SqlDbType.Text).Value = strInformacao[8];
                comm.Parameters.Add("@NrLat", SqlDbType.Text).Value = strInformacao[9];

                _strRetorno = comm.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento cerca.
        //--------------------------------
        public string execProcedureSqlCercaConverte       (ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[27].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CODIGO_VEI", SqlDbType.Text).Value = strInformacao[4];
                comm.Parameters.Add("@LONGITUDE", SqlDbType.Float).Value = strInformacao[8];
                comm.Parameters.Add("@LATITUDE", SqlDbType.Float).Value  = strInformacao[9];
                comm.Parameters.Add("@DATA", SqlDbType.DateTime).Value   = Convert.ToDateTime(strInformacao[10]).ToString("yyyy/MM/dd HH:mm:ss");

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento velocidade.
        //--------------------------------
        public string execProcedureSqlVelocidadeConverte  (ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {
                SqlCommand comm = new SqlCommand(strParametro[28].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CODIGO_VEI", SqlDbType.Text).Value    = strInformacao[4]; ;
                comm.Parameters.Add("@DATA_HORA", SqlDbType.DateTime).Value = strInformacao[10]; ;

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }


        //salva dados processados.
        //--------------------------------
        public bool  salvaDadosConverte                    (ref string[] strInformacao, ref string[] strParametro, ref string dbErroConexao, ref string dbErroRecord)
        {
            bool _status = true;

            string _strRetorno = "";

            SqlConnection banco_1 = null;

            //Verifica-se informação é panico ou teclado, evita salvar no banco de dados.
            if (strInformacao[10].IndexOf("PANICO", 0) >= 0)
            {

                //Salva panico como mensagem.

            }
            else
            {


                //Não Salva posição -0.00 -0.00 , gerar um log de erro.
                if (strInformacao[8] != "-0.00" && strInformacao[9] != "-0.00")
                {

                    banco_1 = conexaoBancoSql(strParametro[52], strParametro[53], strParametro[54], strParametro[55], ref dbErroConexao, int.Parse(strParametro[56]), strParametro[57]);


                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: Conexão ao banco de dados.";
                        _status = false;
                    }


                    //execProcedureSqlDescricao     ver retorno **********************************

                    dbErroConexao = "";

                    _strRetorno = execProcedureSqlDescricaoConverte(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: sp_S_Rastreamento_RetDesc ";
                        _status = false;
                    }




                    //execProcedureSqlRastreamento  ver retorno **********************************

                    dbErroConexao = "";

                    _strRetorno = execProcedureSqlRastreamentoConverte(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao, _strRetorno);
                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: sp_osI_PSat_Rastreamento ";
                        _status = false;
                    }



                    //execProcedureSqlOnline         ver retorno **********************************  

                    dbErroConexao = "";

                    _strRetorno = execProcedureSqlOnlineConverte(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: sp_I_PosSat_Online ";
                        _status = false;
                    }


                    //execProcedureSqlP_cerca       ver retorno **********************************  

                    dbErroConexao = "";

                    _strRetorno = execProcedureSqlCercaConverte(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: p_cerca ";
                        _status = false;
                    }


                    //execProcedureSqlVelocidade    ver retorno **********************************  

                    dbErroConexao = "";

                    _strRetorno = execProcedureSqlVelocidadeConverte(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                    if (dbErroConexao.Trim().Length > 0)
                    {

                        dbErroConexao = dbErroConexao + " PROCEDURE: p_vel_limite ";
                        _status = false;
                    }


                }

                //Fecha conexão
                fechaSql();

                if (banco_1 != null)
                {
                    banco_1.Close();
                    banco_1.Dispose();
                }


            }

            return _status;
        }

        //atualiza dados processados.
        //--------------------------------
        public bool atualizaDadosBrutosConverte           (ref string[] strInformacao, ref string[] strParametro, ref string dbErroConexao, ref string dbErroRecord, int tipoProcessamento)
        {

            bool _status = true;

            string sqlUpdate;

            string _dbErroRecord = "";
            string _dbErroConexao = "";



            //update na tabela bruta.


            //tratamento a ser removido futuramente: encoding - sem parametro, sua função é salvar alerta do xml do globalstar.
            if (strInformacao[11].Length > 0)
            {
                sqlUpdate = "update [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "] set " + strParametro[38] + " = " + tipoProcessamento.ToString() + "," + strParametro[34] + " = '" + strInformacao[11] + "'  where " + strParametro[39] + " = " + strInformacao[7];
            }
            else
            {
                sqlUpdate = "update [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "]  set " + strParametro[38] + " = " + tipoProcessamento.ToString() + " where " + strParametro[39] + " = " + strInformacao[7];
            }


            if (conexaoExecutaSql(sqlUpdate, conexaoBancoSql(strParametro[1], strParametro[2], strParametro[3], strParametro[4], ref _dbErroConexao, int.Parse(strParametro[5]), strParametro[6]), ref _dbErroRecord) == false)
            {
                _status = false;
            }

            fechaSql();

            return _status;
        }

        //Executa procedimento Alerta.
        //--------------------------------
        public string execProcedureSqlAlertaConverte      (string strDescritivo,int nTipo, ref string[] strParametro, ref string dbErroConexao)
        {
            string _strRetorno = ""; 

            try
            {

                SqlCommand comm = new SqlCommand("SP_I_AVISO", conexaoBancoSql(strParametro[52], strParametro[53], strParametro[54], strParametro[55], ref dbErroConexao, int.Parse(strParametro[56]), strParametro[57]));

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@DESCR", SqlDbType.Text).Value = strDescritivo;
                comm.Parameters.Add("@TIP", SqlDbType.Int).Value    = nTipo;

                _strRetorno = comm.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento panico.
        //--------------------------------
        public bool execProcedureSqlPanicoConverte        (ref string[] strParametro, string strVeiculo, ref string[] strInformacao, int intTipo, int intStatus, string strAviso)
        {
            bool _status = true;

            string _dbErroRecord = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[23].Trim(), conexaoBancoSql(strParametro[52], strParametro[53], strParametro[54], strParametro[55], ref _dbErroRecord, int.Parse(strParametro[56]), strParametro[57]));

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CdVei", SqlDbType.Text).Value = strVeiculo.Trim();
                comm.Parameters.Add("@DtHr", SqlDbType.DateTime).Value = strInformacao[10];
                comm.Parameters.Add("@tip", SqlDbType.Int).Value = intTipo;
                comm.Parameters.Add("@st", SqlDbType.Int).Value = intStatus;
                comm.Parameters.Add("@desc", SqlDbType.Text).Value = strAviso;

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                _status = false;
                _dbErroRecord = "ERRO " + e.Message;
            }

            return _status;
        }


        
        #endregion

        

        #region TELESEED

        //Atualiza dados.
        //--------------------------------
        public bool atualizaTeleseed(ref string[] strparametro, int intValor, int IntRegistro, ref string dbErro)
        {

            bool _status          = true;

            string _dbErroConexao = "";

            string strSql;

            try
            {

                strSql = "update [" + strparametro[4] + "].[dbo].[" + strparametro[5] + "] set " + strparametro[10] + " = " + intValor + "  where " + strparametro[8] + " = " + IntRegistro + "";

                //Carrega conexão.
                conSqlGlobal = conexaoBancoSql(strparametro[1], strparametro[2], strparametro[3], strparametro[4], ref _dbErroConexao, int.Parse(strparametro[23]), strparametro[24]);

                if (_dbErroConexao.Trim().Length > 0)
                {
                    dbErro = dbErro + " " + _dbErroConexao;
                    _status = false;
                }

                if (conexaoExecutaSql(strSql, conSqlGlobal, ref dbErro) == false)
                {
                    _status = false;
                }

            }
            catch (Exception)
            {
                _status = false;
            }

            finally
            {
                //Fecha Conexão sql.
                fechaSql();
            }
            
            return _status;

        }

        #endregion

        #region VERIFICAR pilhas boas ou ruim
        public int[] verificaPilhas(string codigoHEXA)
        {
            //Adicionado por Adenis outubro 2014 - verifica as pilhas do Globalstar
            int[] retorno = new int[2]; // 
            try
            {
                int statusPilhas = 1;// 1 pilha boa 0 pilha ruim, 
                string[] splitByte = codigoHEXA.Split(' ');
                #region byte 0 e 1 pilhas

                //Variaveis byte0
                int convertByte0 = Convert.ToInt32(splitByte[1], 16);//covert to  hex to int
                int normal = convertByte0 & 3;//And logico com 3 caso 0 messagem padrão
                string byte0Binario = Convert.ToString(convertByte0, 2);// convert byte 0 para binario
                string byte0Formatado = "".PadRight(8 - byte0Binario.Length, '0') + byte0Binario;// colocar 0 a esquerda 

                //Variaveis bytes 1
                int ConvertBase16 = Convert.ToInt32(splitByte[2], 16);//converte decimal
                string convertBinario = Convert.ToString(ConvertBase16, 2);//convert binario
                string ZeroAEsquerda = "".PadRight(8 - convertBinario.Length, '0') + convertBinario;// colocar 0 a esquerda            
                int diag = ConvertBase16 / 4;

                //Byte 0
                if (normal == 0)
                {
                    string Bit2 = byte0Formatado.Substring(5, 1);//Indeces: substring c-01234567 Globalstar-76543210
                    string Bit3 = byte0Formatado.Substring(4, 1);
                    if (Bit2 == "1") statusPilhas = 0;
                }

                //Byte 1
                else
                {
                    //if (diag == 21) resposta += "Menssagem diagnístico  !\n";
                    //if (diag / 4 == 22) resposta += " Urgente troque a bateria! \n";
                    //if (diag / 4 == 23) resposta += "Alerta ariasat! \n";   
                    string Bit4 = ZeroAEsquerda.Substring(3, 1);//Indeces: substring c-01234567 Globalstar-76543210
                    string Bit5 = ZeroAEsquerda.Substring(4, 1);
                    if (Bit4 == "1") statusPilhas = 0;
                }
                #endregion
                string pegaByte = splitByte[8];
                int decValue = int.Parse(pegaByte, System.Globalization.NumberStyles.HexNumber);

                retorno[0] = statusPilhas;
                retorno[1] = decValue;
            }
            catch
            {
                retorno[0] = 3;
                retorno[1] = 3;
            }


            return retorno;
            ////Fim adenis 
        }
        #endregion

        #region GLOBALSTAR

        //Executa procedimento online.
        //--------------------------------
        public string execProcedureSqlOnlineGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {

            try
            {
                int statusPilhas = verificaPilhas(strInformacao[3])[0];
                int decValue = verificaPilhas(strInformacao[3])[1];

                
                SqlCommand comm = new SqlCommand(strParametro[26].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@codigovei", SqlDbType.Text).Value = strInformacao[4].Trim();
                comm.Parameters.Add("@longitude", SqlDbType.Text).Value = strInformacao[8].Trim();
                comm.Parameters.Add("@latitude", SqlDbType.Text).Value = strInformacao[9].Trim();
                comm.Parameters.Add("@velo", SqlDbType.Text).Value = "0";
                comm.Parameters.Add("@datahora", SqlDbType.Text).Value = Convert.ToDateTime(strInformacao[5]).ToString("MM/dd/yyyy HH:mm:ss");
                comm.Parameters.Add("@status_vei", SqlDbType.Text).Value =""+statusPilhas +""+decValue;//Adicionado por adenis outubro 2014

                var x = comm.ExecuteScalar();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return "";
        }

    

        //Executa procedimento rastreamento grava email em mailmsg.
        //--------------------------------
        public string execProcedureSqlRastreamentoGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao, string _strDescricao)
        {
            string _strRetorno = "";

            try
            {
                int statusPilhas = verificaPilhas(strInformacao[3])[0];
                int decValue = verificaPilhas(strInformacao[3])[1];

                SqlCommand comm = new SqlCommand(strParametro[25].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CdVeiculoID", SqlDbType.Text).Value          = strInformacao[4];
                comm.Parameters.Add("@DtHrRastreamento", SqlDbType.DateTime).Value = strInformacao[5];
                comm.Parameters.Add("@NrLong", SqlDbType.Text).Value               = strInformacao[8];
                comm.Parameters.Add("@NrLat", SqlDbType.Text).Value                = strInformacao[9];
                comm.Parameters.Add("@Desc", SqlDbType.Text).Value                 = _strDescricao + strInformacao[15];
                comm.Parameters.Add("@Velo", SqlDbType.Int).Value                  = "0";

                comm.Parameters.Add("@st", SqlDbType.Text).Value = "" + statusPilhas + "" + decValue;//Adicionado por adenis outubro 2014
                //1 pilhas boas- 0 pilhas fracas -3 sem status (online);

                //Grava em mailmsg
                //if (statusPilhas == 0)
                //{
                //    try
                //    {
                //        SqlCommand comm2 = new SqlCommand("sp_I_Email_Gravar_mailmsg", banco_1);
                //        comm2.CommandType = CommandType.StoredProcedure;
                //        comm2.Parameters.Add("@codigo_vei", SqlDbType.VarChar).Value = strInformacao[4].Trim();
                //        comm2.Parameters.Add("@msg", SqlDbType.VarChar).Value = "Bateria interna fraca. Suporte Ariasat  11 2877 2000";
                //        comm2.Parameters.Add("@placa", SqlDbType.VarChar).Value = "Alerta";
                //        string teste = strInformacao[4].Trim();
                //        comm2.ExecuteNonQuery();
                //    }
                //    catch { }
              //  }

                _strRetorno = comm.ExecuteScalar().ToString();
                }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento descricao.
        //--------------------------------
        public string execProcedureSqlDescricaoGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[24].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@NrLong", SqlDbType.Text).Value = strInformacao[8];
                comm.Parameters.Add("@NrLat", SqlDbType.Text).Value = strInformacao[9];

                _strRetorno = comm.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento cerca.
        //--------------------------------
        public string execProcedureSqlCercaGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[27].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CODIGO_VEI", SqlDbType.Text).Value = strInformacao[4];
                comm.Parameters.Add("@LONGITUDE", SqlDbType.Float).Value = strInformacao[8];
                comm.Parameters.Add("@LATITUDE", SqlDbType.Float).Value = strInformacao[9];
                comm.Parameters.Add("@DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(strInformacao[5]).ToString("yyyy/MM/dd HH:mm:ss");

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento velocidade.
        //--------------------------------
        public string execProcedureSqlVelocidadeGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref SqlConnection banco_1, ref  string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {
                SqlCommand comm = new SqlCommand(strParametro[28].Trim(), banco_1);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CODIGO_VEI", SqlDbType.Text).Value = strInformacao[4]; ;
                comm.Parameters.Add("@DATA_HORA", SqlDbType.DateTime).Value = strInformacao[5]; ;

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }


        //salva dados processados.
        //--------------------------------
        public bool salvaDadosGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref string dbErroConexao, ref string dbErroRecord)
        {
            bool _status = true;

            string _strRetorno = "";

            SqlConnection banco_1 = null;

            //Verifica-se informação é panico ou teclado, evita salvar no banco de dados.
            if (strInformacao[10].IndexOf("PANICO", 0) >= 0)
            {

                //Salva panico como mensagem.

            }
            else
            {
                try
                {
                    
                    //Não Salva posição -0.00 -0.00 , gerar um log de erro.
                    if (strInformacao[8] != "-0.00" && strInformacao[9] != "-0.00")
                    {


                        banco_1 = conexaoBancoSql(strParametro[60], strParametro[61], strParametro[62], strParametro[63], ref dbErroConexao, int.Parse(strParametro[64]), strParametro[65]);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: Conexão ao banco de dados.";
                            return false;
                        }


                        //execProcedureSqlDescricao     ver retorno **********************************

                        dbErroConexao = "";

                        _strRetorno = execProcedureSqlDescricaoGlobalstar(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: sp_S_Rastreamento_RetDesc ";
                            return false;
                        }




                        //execProcedureSqlRastreamento  ver retorno **********************************

                        dbErroConexao = "";

                        _strRetorno = execProcedureSqlRastreamentoGlobalstar(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao, _strRetorno);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: sp_I_PosSat_Rastreamento ";
                            return false;
                        }



                        //execProcedureSqlOnline         ver retorno **********************************  

                        dbErroConexao = "";

                        _strRetorno = execProcedureSqlOnlineGlobalstar(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: sp_I_PosSat_Online ";
                            return false;
                        }


                        //execProcedureSqlP_cerca       ver retorno **********************************  

                        dbErroConexao = "";

                        _strRetorno = execProcedureSqlCercaGlobalstar(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: p_cerca ";
                            return false;
                        }


                        //execProcedureSqlVelocidade    ver retorno **********************************  

                        dbErroConexao = "";

                        _strRetorno = execProcedureSqlVelocidadeGlobalstar(ref strInformacao, ref strParametro, ref banco_1, ref dbErroConexao);
                        if (dbErroConexao.Trim().Length > 0)
                        {

                            dbErroConexao = dbErroConexao + " PROCEDURE: p_vel_limite ";
                            return false;
                        }


                    }

          
                }
                catch (Exception e)
                {
                    _status = false;
                }


                finally
                {
                   
                    ////Fecha conexão
                  

                    if (banco_1 != null)
                    {
                      //  fechaSql();
                        banco_1.Close();
                        banco_1.Dispose();
                    }
                    
                }
            }
            
            return _status;
           
        }

        //atualiza dados processados.
        //--------------------------------
        public bool atualizaDadosBrutosGlobalstar(ref string[] strInformacao, ref string[] strParametro, ref string dbErroConexao, ref string dbErroRecord, int tipoProcessamento)
        {

            bool _status = true;

            string sqlUpdate;

            string _dbErroRecord = "";
            string _dbErroConexao = "";



            //update na tabela bruta.


            //tratamento a ser removido futuramente: encoding - sem parametro, sua função é salvar alerta do xml do globalstar.
            if (strInformacao[11].Length > 0)
            {
                sqlUpdate = "update [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "] set " + strParametro[38] + " = " + tipoProcessamento.ToString() + "," + strParametro[34] + " = '" + strInformacao[11] + "'  where " + strParametro[39] + " = " + strInformacao[7];
            }
            else
            {
                sqlUpdate = "update [" + strParametro[4] + "" + "].[dbo].[" + strParametro[33] + "" + "]  set " + strParametro[38] + " = " + tipoProcessamento.ToString() + " where " + strParametro[39] + " = " + strInformacao[7];
            }


            if (conexaoExecutaSql(sqlUpdate, conexaoBancoSql(strParametro[1], strParametro[2], strParametro[3], strParametro[4], ref _dbErroConexao, int.Parse(strParametro[5]), strParametro[6]), ref _dbErroRecord) == false)
            {
                _status = false;
            }

            fechaSql();

            return _status;
        }

        //Executa procedimento Alerta.
        //--------------------------------
        public string execProcedureSqlAlertaGlobalstar(string strDescritivo, int nTipo, ref string[] strParametro, ref string dbErroConexao)
        {
            string _strRetorno = "";

            try
            {

                SqlCommand comm = new SqlCommand("SP_I_AVISO", conexaoBancoSql(strParametro[1], strParametro[2], strParametro[3], strParametro[4], ref dbErroConexao, int.Parse(strParametro[5]), strParametro[6]));

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@DESCR", SqlDbType.Text).Value = strDescritivo;
                comm.Parameters.Add("@TIP", SqlDbType.Int).Value = nTipo;

                _strRetorno = comm.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                dbErroConexao = "ERRO " + e.Message;
            }

            return _strRetorno;
        }

        //Executa procedimento panico.
        //--------------------------------
        public bool execProcedureSqlPanicoGlobalstar(ref string[] strParametro, string strVeiculo, ref string[] strInformacao, int intTipo, int intStatus, string strAviso)
        {
            bool _status = true;

            string _dbErroRecord = "";

            try
            {

                SqlCommand comm = new SqlCommand(strParametro[23].Trim(), conexaoBancoSql(strParametro[60], strParametro[61], strParametro[62], strParametro[63], ref _dbErroRecord, int.Parse(strParametro[64]), strParametro[65]));

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@CdVei", SqlDbType.Text).Value = strVeiculo.Trim();
                comm.Parameters.Add("@DtHr", SqlDbType.DateTime).Value = strInformacao[5];
                comm.Parameters.Add("@tip", SqlDbType.Int).Value = intTipo;
                comm.Parameters.Add("@st", SqlDbType.Int).Value = intStatus;
                comm.Parameters.Add("@desc", SqlDbType.Text).Value = strAviso;

                comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                _status = false;
                _dbErroRecord = "ERRO " + e.Message;
            }

            return _status;
        }



        #endregion

        //*****************************************************************************************

    }
}
