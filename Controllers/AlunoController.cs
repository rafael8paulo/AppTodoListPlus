using Microsoft.AspNetCore.Mvc;
using AppTodoListPlus.Models;

namespace AppTodoListPlus.Controllers
{

    

    public class AlunoController : Controller
    {

        Services.AlunoService service = new Services.AlunoService();

        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            bool sucesso = false;
            string msg = "";

            string nome = "";
            int id = 0;
            string ra = "";
            string email = "";
            try
            {
                nome = dados.GetProperty("nome").ToString();
                ra = dados.GetProperty("ra").ToString();
                email = dados.GetProperty("email").ToString();         
                if(dados.GetProperty("id").ToString() != "")
                    id = Convert.ToInt32(dados.GetProperty("id").ToString());
            }
            catch
            {
                msg = "Verifique os valores dos campos.";
            }

            if (msg == "")
            {
                //var pessoa = (Pessoa)dados.GetProperty("pessoa")

                Aluno aluno = new Aluno();
                
                aluno.Id = id;
                aluno.Nome = nome;
                aluno.Ra = ra;
                aluno.Email = email;

                (sucesso, msg) = aluno.Validar();

                if (sucesso)
                {
                    service = new();
                    sucesso = service.Gravar(aluno);

                    if (!sucesso)
                    {
                        msg = "Não deu certo.";

                    }
                    else msg = "Dados salvos com sucesso.";
                }
            }


            var obj = new
            {
                sucesso = sucesso,
                msg = msg
            };

            return Json(obj);
        }

        public IActionResult Excluir(int id)
        {
            string msg = "Erro ao realizar opção.";

            bool sucesso = false;

            if(id <= 0)
            {
                msg = "Intem não selecionado!!";
            }
            else
            {
                sucesso = service.Excluir(id);
                if(sucesso)
                    msg = "Excluido com sucesso!";
            }

            return Json(new
            {
                sucesso = sucesso,
                msg = msg
            });
            
        }

        public Models.Aluno ObterPorId(int id)
        {
            Models.Aluno aluno = null;

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);

            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @"select * 
                                from aluno 
                                where id = @aluno_id";

            cmd.Parameters.AddWithValue("@aluno_id", id);

            conexao.Open();
            var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                aluno = new Models.Aluno();
                aluno.Id = Convert.ToInt32(dr["id"]);;
                aluno.Ra = dr["ra"].ToString();
                aluno.Email = dr["email"].ToString();
                aluno.Nome = dr["nome"].ToString();
            }

            conexao.Close();
            return aluno;

        }

        public List<Models.Aluno> ObterPorNome(string nome)
        {
            var alunos = new List<Models.Aluno>();

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);

            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @"select * 
                                from aluno 
                                where nome like @nome";

            cmd.Parameters.AddWithValue("@nome", nome + "%");

            conexao.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var aluno = new Models.Aluno();
                aluno.Id = Convert.ToInt32(dr["aluno_id"]);
                aluno.Ra = dr["ra"].ToString();
                aluno.Email = dr["email"].ToString();
                aluno.Nome = dr["nome"].ToString();

                alunos.Add(aluno);
            }

            conexao.Close();

            return alunos;

        }

        public IActionResult ObterTodos()
        {            
            return Json(service.ObterTodos());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
