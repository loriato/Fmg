# Empresa de Vendas

Fontes do Empresa de Vendas


## Configurações e Orientações de Ambiente

### Sobre Ambiente de DEV

TODO

### Sobre Ambiente de QA

TODO TESTE

### Sobre Ambiente de PRD

TODO


---
## Configurações e Orientações de Projeto

### **Connections Strings**

É necessária a configuração de 3 CS no projeto. Todos elas apontam para o mesmo banco. São elas:
 - CS_TND_EMPRESA_VENDA
 - CS_TND_EMPRESA_VENDA_SEG
 - CS_TND_EMPRESA_VENDA_PORTAL

### **Sobre Diretórios Virtuais (erro C:/tmp/static-cache)**

O projeto agora está usando diretórios virtuais para suportar o mecanismo de publicação de conteúdo estático.

É necessário criar as pastas C:/tmp/static-cache e C:/tmp/static-cache-thumbnail, e, em cada uma dos projetos no IIS, criar dois diretórios virtuais, conforme abaixo:

- empresa-venda-portal
    - static-cache: apontando para C:/tmp/static-cache
    - static-cache-thumbnail: apontando para C:/tmp/static-cache-thumbnail
- empresa-venda-web
    - static-cache: apontando para C:/tmp/static-cache
    - static-cache-thumbnail: apontando para C:/tmp/static-cache-thumbnail

### **Orientações de Desenvolvimento**

Foram realizadas  atualizações e configurações no framework. Então, atentos a recomendações e mudanças:

- Ao implementar alguma tela/UF que tenha que ser realizada validações, passar a usar o FluentValidation. Ele vai facilitar bastante a validação de dados.

- No mapeamento de dados do NHibernate, agora todos os campos são por convenção obrigatórios. Sendo assim, não existe necessidade de marcar .Not.Nullable(). O mapeamento correto agora é dizer se ele é nullable (.Nullable()). A intenção essa mudança é dar mais qualidade ao modelo de dados que é gerado.

- Houve uma mudança no controle de transação. Agora o programador terá que marcar em qual action ele precisa que uma transação esteja ativa. Para fazer com que exista uma transação aberta, colocar a anotação ```[Transaction(TransactionAttributeType.Required)]``` sobre a Action. 