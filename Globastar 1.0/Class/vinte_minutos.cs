using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;


class vinte_minutos
{
    public string m_vinte_minutos(string string_conexao)
    {
        string retorno = "";
        SqlConnection con = new SqlConnection(string_conexao);
        string indice = pega_indice(string_conexao);        
        try
        {
            con.Open();
            string sql = "select equipamento from vinte_minutos_equipamentos";
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                string infor = info(string_conexao, indice, dr["equipamento"].ToString());               
                retorno+=contas(infor, string_conexao);//faz as contas,insere novas posições                
            }
        }
        catch (Exception ex)
        {
            grava_logs("Erro: " + ex.Message);
        }
        finally
        {
            con.Close();
        }
        atualizar_indice(string_conexao);
        return retorno;
    }

    public string atualizar_indice (string string_conexao)
        {
         string retorno = "";
        SqlConnection con = new SqlConnection(string_conexao);
        try
        {
            con.Open();
            string sql = "declare @indice int;";
            sql += "set @indice = (select top 1 codigo_ras from RAS_Rastreamento order by Codigo_ras desc)";
            sql += "update vinte_minutos_indice set codigo_ras=@indice";
            SqlCommand cmd = new SqlCommand(sql,con);           
            cmd.ExecuteNonQuery();
            retorno = "Indices atualizados";

        }
        catch(Exception ex)
        {
            grava_logs("Erro classe:vinte_minutos método:atualiza_indice --- " + ex.Message);
            retorno = "Erro ao atualizar:" +ex.Message;
        }
        finally
        {
            con.Close();
        }
        return "";
        
        }
    public string pega_indice(string string_conexao)
    {
        string retorno = "";
        SqlConnection con = new SqlConnection(string_conexao);
        try
        {
            con.Open();
            string sql = "select top 1 codigo_ras from vinte_minutos_indice";
            SqlCommand com = new SqlCommand(sql, con);            
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                retorno = dr["codigo_ras"].ToString();

            }

        }
        catch (Exception ex)
        {
            retorno = "Erro: " + ex.Message;
            grava_logs("Erro: " + ex.Message);
        }
        finally
        {
            con.Close();
        }
        return retorno;
    }



    public string info(string string_conexao,string indice,string codigo_vei)
    {
        string retorno = "";
        
        SqlConnection con = new SqlConnection(string_conexao);
        try
        {
            con.Open();
            string sql = "select top 1 codigo_ras,codigo_vei,data_hora_ras,latitude_ras,longitude_ras,descricao_ras,status from ras_rastreamento where codigo_ras>@par and codigo_vei=@par2 order by data_hora_ras desc";           
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@par",indice);
            com.Parameters.AddWithValue("@par2",codigo_vei);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                string dt = Convert.ToDateTime(dr["data_hora_ras"]).ToString("yyyy/MM/dd HH:mm:ss");
                retorno =""+ dt+"$"+dr["latitude_ras"].ToString()+"$"+dr["longitude_ras"].ToString()+"$"+ dr["descricao_ras"].ToString()+"$"+dr["status"]+"$"+codigo_vei+"$|";
                retorno += ""+ penultima_info(string_conexao,Convert.ToDateTime(dr["data_hora_ras"]).ToString("yyyy/MM/dd HH:mm:ss"), codigo_vei);                
                //MessageBox.Show(retorno);
            }
        }
        catch (Exception ex)
        {
            retorno = "Erro classe:vinte_minutos método:info --- " + ex.Message;
            grava_logs("Erro classe:vinte_minutos método:info --- " + ex.Message);
        }
        finally
        {
            con.Close();
        }
        return retorno;
    }
    public string penultima_info(string string_conexao, string data_hora, string codigo_vei)
    {
        string retorno = "";
        SqlConnection con = new SqlConnection(string_conexao);
        try
        {
            con.Open();
            string sql = "select top 1 codigo_vei,data_hora_ras,latitude_ras,longitude_ras from ras_rastreamento where data_hora_ras<@par and codigo_vei=@par2 order by data_hora_ras desc";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@par", data_hora);
            com.Parameters.AddWithValue("@par2", codigo_vei);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                retorno += Convert.ToDateTime(dr["data_hora_ras"]).ToString("yyyy/MM/dd HH:mm:ss") + "$" + dr["latitude_ras"].ToString() + "$" + dr["longitude_ras"].ToString()+"|";                
            }

        }
        catch (Exception ex)
        {

            grava_logs("Erro classe:vinte_minutos método:penultima_info --- " + ex.Message);
        }
        finally
        {
            con.Close();
        }
        return retorno;

    }

    public string contas(string dados,string string_conexao)
    {
        string retorno = "";
        
        if (dados.Trim() != "")
        {
           
            try
            {
                string[] delimita_info = dados.Split('|');
                int x = 0;

                string ultima_dt = "";
                double ultima_lat = 0;
                double ultima_long = 0;

                string p_dt = "";
                double p_lat = 0;
                double p_long = 0;
                foreach (string var_info in delimita_info)
                {
                    if (var_info.Trim() != "")
                    {
                        if (var_info != "")
                        {

                            string[] delimita_ultima_info = var_info.Split('$');
                            if (x == 0)
                            {
                                ultima_dt = delimita_ultima_info[0];
                                ultima_lat = Convert.ToDouble(delimita_ultima_info[1]);
                                ultima_long = Convert.ToDouble(delimita_ultima_info[2]);
                            }

                            if (x == 1)
                            {
                                p_dt = delimita_ultima_info[0];
                                p_lat = Convert.ToDouble(delimita_ultima_info[1]);
                                p_long = Convert.ToDouble(delimita_ultima_info[2]);

                            }
                            x++;

                        }
                    }
                }
                //Contas a partir daqui 
                //Basicamente a conta é assim:
                //Data_hora anterior +20 +20    
                if (p_dt.Trim() != "")
                {
                    string[] info_de_insercao = dados.Split('$');
                    string codigo_vei = info_de_insercao[5];
                    string descricao= info_de_insercao[3];
                    string status = info_de_insercao[4];
                    double get_lat = (ultima_lat - p_lat) / 3;
                    double get_long = (ultima_long - p_long) / 3;

                    //Pos1
                    double soma_lat = p_lat + get_lat;
                    double soma_long = p_long + get_long;
                    string dt_mais_20 = Convert.ToDateTime(p_dt).AddMinutes(20).ToString("yyyy/MM/dd HH:mm:ss");                    
                    string pos1= inserir_nova_pos(string_conexao, codigo_vei, dt_mais_20.ToString(), soma_lat.ToString(), soma_long.ToString(), descricao, status);

                    //Pos2
                    double soma_lat_p2 = p_lat + (get_lat * 2);
                    double soma_long_p2 = p_long + (get_long * 2);
                    string dt_mais_40 = Convert.ToDateTime(ultima_dt).AddMinutes(-20).ToString("yyyy/MM/dd HH:mm:ss");
                    string pos2=inserir_nova_pos(string_conexao, codigo_vei, dt_mais_40.ToString(), soma_lat_p2.ToString(), soma_long_p2.ToString(), descricao, status);
                    retorno = pos1 + "" + pos2;

                }

            }
            catch (Exception ex)
            { 
                retorno= "Erro classe vinte_minutos metodo:contas ---" + ex.Message;
                grava_logs("Erro classe vinte_minutos metodo:contas ---" + ex.Message);
            }
        }
        return retorno;
    }

    public string inserir_nova_pos(string string_conexao,string codigo_vei,string data_hora_ras,string latitude_ras,string longitude_ras,string descricao_ras,string status)
    {
        string retorno = "";
        //MessageBox.Show("str" + string_conexao + "\ncodigo_vei" + codigo_vei + "\ndt_hora" + data_hora_ras + "\nlat" + latitude_ras + "\nLong" + longitude_ras + "\nDesc" + descricao_ras + "\nstatus"+status);
        
      
        SqlConnection con = new SqlConnection(string_conexao);
        try
        {            
           con.Open();
            string sql = "insert into ras_rastreamento(codigo_vei,data_hora_ras,latitude_ras,longitude_ras,descricao_ras,velocidade_ras,status) values (@par1,@par2,@par3,@par4,@par5,@par6,@par7)";            
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@par1", codigo_vei);
            com.Parameters.AddWithValue("@par2", data_hora_ras);
            com.Parameters.AddWithValue("@par3", float.Parse(latitude_ras));
            com.Parameters.AddWithValue("@par4", float.Parse(longitude_ras));
            com.Parameters.AddWithValue("@par5", descricao_ras);
            com.Parameters.AddWithValue("@par6",0);
            com.Parameters.AddWithValue("@par7", status);
            com.ExecuteNonQuery();
            retorno = "Posição vetorizado - equipamento:"+codigo_vei+" Data hora:"+data_hora_ras+" inserido com sucesso!   ---";
        }
        catch (Exception ex)
        {
            retorno = "Erro classe:vinte_minutos método:inserir_nova_pos --- " + ex.Message;
            grava_logs("Erro classe:vinte_minutos método:inserir_nova_pos --- " + ex.Message);
        }
        finally
        {
            con.Close();
        }
        return retorno;
    }

     public string str_con()
    {
        string retorno="";
        try
        {
            StreamReader ler = new StreamReader(@"configGlobal.ini");
            string linha = ler.ReadLine();
            int x = 0;            
            while (linha != null)
            {
                //MessageBox.Show(linha);
                linha = ler.ReadLine();
                if(x==71)
                {
                    string[] delimitaIP = linha.Split('=');
                    string ip ="Server="+ delimitaIP[1]+";";
                    retorno +=ip;
                }
                if(x==72)
                {
                    string[] delimitaUSU = linha.Split('=');
                    string usuario ="User Id="+ delimitaUSU[1]+";";
                    retorno +=usuario ;
                }

                if (x == 73)
                {
                    string[] delimitaSenha = linha.Split('=');
                    string senha ="Password="+ delimitaSenha[1]+";";
                    retorno +=senha;
                }

                if (x == 74)
                {
                    string[] delimitaBD = linha.Split('=');
                    string BD ="Database="+ delimitaBD[1];
                    retorno += BD;
                      
                }         
                x++;
            }
            ler.Close();
            
        }
        catch
        {
           MessageBox.Show("Erro ao consultar classe vinte_minutos configGlobal.ini");
            grava_logs("Erro ao consultar classe vinte_minutos configGlobal.ini");
        }
        return retorno;

    }
    public string grava_logs(string info)
    {
        string retorno="";
        try
        {
            string caminho = "logs_20_minutos/20m_" + DateTime.Now.ToString().Replace("/","-").Replace(":","-");
            caminho = caminho + ".txt";
            StreamWriter sw = new StreamWriter(caminho);
            sw.Write(info);
            sw.Close();
            
        }
        catch (Exception ex)
        {
            retorno ="Erro ao gravar logs: "+ ex.Message;
        }
        return retorno;
    }
    }

