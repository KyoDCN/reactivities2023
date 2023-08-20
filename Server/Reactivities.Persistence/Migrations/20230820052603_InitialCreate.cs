using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Reactivities.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("442b783d-3f37-4a96-8a4a-f47411f57615"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("4d1240a2-f4f3-4e2d-8839-f0f637af4f1b"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("5105135f-9547-492f-a2a0-43216fc37eb1"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("685d649d-7651-4eb9-bb55-9b14e1f13422"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("7bb88705-96e1-48c2-9ac8-0089dcba9553"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("8199fa32-9f2f-496c-a496-001fc20f249f"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("8da93413-9e7e-4286-a078-126d9420a958"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("b077a5a6-4ccd-4079-881a-bf72f7b4a25f"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("d97481aa-9f64-4bc5-82ea-049ec2359937"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("dc945322-6548-46ba-b80e-5df1d164c259"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("442b783d-3f37-4a96-8a4a-f47411f57615"), "film", "London", new DateTime(2024, 4, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(453), "Activity 8 months in future", "Future Activity 8", "Cinema" },
                    { new Guid("4d1240a2-f4f3-4e2d-8839-f0f637af4f1b"), "music", "London", new DateTime(2024, 2, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(445), "Activity 6 months in future", "Future Activity 6", "Roundhouse Camden" },
                    { new Guid("5105135f-9547-492f-a2a0-43216fc37eb1"), "travel", "London", new DateTime(2024, 3, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(448), "Activity 2 months ago", "Future Activity 7", "Somewhere on the Thames" },
                    { new Guid("685d649d-7651-4eb9-bb55-9b14e1f13422"), "drinks", "London", new DateTime(2023, 6, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(392), "Activity 2 months ago", "Past Activity 1", "Pub" },
                    { new Guid("7bb88705-96e1-48c2-9ac8-0089dcba9553"), "drinks", "London", new DateTime(2023, 12, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(424), "Activity 4 months in future", "Future Activity 4", "Yet another pub" },
                    { new Guid("8199fa32-9f2f-496c-a496-001fc20f249f"), "drinks", "London", new DateTime(2024, 1, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(441), "Activity 5 months in future", "Future Activity 5", "Just another pub" },
                    { new Guid("8da93413-9e7e-4286-a078-126d9420a958"), "drinks", "London", new DateTime(2023, 11, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(419), "Activity 3 months in future", "Future Activity 3", "Another pub" },
                    { new Guid("b077a5a6-4ccd-4079-881a-bf72f7b4a25f"), "music", "London", new DateTime(2023, 10, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(416), "Activity 2 months in future", "Future Activity 2", "O2 Arena" },
                    { new Guid("d97481aa-9f64-4bc5-82ea-049ec2359937"), "culture", "Paris", new DateTime(2023, 7, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(410), "Activity 1 month ago", "Past Activity 2", "Louvre" },
                    { new Guid("dc945322-6548-46ba-b80e-5df1d164c259"), "culture", "London", new DateTime(2023, 9, 20, 4, 49, 11, 975, DateTimeKind.Utc).AddTicks(413), "Activity 1 month in future", "Future Activity 1", "Natural History Museum" }
                });
        }
    }
}
