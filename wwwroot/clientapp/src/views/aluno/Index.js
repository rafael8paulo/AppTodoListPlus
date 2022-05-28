import React from "react";
import ReactDOM from "react-dom";

export default class Index extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      item: {
        id: "",
        nome: "",
        ra: "",
        email: "",
      },
      filtro: "",
      itens: [],
      indexEdicao: -1,
    };
  }
  //Executar dps do primeiro render
  componentDidMount = () => {
    this.obterTodos();
  };

  adicionar = () => {
    HTTPClient.post("Aluno/Gravar", this.state.item)
      .then((respostaJSON) => respostaJSON.json())
      .then((respostaObj) => {

        alert(respostaObj.msg);

        if (respostaObj.sucesso) {

          if(!this.state.editando){
            this.obterTodos();            
          } else {
            this.obter(this.state.item.id);
          }

          this.setState({
            ...this.state,
            indexEdicao: -1,
            item: {
              id: "",
              nome: "",
              ra: "",
              email: "",
            },
          });
          this.obterTodos();
        }
      })
      .catch((err) => {
        alert("Erro ao realizar opção.");
        console.log(err.message);
      });
  };

  obterTodos = () => {
    HTTPClient.get("Aluno/ObterTodos")
      .then((respostaJSON) => respostaJSON.json())
      .then((respostaObj) => {
        this.setState({
          ...this.state,
          itens: respostaObj,
        });
      })
      .catch((err) => {
        alert("Erro ao realizar opção.");
        console.log(err.message);
      });
  };

  excluir = (i) => {
    
    if(!confirm(`Deseja excluir ${i.nome}`))
    {
      return;
    }

    let url = "Aluno/Excluir?id="+i.id;

    HTTPClient.delete(url)
    .then(resposta => resposta.json())
    .then(resposta => {

      if(resposta.sucesso)
      {
        let itens = this.state.itens;
        let index = itens.findIndex(item => item.id == i.id);

        if(index > -1){
          itens.splice(index, 1);

          this.setState({
            ...this.state,
            itens: itens
          });

        }

      }
      alert(resposta.msg);

    })
    .catch((err) => {
      alert("Erro ao realizar opção.");
      console.log(err.message);
    })

  };

  obter = (id) => {
    let url = "Aluno/ObterPorId?id="+id;

    HTTPClient.get(url)
    .then(r => r.json())
    .then(r => {
      let itens = this.state.itens;
      let index = itens.findIndex(item => item.id == id)

      if(index > -1){
        itens[index].id = r.id,
        itens[index].nome = r.nome;
        itens[index].ra = r.ra;
        itens[index].email = r.email;

        this.setState({
          ...this.state,
          itens: itens
        })

      }

    })
    .catch((err) => {
      alert("Erro ao realizar opção.");
      console.log(err.message);
    })

  }

  editar = (i) => {
    
    HTTPClient.get("Aluno/ObterPorId?id="+i.id)
    .then(r => r.json())
    .then(r => {
      this.setState({
        ...this.state,
        item: r,
        editando: true
      })
    })
    .catch((err) => {
      alert("Erro ao realizar opção.");
      console.log(err.message);
    })

  };

  pesquisar = (i) => {

    let filtro = this.state.filtro;

    let itens = this.state.itens;

    itens.forEach((i) => {
      if (i.nome.indexOf(filtro) > -1) {
        i.filtrado = true;
      } else i.filtrado = false;
    });

    this.setState({
      ...this.state,
      itens: itens,
    });
  };

  render = () => {
    let saida = (
      <>
        {/* <!-- Modal --> */}
        <div
          className="modal fade"
          id="exampleModal"
          tabindex="-1"
          aria-labelledby="exampleModalLabel"
          aria-hidden="true"
        >
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="exampleModalLabel">
                  Incluir/Alterar
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                ></button>
              </div>
              <div className="modal-body">
                <form className="forms-sample">
                  <div className="forms-group">
                    <label for="id">
                      Id
                    </label>
                    <input
                      readOnly
                      type="text"
                      className="form-control"
                      value={this.state.item.id}
                      onChange={(e) =>
                        this.setState({
                          ...this.state,
                          item: {
                            ...this.state.item,
                            id: e.target.value,
                          },
                        })
                      }    
                                        
                    />
                  </div>
                  <div className="forms-group">
                    <label for="nome">
                      Nome
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      value={this.state.item.nome}
                      onChange={(e) =>
                        this.setState({
                          ...this.state,
                          item: {
                            ...this.state.item,
                            nome: e.target.value,
                          },
                        })
                      }
                    />
                  </div>

                  <div className="forms-group">
                    <label for="ra">
                      R.A
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      value={this.state.item.ra}
                      onChange={(e) =>
                        this.setState({
                          ...this.state,
                          item: {
                            ...this.state.item,
                            ra: e.target.value,
                          },
                        })
                      }
                    />
                  </div>
                  <div className="forms-group">
                    <label for="ra">
                      Email
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      value={this.state.item.email}
                      onChange={(e) =>
                        this.setState({
                          ...this.state,
                          item: {
                            ...this.state.item,
                            email: e.target.value,
                          },
                        })
                      }
                    />
                  </div>
                </form>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-light"
                  data-bs-dismiss="modal"
                >
                  Fechar
                </button>
                <button
                  type="button"
                  className="btn btn-primary me-2"
                  onClick={this.adicionar}
                >
                  Salvar
                </button>
              </div>
            </div>
          </div>
        </div>
        <br />


        <div className="card">
          <div className="card-header">
            {/* <h4 className="card-title">Pesquisar</h4> */}
            <form>
              <div className="mb-3">
                <input
                  type="search"
                  className="form-control"
                  value={this.state.filtro}
                  onChange={(e) =>
                    this.setState(
                      {
                        ...this.state,
                        filtro: e.target.value,
                      },
                      () => {
                        this.pesquisar();
                      }
                    )
                  }
                  placeholder="Pesquisar..."
                />
              </div>
            </form>
          </div>
          <div className="card-body">
            <table className="table">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Nome</th>
                  <th>R.A</th>
                  <th>E-mail</th>
                  <th colSpan={2}></th>
                </tr>
              </thead>
              <tbody>
                {this.state.itens.map((i) => {
                  if (i.filtrado == undefined || i.filtrado) {
                    return (
                      <tr id={i.id}>
                        <td>{i.id}</td>
                        <td>{i.nome}</td>
                        <td>{i.ra}</td>
                        <td>{i.email}</td>
                        <td>
                          <button
                            type="button"
                            className="btn btn-danger"
                            onClick={() => this.excluir(i)}
                          >
                            Excluir
                          </button>
                        </td>
                        <td>
                          <button
                            type="button"
                            className="btn btn-warning"
                            onClick={() => this.editar(i)}
                            data-bs-toggle="modal" data-bs-target="#exampleModal"
                          >
                            Editar
                          </button>
                        </td>
                      </tr>
                    );
                  }
                })}
              </tbody>
            </table>
          </div>
        </div>


      </>
    );

    return saida;
  };
}

ReactDOM.render(<Index />, document.getElementById("root"));
