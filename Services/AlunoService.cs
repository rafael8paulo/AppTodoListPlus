using AppTodoListPlus.Models;

namespace AppTodoListPlus.Services
{
    public class AlunoService
    {
        public bool Gravar(Models.Aluno aluno)
        {           

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);


            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            if (aluno.Id == 0)
            {
                cmd.CommandText = "insert into aluno (nome, ra, email) values (@nome, @ra, @email)";
            }
            else
            {
                cmd.CommandText = @"update aluno 
                                    set nome = @nome, 
                                        ra = @ra,
                                        email = @email
                                    where id = @aluno_id";

                cmd.Parameters.AddWithValue("@aluno_id", aluno.Id);
            }

            cmd.Parameters.AddWithValue("@nome", aluno.Nome);
            cmd.Parameters.AddWithValue("@ra", aluno.Ra);
            cmd.Parameters.AddWithValue("@email", aluno.Email);

            conexao.Open();

            //insert, delete, update
            cmd.ExecuteNonQuery();

            conexao.Close();

            return true;
        }

        public bool Excluir(int alunoId)
        {
            bool flag = false;

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);            

            try
            {

                Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();
                cmd.CommandText = "delete from aluno where id = @aluno_id";
                cmd.Parameters.AddWithValue("@aluno_id", alunoId);

                conexao.Open();

                //insert, delete, update, stored procedure
                cmd.ExecuteNonQuery();

                flag = true;

            }
            catch (Exception ex)
            {
                //_logger.Error(ex)
            }
            finally
            {

                conexao.Close();

            }

            return flag;
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
                                where aluno_id = @aluno_id";

            cmd.Parameters.AddWithValue("@aluno_id", id);

            conexao.Open();
            var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                aluno = new Aluno();
                aluno.Id = Convert.ToInt32(dr["aluno_id"]);
                aluno.Ra = dr["ra"].ToString();
                aluno.Email = dr["email"].ToString();
                aluno.Nome = dr["nome"].ToString();
            }

            conexao.Close();
            return aluno;

        }


        public List<Models.Aluno> ObterTodos()
        {
            var itens = new List<Models.Aluno>();

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);

            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @"select * 
                                from aluno";

            conexao.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var aluno = new Aluno();
                aluno.Id = Convert.ToInt32(dr["id"]);
                aluno.Ra = dr["ra"].ToString();
                aluno.Email = dr["email"].ToString();
                aluno.Nome = dr["nome"].ToString();

                itens.Add(aluno);
            }

            conexao.Close();

            return itens;

        }        
    }
}
