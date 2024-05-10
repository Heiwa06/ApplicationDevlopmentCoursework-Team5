using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Bislerium.Migrations
{
    /// <inheritdoc />
    public partial class comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlogCommentcommentId",
                table: "BlogVotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogComments",
                columns: table => new
                {
                    commentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    postId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComments", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_BlogComments_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogComments_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentVotes",
                columns: table => new
                {
                    BlogCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    postId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogVoteType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVotes", x => x.BlogCommentId);
                    table.ForeignKey(
                        name: "FK_CommentVotes_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentVotes_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogVotes_BlogCommentcommentId",
                table: "BlogVotes",
                column: "BlogCommentcommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_postId",
                table: "BlogComments",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_userId",
                table: "BlogComments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_postId",
                table: "CommentVotes",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_userId",
                table: "CommentVotes",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogVotes_BlogComments_BlogCommentcommentId",
                table: "BlogVotes",
                column: "BlogCommentcommentId",
                principalTable: "BlogComments",
                principalColumn: "commentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogVotes_BlogComments_BlogCommentcommentId",
                table: "BlogVotes");

            migrationBuilder.DropTable(
                name: "BlogComments");

            migrationBuilder.DropTable(
                name: "CommentVotes");

            migrationBuilder.DropIndex(
                name: "IX_BlogVotes_BlogCommentcommentId",
                table: "BlogVotes");

            migrationBuilder.DropColumn(
                name: "BlogCommentcommentId",
                table: "BlogVotes");
        }
    }
}
