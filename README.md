# API de Cadastro de Clientes

Uma **API RESTful** completa para **gerenciamento de clientes e seus endereços**, construída com **ASP.NET Core 8** e **Entity Framework Core 8**. O projeto foi desenhado para ser robusto e eficiente, incluindo **validação de CPF em tempo real** contra um serviço do Governo Federal e **documentação interativa** com Swagger (OpenAPI).

---

## ✨ Principais Funcionalidades

* **Gerenciamento de Clientes (CRUD):** crie, consulte, atualize e delete clientes.
* **Gerenciamento de Endereços (CRUD):** associe e gerencie múltiplos endereços por cliente.
* **Validação de CPF em Tempo Real:** verificação automática no cadastro usando o serviço público `scpa-backend.saude.gov.br`.
* **Relacionamento de Dados:** um cliente pode ter vários endereços; ao excluir um cliente, seus endereços são removidos **em cascata**.
* **Documentação Interativa:** explore e teste todos os endpoints via **Swagger UI**.
* **CORS para Dev:** política configurada para facilitar integrações em ambiente de desenvolvimento.

---

## 🚀 Tecnologias

* **Backend:** C#, ASP.NET Core 8
* **ORM:** Entity Framework Core 8
* **Banco de Dados:** Microsoft SQL Server
* **Documentação:** Swashbuckle (Swagger)
* **Padrões:** Injeção de Dependência nativa do ASP.NET Core

---

## 🏗️ Estrutura do Projeto

```
ApiClientes/
├─ Controllers/
│  ├─ ClienteController.cs
│  └─ EnderecoController.cs
├─ Models/
│  ├─ Cliente.cs
│  └─ Endereco.cs
├─ Data/
│  └─ AppDbContext.cs
├─ Services/
│  └─ ClienteService.cs        # validação de CPF e regras de negócio
├─ Migrations/
├─ appsettings.json
├─ Program.cs
└─ ApiClientes.csproj
```

---

## ⚙️ Configuração e Execução

### Pré-requisitos

* **.NET 8 SDK**
* **SQL Server** (Express, Developer, etc.)
* Editor de código (Visual Studio 2022, VS Code, Rider)

### Passo a Passo

1. **Clone o repositório**

   ```bash
   git clone <URL_DO_SEU_REPOSITORIO>
   cd ApiClientes
   ```

2. **Configure a string de conexão**
   Edite `appsettings.json` e ajuste `DefaultConnection` para sua instância local do SQL Server.

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=SEU_SERVIDOR;Database=DbClientes;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Instale as ferramentas do EF Core (se necessário)**

   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Aplique as migrations**
   Cria o banco **DbClientes** e o schema necessário.

   ```bash
   dotnet ef database update
   ```

5. **Execute a API**

   ```bash
   dotnet run
   ```

6. **Acesse a documentação (Swagger UI)**
   Com a aplicação rodando:

   * URL: `http://localhost:5222/swagger`

> **O que é o Swagger UI?** É uma interface web que lê o documento OpenAPI da sua API e permite **visualizar a especificação**, **testar endpoints** e **enviar requisições** diretamente do navegador.

---

## 🔐 Validação de CPF em Tempo Real

* No momento do **POST** de um cliente, o CPF é validado contra o serviço público `scpa-backend.saude.gov.br`.
* Em caso de CPF inválido ou inconsistência, a API retorna um **erro de validação** impedindo o cadastro.

> Obs.: certifique-se de que o host possui acesso de rede a esse serviço público no ambiente onde a API está executando.

---

## 📚 Endpoints da API

### Clientes (`/api/Cliente`)

|  Verbo | Rota    | Descrição                                                        |
| -----: | ------- | ---------------------------------------------------------------- |
|    GET | `/`     | Retorna uma **lista simplificada** de todos os clientes.         |
|    GET | `/{id}` | Retorna os **detalhes** de um cliente, incluindo seus endereços. |
|   POST | `/`     | Cria um **novo cliente** com **validação de CPF**.               |
|    PUT | `/{id}` | Atualiza os dados de um **cliente existente**.                   |
| DELETE | `/{id}` | Exclui um **cliente** e seus **endereços em cascata**.           |

#### Exemplos de payload (Cliente)

**POST /api/Cliente**

```json
{
  "nome": "Maria Silva",
  "cpf": "12345678909",
  "dataNascimento": "1990-05-20",
  "email": "maria.silva@exemplo.com"
}
```

**PUT /api/Cliente/{id}**

```json
{
  "nome": "Maria S. Oliveira",
  "email": "maria.oliveira@exemplo.com"
}
```

---

### Endereços (`/api/Endereco`)

|  Verbo | Rota    | Descrição                                           |
| -----: | ------- | --------------------------------------------------- |
|    GET | `/`     | Retorna **todos os endereços** cadastrados.         |
|    GET | `/{id}` | Retorna os **detalhes** de um endereço específico.  |
|   POST | `/`     | Adiciona um **novo endereço** (requer `ClienteId`). |
|    PUT | `/{id}` | Atualiza os dados de um **endereço existente**.     |
| DELETE | `/{id}` | Exclui um **endereço**.                             |

#### Exemplos de payload (Endereco)

**POST /api/Endereco**

```json
{
  "clienteId": 1,
  "logradouro": "Av. Paulista",
  "numero": "1000",
  "complemento": "Conjunto 101",
  "bairro": "Bela Vista",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01310-100"
}
```

**PUT /api/Endereco/{id}**

```json
{
  "logradouro": "Av. Paulista",
  "numero": "1100",
  "complemento": "Conjunto 201"
}
```

---

## 🧪 Testes rápidos via cURL

> Substitua os valores conforme seu ambiente.

**Listar clientes**

```bash
curl -X GET http://localhost:5222/api/Cliente
```

**Criar cliente (com validação de CPF)**

```bash
curl -X POST http://localhost:5222/api/Cliente \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Silva",
    "cpf": "12345678909",
    "dataNascimento": "1990-05-20",
    "email": "maria.silva@exemplo.com"
  }'
```

---

## 🔄 Comportamento de Exclusão em Cascata

* Ao **excluir um cliente**, todos os **endereços vinculados** são removidos automaticamente.
* A configuração é aplicada no **mapeamento do EF Core** (fluente ou data annotations) dentro do `AppDbContext`.

---

## 📝 Changelog

* **\[1.0.0] – 2025-08-27**

  * Estrutura inicial do projeto com ASP.NET Core 8.
  * Endpoints CRUD completos para **Clientes** e **Endereços**.
  * Integração com **Entity Framework Core 8** e **SQL Server**.
  * **Validação de CPF** em tempo real utilizando serviço externo do Governo.
  * Configuração de **exclusão em cascata** para endereços ao deletar um cliente.
  * Geração automática de documentação da API com **Swagger/OpenAPI**.
  * Política de **CORS** para ambiente de desenvolvimento.


