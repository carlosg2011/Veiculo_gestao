using System.ComponentModel.DataAnnotations;

namespace Gestao_veiculos.DTOs
{
    public class PaginationParams
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 500)]
        public int PageSize { get; set; } = 10;
    }
}
