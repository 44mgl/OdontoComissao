using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiOdonto.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UltimoAcesso = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    DataHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Local = table.Column<string>(type: "TEXT", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembrosComissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cargo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    FotoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    OrdemExibicao = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembrosComissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembrosVip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroIdentificacao = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembrosVip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false),
                    ExclusivoVip = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    OrdemExibicao = table.Column<int>(type: "INTEGER", nullable: false),
                    Destaque = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsAdministrativos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdministradorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Acao = table.Column<string>(type: "TEXT", nullable: false),
                    Entidade = table.Column<string>(type: "TEXT", nullable: false),
                    RegistroId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataHora = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAdministrativos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsAdministrativos_Administradores_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Administradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodigoReserva = table.Column<string>(type: "TEXT", nullable: false),
                    NomeCliente = table.Column<string>(type: "TEXT", nullable: false),
                    Contato = table.Column<string>(type: "TEXT", nullable: false),
                    MembroVipId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataReserva = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_MembrosVip_MembroVipId",
                        column: x => x.MembroVipId,
                        principalTable: "MembrosVip",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VariacoesProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tamanho = table.Column<string>(type: "TEXT", nullable: false),
                    QuantidadeDisponivel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariacoesProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariacoesProdutos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensReservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservaId = table.Column<int>(type: "INTEGER", nullable: false),
                    VariacaoProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensReservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensReservas_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensReservas_VariacoesProdutos_VariacaoProdutoId",
                        column: x => x.VariacaoProdutoId,
                        principalTable: "VariacoesProdutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administradores_Email",
                table: "Administradores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensReservas_ReservaId",
                table: "ItensReservas",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensReservas_VariacaoProdutoId",
                table: "ItensReservas",
                column: "VariacaoProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAdministrativos_AdministradorId",
                table: "LogsAdministrativos",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_MembrosVip_NumeroIdentificacao",
                table: "MembrosVip",
                column: "NumeroIdentificacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_CodigoReserva",
                table: "Reservas",
                column: "CodigoReserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MembroVipId",
                table: "Reservas",
                column: "MembroVipId");

            migrationBuilder.CreateIndex(
                name: "IX_VariacoesProdutos_ProdutoId",
                table: "VariacoesProdutos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "ItensReservas");

            migrationBuilder.DropTable(
                name: "LogsAdministrativos");

            migrationBuilder.DropTable(
                name: "MembrosComissao");

            migrationBuilder.DropTable(
                name: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "VariacoesProdutos");

            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "MembrosVip");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
