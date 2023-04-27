using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Globastar_1._0.Class
{
    class BloqEtapa2
    {
        public bool verificaBloco(string idEquipamento)
        {
            //ler bloqueioetapa2
            StreamReader arquivo = new StreamReader("bloqueioEtapa2.txt");
            string linha = arquivo.ReadLine();         
            while (linha != null)
            {
                if (linha == idEquipamento)
                {
                    arquivo.Close();
                    return true;
                }               

                linha = arquivo.ReadLine();

            }
            arquivo.Close();
            //Casa não passe no if return false ou seja não está no txt
            return false;
        }
    }
}
