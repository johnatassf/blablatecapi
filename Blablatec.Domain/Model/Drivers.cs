using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Model
{
    public class Drivers
    {
        public static List<Drivers> MotoristasCadastrados = new List<Drivers> { new Drivers() { Id = 1, Age = 20, Name = "Johnatas" } };

        public Drivers()
        {

        }

        public decimal Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        
        //Temporario até ser criado as camadas de repositórios, serviços e data base
        public List<Drivers> ObterMotoristasCadastrados()
        {
            return MotoristasCadastrados;
        }
    }
}
