# Therabee — Requisitos Funcionais MVP

## 1. Visão geral

A Therabee é uma plataforma SaaS para terapeutas acompanharem pacientes, planearem intervenções terapêuticas, registarem sessões, controlarem progresso clínico, gerirem agenda e gerarem relatórios terapêuticos com base na evolução do paciente.

O objetivo principal é reduzir trabalho administrativo, melhorar a organização clínica e ajudar o terapeuta a tomar decisões mais estruturadas com base nos dados registados.

---

## 2. Tipos de utilizador

### 2.1. Terapeuta

Utilizador principal da plataforma.

Pode:

- criar e gerir pacientes;
- registar avaliações;
- criar planos terapêuticos;
- agendar sessões;
- registar sessões realizadas;
- acompanhar progresso;
- gerar relatórios;
- consultar histórico clínico;
- receber alertas e recomendações.

### 2.2. Administrador da clínica / organização

Utilizador responsável pela gestão de uma equipa ou entidade clínica.

Pode:

- gerir terapeutas;
- consultar pacientes associados à organização;
- definir permissões;
- consultar métricas gerais;
- gerir subscrições e configurações da organização.

Este perfil pode ficar fora do MVP inicial, mas deve ser previsto no desenho do sistema.

### 2.3. Paciente / cuidador

Utilizador opcional para fases futuras.

Pode:

- consultar marcações;
- receber lembretes;
- preencher questionários;
- consultar recomendações simples;
- receber documentos ou planos partilhados pelo terapeuta.

No MVP, o paciente não precisa necessariamente de ter login.

---

## 3. Requisitos de autenticação e segurança

### RF-001 — Registo de terapeuta

O sistema deve permitir que um terapeuta crie conta com nome, email, password e profissão.

### RF-002 — Login

O sistema deve permitir autenticação por email e password.

### RF-003 — Logout

O sistema deve permitir terminar sessão.

### RF-004 — Perfil do terapeuta

O terapeuta deve poder editar os seus dados profissionais, incluindo:

- nome;
- email;
- profissão;
- número profissional, se aplicável;
- especialidades;
- contacto;
- local de trabalho;
- assinatura profissional para relatórios.

### RF-005 — Isolamento de dados

Cada terapeuta só deve conseguir aceder aos seus próprios pacientes, sessões, planos e relatórios, exceto quando houver associação formal ou consentimento de partilha.

### RF-006 — Dados clínicos privados

Dados clínicos, notas de sessão, relatórios, avaliações e planos terapêuticos nunca devem ser públicos.

---

## 4. Gestão de pacientes

### RF-007 — Criar paciente

O terapeuta deve poder criar um paciente com os seguintes dados:

- nome completo;
- data de nascimento;
- idade calculada automaticamente;
- género;
- contacto;
- email;
- morada;
- nome do cuidador/responsável, se aplicável;
- contacto do cuidador;
- diagnóstico principal;
- motivo de acompanhamento;
- observações gerais;
- estado do paciente: ativo, inativo, alta, suspenso.

### RF-008 — Validação de duplicados

Ao criar um paciente, o sistema deve verificar possíveis duplicados através de dados como:

- nome;
- data de nascimento;
- contacto;
- email.

Se existir um possível duplicado, o sistema deve alertar o terapeuta e permitir:

- continuar com novo paciente;
- associar paciente existente ao terapeuta;
- cancelar criação.

### RF-009 — Listar pacientes

O terapeuta deve poder consultar uma lista dos seus pacientes.

A lista deve permitir filtrar por:

- nome;
- estado;
- diagnóstico;
- idade;
- data da última sessão;
- pacientes sem sessão agendada.

### RF-010 — Consultar ficha do paciente

O terapeuta deve poder abrir a ficha completa do paciente, contendo:

- dados pessoais;
- informação clínica;
- histórico de sessões;
- plano terapêutico ativo;
- avaliações;
- evolução;
- relatórios gerados;
- documentos associados.

### RF-011 — Editar paciente

O terapeuta deve poder editar os dados do paciente.

### RF-012 — Arquivar/inativar paciente

O terapeuta deve poder marcar um paciente como inativo, em alta ou suspenso, sem apagar o histórico clínico.

---

## 5. Avaliação inicial

### RF-013 — Registar avaliação inicial

O terapeuta deve poder criar uma avaliação inicial para um paciente.

A avaliação pode incluir:

- motivo de referenciação;
- contexto familiar/social;
- diagnóstico conhecido;
- dificuldades observadas;
- áreas fortes;
- objetivos gerais;
- observações clínicas;
- instrumentos de avaliação aplicados;
- conclusão inicial.

### RF-014 — Guardar instrumentos de avaliação

O terapeuta deve poder associar instrumentos/testes aplicados ao paciente.

Exemplos:

- Mini Mental;
- avaliação psicomotora;
- escalas funcionais;
- grelhas de observação;
- questionários próprios.

### RF-015 — Histórico de avaliações

O sistema deve guardar todas as avaliações feitas ao paciente, com data e autor.

---

## 6. Plano terapêutico

### RF-016 — Criar plano terapêutico

O terapeuta deve poder criar um plano terapêutico para um paciente.

O plano deve conter:

- data de início;
- duração prevista;
- frequência recomendada;
- número de sessões por semana;
- objetivos gerais;
- objetivos específicos;
- áreas de intervenção;
- estratégias recomendadas;
- observações;
- estado: ativo, concluído, suspenso.

### RF-017 — Recomendar plano terapêutico

O sistema deve sugerir um plano base com base nos dados do paciente, avaliação inicial e objetivos escolhidos pelo terapeuta.

A recomendação pode incluir:

- número de sessões por semana;
- duração estimada do acompanhamento;
- principais áreas de foco;
- sugestões de atividades;
- indicadores de progresso.

A recomendação deve ser sempre editável pelo terapeuta.

### RF-018 — Objetivos terapêuticos

O terapeuta deve poder adicionar objetivos terapêuticos ao plano.

Cada objetivo deve conter:

- descrição;
- área terapêutica;
- prioridade;
- estado: não iniciado, em progresso, atingido, suspenso;
- data de criação;
- data prevista de revisão.

### RF-019 — Rever plano terapêutico

O terapeuta deve poder rever e atualizar o plano terapêutico ao longo do tempo.

### RF-020 — Histórico de alterações do plano

O sistema deve guardar histórico básico de alterações relevantes ao plano.

---

## 7. Sessões terapêuticas

### RF-021 — Agendar sessão

O terapeuta deve poder agendar uma sessão para um paciente.

A sessão deve conter:

- paciente;
- terapeuta;
- data;
- hora de início;
- hora de fim;
- tipo de sessão;
- local;
- estado;
- observações.

Estados possíveis:

- agendada;
- realizada;
- cancelada;
- falta do paciente;
- falta do terapeuta;
- reagendada.

### RF-022 — Tipos de sessão

O sistema deve permitir classificar sessões como:

- avaliação;
- intervenção;
- reavaliação;
- reunião com cuidador;
- reunião multidisciplinar;
- supervisão;
- outro.

### RF-023 — Registar sessão realizada

Após a sessão, o terapeuta deve poder registar:

- resumo da sessão;
- atividades realizadas;
- resposta do paciente;
- objetivos trabalhados;
- evolução observada;
- dificuldades;
- recomendações;
- próximos passos;
- presença/falta;
- duração real da sessão.

### RF-024 — Associar sessão a objetivos

O terapeuta deve poder indicar que objetivos terapêuticos foram trabalhados em cada sessão.

### RF-025 — Classificar progresso da sessão

O terapeuta deve poder atribuir uma classificação simples de progresso, por exemplo:

- regrediu;
- sem alteração;
- melhoria ligeira;
- melhoria moderada;
- melhoria significativa.

### RF-026 — Histórico de sessões

O sistema deve apresentar todas as sessões do paciente por ordem cronológica.

### RF-027 — Sessões em atraso

O sistema deve identificar sessões agendadas que ainda não foram registadas como realizadas, canceladas ou falta.

---

## 8. Agenda

### RF-028 — Calendário do terapeuta

O terapeuta deve ter acesso a uma vista de calendário com as suas sessões.

Vistas desejadas:

- diária;
- semanal;
- mensal.

### RF-029 — Reagendar sessão

O terapeuta deve poder alterar a data/hora de uma sessão já marcada.

### RF-030 — Cancelar sessão

O terapeuta deve poder cancelar uma sessão e indicar motivo.

### RF-031 — Alertas de falta de agendamento

O sistema deve alertar quando um paciente ativo não tem sessão futura agendada, principalmente se existir um plano terapêutico ativo com frequência definida.

Exemplo:

- paciente tem plano de 2 sessões por semana;
- só tem 1 sessão agendada;
- sistema alerta terapeuta.

### RF-032 — Integração futura com calendário externo

O sistema deve prever futura integração com Google Calendar, Outlook ou calendário nativo, mas esta funcionalidade pode ficar fora do MVP.

---

## 9. Notas clínicas

### RF-033 — Criar nota clínica

O terapeuta deve poder criar notas clínicas associadas ao paciente.

Cada nota deve conter:

- paciente;
- terapeuta;
- data;
- título;
- conteúdo;
- tipo de nota;
- visibilidade;
- anexos, se aplicável.

### RF-034 — Tipos de nota

Tipos possíveis:

- observação clínica;
- contacto com cuidador;
- contacto com escola/instituição;
- reunião técnica;
- evolução relevante;
- alerta;
- outro.

### RF-035 — Notas privadas

O terapeuta deve poder marcar uma nota como privada.

### RF-036 — Histórico de notas

O sistema deve listar todas as notas clínicas do paciente.

---

## 10. Progresso do paciente

### RF-037 — Dashboard de progresso

A ficha do paciente deve apresentar uma visão resumida da evolução.

Pode incluir:

- número de sessões realizadas;
- assiduidade;
- objetivos em progresso;
- objetivos atingidos;
- última sessão;
- próxima sessão;
- evolução média registada;
- alertas clínicos.

### RF-038 — Evolução por objetivo

O sistema deve mostrar o progresso de cada objetivo terapêutico ao longo das sessões.

### RF-039 — Indicadores de assiduidade

O sistema deve calcular:

- sessões agendadas;
- sessões realizadas;
- faltas;
- cancelamentos;
- taxa de assiduidade.

### RF-040 — Alertas de baixa evolução

O sistema deve sinalizar pacientes com várias sessões consecutivas sem progresso relevante.

---

## 11. Relatórios terapêuticos

### RF-041 — Gerar relatório terapêutico

O terapeuta deve poder gerar um relatório com base nos dados do paciente.

O relatório deve poder incluir:

- identificação do paciente;
- motivo do acompanhamento;
- avaliação inicial;
- objetivos terapêuticos;
- plano terapêutico;
- resumo da intervenção;
- evolução observada;
- sessões realizadas;
- assiduidade;
- conclusões;
- recomendações;
- assinatura profissional.

### RF-042 — Relatório baseado em intervalo temporal

O terapeuta deve poder escolher o período do relatório.

Exemplos:

- último mês;
- últimos 3 meses;
- desde o início do acompanhamento;
- intervalo personalizado.

### RF-043 — Editar relatório antes de exportar

O relatório gerado deve ser editável pelo terapeuta antes de ser exportado.

### RF-044 — Exportar relatório em PDF

O terapeuta deve poder exportar o relatório em PDF.

### RF-045 — Guardar relatório no histórico

O sistema deve guardar relatórios gerados na ficha do paciente.

---

## 12. Recomendações clínicas assistidas

### RF-046 — Sugerir objetivos

Com base na avaliação inicial e nos dados do paciente, o sistema pode sugerir objetivos terapêuticos.

### RF-047 — Sugerir atividades

Com base nos objetivos definidos, o sistema pode sugerir atividades terapêuticas.

### RF-048 — Sugerir revisão do plano

O sistema pode sugerir revisão do plano quando:

- há pouca evolução;
- há muitas faltas;
- objetivos foram atingidos;
- plano está ativo há muito tempo sem atualização.

### RF-049 — Responsabilidade clínica

Todas as recomendações devem ser apresentadas como apoio à decisão, nunca como decisão automática.

O terapeuta deve validar, editar ou rejeitar qualquer recomendação.

---

## 13. Documentos e anexos

### RF-050 — Upload de documentos

O terapeuta deve poder anexar documentos à ficha do paciente.

Exemplos:

- relatórios externos;
- avaliações;
- consentimentos;
- imagens;
- documentos escolares;
- prescrições;
- PDFs.

### RF-051 — Listar documentos

O sistema deve listar os documentos associados ao paciente.

### RF-052 — Remover documentos

O terapeuta deve poder remover documentos, respeitando regras de auditoria e retenção legal.

---

## 14. Consentimentos e partilha

### RF-053 — Registar consentimento

O sistema deve permitir registar consentimentos do paciente ou responsável.

Consentimentos possíveis:

- tratamento de dados;
- partilha com outros profissionais;
- contacto por email;
- contacto por WhatsApp/SMS;
- envio de relatórios;
- utilização de dados para recomendações assistidas.

### RF-054 — Partilha entre profissionais

O sistema deve permitir, numa fase futura, que um paciente seja partilhado entre terapeutas mediante consentimento.

### RF-055 — Acesso limitado por profissional

Mesmo quando partilhado, o sistema deve permitir limitar o acesso a determinados dados.

---

## 15. Notificações

### RF-056 — Notificações internas

O sistema deve mostrar notificações ao terapeuta sobre:

- sessões próximas;
- sessões por registar;
- pacientes sem marcações futuras;
- planos a rever;
- relatórios pendentes;
- objetivos sem atualização.

### RF-057 — Notificações externas futuras

O sistema pode futuramente permitir notificações por:

- email;
- SMS;
- WhatsApp.

No MVP, pode ficar limitado a notificações internas.

---

## 16. Dashboard principal

### RF-058 — Dashboard do terapeuta

Ao entrar na plataforma, o terapeuta deve ver:

- sessões de hoje;
- próximas sessões;
- sessões por registar;
- pacientes ativos;
- pacientes sem sessão futura;
- alertas importantes;
- relatórios recentes.

---

## 17. Pesquisa e filtros

### RF-059 — Pesquisa global

O terapeuta deve poder pesquisar por:

- paciente;
- diagnóstico;
- sessão;
- relatório;
- nota clínica.

### RF-060 — Filtros

As principais listagens devem permitir filtros por:

- data;
- estado;
- terapeuta;
- paciente;
- tipo;
- prioridade.

---

## 18. Requisitos não funcionais

### RNF-001 — Segurança

O sistema deve proteger dados pessoais e clínicos com autenticação, autorização e controlo de acessos.

### RNF-002 — RGPD

O sistema deve cumprir princípios do RGPD, incluindo:

- minimização de dados;
- consentimento;
- direito de acesso;
- direito de retificação;
- direito ao apagamento quando legalmente aplicável;
- controlo de partilha;
- registo de tratamento de dados.

### RNF-003 — Auditoria

O sistema deve registar ações críticas, como:

- criação de paciente;
- edição de dados clínicos;
- geração de relatório;
- exportação de relatório;
- partilha de paciente;
- remoção de documentos.

### RNF-004 — Performance

As páginas principais devem carregar rapidamente mesmo com vários pacientes e sessões.

### RNF-005 — Escalabilidade

A arquitetura deve permitir no futuro suportar:

- múltiplos terapeutas;
- clínicas;
- equipas;
- pacientes partilhados;
- subscrições;
- integrações externas.

### RNF-006 — Usabilidade

A aplicação deve ser simples, clara e rápida de usar durante o dia clínico do terapeuta.

### RNF-007 — Responsividade

A aplicação deve funcionar bem em desktop, tablet e telemóvel.

---

## 19. MVP recomendado

A primeira versão deve incluir apenas o essencial:

### Módulo 1 — Autenticação

- registo;
- login;
- logout;
- perfil do terapeuta.

### Módulo 2 — Pacientes

- criar paciente;
- listar pacientes;
- editar paciente;
- consultar ficha;
- inativar paciente;
- validação simples de duplicados.

### Módulo 3 — Sessões

- agendar sessão;
- listar sessões;
- registar sessão realizada;
- cancelar/reagendar;
- histórico por paciente.

### Módulo 4 — Plano terapêutico

- criar plano;
- criar objetivos;
- associar objetivos a sessões;
- acompanhar estado dos objetivos.

### Módulo 5 — Relatórios

- gerar relatório simples;
- editar antes de exportar;
- exportar PDF;
- guardar histórico.

### Módulo 6 — Dashboard

- sessões de hoje;
- próximas sessões;
- sessões por registar;
- pacientes sem sessão futura.

---

## 20. Funcionalidades para fase 2

- recomendações automáticas de plano terapêutico;
- sugestões de atividades;
- alertas inteligentes;
- partilha entre profissionais;
- consentimentos avançados;
- upload de documentos;
- integração com Google Calendar;
- notificações por email/WhatsApp;
- área do paciente/cuidador;
- métricas clínicas avançadas;
- subscrições e pagamentos;
- multi-clínica/multi-organização.

---

## 21. Entidades principais do sistema

### User

Representa o utilizador autenticado.

Campos principais:

- id;
- name;
- email;
- passwordHash;
- role;
- createdAt;
- updatedAt.

### TherapistProfile

Representa o perfil profissional do terapeuta.

Campos principais:

- id;
- userId;
- professionalName;
- profession;
- specialties;
- professionalNumber;
- phoneNumber;
- workplace;
- reportSignature.

### Patient

Representa o paciente.

Campos principais:

- id;
- therapistId;
- fullName;
- birthDate;
- gender;
- phoneNumber;
- email;
- address;
- caregiverName;
- caregiverPhone;
- mainDiagnosis;
- referralReason;
- notes;
- status;
- createdAt;
- updatedAt.

### TherapyPlan

Representa o plano terapêutico.

Campos principais:

- id;
- patientId;
- therapistId;
- startDate;
- expectedEndDate;
- weeklyFrequency;
- generalGoals;
- interventionAreas;
- status;
- createdAt;
- updatedAt.

### TherapeuticGoal

Representa um objetivo terapêutico.

Campos principais:

- id;
- therapyPlanId;
- description;
- area;
- priority;
- status;
- reviewDate;
- createdAt;
- updatedAt.

### Session

Representa uma sessão terapêutica.

Campos principais:

- id;
- patientId;
- therapistId;
- therapyPlanId;
- startDateTime;
- endDateTime;
- type;
- location;
- status;
- notes;
- createdAt;
- updatedAt.

### SessionNote

Representa o registo clínico da sessão.

Campos principais:

- id;
- sessionId;
- summary;
- activities;
- patientResponse;
- difficulties;
- progressRating;
- recommendations;
- nextSteps;
- createdAt;
- updatedAt.

### Report

Representa um relatório terapêutico.

Campos principais:

- id;
- patientId;
- therapistId;
- title;
- periodStart;
- periodEnd;
- content;
- status;
- pdfUrl;
- createdAt;
- updatedAt.

### Consent

Representa consentimentos do paciente.

Campos principais:

- id;
- patientId;
- consentType;
- granted;
- grantedAt;
- expiresAt;
- documentUrl.

### Document

Representa documentos associados ao paciente.

Campos principais:

- id;
- patientId;
- therapistId;
- filename;
- fileType;
- fileUrl;
- description;
- createdAt.

---

## 22. Ordem recomendada de desenvolvimento

### Fase 1 — Base técnica

- criar solução backend;
- configurar base de dados;
- configurar Docker Compose;
- configurar autenticação;
- criar estrutura por módulos;
- criar migrations iniciais.

### Fase 2 — Pacientes

- entidade Patient;
- endpoints CRUD;
- validação de duplicados;
- frontend de listagem;
- frontend de criação/edição;
- ficha do paciente.

### Fase 3 — Sessões

- entidade Session;
- endpoints de agendamento;
- endpoints de registo de sessão;
- calendário simples;
- histórico por paciente.

### Fase 4 — Planos terapêuticos

- entidade TherapyPlan;
- entidade TherapeuticGoal;
- associação sessão-objetivo;
- visualização de progresso.

### Fase 5 — Relatórios

- geração de relatório;
- edição de conteúdo;
- exportação PDF;
- histórico.

### Fase 6 — Dashboard e alertas

- sessões de hoje;
- próximas sessões;
- sessões por registar;
- pacientes sem sessão futura;
- planos a rever.

---

## 23. Ecrãs frontend MVP

### Login

- email;
- password;
- botão entrar;
- link para criar conta.

### Registo

- nome;
- email;
- password;
- profissão.

### Dashboard

- resumo do dia;
- sessões de hoje;
- próximas sessões;
- alertas;
- pacientes sem sessão futura.

### Lista de pacientes

- tabela/lista de pacientes;
- pesquisa;
- filtros;
- botão criar paciente.

### Criar/editar paciente

- formulário de dados pessoais;
- dados clínicos;
- cuidador/responsável;
- estado.

### Ficha do paciente

- dados gerais;
- plano ativo;
- sessões;
- notas;
- relatórios;
- documentos;
- progresso.

### Agenda

- calendário semanal/mensal;
- criar sessão;
- reagendar;
- cancelar.

### Registo de sessão

- resumo;
- atividades;
- objetivos trabalhados;
- progresso;
- recomendações;
- próximos passos.

### Plano terapêutico

- objetivos;
- frequência;
- áreas de intervenção;
- estado;
- revisão.

### Relatórios

- criar relatório;
- selecionar período;
- gerar conteúdo;
- editar;
- exportar PDF.

---

## 24. Módulos backend recomendados

| Módulo | Responsabilidade |
|---|---|
| Auth | Login, registo, tokens, roles |
| Users | Conta do utilizador |
| Therapists | Perfil profissional |
| Patients | Ficha clínica do paciente |
| TherapyPlans | Planos e objetivos |
| Sessions | Agenda e registo de sessões |
| Reports | Geração e histórico de relatórios |
| Documents | Anexos |
| Notifications | Alertas internos |
| Consents | RGPD e permissões |

---

## 25. Prioridade técnica

Para começar bem, a primeira branch funcional deve focar-se em:

- autenticação;
- criação de terapeuta;
- criação de paciente;
- listagem de pacientes;
- detalhe do paciente.

A segunda branch deve adicionar:

- agendamento de sessões;
- registo de sessão;
- histórico por paciente.

A terceira branch deve adicionar:

- plano terapêutico;
- objetivos;
- associação sessão-objetivo.

A quarta branch deve adicionar:

- geração de relatório;
- exportação PDF;
- dashboard inicial.

---

## 26. Nota de arquitetura

A Therabee deve ser desenhada desde início como uma aplicação multi-terapeuta, mesmo que o MVP inicial só suporte terapeutas individuais.

Isto evita refatorações pesadas quando forem adicionadas funcionalidades como:

- clínicas;
- equipas;
- partilha de pacientes;
- permissões por organização;
- planos de subscrição;
- colaboração entre profissionais.

A ordem lógica do domínio deve ser:

```text
Paciente → Plano Terapêutico → Sessão → Progresso → Relatório
```

As funcionalidades de recomendação assistida devem ser implementadas apenas depois de existir uma boa estrutura de dados clínicos registados.
