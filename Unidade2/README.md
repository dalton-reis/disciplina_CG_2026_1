# Computação Gráfica - Unidade 2  

Conceitos básicos de Computação Gráfica: estruturas de dados para geometria, sistemas de coordenadas na biblioteca gráfica (OpenGL/OpenTK), primitivas básicas (vértices, linhas, polígonos, círculos e curvas cúbicas – splines).  

Objetivo: aplicar os conceitos básicos de sistemas de referências e modelagem geométrica em Computação Gráfica.  

## Material  

[cg-slides_u2.pdf](./cg-slides_u2.pdf "cg-slides_u2.pdf")  
[anotações do quadro](aulaAnotacoesQuadro)  

## Ambiente de Desenvolvimento

Para iniciar as atividades precisamos configurar o [Ambiente de Desenvolvimento](AmbienteDesenvolvimento.md "Ambiente de Desenvolvimento")  

### OpenTK - Testar Ambiente

Agora que já temos o [Ambiente de Desenvolvimento](AmbienteDesenvolvimento.md "Ambiente de Desenvolvimento") instalado vamos testá-lo usando alguns projetos de exemplo.  

#### OpenGL - Pipeline Gráfico

##### Visão geral

```mermaid
  flowchart LR
    A[Aplicação C#/CPU<br/>Cria vértices e estado] --> B[VBO/VAO<br/>Upload de dados para GPU]
    B --> C[Vertex Shader<br/>Transforma cada vértice]
    C --> D[Primitive Assembly<br/>Monta triângulos/linhas]
    D --> E[Rasterization<br/>Converte primitivas em fragmentos]
    E --> F[Fragment Shader<br/>Calcula cor de cada fragmento]
    F --> G[Testes por fragmento<br/>Depth/Stencil/Alpha]
    G --> H[Blending<br/>Combina com cor já existente]
    H --> I[Framebuffer]
    I --> J[Swap Buffers<br/>Imagem exibida na tela]

    K[Uniforms/Texturas] --> C
    K --> F
```

##### Visão detalhada por etapas

```mermaid
  flowchart LR
    %% ---------------------------------
    %% OpenGL Graphics Pipeline (staged)
    %% ---------------------------------

    subgraph IA[Input Assembler]
        IA1[Vertex Buffers VBO]
        IA2[Index Buffer EBO opcional]
        IA3[Vertex Array Object VAO<br/>layout dos atributos]
        IA4[Montagem de primitivas<br/>pontos linhas triangulos]
        IA1 --> IA3
        IA2 --> IA4
        IA3 --> IA4
    end

    subgraph VS[Vertex Processing / Shaders]
        VS1[Vertex Shader<br/>transformacao por vertice]
        VS2[Uniforms<br/>M V P materiais parametros]
        VS3[Saidas por vertice<br/>posicao clip-space e varyings]
        VS2 --> VS1
        VS1 --> VS3
    end

    subgraph PR[Primitive Processing]
        PR1[Clipping no volume de visao]
        PR2[Perspective Divide]
        PR3[Viewport Transform<br/>NDC para coordenadas de tela]
        PR4[Rasterization<br/>geracao de fragmentos]
        PR5[Interpolacao de varyings]
        PR1 --> PR2 --> PR3 --> PR4 --> PR5
    end

    subgraph FS[Fragment Processing / Shaders]
        FS1[Fragment Shader<br/>cor profundidade opcional]
        FS2[Texturas samplers]
        FS3[Uniforms por draw call]
        FS2 --> FS1
        FS3 --> FS1
    end

    subgraph PFO[Per-Fragment Operations]
        PFO1[Scissor Test opcional]
        PFO2[Stencil Test opcional]
        PFO3[Depth Test]
        PFO4[Blending]
        PFO5[Color Mask / Depth Mask]
        PFO1 --> PFO2 --> PFO3 --> PFO4 --> PFO5
    end

    subgraph OM[Output Merger]
        OM1[Framebuffer alvo<br/>color depth stencil]
        OM2[Back Buffer]
        OM3[Swap Buffers<br/>apresentacao]
        OM1 --> OM2 --> OM3
    end

    IA --> VS --> PR --> FS --> PFO --> OM
```

##### Visão detalhada por etapas - sem DirectX

```mermaid
  flowchart LR
    %% OpenGL Core Profile 3.3+ Pipeline

    subgraph APP[Aplicacao CPU]
        APP1[Configura estado GL]
        APP2[glBindVertexArray]
        APP3[glUseProgram]
        APP4[glBindBuffer glBindTexture]
        APP5[glDrawArrays / glDrawElements]
        APP1 --> APP2 --> APP3 --> APP4 --> APP5
    end

    subgraph VERTEX[Vertex Specification + Vertex Shader]
        V1[Vertex Arrays VAO]
        V2[Vertex Buffer Objects VBO]
        V3[Element Buffer Object EBO opcional]
        V4[glVertexAttribPointer / glEnableVertexAttribArray]
        V5[Vertex Shader]
        V6[Saidas em clip space gl_Position + varyings]
        V1 --> V4
        V2 --> V4
        V3 --> V4
        V4 --> V5 --> V6
    end

    subgraph PRIM[Primitive Assembly and Rasterization]
        P1[Primitive Assembly]
        P2[Clipping]
        P3[Perspective Divide]
        P4[Viewport Transform]
        P5[Rasterization]
        P6[Interpolacao de varyings]
        P1 --> P2 --> P3 --> P4 --> P5 --> P6
    end

    subgraph FRAG[Fragment Shader]
        F1[Fragment Shader]
        F2[Uniforms]
        F3[Samplers / Textures]
        F4[Saidas: cor e opcional gl_FragDepth]
        F2 --> F1
        F3 --> F1
        F1 --> F4
    end

    subgraph PERFRAG[Per-Fragment Operations]
        PF1[Scissor Test]
        PF2[Stencil Test]
        PF3[Depth Test]
        PF4[Blending]
        PF5[Color/Depth/Stencil Write Masks]
        PF1 --> PF2 --> PF3 --> PF4 --> PF5
    end

    subgraph FB[Framebuffer]
        FB1[Default Framebuffer ou FBO]
        FB2[Color Attachments]
        FB3[Depth/Stencil Attachments]
        FB4[Double Buffer SwapBuffers]
        FB1 --> FB2
        FB1 --> FB3
        FB2 --> FB4
    end

    APP --> VERTEX --> PRIM --> FRAG --> PERFRAG --> FB
```

#### 1-CreatingAWindow

Cria uma janela usando o OpenTK.  
Ver a pasta: [1-CreatingAWindow](OpenTK/Chapter1/1-CreatingAWindow)  
Este projeto usar a definição de Shaders: [OpenTK/Common](OpenTK/Common)  

Diagrama de Classes:  
![Diagrama de Classes](OpenTK/Chapter1/1-CreatingAWindow/svg/plantuml/include.svg)  

#### 2-HelloTriangle

Exibe a representação de um triângulo usando OpenTK.  
Ver a pasta: [2-HelloTriangle](OpenTK/Chapter1/2-HelloTriangle)  
Este projeto usar a definição de Shaders: [OpenTK/Common](OpenTK/Common)  

Diagrama de Classes:  
![Diagrama de Classes](OpenTK/Chapter1/2-HelloTriangle/svg/plantuml/include.svg)  

#### Atividade de Teste

Usando o fonte do projeto: **2-HelloTriangle** faça.

##### Exercício A

- Mudar a cor  
  fundo:  
    R: 115      <!-- 115/256 = 0.44921875 -->
    G: 252      <!-- 252/256 = 0.98437500 -->
    B: 214      <!-- 214/256 = 0.83593750 -->
  triângulo:  
    R: 122      <!-- 122/256 = 0.47656250 -->  
    G: 129      <!-- 129/256 = 0.50390625 -->
    B: 255      <!-- 255/256 = 0.99609375 -->

Antes - Depois  
![2-HelloTriangle_antes](OpenTK/Chapter1/2-HelloTriangle/2-HelloTriangle_antes.png) ![2-HelloTriangle_depois](OpenTK/Chapter1/2-HelloTriangle/2-HelloTriangle_depois.png)  

#### Exercício B

- Desenhar um quadrado em vez de um triângulo usando os pontos abaixo:  
<https://www.geogebra.org/geometry/ef2ghh35>  
![geogebraQuadrado](OpenTK/Chapter1/2-HelloTriangle/geogebraQuadrado.png)  

## [Atividades - Aula](Atividade2/README.md "Atividades - Aula")  

----------

## ⏭ [Unidade 3)](../Unidade3/README.md "Unidade 3")  

## Principais Referências Bibliográficas​

O material utilizado nesta disciplina é baseado nessas Referências Bibliográficas​.  

### Links OpenGL

- OpenGL: <https://en.wikipedia.org/wiki/OpenGL>  
- OpenGL (aprendendo): <https://learnopengl.com/>  
- OpenGL (aprendendo, livro): <https://learnopengl.com/book/book_pdf.pdf>
- OpenGL (aprendendo, GitHub): <https://github.com/JoeyDeVries/LearnOpenGL>  
- Khronos: <https://www.khronos.org/api/opengl>  

### Links OpenTK

- OpenTK: <https://opentk.net/>  
- OpenTK GitHub: <https://github.com/opentk>  
- OpenTK FAQ: <https://opentk.net/faq.html>  
- OpenTK FAQ (usando OpenTK): <https://opentk.net/faq.html#installing-and-using-opentk>  
- OpenTK (aprendendo): <https://opentk.net/learn/index.html>  
- OpenTK (aprendendo, GitHub): <https://github.com/opentk/LearnOpenTK>  
- OpenTK (API): <https://opentk.net/api/index.html>  

### Links OpenTK fontes

- OpenTK (fontes, não usar): <https://github.com/opentk/opentk>  

### Links IDE VSCode

<https://github.com/LDTTFURB/site/tree/main/ProjetosEnsino/Topicos/VSCode>  

### Links C\#

<https://github.com/LDTTFURB/site/tree/main/ProjetosEnsino/Topicos/CSharp>  

## Rabiscos

![aulaRabiscos](aulaRabiscos.drawio.svg)
