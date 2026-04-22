using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Gestao_veiculos.Models
{
    public class Vistoria
    {
        public int Id_vistoria { get; set; }
        public DateTime Data_solicitação { get; set; }
        public DateTime? data_inicio { get; set; }
        public DateTime? data_conclusao { get; set; }
        public string status_vistoria { get; set; } = string.Empty;
        public int Id_proposta { get; set; }
        public int Id_usuario { get; set; }

    }
}