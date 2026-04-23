using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "user_roles");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "role_permissions");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "role_permissions",
                newName: "IX_role_permissions_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permissions",
                table: "role_permissions",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_Permissions_PermissionId",
                table: "role_permissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_Roles_RoleId",
                table: "role_permissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_Roles_RoleId",
                table: "user_roles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_Users_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_Permissions_PermissionId",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_Roles_RoleId",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_Roles_RoleId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_Users_UserId",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permissions",
                table: "role_permissions");

            migrationBuilder.RenameTable(
                name: "user_roles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "role_permissions",
                newName: "RolePermissions");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
