using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeToDo.Domain.Models;
public class AuthResult
{
    public IEnumerable<string> Errors { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
}
