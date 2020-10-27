using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DigitalKanBan.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}