using Restful_Service.Services;
using WcfICarteiraService = ServicoWCFSoap.IWcfCarteiraService;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar o CarteiraService
builder.Services.AddScoped<ICarteiraService>(provider =>
{
    string wcfEndpoint = "http://localhost:5001/ServicoWCFSoap.svc";
    return new CarteiraService(wcfEndpoint);
});



// Adicionar serviços para Swagger (documentação da API)
var app = builder.Build();

// Configuração do middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar HTTPS, autorização e mapear os controllers
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia os controllers e faz a execução do servidor
app.MapControllers();

app.Run();
