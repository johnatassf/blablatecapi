using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("Avaliacao")]
    public class Evaluation: IEntity
    {
        [Key]
        public int Id { get; set; }
        public int IdAvaliador { get; set; }
        public int IdAvaliado { get; set; }
        public int IdViagem { get; set; }
        public int Nota { get; set; }

    }
}
