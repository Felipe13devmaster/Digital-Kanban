using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalKanBan.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Status { get; set; }
        
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
    }
}