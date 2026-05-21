using System.Net;
using System.Text.Json;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Tests.Api;

public class QuestionsControllerTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateQuestion_WithoutSession_ReturnsUnauthorized()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task CreateQuestion_WithStudentSession_ReturnsUnauthorized()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async Task CreateQuestion_WithAdminSession_ReturnsCreated()
    {
        await AuthenticateAsAdminAsync();

        var questionDto = new CreateQuestionDto
        {
            ApiIndex = null,
            Language = Language.Ingles,
            Content = ParseJsonElement("""
                                       "year": 2009,
                                       "files": [],
                                       "index": 7,
                                       "title": "Questão 7 - ENEM 2009",
                                       "context": "Um novo método para produzir insulina artificial que utiliza tecnologia de DNA recombinante foi desenvolvido por pesquisadores do Departamento de Biologia Celular da Universidade de Brasília (UnB) em parceria com a iniciativa privada. Os pesquisadores modificaram geneticamente a bactéria Escherichia coli para torná-la capaz de sintetizar o hormônio. O processo permitiu fabricar insulina em maior quantidade e em apenas 30 dias, um terço do tempo necessário para obtê-la pelo método tradicional, que consiste na extração do hormônio a partir do pâncreas de animais abatidos.\n\nCiência Hoje, 24 abr. 2001. Disponível em: http://cienciahoje.uol.com.br (adaptado).",
                                       "language": null,
                                       "discipline": "ciencias-natureza",
                                       "alternatives": [
                                           {
                                               "file": null,
                                               "text": "O aperfeiçoamento do processo de extração de insulina a partir do pâncreas suíno.",
                                               "letter": "A",
                                               "isCorrect": false
                                           },
                                           {
                                               "file": null,
                                               "text": "A seleção de microrganismos resistentes a antibióticos.",
                                               "letter": "B",
                                               "isCorrect": false
                                           },
                                           {
                                               "file": null,
                                               "text": "O progresso na técnica da síntese química de hormônios.",
                                               "letter": "C",
                                               "isCorrect": false
                                           },
                                           {
                                               "file": null,
                                               "text": "Impacto favorável na saúde de indivíduos diabéticos.",
                                               "letter": "D",
                                               "isCorrect": true
                                           },
                                           {
                                               "file": null,
                                               "text": "A criação de animais transgênicos.",
                                               "letter": "E",
                                               "isCorrect": false
                                           }
                                       ],
                                       "correctAlternative": "D",
                                       "alternativesIntroduction": "A produção de insulina pela técnica do DNA recombinante tem, como consequência,"
                                       """),
            
        };

        var response = await Client.PostAsJsonAsync("/api/questions/new", questionDto);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    private static JsonElement ParseJsonElement(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            using var emptyDocument = JsonDocument.Parse("{}");
            return emptyDocument.RootElement.Clone();
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }
}