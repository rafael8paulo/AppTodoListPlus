namespace AppTodoListPlus.Models
{
    public class Aluno
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Ra { get; set; }

        public Aluno(int id, string name, string email, string ra)
        {
            Id = id;
            Nome = name;
            Email = email;
            Ra = ra;
        }
        public Aluno(string name, string email, string ra)
        {
            Nome = name;
            Email = email;
            Ra = ra;
        }

        public Aluno()
        {
        }

        public (bool, string) Validar()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                return (false, "O nome do produto não pode estar em branco!");
            }
            else if (string.IsNullOrEmpty(Email))
            {
                return (false, "O E-mail do produto não pode estar em branco!");
            }
            else if (string.IsNullOrEmpty(Ra))
            {
                return (false, "O Ra do produto não pode estar em branco!");
            }

            return (true, "");
        }
    }
}
