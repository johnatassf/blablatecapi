using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Utilities
{
    public class BaseParameters
    {
        private const int SizeMaxPage = 99999999; //20
        private int _sizePage = 10;

        /// <summary>
        ///     Offset da página atual
        /// </summary>
        public int OffSet { get; set; }

        /// <summary>
        ///     Propriedades do DTO, separadas por vírgula, a serem utilizadas na montagem do DTO de retorno 
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        ///     Json formado por propriedades e valores da entidade a serem utilizados no filtro
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        ///     Json formado por propriedades e valores da entidade a serem utilizados na pesquisa
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        ///     Propriedades da Entidade mais a ordenação (asc ou desc), separadas por vírgula, a serem utilizadas na ordenação
        /// </summary>
        public string OrderBy { get; set; } = "Id";

        /// <summary>
        ///     Tamanho da página a ser considerado na paginação (máx. 20)
        /// </summary>
        public int SizePage
        {
            get => _sizePage;
            set => _sizePage = value > SizeMaxPage ? SizeMaxPage : value;
        }

        /// <summary>
        ///     Indica se as informações referentes à paginação deverão ser consideradas (true), ou não (false)
        /// </summary>
        public bool Pagination { get; set; } = true;
    }
}
