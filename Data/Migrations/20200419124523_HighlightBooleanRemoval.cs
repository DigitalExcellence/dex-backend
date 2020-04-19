/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class HighlightBooleanRemoval : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        99);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        189);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        238);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        300);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        584);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        618);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        678);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        681);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        696);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1276);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1865);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1916);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1945);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2008);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2039);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2171);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2277);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2505);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2531);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2564);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2750);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2760);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3262);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3273);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3348);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3533);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3900);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3909);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3965);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3969);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4060);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4117);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4446);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4577);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4749);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4875);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5066);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5160);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5270);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5284);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5687);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6051);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6428);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6539);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6591);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7001);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7200);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7567);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8206);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8548);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8681);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8743);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9066);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9264);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9384);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9480);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9651);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9711);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9774);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9912);

            migrationBuilder.DropColumn("IsHighlighted",
                                        "Highlight");

            migrationBuilder.InsertData("Collaborators",
                                        new[] {"Id", "FullName", "ProjectId", "Role"},
                                        new object[,]
                                        {
                                            {2046, "Ressie Rodriguez", 1, "Chief Metrics Associate"},
                                            {7690, "Morton Stamm", 17, "National Usability Associate"},
                                            {4430, "Kieran Leuschke", 17, "Internal Tactics Representative"},
                                            {7311, "Richie Skiles", 18, "Dynamic Tactics Technician"},
                                            {6739, "Jon Oberbrunner", 18, "National Optimization Designer"},
                                            {6166, "Colten Waelchi", 19, "Chief Research Liaison"},
                                            {7211, "Talon Schamberger", 19, "Internal Paradigm Designer"},
                                            {2837, "Derrick Waelchi", 20, "Human Quality Manager"},
                                            {9476, "Jamarcus Abshire", 20, "Chief Creative Technician"},
                                            {1360, "Lillian Toy", 21, "Future Marketing Specialist"},
                                            {5177, "Hector Farrell", 21, "Product Assurance Consultant"},
                                            {1495, "Jude Klocko", 22, "District Marketing Administrator"},
                                            {1532, "Stevie Langosh", 22, "Product Factors Associate"},
                                            {7258, "Palma Shields", 23, "District Markets Supervisor"},
                                            {5401, "Mark Hills", 23, "Future Accounts Analyst"},
                                            {2351, "Jayde Runte", 24, "Central Paradigm Agent"},
                                            {2923, "William Lowe", 24, "Corporate Mobility Officer"},
                                            {7543, "Kaleb Harber", 25, "Customer Accounts Representative"},
                                            {1389, "Reinhold Parker", 25, "Forward Operations Director"},
                                            {4672, "Armani Aufderhar", 26, "Dynamic Accounts Supervisor"},
                                            {3963, "Seth Robel", 26, "Principal Directives Technician"},
                                            {6702, "Magali Medhurst", 27, "Investor Solutions Representative"},
                                            {5312, "Khalid Beahan", 27, "Dynamic Quality Facilitator"},
                                            {7094, "Weldon Turner", 28, "District Research Strategist"},
                                            {8163, "Herbert Wisoky", 28, "District Solutions Manager"},
                                            {8807, "Bartholome Erdman", 29, "Global Accounts Facilitator"},
                                            {4187, "Peter Hettinger", 29, "Dynamic Interactions Liaison"},
                                            {3375, "Junior Schmidt", 30, "International Response Representative"},
                                            {8448, "Daniela Torphy", 16, "Forward Data Officer"},
                                            {3224, "Filiberto Swift", 16, "Customer Applications Strategist"},
                                            {3071, "Piper Jacobi", 30, "Corporate Applications Consultant"},
                                            {7262, "Laury Zemlak", 15, "Senior Functionality Liaison"},
                                            {9113, "Elyssa Blick", 2, "Regional Communications Officer"},
                                            {8461, "Bennie Shanahan", 2, "Investor Accountability Liaison"},
                                            {2477, "Gerhard Cartwright", 3, "Legacy Brand Executive"},
                                            {6201, "Imani Bergstrom", 3, "Corporate Response Director"},
                                            {928, "Thurman Daniel", 4, "Future Security Facilitator"},
                                            {1826, "Hermann Mante", 4, "Legacy Creative Consultant"},
                                            {9512, "Sierra Altenwerth", 5, "Human Branding Assistant"},
                                            {8969, "Larissa West", 5, "Investor Quality Planner"},
                                            {9355, "Jensen Wolff", 6, "Chief Research Administrator"},
                                            {6888, "Myra Bahringer", 6, "Legacy Response Technician"},
                                            {9959, "Lavonne Abshire", 7, "Chief Group Architect"},
                                            {5753, "Jimmy Lowe", 7, "National Data Officer"},
                                            {2447, "Alia Barton", 1, "Legacy Configuration Producer"},
                                            {753, "Millie Schamberger", 8, "Future Response Developer"},
                                            {8950, "Darian Hagenes", 9, "Corporate Brand Executive"},
                                            {7525, "Colleen Moore", 9, "National Interactions Associate"},
                                            {7958, "Tyrese Crooks", 10, "Central Solutions Officer"},
                                            {445, "Lacey Pollich", 10, "Forward Accountability Coordinator"},
                                            {6223, "Hermann Koelpin", 11, "National Identity Consultant"},
                                            {4303, "Harmon Green", 11, "Regional Brand Officer"},
                                            {1419, "Heaven King", 12, "District Communications Orchestrator"},
                                            {5569, "Nathaniel Schultz", 12, "Legacy Applications Director"},
                                            {5374, "Sheridan Kohler", 13, "Senior Operations Producer"},
                                            {9190, "Asa Hermann", 13, "Chief Operations Orchestrator"},
                                            {6404, "Hipolito Sipes", 14, "Internal Interactions Analyst"},
                                            {7666, "Granville Towne", 14, "Principal Identity Orchestrator"},
                                            {3803, "Jennings Friesen", 8, "Customer Paradigm Orchestrator"},
                                            {5798, "Shirley Will", 15, "Direct Data Analyst"}
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        1,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 151, DateTimeKind.Local).AddTicks(
                                                2056),
                                            @"Voluptas quia nemo magnam natus dolor.
Eos assumenda dolorem ut consectetur voluptatem.
Et aliquam perferendis deserunt impedit iure eaque hic iste.
Laborum pariatur tempora minima maxime ut assumenda et eveniet beatae.
Laborum rem nulla quae.
Assumenda et molestiae et.
Eveniet ullam consectetur et exercitationem non et fugiat.
Odio vel dolorum quam.
Impedit hic aspernatur reprehenderit quas in quis nostrum.
Libero magnam repellendus.",
                                            "Intelligent Granite Towels", "Perspiciatis praesentium et ut.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 155, DateTimeKind.Local).AddTicks(
                                                5088),
                                            "https://kattie.org", 25
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        2,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 159, DateTimeKind.Local).AddTicks(
                                                8491),
                                            @"Ducimus laboriosam eos aperiam id rerum maxime asperiores.
Sed suscipit exercitationem.
Velit nihil ea commodi id incidunt cupiditate in.
Possimus vitae quis perspiciatis sed ex accusamus.
Molestiae nesciunt voluptates rem rerum.
Quaerat laboriosam iste molestiae.
Quaerat id delectus non neque rerum dolorem id aliquid temporibus.
Quia sint tempora.
Culpa inventore distinctio adipisci iusto reprehenderit quia dolor ad dolor.
Velit laudantium quaerat qui accusantium ab.",
                                            "Handmade Rubber Chair",
                                            "Animi eos est temporibus officiis quia animi vel placeat.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 159, DateTimeKind.Local).AddTicks(
                                                8565),
                                            "http://nicholas.com", 11
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        3,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 164, DateTimeKind.Local).AddTicks(
                                                2951),
                                            @"Qui sed cum.
Asperiores non eos tenetur eligendi.
Omnis maxime ea.
Quia omnis inventore occaecati repudiandae.
Neque temporibus sit.
Deserunt inventore rerum id ullam blanditiis.
Asperiores tenetur blanditiis cum earum.
Est consequatur architecto alias qui.
Nulla et at.
Voluptate ipsam porro.",
                                            "Refined Cotton Bike",
                                            "Enim deleniti neque nulla autem sit modi assumenda itaque.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 164, DateTimeKind.Local).AddTicks(
                                                3019),
                                            "https://jaycee.net", 23
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        4,
                                        new[] {"Created", "Description", "Name", "ShortDescription", "Updated", "Uri"},
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 168, DateTimeKind.Local).AddTicks(
                                                7401),
                                            @"Ut libero voluptatem velit consequatur quo voluptas vitae nisi.
Laboriosam voluptatem quod eaque ut nostrum culpa eligendi.
Sint temporibus laborum.
Molestiae consequuntur et officiis amet rem dolorem et nesciunt est.
Fugit id ab.
Qui animi quidem beatae quas.
Soluta sed accusantium.
Et et architecto ut et ut alias nostrum temporibus laboriosam.
Dignissimos voluptas et laborum dolor.
Sit autem soluta et et labore ad ullam et.",
                                            "Intelligent Metal Cheese",
                                            "Quo odit dolor et et saepe aut sed tempora sunt.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 168, DateTimeKind.Local).AddTicks(
                                                7455),
                                            "https://kacey.name"
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        5,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 173, DateTimeKind.Local).AddTicks(
                                                3937),
                                            @"Sit hic possimus voluptatibus totam voluptas enim dolores.
Nisi reiciendis voluptatem alias quibusdam et modi reprehenderit nostrum.
Consequatur explicabo consequatur unde.
Sed eum dicta quas cum repellat et aspernatur omnis.
Quaerat ut cum saepe et omnis excepturi.
Explicabo omnis repellat eum modi exercitationem beatae veniam.
Voluptas rerum earum et et in rerum et vitae et.
Illum fugiat porro aut mollitia rem ut saepe perspiciatis.
Quae ea dolores ut enim repellat.
Minus nesciunt temporibus molestiae qui consequatur eos blanditiis est.",
                                            "Small Fresh Sausages",
                                            "Id rerum temporibus accusantium neque sed maxime non voluptatem fugiat.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 173, DateTimeKind.Local).AddTicks(
                                                3996),
                                            "http://joan.info", 19
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        6,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 177, DateTimeKind.Local).AddTicks(
                                                4794),
                                            @"Porro ut ducimus enim vel in.
Dolores molestiae qui.
Nihil dolorem vero nihil doloribus.
Recusandae hic possimus totam rerum accusamus placeat.
Aut dolorem facere rerum voluptate aperiam.
Distinctio sint qui modi.
Facilis modi perspiciatis est.
Labore vitae ea molestias odio reiciendis eligendi quibusdam consectetur dolorem.
Voluptatibus voluptatem fugiat consequatur.
At quis ut quaerat consectetur sint quam explicabo.",
                                            "Handmade Concrete Fish",
                                            "Nemo quo sed dolorem sint impedit ea voluptatibus in.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 177, DateTimeKind.Local).AddTicks(
                                                4838),
                                            "http://pansy.net", 20
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        7,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 181, DateTimeKind.Local).AddTicks(
                                                8467),
                                            @"Ratione eius non.
Cum quos enim.
Laudantium autem pariatur enim excepturi nihil quisquam distinctio nemo.
Magni ut quos ut ad fugit fugiat alias.
Amet culpa aliquid earum aut ab.
Ipsa exercitationem non sunt.
Illum in ut.
Quia vero explicabo totam accusamus.
Consequuntur eaque neque iusto et sunt quia rerum explicabo quia.
Ipsa consequuntur ad aut pariatur quae quaerat ipsam.",
                                            "Unbranded Rubber Mouse", "Nihil qui eius distinctio ipsam et qui est.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 181, DateTimeKind.Local).AddTicks(
                                                8536),
                                            "https://pamela.info", 12
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        8,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 185, DateTimeKind.Local).AddTicks(
                                                9186),
                                            @"Ipsam omnis consequatur consequuntur et et.
Sed repudiandae voluptate est ad.
Magnam ad vel.
Sequi quia et cum illum quas quia inventore molestiae.
Autem quo et qui laudantium et qui.
Quasi soluta dolores est quibusdam architecto sit nemo magnam.
Officia quasi ex officiis dignissimos voluptates vel.
Omnis dolor dolores enim dolore.
Velit ullam iste.
Deleniti ut enim velit magnam.",
                                            "Awesome Rubber Ball", "Sed dolores cumque laudantium velit molestiae.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 185, DateTimeKind.Local).AddTicks(
                                                9230),
                                            "http://desiree.net", 3
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        9,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 189, DateTimeKind.Local).AddTicks(
                                                8743),
                                            @"Et reprehenderit cupiditate voluptas.
Odit ipsa ullam.
Et veritatis corrupti dolores sint ab dolorem voluptatibus ut.
Qui ut tempore enim.
Repudiandae sapiente sit iusto.
Sit libero id voluptas dicta quidem necessitatibus a quas eius.
Quia sequi ea cumque quidem.
Repudiandae repudiandae nostrum est enim nisi quis sed nemo dolore.
Ipsam pariatur magni.
Id ipsam dolor voluptas praesentium est voluptate debitis neque.",
                                            "Awesome Granite Pants",
                                            "Deleniti perferendis officiis laborum maxime ut laborum sunt itaque.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 189, DateTimeKind.Local).AddTicks(
                                                8792),
                                            "http://johnson.biz", 10
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        10,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 193, DateTimeKind.Local).AddTicks(
                                                8895),
                                            @"Corporis omnis iure consequatur facilis.
Sunt provident id voluptas fuga itaque dolor voluptas.
Aperiam magni qui autem voluptas esse molestiae.
Voluptatibus totam quod voluptates vitae ex praesentium.
Eaque odio qui.
Et corrupti aliquid modi dolorem.
Nisi aut et facilis cum qui est ut iure perferendis.
Tenetur repellat eveniet vel numquam voluptate.
Et maxime aut dolor qui.
Qui dolores aperiam maiores debitis perspiciatis dolor animi delectus.",
                                            "Incredible Soft Gloves",
                                            "Architecto repudiandae distinctio suscipit temporibus ut.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 193, DateTimeKind.Local).AddTicks(
                                                8953),
                                            "http://heber.com", 18
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        11,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 197, DateTimeKind.Local).AddTicks(
                                                9390),
                                            @"Et delectus velit vitae consequatur voluptatem.
Sint dolorem accusamus.
Inventore dolor ea.
Nam voluptas incidunt officiis maiores deserunt dolorem.
Iste architecto iste iste dignissimos totam.
Eligendi quis et illo.
Voluptatem laudantium laudantium est veniam rerum corrupti et quae.
Ad est doloribus tempore debitis consequuntur numquam.
Aspernatur pariatur esse et vero consequatur qui omnis.
Ducimus a quia autem natus consequuntur similique id.",
                                            "Practical Granite Computer", "Autem blanditiis deserunt aut.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 197, DateTimeKind.Local).AddTicks(
                                                9434),
                                            "http://kamron.org", 16
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        12,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 202, DateTimeKind.Local).AddTicks(
                                                5311),
                                            @"Est deserunt vitae voluptatum dolorum ea quia.
Cupiditate est blanditiis quia cupiditate.
Eaque hic accusamus temporibus.
Atque nihil quasi tempore placeat possimus architecto asperiores.
Dolores beatae aut non molestias consectetur et vero.
Optio ipsum sequi et consectetur vitae mollitia sint dolores quasi.
Quasi aut unde sit beatae praesentium ratione cumque sint.
Quisquam unde velit laudantium debitis.
Est dolore vel sit eum dolor.
Quia dolorum error.",
                                            "Tasty Soft Sausages", "Incidunt doloribus beatae eum fuga alias eos.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 202, DateTimeKind.Local).AddTicks(
                                                5370),
                                            "http://thea.info", 24
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        13,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 207, DateTimeKind.Local).AddTicks(
                                                6145),
                                            @"Neque earum perspiciatis.
Fuga maxime blanditiis ipsam quidem accusamus tempore voluptatem id totam.
Eveniet autem error sint eveniet sint quo quo et explicabo.
Enim autem ex repudiandae accusantium fugit assumenda.
Modi ut provident adipisci asperiores.
Odit sint aliquid quia sequi corporis ut tempore omnis.
Sequi dolores et voluptatem consequuntur qui aut ipsa.
Vitae rem nam sapiente quisquam nulla natus omnis qui dolore.
Consequatur dolor similique optio temporibus alias ea et.
Autem minima animi perferendis qui.",
                                            "Handcrafted Wooden Car",
                                            "Saepe dolores et nesciunt assumenda magni corrupti enim repudiandae.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 207, DateTimeKind.Local).AddTicks(
                                                6209),
                                            "https://judd.name", 7
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        14,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 212, DateTimeKind.Local).AddTicks(
                                                5827),
                                            @"Vel nihil quia aliquam aut error et commodi consequuntur.
Voluptatum optio sunt.
Officiis et incidunt sapiente tenetur et itaque nostrum voluptatum.
Debitis et expedita ut aliquid ipsa qui.
Placeat ipsa laudantium quia saepe.
Temporibus voluptas sunt aliquam doloremque cum.
Autem modi aut voluptatum temporibus est libero velit.
Vel occaecati hic mollitia vel odit voluptates.
Aut vitae consequatur non rerum quae laboriosam.
Omnis possimus quia fuga quos soluta voluptatem et.",
                                            "Generic Cotton Soap",
                                            "Dicta facilis perspiciatis officiis error voluptatem inventore at eius.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 212, DateTimeKind.Local).AddTicks(
                                                5890),
                                            "http://idella.org", 13
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        15,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 217, DateTimeKind.Local).AddTicks(
                                                9081),
                                            @"Repellat et rerum nemo ducimus pariatur.
Assumenda est accusantium delectus.
Temporibus eaque magni quaerat voluptatem qui.
Laudantium commodi cumque ducimus voluptas.
Ea qui illum qui voluptatum.
Nam quos quia dolores odio ipsam.
Quam labore laudantium omnis aspernatur sed.
Natus veritatis dolor aspernatur nobis.
Error et eius.
Exercitationem autem sit.",
                                            "Incredible Wooden Chicken", "Dicta quia ea.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 217, DateTimeKind.Local).AddTicks(
                                                9130),
                                            "http://tyler.biz", 16
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        16,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 223, DateTimeKind.Local).AddTicks(
                                                3412),
                                            @"Eligendi rerum expedita nemo illo repellat officia.
Repudiandae velit placeat exercitationem unde porro quo.
Ut eos enim sequi velit facilis.
Quos suscipit fugit odit.
Et reiciendis ab iste atque quidem.
Id quam aliquam placeat modi laudantium.
Aut assumenda magnam autem sunt rerum distinctio eveniet.
Odio id nihil ut quae vel dolorem aut.
Saepe sit et itaque commodi adipisci consequatur voluptates enim aut.
Est fugit quae eligendi quas voluptas ea labore perspiciatis ut.",
                                            "Incredible Steel Towels", "Maxime ut voluptatem est illum veritatis.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 223, DateTimeKind.Local).AddTicks(
                                                3490),
                                            "http://eliezer.name", 6
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        17,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 228, DateTimeKind.Local).AddTicks(
                                                4212),
                                            @"Minima necessitatibus earum voluptate inventore.
Nam sit nobis sint aut voluptas est qui voluptas minima.
Molestiae et qui et odio ut.
Facere autem placeat dolorum hic ab qui.
Nemo porro velit et qui.
Dolor natus debitis magni veritatis maiores nemo veniam magni sit.
Vitae rem quia pariatur velit eveniet.
Doloribus quia et nobis incidunt ipsa vel harum amet.
Voluptatem ut voluptatem tempora blanditiis et placeat accusamus.
Ullam autem sint enim rem et quam.",
                                            "Gorgeous Plastic Bacon", "Et culpa ea sint.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 228, DateTimeKind.Local).AddTicks(
                                                4280),
                                            "http://francesca.org", 8
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        18,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 232, DateTimeKind.Local).AddTicks(
                                                9385),
                                            @"Sit quo placeat molestiae quibusdam nam.
Neque perferendis modi et ut.
Reiciendis saepe animi laudantium quasi perferendis dolor.
Vel accusamus excepturi illo dolor magni.
Voluptatem maxime mollitia a.
Magnam ut soluta unde.
Sed est numquam vitae voluptate rerum labore est eum sed.
Eius odit iste.
Quia illo sint delectus labore quis.
Provident dolores aut.",
                                            "Generic Granite Gloves", "Fuga libero est voluptas odio.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 232, DateTimeKind.Local).AddTicks(
                                                9443),
                                            "https://rebeka.name", 7
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        19,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 237, DateTimeKind.Local).AddTicks(
                                                7800),
                                            @"Voluptate molestiae ad ducimus tenetur.
Quia recusandae rerum ipsa rem architecto adipisci.
Porro enim in.
Fugit beatae quasi.
Voluptatem sunt quo quas.
Autem dolorem deserunt.
Sed nesciunt optio aut ipsa delectus molestiae.
Earum perferendis beatae.
Qui adipisci optio magni esse dolores quod earum.
Aliquam necessitatibus quis ut id cumque laborum qui dolores.",
                                            "Tasty Cotton Chips", "Assumenda quae dignissimos ex sint.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 237, DateTimeKind.Local).AddTicks(
                                                7853),
                                            "http://amie.name", 8
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        20,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 241, DateTimeKind.Local).AddTicks(
                                                9419),
                                            @"Veritatis asperiores excepturi id.
Eos quis soluta exercitationem nihil.
Maxime eos enim velit nulla odio voluptatem ipsam.
Dignissimos soluta enim culpa eos voluptas dolor.
Et qui qui nostrum qui vitae dolorem nostrum.
Et voluptas rem beatae vel aperiam nisi qui optio.
Est sunt quasi ut dicta suscipit.
Qui vel nobis non animi provident.
Quod sit voluptatum accusantium ea.
Dolor dignissimos sed sunt sunt recusandae voluptas sed harum est.",
                                            "Incredible Metal Chair", "Sit odio ut dolores ut et officia enim a in.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 241, DateTimeKind.Local).AddTicks(
                                                9483),
                                            "http://taryn.net", 21
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        21,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 246, DateTimeKind.Local).AddTicks(
                                                3414),
                                            @"Suscipit harum natus.
Aperiam est quo sed ut.
Distinctio sequi quos aut aspernatur minus porro maxime nesciunt amet.
Velit placeat rerum porro.
Cupiditate molestiae alias molestias ut nemo.
Est sed repellat dicta.
Ratione libero harum recusandae sunt blanditiis molestiae nesciunt.
Occaecati aliquam impedit necessitatibus beatae odit.
Eveniet dolores rerum tempore animi harum magnam.
Velit laudantium quo aliquid perferendis.",
                                            "Refined Concrete Bacon",
                                            "Cum aspernatur est quasi animi quos omnis quis et.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 246, DateTimeKind.Local).AddTicks(
                                                3468),
                                            "http://imelda.name", 8
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        22,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 250, DateTimeKind.Local).AddTicks(
                                                6015),
                                            @"Et consectetur sed non eum.
Fugit velit quae.
Quam aliquid minus odit quod sunt qui.
Voluptas nesciunt veritatis qui deleniti et illo est.
Deserunt quo eaque optio dolore fugit et.
Cupiditate voluptatem corrupti itaque laborum voluptatibus ut voluptatem vitae.
Sed iste cupiditate.
Iure quam labore neque aut incidunt quibusdam.
Qui rem modi natus cupiditate cumque consequatur qui.
Ut dolorem quaerat numquam.",
                                            "Unbranded Steel Mouse", "Expedita omnis incidunt quia dolorem quibusdam.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 250, DateTimeKind.Local).AddTicks(
                                                6064),
                                            "http://victoria.name", 17
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        23,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 254, DateTimeKind.Local).AddTicks(
                                                7063),
                                            @"Dolorum blanditiis dicta aut tempore.
Vel similique perferendis.
Ab praesentium sed perferendis voluptatem ut delectus qui.
Suscipit delectus voluptas voluptatum non.
Voluptate repellat ipsam ipsa dolorum ipsum in corrupti autem.
Commodi dicta dolores voluptatem non ea.
Rerum ipsa sit porro quis dolor magni facilis necessitatibus.
Quisquam cumque ea vero voluptatibus.
Ut ut ab dolores.
Qui unde rem aut neque et ut.",
                                            "Small Frozen Chair", "Eum in illo recusandae aut quia et placeat sed.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 254, DateTimeKind.Local).AddTicks(
                                                7116),
                                            "http://karianne.org", 4
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        24,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 258, DateTimeKind.Local).AddTicks(
                                                5240),
                                            @"Repellendus sit non vel non accusamus rerum nesciunt et.
Dolore dolores impedit maxime consequuntur.
Incidunt eum inventore maxime.
Voluptas dolore id unde voluptate voluptatem ratione quo.
Consequuntur nam cumque et amet.
Aut occaecati aut sapiente.
Eveniet accusantium dolor officiis itaque ipsa quis asperiores.
Hic similique tempore.
Consectetur sit est.
Fugiat cupiditate quisquam quod quia deserunt sed qui quibusdam.",
                                            "Fantastic Soft Pizza", "Natus voluptatibus illo.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 258, DateTimeKind.Local).AddTicks(
                                                5284),
                                            "http://crystel.biz", 28
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        25,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 262, DateTimeKind.Local).AddTicks(
                                                9470),
                                            @"Eos laborum consequatur omnis voluptatem est.
Natus doloribus deserunt molestiae dolorem.
Nemo est ex et hic.
Iusto in nostrum eos hic vel perferendis.
Labore dolorum in.
Quam veniam nemo.
Eius nesciunt earum deserunt unde incidunt facilis mollitia sed nam.
Est maxime accusantium deserunt at aut.
Corporis rerum sunt voluptatum.
Sapiente dolores voluptas quia itaque velit.",
                                            "Small Soft Fish", "Consequatur rerum et autem totam velit ullam.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 262, DateTimeKind.Local).AddTicks(
                                                9533),
                                            "https://uriah.com", 15
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        26,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 267, DateTimeKind.Local).AddTicks(
                                                5661),
                                            @"Quam voluptate voluptatem numquam accusantium.
Hic modi reiciendis quam ea maxime quod reiciendis.
Dolor odit nihil sit distinctio expedita.
Perspiciatis consequatur natus qui omnis maxime dolorem.
Tenetur debitis ut.
Ut qui quas voluptates.
Nobis deserunt repellendus et est sit quisquam blanditiis voluptatem ut.
Aspernatur tempora molestias enim.
Repudiandae et saepe expedita harum ipsum quia illo.
Laudantium atque sed est odio est minima velit autem et.",
                                            "Unbranded Metal Sausages", "Officiis animi saepe.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 267, DateTimeKind.Local).AddTicks(
                                                5705),
                                            "http://alena.biz", 27
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        27,
                                        new[] {"Created", "Description", "Name", "ShortDescription", "Updated", "Uri"},
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 271, DateTimeKind.Local).AddTicks(
                                                7206),
                                            @"Est consequatur ut quas fuga et est aliquam minima sint.
Qui voluptatem sit provident occaecati nulla.
Quia ut ut iusto aut velit possimus ipsam ut perspiciatis.
Optio itaque consequatur dolores rerum id sed recusandae odit.
Eos libero earum sint voluptatibus est qui in quo.
Ipsam sunt aliquid nam hic explicabo nostrum ex quasi.
Enim eveniet et iusto qui in sit deserunt.
Dignissimos esse qui quod.
Ipsam ullam a.
Doloremque excepturi laborum id aliquam est quas id qui cumque.",
                                            "Intelligent Fresh Sausages", "Autem dignissimos omnis et.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 271, DateTimeKind.Local).AddTicks(
                                                7255),
                                            "http://robyn.org"
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        28,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 275, DateTimeKind.Local).AddTicks(
                                                7715),
                                            @"Tempore omnis odio ut in minus.
Veniam aliquid est dolores quis.
Natus eos sunt facilis laboriosam.
Voluptatibus doloribus ipsam voluptate nisi repellat expedita porro iure similique.
Optio qui doloribus fuga quo.
Ullam maiores voluptas.
Magnam corporis aliquid provident expedita architecto iusto nemo numquam.
Voluptatem voluptatem in hic esse ex qui explicabo enim maxime.
Delectus quo repellat.
Nobis eum nihil quisquam vitae enim expedita perferendis neque qui.",
                                            "Generic Granite Sausages",
                                            "Delectus officiis est molestiae eius voluptate ipsum rerum.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 275, DateTimeKind.Local).AddTicks(
                                                7764),
                                            "http://jettie.org", 27
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        29,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 280, DateTimeKind.Local)
                                                .AddTicks(273),
                                            @"Quia porro qui facere quis officia ex culpa aut est.
Officia dolores expedita distinctio magnam esse modi est.
Ut ea nisi cupiditate est autem.
Delectus sint vel.
Dolores ut dolorem et earum.
Velit dignissimos aut est nostrum porro.
Corrupti qui aperiam maiores.
Sit praesentium illum atque sunt.
Et quis inventore qui quos sint reprehenderit veritatis.
Laboriosam ut aliquid ut.",
                                            "Small Concrete Shoes", "Quisquam qui alias dolor fuga dolor reiciendis.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 280, DateTimeKind.Local)
                                                .AddTicks(317),
                                            "http://hosea.org", 9
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        30,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 17, 14, 45, 22, 286, DateTimeKind.Local).AddTicks(
                                                2827),
                                            @"Suscipit aliquid provident aut ratione occaecati voluptatem.
Quis ut hic qui eius tempora iste.
Aliquam totam veritatis qui est ea blanditiis aperiam eos nemo.
Aut ut accusantium ex minus molestiae quibusdam qui.
Consequatur tempora quia qui quia voluptatem qui id eum.
Error placeat dolores.
Dolor voluptatem optio eligendi.
Aut eveniet dolores voluptatem quo pariatur sequi corporis nisi voluptates.
Nesciunt quam et placeat.
Accusantium quaerat laborum.",
                                            "Licensed Soft Computer", "Et voluptates quisquam.",
                                            new DateTime(2020, 4, 19, 14, 45, 22, 286, DateTimeKind.Local).AddTicks(
                                                3213),
                                            "https://benedict.com", 6
                                        });

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        1,
                                        new[] {"Email", "Name"},
                                        new object[] {"Rudy_Rohan@yahoo.com", "Annalise"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        2,
                                        new[] {"Email", "Name"},
                                        new object[] {"Abraham_Turcotte21@gmail.com", "Katharina"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        3,
                                        new[] {"Email", "Name"},
                                        new object[] {"Brant_Kassulke45@hotmail.com", "Justus"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        4,
                                        new[] {"Email", "Name"},
                                        new object[] {"Mossie.Lesch@hotmail.com", "Luciano"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        5,
                                        new[] {"Email", "Name"},
                                        new object[] {"Raleigh25@gmail.com", "Emely"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        6,
                                        new[] {"Email", "Name"},
                                        new object[] {"Martina_Johnson16@yahoo.com", "Ross"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        7,
                                        new[] {"Email", "Name"},
                                        new object[] {"Mohammed_Baumbach@hotmail.com", "Hyman"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        8,
                                        new[] {"Email", "Name"},
                                        new object[] {"Yvonne.Little@yahoo.com", "Karelle"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        9,
                                        new[] {"Email", "Name"},
                                        new object[] {"Ally_Farrell@gmail.com", "Laila"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        10,
                                        new[] {"Email", "Name"},
                                        new object[] {"Eulah.Grimes@gmail.com", "Emmy"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        11,
                                        new[] {"Email", "Name"},
                                        new object[] {"Jane_Schuster56@hotmail.com", "Jayne"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        12,
                                        new[] {"Email", "Name"},
                                        new object[] {"Lucy_Kohler68@hotmail.com", "Hilda"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        13,
                                        new[] {"Email", "Name"},
                                        new object[] {"Selina56@hotmail.com", "Enrique"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        14,
                                        new[] {"Email", "Name"},
                                        new object[] {"Liam.Wehner35@gmail.com", "Cyril"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        15,
                                        new[] {"Email", "Name"},
                                        new object[] {"Emanuel31@yahoo.com", "Lloyd"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        16,
                                        new[] {"Email", "Name"},
                                        new object[] {"Michale_Dach14@yahoo.com", "Declan"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        17,
                                        new[] {"Email", "Name"},
                                        new object[] {"Makenna.West31@gmail.com", "Weldon"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        18,
                                        new[] {"Email", "Name"},
                                        new object[] {"Herminia_Kling@yahoo.com", "Mable"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        19,
                                        new[] {"Email", "Name"},
                                        new object[] {"Guillermo.Blanda@gmail.com", "Shawn"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        20,
                                        new[] {"Email", "Name"},
                                        new object[] {"Triston_Beatty@yahoo.com", "Herbert"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        21,
                                        new[] {"Email", "Name"},
                                        new object[] {"Marcos_Cassin@yahoo.com", "Emmanuel"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        22,
                                        new[] {"Email", "Name"},
                                        new object[] {"Chanel41@gmail.com", "Carlee"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        23,
                                        new[] {"Email", "Name"},
                                        new object[] {"Jacklyn.Witting95@gmail.com", "Margaret"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        24,
                                        new[] {"Email", "Name"},
                                        new object[] {"Celine.Kerluke63@yahoo.com", "Ryleigh"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        25,
                                        new[] {"Email", "Name"},
                                        new object[] {"Delfina11@hotmail.com", "Lowell"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        26,
                                        new[] {"Email", "Name"},
                                        new object[] {"Kadin_Wolff@gmail.com", "Clifton"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        27,
                                        new[] {"Email", "Name"},
                                        new object[] {"Elijah.Ebert20@hotmail.com", "Russel"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        28,
                                        new[] {"Email", "Name"},
                                        new object[] {"Isai_Macejkovic@yahoo.com", "Xzavier"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        29,
                                        new[] {"Email", "Name"},
                                        new object[] {"Lemuel93@gmail.com", "Orion"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        30,
                                        new[] {"Email", "Name"},
                                        new object[] {"Wade_Bogisich@hotmail.com", "Matilda"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        445);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        753);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        928);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1360);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1389);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1419);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1495);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1532);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        1826);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2046);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2351);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2447);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2477);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2837);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        2923);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3071);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3224);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3375);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3803);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        3963);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4187);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4303);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4430);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        4672);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5177);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5312);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5374);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5401);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5569);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5753);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        5798);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6166);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6201);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6223);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6404);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6702);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6739);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        6888);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7094);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7211);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7258);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7262);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7311);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7525);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7543);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7666);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7690);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        7958);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8163);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8448);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8461);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8807);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8950);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        8969);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9113);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9190);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9355);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9476);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9512);

            migrationBuilder.DeleteData("Collaborators",
                                        "Id",
                                        9959);

            migrationBuilder.AddColumn<bool>("IsHighlighted",
                                             "Highlight",
                                             "bit",
                                             nullable: false,
                                             defaultValue: false);

            migrationBuilder.InsertData("Collaborators",
                                        new[] {"Id", "FullName", "ProjectId", "Role"},
                                        new object[,]
                                        {
                                            {6428, "Dimitri Pollich", 1, "Lead Metrics Administrator"},
                                            {5066, "Dorthy Hand", 17, "International Assurance Engineer"},
                                            {2750, "Destin Wyman", 17, "Customer Identity Specialist"},
                                            {2008, "Jose Roob", 18, "Central Quality Orchestrator"},
                                            {8206, "Minnie Schuster", 18, "Lead Metrics Liaison"},
                                            {1945, "Jodie Hammes", 19, "Corporate Paradigm Technician"},
                                            {7001, "Susanna Boyer", 19, "Human Program Officer"},
                                            {3273, "Ona Morissette", 20, "Future Configuration Specialist"},
                                            {1865, "Hayden Torp", 20, "District Quality Designer"},
                                            {3965, "Judd Lind", 21, "Product Optimization Representative"},
                                            {3348, "Jamal Gottlieb", 21, "Dynamic Optimization Manager"},
                                            {9384, "Betty Will", 22, "National Configuration Architect"},
                                            {2039, "Emely Kris", 22, "Customer Marketing Representative"},
                                            {189, "Kayden Stoltenberg", 23, "Global Configuration Planner"},
                                            {2531, "Orie Senger", 23, "Forward Accounts Consultant"},
                                            {1276, "Hattie Jakubowski", 24, "Corporate Infrastructure Planner"},
                                            {9066, "Bryana Gleichner", 24, "Regional Tactics Producer"},
                                            {3533, "Sanford Lynch", 25, "District Implementation Coordinator"},
                                            {584, "Reyes Hickle", 25, "Investor Functionality Representative"},
                                            {696, "Retta Monahan", 26, "International Factors Producer"},
                                            {3969, "Carey Herman", 26, "Corporate Implementation Architect"},
                                            {678, "Alize Runolfsdottir", 27, "Human Branding Supervisor"},
                                            {6539, "Lina Farrell", 27, "Direct Marketing Strategist"},
                                            {238, "Romaine Maggio", 28, "District Assurance Representative"},
                                            {4117, "Johnny Corwin", 28, "Central Security Producer"},
                                            {9651, "Corene Ebert", 29, "Global Metrics Officer"},
                                            {3900, "Cesar Pagac", 29, "Direct Tactics Officer"},
                                            {6051, "Carey Cruickshank", 30, "National Usability Technician"},
                                            {99, "Darrin VonRueden", 16, "National Branding Liaison"},
                                            {9774, "Dewitt Rau", 16, "Human Program Liaison"},
                                            {7200, "Valentina Orn", 30, "Investor Branding Executive"},
                                            {2277, "Nelda Bauch", 15, "Global Applications Technician"},
                                            {2171, "Elise Jast", 2, "Regional Factors Developer"},
                                            {4446, "Aisha Hahn", 2, "Global Response Facilitator"},
                                            {9480, "Loyal Ruecker", 3, "Global Identity Orchestrator"},
                                            {300, "Bernard Beahan", 3, "Future Marketing Officer"},
                                            {5284, "Kennedy Nitzsche", 4, "Central Intranet Supervisor"},
                                            {8743, "London Leffler", 4, "Forward Infrastructure Developer"},
                                            {5160, "Wilburn Nienow", 5, "Legacy Applications Representative"},
                                            {9711, "Bertram Wisozk", 5, "Internal Brand Director"},
                                            {6591, "Denis Bosco", 6, "Regional Identity Executive"},
                                            {2760, "Willow Cruickshank", 6, "Future Configuration Coordinator"},
                                            {681, "Verlie Kiehn", 7, "Product Branding Technician"},
                                            {3262, "Cecelia Bode", 7, "Senior Metrics Coordinator"},
                                            {7567, "Ivy Zemlak", 1, "Regional Implementation Director"},
                                            {9912, "Edmund Nicolas", 8, "Direct Functionality Supervisor"},
                                            {9264, "Johathan Gerhold", 9, "District Optimization Planner"},
                                            {5270, "Domenick Cole", 9, "Future Branding Analyst"},
                                            {4875, "Marianne Koch", 10, "Product Optimization Agent"},
                                            {2564, "Kyler Gleason", 10, "International Identity Executive"},
                                            {4577, "Carlo Reichel", 11, "Corporate Brand Liaison"},
                                            {8681, "Norma Champlin", 11, "Direct Identity Liaison"},
                                            {4060, "Skye Orn", 12, "Global Infrastructure Developer"},
                                            {2505, "Misty Moore", 12, "Direct Web Planner"},
                                            {5687, "Shayne Daugherty", 13, "Investor Program Engineer"},
                                            {618, "Cruz Collins", 13, "Corporate Metrics Director"},
                                            {8548, "Gillian Effertz", 14, "Principal Markets Agent"},
                                            {3909, "Gardner Hagenes", 14, "Product Interactions Orchestrator"},
                                            {1916, "Maryjane Kutch", 8, "Product Creative Director"},
                                            {4749, "Evan Leffler", 15, "Direct Intranet Officer"}
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        1,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 450, DateTimeKind.Local).AddTicks(
                                                9567),
                                            @"Quidem corrupti adipisci quo aut nobis qui accusamus.
Aut enim necessitatibus iste nihil eos repellendus magnam voluptas.
Voluptatem tempora omnis.
Corporis dolor enim quas sequi quis.
Molestiae ab cumque quia nesciunt eum adipisci maiores sapiente reiciendis.
Rem aspernatur ea.
Vitae omnis eum soluta molestiae ex.
Ab amet sint dicta.
Necessitatibus recusandae cumque.
Labore culpa pariatur culpa.",
                                            "Awesome Wooden Hat", "Amet iusto ut possimus.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 456, DateTimeKind.Local).AddTicks(
                                                1619),
                                            "https://coy.name", 1
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        2,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 460, DateTimeKind.Local).AddTicks(
                                                5999),
                                            @"Quasi illum cumque molestias cum voluptatum.
Culpa consectetur repellat excepturi est quasi consequatur nemo illo.
Quibusdam vero saepe dolore dolorem.
Saepe voluptas natus molestiae quo.
Officiis voluptatibus sapiente.
Harum rerum quasi incidunt consectetur quas voluptas quisquam enim.
Modi tempore eum adipisci iure veniam.
Aut enim quis eos.
Et omnis aut sit.
Unde voluptatem corrupti.",
                                            "Ergonomic Concrete Cheese", "Perferendis omnis nulla odio nihil.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 460, DateTimeKind.Local).AddTicks(
                                                6078),
                                            "https://diego.info", 17
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        3,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 466, DateTimeKind.Local).AddTicks(
                                                4236),
                                            @"Iusto ipsa modi et consequatur ut iste aut est quo.
Ut molestiae cumque sint molestiae aut iure.
Cumque repellendus quasi quia.
Voluptatibus possimus delectus.
Iure aperiam voluptatibus.
Est incidunt aut molestias culpa.
Placeat consequatur laudantium id perspiciatis corporis a earum.
Quisquam porro reprehenderit.
Ut officiis rem.
Consequatur quo aspernatur sint.",
                                            "Gorgeous Steel Mouse", "In excepturi quos id minus ipsam numquam.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 466, DateTimeKind.Local).AddTicks(
                                                4300),
                                            "http://reginald.com", 28
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        4,
                                        new[] {"Created", "Description", "Name", "ShortDescription", "Updated", "Uri"},
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 471, DateTimeKind.Local).AddTicks(
                                                3853),
                                            @"Labore dolor non ut.
Eaque maxime dolore voluptas.
Reiciendis beatae voluptate numquam eaque odio tempore atque dolorem qui.
Hic aut consequatur sunt facilis qui.
Qui impedit perspiciatis minus voluptatem distinctio incidunt aut voluptas.
Recusandae voluptatum eos sit amet dolores omnis expedita et repellat.
Blanditiis laboriosam commodi sequi.
Omnis quis quasi porro sit occaecati.
Non nihil et qui et.
Maxime voluptatibus accusantium animi id unde eligendi minima aut.",
                                            "Refined Plastic Tuna",
                                            "Et ab sint id qui distinctio consequatur quis sit.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 471, DateTimeKind.Local).AddTicks(
                                                3907),
                                            "http://sophie.com"
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        5,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 476, DateTimeKind.Local).AddTicks(
                                                5387),
                                            @"Velit neque recusandae et similique ipsa sint.
Neque perferendis magni vero id vel laudantium eius tempore.
Blanditiis libero esse repellendus qui ut illo.
Enim nam ipsa vero.
Dicta deleniti magni.
Non nisi adipisci adipisci.
Provident architecto in quo provident molestias assumenda.
Fugit omnis non quam corrupti.
Consectetur et culpa libero at quia.
At et corporis aut ipsa vel rem cum recusandae.",
                                            "Tasty Frozen Computer",
                                            "Voluptatum voluptas rerum eos laboriosam quia accusamus id unde.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 476, DateTimeKind.Local).AddTicks(
                                                5455),
                                            "https://odessa.org", 1
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        6,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 481, DateTimeKind.Local).AddTicks(
                                                2297),
                                            @"Accusantium earum molestias rerum cum nisi officiis.
Soluta ad enim quod corrupti magni exercitationem est dolor.
Id sunt id ipsam pariatur nihil quo non.
Nulla accusamus ullam nemo expedita.
Dignissimos aliquid provident ipsum doloribus.
Magni blanditiis dicta praesentium.
Recusandae magni magnam.
Sit numquam autem vel repudiandae cupiditate.
Quo sunt dicta ut maxime ipsa accusantium molestiae vel delectus.
Eum ipsa omnis voluptas occaecati consectetur deleniti.",
                                            "Sleek Metal Soap", "Quidem sint vero repellendus corrupti qui soluta.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 481, DateTimeKind.Local).AddTicks(
                                                2370),
                                            "http://stevie.name", 23
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        7,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 487, DateTimeKind.Local).AddTicks(
                                                2684),
                                            @"Ducimus rerum odio dicta facere qui sunt et nesciunt.
Aspernatur voluptatem maxime.
Rerum esse eligendi sit.
Nemo necessitatibus deleniti vero corporis.
Est et sequi commodi enim qui rem.
Omnis deserunt dolores quia iusto iusto doloremque excepturi quia.
Omnis est ipsum quo rem.
Dolore sapiente quod neque.
Earum ut repellendus eos corrupti odit.
Itaque et necessitatibus fuga fuga dignissimos.",
                                            "Unbranded Granite Fish", "Et quas explicabo quaerat.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 487, DateTimeKind.Local).AddTicks(
                                                2757),
                                            "https://malcolm.name", 7
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        8,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 493, DateTimeKind.Local).AddTicks(
                                                1179),
                                            @"Molestiae consequatur mollitia et fugit nemo impedit.
Odio est fuga aliquid amet.
Sit est sunt eos totam voluptatem et est facilis at.
Sequi nihil asperiores vero quidem aliquam nulla nihil.
Inventore exercitationem repellendus doloremque tenetur culpa sint.
Consequatur qui doloribus sequi et.
Esse expedita modi unde et.
Omnis nihil dolorem maxime dolorem sapiente animi porro blanditiis et.
Omnis facilis nam necessitatibus aperiam.
Perspiciatis sit necessitatibus ut facere porro inventore eos officia.",
                                            "Handcrafted Wooden Car", "Qui in sed esse.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 493, DateTimeKind.Local).AddTicks(
                                                1267),
                                            "https://ella.net", 6
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        9,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 499, DateTimeKind.Local).AddTicks(
                                                5390),
                                            @"Et asperiores voluptatibus.
Vero quia incidunt numquam.
Et quam vel autem soluta commodi voluptas.
Quibusdam maiores aut aut in quo quia eos.
Iure est qui et eligendi.
Voluptates qui cum consectetur voluptates aperiam saepe.
Eum est architecto alias reiciendis adipisci voluptate.
Nisi molestiae vitae quia consequatur sit nesciunt quis.
Omnis veritatis id qui qui.
Facilis omnis aut rem aut alias ipsum vero.",
                                            "Tasty Plastic Keyboard", "Ut voluptatum minima ut quas quo tempora earum.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 499, DateTimeKind.Local).AddTicks(
                                                5498),
                                            "https://clementina.net", 11
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        10,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 504, DateTimeKind.Local).AddTicks(
                                                6772),
                                            @"Necessitatibus distinctio reprehenderit repudiandae.
Ut nostrum est dolore.
Molestiae qui aspernatur.
Et qui ipsum sunt quam enim necessitatibus id.
Repellendus similique unde quia omnis dolorem minima voluptatum eos aperiam.
Qui ipsam laborum quaerat est eligendi laborum repellendus.
Numquam architecto quaerat.
Eius velit et magni.
Commodi dolore minima sequi id est ut.
Pariatur non occaecati delectus perspiciatis error quam.",
                                            "Rustic Soft Hat", "Qui sed sed repudiandae pariatur qui.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 504, DateTimeKind.Local).AddTicks(
                                                6850),
                                            "https://macy.biz", 27
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        11,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 509, DateTimeKind.Local).AddTicks(
                                                3173),
                                            @"Et aliquid dolore voluptatem saepe et.
Molestiae molestiae in alias voluptates eos nulla dicta voluptatem.
Tempore molestias voluptas.
Architecto nostrum aut numquam minus quidem officia quia.
Accusantium eveniet eaque dolorem earum quia.
Illum est et harum sapiente.
Ut fugit alias quo aut sunt voluptas harum.
Sed totam cum.
Veritatis dignissimos recusandae neque eos adipisci magnam reiciendis omnis.
Officiis ut ut.",
                                            "Handmade Metal Gloves",
                                            "Maiores aliquam voluptatem odio quia omnis commodi placeat est non.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 509, DateTimeKind.Local).AddTicks(
                                                3242),
                                            "https://bennett.net", 10
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        12,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 514, DateTimeKind.Local).AddTicks(
                                                9708),
                                            @"Sunt quos fugit aut fugiat rerum corrupti sed.
Impedit minus doloribus aut magni quod.
Distinctio vero vel perferendis omnis quaerat ut laudantium ipsam facere.
Qui aut quia et nesciunt natus voluptate ullam.
Repellendus et voluptatem quis provident labore.
Et et possimus soluta beatae minima assumenda ea voluptatum.
Reiciendis itaque enim officia est quibusdam.
Neque ut explicabo.
Temporibus in et animi eum.
Adipisci eos nihil aut deleniti.",
                                            "Incredible Fresh Chips",
                                            "Corrupti fugiat fugiat iste architecto rerum eos.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 514, DateTimeKind.Local).AddTicks(
                                                9777),
                                            "http://nicholas.com", 23
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        13,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 520, DateTimeKind.Local).AddTicks(
                                                2156),
                                            @"Necessitatibus dolores alias quasi quia amet ratione molestias minima illo.
Beatae dolore sit sunt et.
Et dolores esse qui possimus illum et consequatur occaecati.
Facere dignissimos laboriosam eos pariatur consequuntur.
Consequatur veritatis vero harum et nostrum autem voluptatibus iste.
Incidunt asperiores reprehenderit voluptas voluptas ipsam cum veniam dicta.
Dolorem dolores voluptatem.
Et est cumque reprehenderit et itaque adipisci deleniti tenetur.
Est excepturi rerum eos harum quia.
Voluptas velit aperiam.",
                                            "Handmade Rubber Shoes", "Aut consequuntur facilis.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 520, DateTimeKind.Local).AddTicks(
                                                2205),
                                            "http://raymundo.info", 22
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        14,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 525, DateTimeKind.Local).AddTicks(
                                                4374),
                                            @"Ut quod aspernatur error fugit deserunt maiores sint.
Officia facere ipsa maiores consectetur aliquam cum.
Et aut sit voluptas recusandae quis placeat.
Natus illo ea.
Ad et porro aut qui esse est perspiciatis sunt sint.
Illo molestiae qui dignissimos quaerat ut eos.
Minus qui consequuntur et aut illo.
Ullam qui aliquid excepturi iste.
Culpa reiciendis laboriosam aut aperiam neque aliquam.
Deleniti dolores delectus eos ut sapiente in aliquid voluptas.",
                                            "Awesome Soft Car", "Dicta quam id tempore atque voluptatem.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 525, DateTimeKind.Local).AddTicks(
                                                4442),
                                            "https://brandyn.name", 19
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        15,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 530, DateTimeKind.Local).AddTicks(
                                                6675),
                                            @"Veritatis sapiente architecto voluptatum.
Et rem modi aut rerum cum magnam voluptatem nesciunt repellendus.
Explicabo hic hic laborum eos nihil dolores.
Et dolores consectetur minima sed suscipit officiis distinctio neque itaque.
Quia consectetur atque blanditiis odit quis ut minima sed non.
Dolorem doloribus voluptates eius minus et.
Nihil suscipit rerum dolorem.
Similique qui illo adipisci.
Rem aut perferendis.
Vel quaerat est.",
                                            "Fantastic Wooden Gloves", "Est quos voluptas et deserunt qui.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 530, DateTimeKind.Local).AddTicks(
                                                6749),
                                            "https://rita.net", 10
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        16,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 535, DateTimeKind.Local).AddTicks(
                                                1546),
                                            @"Animi odio quos blanditiis id voluptatem aliquid ab sint alias.
Quia dolore consequatur.
Repudiandae sit repellendus minus ullam autem consequuntur.
Voluptas laudantium quod dolores modi eos vitae quia at qui.
Officiis similique cupiditate.
Est asperiores quaerat quis est corrupti temporibus et.
Suscipit architecto sunt sequi perspiciatis.
Quasi consectetur rem et neque asperiores.
Voluptas consequuntur voluptates fugiat aut sed aliquid non recusandae.
Reprehenderit cumque perferendis quod ipsa quam iusto qui ut in.",
                                            "Incredible Concrete Pants", "Nesciunt voluptatem ab non culpa iure.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 535, DateTimeKind.Local).AddTicks(
                                                1615),
                                            "https://abdul.info", 16
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        17,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 539, DateTimeKind.Local).AddTicks(
                                                2730),
                                            @"Tempora magni laudantium vero est officia quo eos in sed.
Qui non est doloremque assumenda eaque ab fuga.
Voluptatum id reiciendis vero ut ullam vel.
Consequatur sed necessitatibus doloremque veritatis odit excepturi.
A culpa voluptas tenetur nobis aliquam omnis doloremque aliquid deleniti.
Distinctio est architecto.
Sit minus quae et fuga.
Est tenetur nemo est.
Est eos incidunt.
Iure nihil numquam fugit vel praesentium quia tempore.",
                                            "Ergonomic Plastic Tuna",
                                            "Eius nulla hic quas nihil magnam sequi debitis nostrum dolor.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 539, DateTimeKind.Local).AddTicks(
                                                2779),
                                            "https://rosalinda.info", 26
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        18,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 543, DateTimeKind.Local).AddTicks(
                                                4163),
                                            @"Fuga provident dolor sunt atque.
Et nobis sunt fuga dignissimos est voluptatem ut consequatur inventore.
Numquam omnis sed aut.
Inventore dolorum ut deleniti nobis ea quis.
Facere voluptas ex laudantium fugiat.
Dolores laboriosam id hic voluptatem qui suscipit fuga.
Enim doloremque ad veritatis labore rerum praesentium aut.
Dolore aspernatur voluptatum natus suscipit alias non.
Ipsam quisquam officiis sed magnam et.
Dolores error quibusdam.",
                                            "Handcrafted Fresh Cheese", "Eum illo veniam adipisci et repudiandae.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 543, DateTimeKind.Local).AddTicks(
                                                4212),
                                            "https://eleonore.com", 9
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        19,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 547, DateTimeKind.Local).AddTicks(
                                                8520),
                                            @"Numquam quibusdam numquam id velit.
Alias id id impedit quas laborum facere officia sit tempora.
Molestiae tempore dicta voluptatibus.
Quas delectus ut ipsa facilis.
Unde et qui dolor ut.
Ut blanditiis qui suscipit itaque et autem.
Sunt ut omnis qui soluta quia hic modi.
Quis praesentium in enim.
Nostrum dolore fugiat nesciunt nostrum quidem amet itaque error voluptas.
Ipsum nobis error facilis accusamus doloribus.",
                                            "Incredible Wooden Shoes", "Exercitationem et consectetur.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 547, DateTimeKind.Local).AddTicks(
                                                8584),
                                            "https://ivy.info", 14
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        20,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 552, DateTimeKind.Local).AddTicks(
                                                3332),
                                            @"Deserunt est porro quo enim.
Neque laboriosam et omnis laboriosam nesciunt iste sequi dolor qui.
Excepturi quidem eum culpa.
Vel esse aut quasi quod.
Inventore sint quibusdam delectus.
Vero nisi temporibus est.
Ut porro in nemo consequatur non impedit.
Nobis eaque autem id.
Pariatur est aperiam.
Aliquam reprehenderit ullam id sed incidunt accusantium minima.",
                                            "Fantastic Plastic Table",
                                            "Voluptatibus quis earum optio vel doloribus voluptas nam est et.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 552, DateTimeKind.Local).AddTicks(
                                                3385),
                                            "http://clair.biz", 28
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        21,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 556, DateTimeKind.Local).AddTicks(
                                                5870),
                                            @"Nihil omnis quia ea hic minima.
Fugit dicta qui nemo.
Cumque atque autem accusantium eum incidunt suscipit velit doloribus.
Eum saepe qui et.
Ab rem quis tempora deserunt est illum non ut.
Qui doloremque explicabo voluptas qui est deserunt sed vero et.
Est dolore qui recusandae autem voluptas.
Deleniti autem molestiae tenetur impedit excepturi.
Rerum et ipsum porro.
Aut tenetur odit.",
                                            "Rustic Metal Chips", "Et sed et.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 556, DateTimeKind.Local).AddTicks(
                                                5934),
                                            "https://dorothy.org", 25
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        22,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 561, DateTimeKind.Local)
                                                .AddTicks(423),
                                            @"Sit dignissimos modi cumque nostrum.
Aut molestiae ab saepe.
Et quia qui quidem earum molestiae nihil.
Dolorem accusantium atque quas quia dolor minus magni sit.
Voluptatibus qui et ea autem sit nobis esse aspernatur debitis.
Accusantium blanditiis et numquam ut ipsum.
Distinctio quibusdam laudantium sunt consequatur ut assumenda enim nulla earum.
Et natus dicta.
Dignissimos et enim laudantium enim hic in tenetur.
Eum perspiciatis voluptatem et similique in quis.",
                                            "Licensed Plastic Soap",
                                            "Laborum at rem sed occaecati velit numquam cupiditate.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 561, DateTimeKind.Local)
                                                .AddTicks(486),
                                            "http://gwen.info", 13
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        23,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 565, DateTimeKind.Local).AddTicks(
                                                2032),
                                            @"Velit ut perferendis voluptas et.
Ut non quaerat.
Earum maxime dolorem modi ut quod enim et velit.
Nihil et qui et expedita.
Earum soluta rerum.
Non sequi et.
Et qui iste id.
Natus impedit molestiae non eligendi in.
Reprehenderit ullam dolorum quia veritatis sunt tenetur et.
Quibusdam quia sunt amet consequatur dolores modi.",
                                            "Handmade Cotton Mouse", "Consectetur harum repellendus.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 565, DateTimeKind.Local).AddTicks(
                                                2100),
                                            "https://anibal.info", 13
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        24,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 573, DateTimeKind.Local).AddTicks(
                                                6340),
                                            @"At animi dolor ea ducimus.
Maiores cumque rerum inventore error.
Sed sed repellendus.
Aut et totam unde sunt nihil corrupti.
Sit veritatis autem in autem maiores odit dolores non dolorem.
Ut fugiat reprehenderit.
Maxime velit odit dolores neque deleniti amet excepturi culpa.
Doloribus sed enim quisquam maiores.
Et aut eveniet eum eum omnis velit voluptates.
Tenetur molestiae voluptatum exercitationem aut sint omnis quisquam.",
                                            "Sleek Concrete Hat", "Quisquam aliquid laudantium non voluptas ea.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 573, DateTimeKind.Local).AddTicks(
                                                6413),
                                            "http://lavon.biz", 10
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        25,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 577, DateTimeKind.Local).AddTicks(
                                                7558),
                                            @"Esse ut consequuntur libero sequi itaque consequatur autem adipisci.
Velit voluptas non ab quo eius deleniti temporibus non.
Temporibus repellat quidem incidunt et repellendus.
Omnis fugiat laboriosam qui odit officiis.
Aut itaque dolorem.
Ea voluptas quo odit non error.
Sunt fugit aliquid quia sit.
Deleniti deserunt exercitationem quae quam et eveniet ipsum.
Dignissimos blanditiis perspiciatis at facilis.
Delectus sunt ipsa.",
                                            "Gorgeous Soft Cheese",
                                            "Ab ab molestiae quis est soluta totam quasi delectus.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 577, DateTimeKind.Local).AddTicks(
                                                7617),
                                            "https://katelynn.biz", 4
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        26,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 581, DateTimeKind.Local).AddTicks(
                                                8963),
                                            @"Excepturi in architecto officia doloremque enim quaerat doloribus.
Aliquam error cum et quia quia.
Enim ut minima.
Ab eius occaecati commodi tenetur fugiat recusandae omnis.
Ullam accusantium eaque dolore voluptatibus culpa reprehenderit sit.
Maxime dolor temporibus iusto aut occaecati.
Autem voluptatem autem.
Est fugiat vitae tempora molestiae.
Nostrum illo impedit quidem velit suscipit et.
Aspernatur incidunt non molestiae earum.",
                                            "Practical Granite Soap",
                                            "Minus consequatur vero id dolores saepe laborum.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 581, DateTimeKind.Local).AddTicks(
                                                9031),
                                            "http://margaret.com", 25
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        27,
                                        new[] {"Created", "Description", "Name", "ShortDescription", "Updated", "Uri"},
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 591, DateTimeKind.Local).AddTicks(
                                                4227),
                                            @"Ab totam earum vel voluptatem consequatur.
Suscipit nemo vel perspiciatis ducimus.
Ratione debitis quis officia commodi.
Velit iste facilis aut.
Qui doloremque repellat quae ab laudantium.
Occaecati adipisci accusantium et harum quis.
Odit sunt nemo in optio ipsa veritatis ab.
Totam consequatur porro expedita quibusdam totam earum.
Ea quos laudantium vitae quis consequatur labore veniam nulla laboriosam.
Magnam voluptatem ut.",
                                            "Rustic Metal Keyboard", "Repellat officiis aliquam rerum.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 591, DateTimeKind.Local).AddTicks(
                                                5088),
                                            "http://abe.biz"
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        28,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 597, DateTimeKind.Local).AddTicks(
                                                3589),
                                            @"Non soluta suscipit aut soluta ab.
Odit quo eos dolorem harum.
Sed et sed at nihil aperiam quibusdam nisi similique.
Porro dolor ut.
Ab voluptas blanditiis natus ipsa ea eos voluptas natus.
Eaque expedita aut repellat sequi veritatis aut quia et odio.
Dolorem eaque explicabo tempora eum aut.
Sapiente iste dolorum.
Qui modi velit.
Eum in dolor incidunt a.",
                                            "Unbranded Cotton Chicken", "Quas ipsum vel ab veritatis.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 597, DateTimeKind.Local).AddTicks(
                                                3662),
                                            "https://carleton.org", 14
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        29,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 604, DateTimeKind.Local)
                                                .AddTicks(601),
                                            @"Reprehenderit modi est.
Suscipit distinctio occaecati omnis debitis.
Voluptas perferendis excepturi.
Et consequatur soluta provident reprehenderit aut deleniti quasi quia.
Veniam dolor voluptatem vitae eveniet praesentium perspiciatis architecto atque.
Eum aut dolorum.
Et quia impedit.
Corrupti et nulla quibusdam.
Enim repellat autem qui atque sequi non.
Voluptatem ut vero beatae sequi laboriosam odio.",
                                            "Small Granite Hat",
                                            "Quam praesentium magnam provident rem et dolorem consectetur expedita quis.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 604, DateTimeKind.Local)
                                                .AddTicks(669),
                                            "https://cicero.name", 15
                                        });

            migrationBuilder.UpdateData("Project",
                                        "Id",
                                        30,
                                        new[]
                                        {
                                            "Created", "Description", "Name", "ShortDescription", "Updated", "Uri",
                                            "UserId"
                                        },
                                        new object[]
                                        {
                                            new DateTime(2020, 4, 13, 17, 42, 58, 607, DateTimeKind.Local).AddTicks(
                                                7840),
                                            @"Rerum aut vitae occaecati quidem veritatis.
Tenetur nihil molestias quasi omnis rerum dicta repellendus enim corporis.
Ipsa mollitia ut.
Reprehenderit quia amet.
Adipisci rerum tempora voluptatem.
Vitae quo deserunt sint sit ex ex repellendus.
Sit in ab velit facere molestiae.
Sed atque accusantium repellat occaecati dolor molestiae eius.
Dolorum voluptates suscipit rem accusamus aperiam est.
Est doloribus molestiae.",
                                            "Awesome Fresh Chair", "Sunt qui sint hic veniam.",
                                            new DateTime(2020, 4, 15, 17, 42, 58, 607, DateTimeKind.Local).AddTicks(
                                                7893),
                                            "http://elisabeth.biz", 25
                                        });

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        1,
                                        new[] {"Email", "Name"},
                                        new object[] {"Alycia2@yahoo.com", "Gardner"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        2,
                                        new[] {"Email", "Name"},
                                        new object[] {"Conner.McGlynn@hotmail.com", "Bryon"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        3,
                                        new[] {"Email", "Name"},
                                        new object[] {"Tomas29@gmail.com", "Fannie"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        4,
                                        new[] {"Email", "Name"},
                                        new object[] {"Hildegard.Sporer23@yahoo.com", "Rudolph"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        5,
                                        new[] {"Email", "Name"},
                                        new object[] {"Moriah_Effertz2@gmail.com", "Brent"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        6,
                                        new[] {"Email", "Name"},
                                        new object[] {"Reinhold.Lockman@gmail.com", "Damian"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        7,
                                        new[] {"Email", "Name"},
                                        new object[] {"Dayne35@gmail.com", "Ethyl"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        8,
                                        new[] {"Email", "Name"},
                                        new object[] {"Tracey59@gmail.com", "Markus"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        9,
                                        new[] {"Email", "Name"},
                                        new object[] {"Eli_Lowe65@gmail.com", "Maybell"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        10,
                                        new[] {"Email", "Name"},
                                        new object[] {"Art_Ryan@hotmail.com", "Phyllis"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        11,
                                        new[] {"Email", "Name"},
                                        new object[] {"Laura46@hotmail.com", "Jordon"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        12,
                                        new[] {"Email", "Name"},
                                        new object[] {"Alexandrine.Windler@hotmail.com", "Geraldine"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        13,
                                        new[] {"Email", "Name"},
                                        new object[] {"Esteban19@hotmail.com", "Josue"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        14,
                                        new[] {"Email", "Name"},
                                        new object[] {"Lesly_Yost@yahoo.com", "Margot"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        15,
                                        new[] {"Email", "Name"},
                                        new object[] {"Eldridge.Torphy25@yahoo.com", "Margret"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        16,
                                        new[] {"Email", "Name"},
                                        new object[] {"Fernando.Murphy@hotmail.com", "Hollie"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        17,
                                        new[] {"Email", "Name"},
                                        new object[] {"Bianka_Rosenbaum27@hotmail.com", "Delilah"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        18,
                                        new[] {"Email", "Name"},
                                        new object[] {"Danielle23@gmail.com", "Mabelle"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        19,
                                        new[] {"Email", "Name"},
                                        new object[] {"Barrett_Shanahan17@hotmail.com", "Preston"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        20,
                                        new[] {"Email", "Name"},
                                        new object[] {"Lucinda.Cummings@gmail.com", "Santino"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        21,
                                        new[] {"Email", "Name"},
                                        new object[] {"Cristopher7@hotmail.com", "Ryley"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        22,
                                        new[] {"Email", "Name"},
                                        new object[] {"Rey_Howe@yahoo.com", "Myrtice"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        23,
                                        new[] {"Email", "Name"},
                                        new object[] {"Elsie.Bins@gmail.com", "Piper"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        24,
                                        new[] {"Email", "Name"},
                                        new object[] {"Irving.Hilpert@yahoo.com", "Jammie"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        25,
                                        new[] {"Email", "Name"},
                                        new object[] {"Barbara0@yahoo.com", "Abelardo"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        26,
                                        new[] {"Email", "Name"},
                                        new object[] {"Dexter_Quigley43@hotmail.com", "Cesar"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        27,
                                        new[] {"Email", "Name"},
                                        new object[] {"Nicklaus_McKenzie49@gmail.com", "Shea"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        28,
                                        new[] {"Email", "Name"},
                                        new object[] {"Bryce_Ebert86@hotmail.com", "Karina"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        29,
                                        new[] {"Email", "Name"},
                                        new object[] {"Travon.Ondricka@hotmail.com", "Gilbert"});

            migrationBuilder.UpdateData("User",
                                        "Id",
                                        30,
                                        new[] {"Email", "Name"},
                                        new object[] {"Nicolas25@yahoo.com", "Shawna"});
        }

    }

}
