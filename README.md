# NKey Bookstore - Biblioteca virtual.
## Visão geral.
Este documento tem por objetivo descrever os recursos que compõem a API bookstore.

## Tecnologias aplicadas
- Dotnet Core 5
- Entity Framework Core 5.0.7
- FluentValidation  10.2.3
- Sql Server Express 2017

Todo o acesso é feito por HTTPS

Todos os dados são enviados e recebidos no formato JSON.

Toda requisição deve conter a anotação no cabeçalho do tipo de dados trafegados

Accept: application/json

Toda Data/Hora deve estar no formato UTC\YYYY-MM-DDTHH:MM:SSZ

Toda requisição inválida recebe o retorno 400 Bad request

Toda requisição realizada para um endereço inexistente recebe o retorno:

`404 Not found`

Todo processamento realizado com erro recebo o retorno:

`500 Internal Server Error`

Contendo no corpo da resposta, a lista de erros encontradas

## Iniciando a aplicação
Primeiramente deve ser feito a restauração dos pacotes de terceiros através do comando
`dotnet restore`

Feito isso, deverá ser restaurado o banco de dados, atualmente existem dois contextos, um para o Entity Framework(ApplicationDbContext), e outro para as entidades de negócio da aplicação(BookStoreDbContext).
Para restaurá-los utilize os seguintes comandos:
`Update-Database -Context ApplicationDbContext`
`Update-Database -Context BookStoreDbContext`

Feito isso já é possível iniciar a aplicação através do depurador do visual studio, caso queira roda-la no IIS local, deverá ser instalado um pacote adicional, para isso siga as instruções contidas nesse link:
https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/iis/hosting-bundle?view=aspnetcore-5.0

