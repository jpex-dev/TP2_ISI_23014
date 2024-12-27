using Restful_Service.Services;
using WcfICarteiraService = ServicoWCFSoap.IWcfCarteiraService;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar o CarteiraService
builder.Services.AddScoped<ICarteiraService>(provider =>
{
    string wcfEndpoint = "http://localhost:5001/ServicoWCFSoap.svc";
    return new CarteiraService(wcfEndpoint);
});



// Adicionar servi�os para Swagger (documenta��o da API)
var app = builder.Build();

// Configura��o do middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar HTTPS, autoriza��o e mapear os controllers
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia os controllers e faz a execu��o do servidor
app.MapControllers();

app.Run();
